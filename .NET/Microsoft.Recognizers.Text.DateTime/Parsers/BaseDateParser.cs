﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date";

        public static readonly DateObject NoDate = DateObject.MinValue.SafeCreateFromValue(0, 0, 0);

        private readonly IDateParserConfiguration config;

        public BaseDateParser(IDateParserConfiguration config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject reference)
        {
            var referenceDate = reference;

            object value = null;
            if (er.Type.Equals(ParserName, StringComparison.Ordinal))
            {
                var innerResult = ParseBasicRegexMatch(er.Text, referenceDate);
                if (!innerResult.Success)
                {
                    innerResult = ParseImplicitDate(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseWeekdayOfMonth(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseDurationWithAgoAndLater(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseDurationWithDate(er.Text, referenceDate);
                }

                // NumberWithMonth must be the second last one, because it only need to find a number and a month to get a "success"
                if (!innerResult.Success)
                {
                    innerResult = ParseNumberWithMonth(er.Text, referenceDate);
                }

                // SingleNumber last one
                if (!innerResult.Success)
                {
                    innerResult = ParseSingleNumber(er.Text, referenceDate);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)innerResult.FutureValue) },
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)innerResult.PastValue) },
                    };

                    value = innerResult;
                }
            }

            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = value,
                TimexStr = value == null ? string.Empty : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private static DateObject ComputeDate(int cardinal, int weekday, int month, int year)
        {
            var firstDay = DateObject.MinValue.SafeCreateFromValue(year, month, 1);
            var firstWeekday = firstDay.This((DayOfWeek)weekday);
            int dayOfWeekOfFirstDay = (int)firstDay.DayOfWeek;

            if (weekday == 0)
            {
                weekday = 7;
            }

            if (dayOfWeekOfFirstDay == 0)
            {
                dayOfWeekOfFirstDay = 7;
            }

            if (weekday < dayOfWeekOfFirstDay)
            {
                firstWeekday = firstDay.Next((DayOfWeek)weekday);
            }

            return firstWeekday.AddDays(7 * (cardinal - 1));
        }

        private static bool EndsWithTerms(string text, IImmutableList<string> terms)
        {
            var result = false;

            foreach (var term in terms)
            {
                if (text.EndsWith(term, StringComparison.Ordinal))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        // Parse basic patterns in DateRegexList
        private DateTimeResolutionResult ParseBasicRegexMatch(string text, DateObject referenceDate)
        {
            var trimmedText = text.Trim();

            foreach (var regex in this.config.DateRegexes)
            {
                var offset = 0;
                string relativeStr = null;

                var match = regex.Match(trimmedText);

                if (!match.Success)
                {
                    match = regex.Match(this.config.DateTokenPrefix + trimmedText);

                    // Handing cases like "(this)? 5.12" which only be recognized in "on (this)? 5.12"
                    if (match.Success)
                    {
                        offset = this.config.DateTokenPrefix.Length;
                        relativeStr = match.Groups["order"].Value;
                    }
                }

                if (match.Success)
                {
                    var relativeRegex = this.config.StrictRelativeRegex.MatchEnd(text.Substring(0, match.Index), trim: true);

                    if ((match.Index == offset && match.Length == trimmedText.Length) || (relativeRegex.Success && match.Index + match.Length == trimmedText.Length))
                    {
                        // Handing cases which contain relative term like "this 5/12"
                        if (match.Index != offset)
                        {
                            relativeStr = relativeRegex.Value;
                        }

                        // Value string will be set in Match2Date method
                        var ret = Match2Date(match, referenceDate, relativeStr);

                        return ret;
                    }
                }
            }

            return new DateTimeResolutionResult();
        }

        // Match several other cases
        // Including 'today', 'the day after tomorrow', 'on 13'
        private DateTimeResolutionResult ParseImplicitDate(string text, DateObject referenceDate)
        {
            var trimmedText = text.Trim();

            var ret = new DateTimeResolutionResult();

            // Handle "on 12"
            var match = this.config.OnRegex.Match(this.config.DateTokenPrefix + trimmedText);
            if (match.Success && match.Index == 3 && match.Length == trimmedText.Length)
            {
                int month = referenceDate.Month, year = referenceDate.Year;
                var dayStr = match.Groups["day"].Value;
                var day = this.config.DayOfMonth[dayStr];

                ret.Timex = DateTimeFormatUtil.LuisDate(-1, -1, day);

                DateObject futureDate, pastDate;
                var tryStr = DateTimeFormatUtil.LuisDate(year, month, day);
                if (DateObject.TryParse(tryStr, out DateObject _))
                {
                    futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
                    pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

                    if (futureDate < referenceDate)
                    {
                        futureDate = futureDate.AddMonths(+1);
                    }

                    if (pastDate >= referenceDate)
                    {
                        pastDate = pastDate.AddMonths(-1);
                    }
                }
                else
                {
                    futureDate = DateObject.MinValue.SafeCreateFromValue(year, month + 1, day);
                    pastDate = DateObject.MinValue.SafeCreateFromValue(year, month - 1, day);
                }

                ret.FutureValue = futureDate;
                ret.PastValue = pastDate;
                ret.Success = true;

                return ret;
            }

            // Handle "today", "the day before yesterday"
            var exactMatch = this.config.SpecialDayRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var swift = GetSwiftDay(exactMatch.Value);

                var value = referenceDate.Date.AddDays(swift);

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // Handle "two days from tomorrow"
            exactMatch = this.config.SpecialDayWithNumRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var swift = GetSwiftDay(exactMatch.Groups["day"].Value);
                var numErs = this.config.IntegerExtractor.Extract(trimmedText);
                var numOfDays = Convert.ToInt32((double)(this.config.NumberParser.Parse(numErs[0]).Value ?? 0));

                var value = referenceDate.AddDays(numOfDays + swift);

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(value.Year, value.Month, value.Day);
                ret.Success = true;

                return ret;
            }

            // Handle "two sundays from now"
            exactMatch = this.config.RelativeWeekDayRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var numErs = this.config.IntegerExtractor.Extract(trimmedText);
                var num = Convert.ToInt32((double)(this.config.NumberParser.Parse(numErs[0]).Value ?? 0));
                var weekdayStr = exactMatch.Groups["weekday"].Value;
                var value = referenceDate;

                // Check whether the determined day of this week has passed.
                if (value.DayOfWeek > (DayOfWeek)this.config.DayOfWeek[weekdayStr])
                {
                    num--;
                }

                while (num-- > 0)
                {
                    value = value.Next((DayOfWeek)this.config.DayOfWeek[weekdayStr]);
                }

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(value.Year, value.Month, value.Day);
                ret.Success = true;

                return ret;
            }

            // Handle "next Sunday", "upcoming Sunday"
            // We define "upcoming Sunday" as the nearest Sunday to come (not include today)
            // We define "next Sunday" as Sunday of next week
            exactMatch = this.config.NextRegex.MatchExact(trimmedText, trim: true);
            if (exactMatch.Success)
            {
                var weekdayStr = exactMatch.Groups["weekday"].Value;
                var value = referenceDate.Next((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                if (this.config.UpcomingPrefixRegex.MatchBegin(trimmedText, trim: true).Success)
                {
                    value = referenceDate.Upcoming((DayOfWeek)this.config.DayOfWeek[weekdayStr]);
                }
                else if (config.GetSwiftMonthOrYear(trimmedText) == 2)
                {
                    value = value.AddDays(7);
                }

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(value.Year, value.Month, value.Day);
                ret.Success = true;

                return ret;
            }

            // Handle "this Friday"
            exactMatch = this.config.ThisRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var weekdayStr = exactMatch.Groups["weekday"].Value;
                var value = referenceDate.This((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(value.Year, value.Month, value.Day);
                ret.Success = true;

                return ret;
            }

            // Handle "last Friday", "last mon"
            // We define "past Sunday" as the nearest Sunday that has already passed (not include today)
            // We define "previous Sunday" as Sunday of previous week
            exactMatch = this.config.LastRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var weekdayStr = exactMatch.Groups["weekday"].Value;
                var value = referenceDate.Last((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                if (this.config.PastPrefixRegex.MatchBegin(trimmedText, trim: true).Success)
                {
                    value = referenceDate.Past((DayOfWeek)this.config.DayOfWeek[weekdayStr]);
                }

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(value.Year, value.Month, value.Day);
                ret.Success = true;

                return ret;
            }

            // Handle "Friday"
            exactMatch = this.config.WeekDayRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var weekdayStr = exactMatch.Groups["weekday"].Value;
                var weekDay = this.config.DayOfWeek[weekdayStr];
                var value = referenceDate.This((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                if (weekDay == 0)
                {
                    weekDay = 7;
                }

                if (weekDay < (int)referenceDate.DayOfWeek)
                {
                    value = referenceDate.Next((DayOfWeek)weekDay);
                }

                ret.Timex = "XXXX-WXX-" + weekDay;
                var futureDate = value;
                var pastDate = value;
                if (futureDate < referenceDate)
                {
                    futureDate = futureDate.AddDays(7);
                }

                if (pastDate >= referenceDate)
                {
                    pastDate = pastDate.AddDays(-7);
                }

                ret.FutureValue = DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day);
                ret.PastValue = DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day);
                ret.Success = true;

                return ret;
            }

            // Handle "for the 27th."
            match = this.config.ForTheRegex.Match(text);

            if (match.Success)
            {
                int day = 0, month = referenceDate.Month, year = referenceDate.Year;
                var dayStr = match.Groups["DayOfMonth"].Value;

                // Create a extract result which content ordinal string of text
                ExtractResult er = new ExtractResult
                {
                    Text = dayStr,
                    Start = match.Groups["DayOfMonth"].Index,
                    Length = match.Groups["DayOfMonth"].Length,
                };

                day = Convert.ToInt32((double)(this.config.NumberParser.Parse(er).Value ?? 0));

                ret.Timex = DateTimeFormatUtil.LuisDate(-1, -1, day);

                DateObject futureDate, pastDate;
                var tryStr = DateTimeFormatUtil.LuisDate(year, month, day);
                if (DateObject.TryParse(tryStr, out DateObject _))
                {
                    futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
                    pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

                    if (futureDate < referenceDate)
                    {
                        futureDate = futureDate.AddMonths(+1);
                    }

                    if (pastDate >= referenceDate)
                    {
                        pastDate = pastDate.AddMonths(-1);
                    }
                }
                else
                {
                    futureDate = DateObject.MinValue.SafeCreateFromValue(year, month + 1, day);
                    pastDate = DateObject.MinValue.SafeCreateFromValue(year, month - 1, day);
                }

                ret.FutureValue = futureDate;
                ret.PastValue = pastDate;
                ret.Success = true;

                return ret;
            }

            // Handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
            match = this.config.WeekDayAndDayOfMothRegex.Match(text);
            if (match.Success)
            {
                int month = referenceDate.Month, year = referenceDate.Year;

                // create a extract result which content ordinal string of text
                ExtractResult extractResultTmp = new ExtractResult
                {
                    Text = match.Groups["DayOfMonth"].Value,
                    Start = match.Groups["DayOfMonth"].Index,
                    Length = match.Groups["DayOfMonth"].Length,
                };

                // parse the day in text into number
                var day = Convert.ToInt32((double)(this.config.NumberParser.Parse(extractResultTmp).Value ?? 0));

                // The validity of the phrase is guaranteed in the Date Extractor
                ret.Timex = DateTimeFormatUtil.LuisDate(year, month, day);
                ret.FutureValue = new DateObject(year, month, day);
                ret.PastValue = new DateObject(year, month, day);
                ret.Success = true;

                return ret;
            }

            // Handling cases like 'Monday 21', which both 'Monday' and '21' refer to the same date.
            // The year of expected date can be different to the year of referenceDate.
            match = this.config.WeekDayAndDayRegex.Match(text);
            if (match.Success)
            {
                int month = referenceDate.Month, year = referenceDate.Year;

                // Create a extract result which content ordinal string of text
                ExtractResult ertmp = new ExtractResult
                {
                    Text = match.Groups["day"].Value,
                    Start = match.Groups["day"].Index,
                    Length = match.Groups["day"].Length,
                };

                // Parse the day in text into number
                var day = Convert.ToInt32((double)(this.config.NumberParser.Parse(ertmp).Value ?? 0));

                // Firstly, find a latest date with the "day" as pivotDate.
                // Secondly, if the pivotDate equals the referenced date, in other word, the day of the referenced date is exactly the "day".
                // In this way, check if the pivotDate is the weekday. If so, then the futureDate and the previousDate are the same date (referenced date).
                // Otherwise, increase the pivotDate month by month to find the latest futureDate and decrease the pivotDate month
                // by month to the latest previousDate.
                // Notice: if the "day" is larger than 28, some months should be ignored in the increase or decrease procedure.
                var pivotDate = new DateObject(year, month, 1);
                var daysInMonth = DateObject.DaysInMonth(year, month);
                if (daysInMonth >= day)
                {
                    pivotDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
                }
                else
                {
                    // Add 1 month is enough, since 1, 3, 5, 7, 8, 10, 12 months has 31 days
                    pivotDate = pivotDate.AddMonths(1);
                    pivotDate = DateObject.MinValue.SafeCreateFromValue(pivotDate.Year, pivotDate.Month, day);
                }

                var numWeekDayInt = (int)pivotDate.DayOfWeek;
                var extractedWeekDayStr = match.Groups["weekday"].Value;
                var weekDay = this.config.DayOfWeek[extractedWeekDayStr];
                if (!pivotDate.Equals(DateObject.MinValue))
                {
                    if (day == referenceDate.Day && numWeekDayInt == weekDay)
                    {
                        // The referenceDate is the weekday and with the "day".
                        ret.FutureValue = new DateObject(year, month, day);
                        ret.PastValue = new DateObject(year, month, day);
                        ret.Timex = DateTimeFormatUtil.LuisDate(year, month, day);
                    }
                    else
                    {
                        var futureDate = pivotDate;
                        var pastDate = pivotDate;

                        while ((int)futureDate.DayOfWeek != weekDay || futureDate.Day != day || futureDate < referenceDate)
                        {
                            // Increase the futureDate month by month to find the expected date (the "day" is the weekday) and
                            // make sure the futureDate not less than the referenceDate.
                            futureDate = futureDate.AddMonths(1);
                            var tmp = DateObject.DaysInMonth(futureDate.Year, futureDate.Month);
                            if (tmp >= day)
                            {
                                // For months like January 31, after add 1 month, February 31 won't be returned, so the day should be revised ASAP.
                                futureDate = futureDate.SafeCreateFromValue(futureDate.Year, futureDate.Month, day);
                            }
                        }

                        ret.FutureValue = futureDate;

                        while ((int)pastDate.DayOfWeek != weekDay || pastDate.Day != day || pastDate > referenceDate)
                        {
                            // Decrease the pastDate month by month to find the expected date (the "day" is the weekday) and
                            // make sure the pastDate not larger than the referenceDate.
                            pastDate = pastDate.AddMonths(-1);
                            var tmp = DateObject.DaysInMonth(pastDate.Year, pastDate.Month);
                            if (tmp >= day)
                            {
                                // For months like March 31, after minus 1 month, February 31 won't be returned, so the day should be revised ASAP.
                                pastDate = pastDate.SafeCreateFromValue(pastDate.Year, pastDate.Month, day);
                            }
                        }

                        ret.PastValue = pastDate;

                        if (weekDay == 0)
                        {
                            weekDay = 7;
                        }

                        ret.Timex = "XXXX-WXX-" + weekDay;
                    }
                }

                ret.Success = true;

                return ret;
            }

            return ret;
        }

        // Handle cases like "January first", "twenty-two of August"
        // Handle cases like "20th of next month"
        private DateTimeResolutionResult ParseNumberWithMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimmedText = text.Trim();
            int month = 0, day = 0, year = referenceDate.Year;
            bool ambiguous = true;

            var er = this.config.OrdinalExtractor.Extract(trimmedText);

            // check if the extraction is empty or a relative ordinal (e.g. "next", "previous")
            if (er.Count == 0 || er[0].Metadata.IsOrdinalRelative)
            {
                er = this.config.IntegerExtractor.Extract(trimmedText);
            }

            if (er.Count == 0)
            {
                return ret;
            }

            var num = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));

            var match = this.config.MonthRegex.Match(trimmedText);
            if (match.Success)
            {
                month = this.config.MonthOfYear[match.Value.Trim()];
                day = num;

                var suffix = trimmedText.Substring(er[0].Start + er[0].Length ?? 0);
                GetYearInAffix(suffix, ref year, ref ambiguous, out bool success);

                // Check also in prefix
                if (!success && this.config.CheckBothBeforeAfter)
                {
                    var prefix = trimmedText.Substring(0, er[0].Start ?? 0);
                    GetYearInAffix(prefix, ref year, ref ambiguous, out success);
                }
            }

            // Handling relative month
            if (!match.Success)
            {
                match = this.config.RelativeMonthRegex.Match(trimmedText);
                if (match.Success)
                {
                    var monthStr = match.Groups["order"].Value;
                    var swift = this.config.GetSwiftMonthOrYear(monthStr);
                    month = referenceDate.AddMonths(swift).Month;
                    year = referenceDate.AddMonths(swift).Year;
                    day = num;
                    ambiguous = false;
                }
            }

            // Handling cases like 'second Sunday'
            if (!match.Success)
            {
                match = this.config.WeekDayRegex.Match(trimmedText);
                if (match.Success)
                {
                    month = referenceDate.Month;

                    // Resolve the date of wanted week day
                    var wantedWeekDay = this.config.DayOfWeek[match.Groups["weekday"].Value];
                    var firstDate = DateObject.MinValue.SafeCreateFromValue(referenceDate.Year, referenceDate.Month, 1);
                    var firstWeekDay = (int)firstDate.DayOfWeek;
                    var firstWantedWeekDay = firstDate.AddDays(wantedWeekDay > firstWeekDay ? wantedWeekDay - firstWeekDay : wantedWeekDay - firstWeekDay + 7);
                    var answerDay = firstWantedWeekDay.Day + ((num - 1) * 7);
                    day = answerDay;
                    ambiguous = false;
                }
            }

            if (!match.Success)
            {
                return ret;
            }

            // For LUIS format value string
            var futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

            if (ambiguous)
            {
                ret.Timex = DateTimeFormatUtil.LuisDate(-1, month, day);
                if (futureDate < referenceDate)
                {
                    futureDate = futureDate.AddYears(+1);
                }

                if (pastDate >= referenceDate)
                {
                    pastDate = pastDate.AddYears(-1);
                }
            }
            else
            {
                ret.Timex = DateTimeFormatUtil.LuisDate(year, month, day);
            }

            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        // Handle cases like "the 27th". In the extractor, only the unmatched weekday and date will output this date.
        private DateTimeResolutionResult ParseSingleNumber(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimmedText = text.Trim();
            int month = referenceDate.Month, day = 0, year = referenceDate.Year;

            var er = this.config.OrdinalExtractor.Extract(trimmedText);
            if (er.Count == 0)
            {
                er = this.config.IntegerExtractor.Extract(trimmedText);
            }

            if (er.Count == 0)
            {
                return ret;
            }

            day = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));

            // For LUIS format value string
            ret.Timex = DateTimeFormatUtil.LuisDate(-1, -1, day);
            var pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

            if (!futureDate.Equals(DateObject.MinValue) && futureDate < referenceDate)
            {
                futureDate = futureDate.AddMonths(1);
            }

            if (!pastDate.Equals(DateObject.MinValue) && pastDate >= referenceDate)
            {
                pastDate = pastDate.AddMonths(-1);
            }

            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        // Handle cases like "two days ago"
        private DateTimeResolutionResult ParseDurationWithAgoAndLater(string text, DateObject referenceDate)
        {
            return AgoLaterUtil.ParseDurationWithAgoAndLater(
                text,
                referenceDate,
                config.DurationExtractor,
                config.DurationParser,
                config.NumberParser,
                config.UnitMap,
                config.UnitRegex,
                config.UtilityConfiguration,
                GetSwiftDay);
        }

        // Parse combined patterns Duration + Date, e.g. '3 days before Monday', '4 weeks after January 15th'
        private DateTimeResolutionResult ParseDurationWithDate(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var durationRes = config.DurationExtractor.Extract(text, referenceDate);

            foreach (var duration in durationRes)
            {
                var matches = config.UnitRegex.Matches(duration.Text);
                if (matches.Count > 0)
                {
                    var afterStr = text.Substring((int)duration.Start + (int)duration.Length);

                    // Check if the Duration entity is followed by "before|from|after"
                    var connector = config.BeforeAfterRegex.MatchBegin(afterStr, trim: true);
                    if (connector.Success)
                    {
                        // Parse Duration
                        var pr = config.DurationParser.Parse(duration, referenceDate);

                        // Parse Date
                        if (pr.Value != null)
                        {
                            var dateString = afterStr.Substring(connector.Index + connector.Length).Trim();
                            var innerResult = ParseBasicRegexMatch(dateString, referenceDate);
                            if (!innerResult.Success)
                            {
                                innerResult = ParseImplicitDate(dateString, referenceDate);
                            }

                            if (!innerResult.Success)
                            {
                                innerResult = ParseWeekdayOfMonth(dateString, referenceDate);
                            }

                            if (!innerResult.Success)
                            {
                                innerResult = ParseNumberWithMonth(dateString, referenceDate);
                            }

                            if (!innerResult.Success)
                            {
                                innerResult = ParseSingleNumber(dateString, referenceDate);
                            }

                            if (!innerResult.Success)
                            {
                                var holidayEr = new ExtractResult
                                {
                                    Start = 0,
                                    Length = dateString.Length,
                                    Text = dateString,
                                    Type = Constants.SYS_DATETIME_DATE,
                                    Data = null,
                                    Metadata = new Metadata { IsHoliday = true },
                                };
                                innerResult = (DateTimeResolutionResult)config.HolidayParser.Parse(holidayEr, referenceDate).Value;
                            }

                            // Combine parsed results Duration + Date
                            if (innerResult.Success)
                            {
                                var isFuture = connector.Groups["after"].Success ? true : false;
                                DateObject date = (DateObject)innerResult.FutureValue;
                                var resultDateTime = DurationParsingUtil.ShiftDateTime(pr.TimexStr, date, future: isFuture);
                                ret.Timex = $"{DateTimeFormatUtil.LuisDate(resultDateTime)}";

                                ret.FutureValue = ret.PastValue = resultDateTime;
                                ret.SubDateTimeEntities = new List<object> { pr };
                                ret.Success = true;
                            }
                        }
                    }
                }
            }

            return ret;
        }

        // Parse a regex match which includes 'day', 'month' and 'year' (optional) group
        private DateTimeResolutionResult Match2Date(Match match, DateObject referenceDate, string relativeStr)
        {
            var ret = new DateTimeResolutionResult();
            int month = 0, day = 0, year = 0;

            var monthStr = match.Groups["month"].Value;
            var dayStr = match.Groups["day"].Value;
            var weekdayStr = match.Groups["weekday"].Value;
            var yearStr = match.Groups["year"].Value;
            var writtenYear = match.Groups["fullyear"].Value;
            var ambiguousCentury = false;

            if (this.config.MonthOfYear.ContainsKey(monthStr) && this.config.DayOfMonth.ContainsKey(dayStr))
            {
                month = this.config.MonthOfYear[monthStr];
                day = this.config.DayOfMonth[dayStr];

                if (!string.IsNullOrEmpty(writtenYear))
                {
                    year = this.config.DateExtractor.GetYearFromText(match);
                }
                else if (!string.IsNullOrEmpty(yearStr))
                {
                    year = int.Parse(yearStr, CultureInfo.InvariantCulture);
                    if (year < 100 && year >= Constants.MinTwoDigitYearPastNum)
                    {
                        year += Constants.BASE_YEAR_PAST_CENTURY;
                    }
                    else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum)
                    {
                        year += Constants.BASE_YEAR_CURRENT_CENTURY;
                    }
                    else if (year >= Constants.MaxTwoDigitYearFutureNum && year < Constants.MinTwoDigitYearPastNum)
                    {
                        // Two-digit years in the range [30, 40) are ambiguos
                        ambiguousCentury = true;
                    }
                }
            }

            var noYear = false;
            if (year == 0)
            {
                year = referenceDate.Year;
                if (!string.IsNullOrEmpty(relativeStr))
                {
                    var swift = this.config.GetSwiftMonthOrYear(relativeStr);

                    // @TODO Improve handling of next/last in particular cases "next friday 5/12" when the next friday is not 5/12.
                    if (!string.IsNullOrEmpty(weekdayStr))
                    {
                        swift = 0;
                    }

                    year += swift;
                }
                else
                {
                    noYear = true;
                }

                ret.Timex = DateTimeFormatUtil.LuisDate(-1, month, day);
            }
            else
            {
                ret.Timex = DateTimeFormatUtil.LuisDate(year, month, day);
            }

            var futurePastDates = DateContext.GenerateDates(noYear, referenceDate, year, month, day);
            ret.FutureValue = futurePastDates.future;
            ret.PastValue = futurePastDates.past;
            ret.Success = true;

            // Ambiguous two-digit years are assigned values in both centuries (e.g. 35 -> 1935, 2035)
            if (ambiguousCentury)
            {
                ret.PastValue = futurePastDates.past.AddYears(Constants.BASE_YEAR_PAST_CENTURY);
                ret.FutureValue = futurePastDates.future.AddYears(Constants.BASE_YEAR_CURRENT_CENTURY);
                ret.Timex = TimexUtility.ModifyAmbiguousCenturyTimex(ret.Timex);
            }

            return ret;
        }

        private DateTimeResolutionResult ParseWeekdayOfMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimmedText = text.Trim();
            var match = this.config.WeekDayOfMonthRegex.Match(trimmedText);
            if (!match.Success)
            {
                return ret;
            }

            var cardinalStr = match.Groups["cardinal"].Value;
            var weekdayStr = match.Groups["weekday"].Value;
            var monthStr = match.Groups["month"].Value;
            var noYear = false;
            int year;

            var cardinal = this.config.IsCardinalLast(cardinalStr) ? 5 : this.config.CardinalMap[cardinalStr];

            var weekday = this.config.DayOfWeek[weekdayStr];
            int month;
            if (string.IsNullOrEmpty(monthStr))
            {
                var swift = this.config.GetSwiftMonthOrYear(trimmedText);

                month = referenceDate.AddMonths(swift).Month;
                year = referenceDate.AddMonths(swift).Year;
            }
            else
            {
                month = this.config.MonthOfYear[monthStr];
                year = referenceDate.Year;
                noYear = true;
            }

            var value = ComputeDate(cardinal, weekday, month, year);
            if (value.Month != month)
            {
                cardinal -= 1;
                value = value.AddDays(-7);
            }

            var futureDate = value;
            var pastDate = value;
            if (noYear && futureDate < referenceDate)
            {
                futureDate = ComputeDate(cardinal, weekday, month, year + 1);
                if (futureDate.Month != month)
                {
                    futureDate = futureDate.AddDays(-7);
                }
            }

            if (noYear && pastDate >= referenceDate)
            {
                pastDate = ComputeDate(cardinal, weekday, month, year - 1);
                if (pastDate.Month != month)
                {
                    pastDate = pastDate.AddDays(-7);
                }
            }

            // Here is a very special case, timeX follow future date
            ret.Timex = $@"XXXX-{month.ToString("D2", CultureInfo.InvariantCulture)}-WXX-{weekday}-#{cardinal}";
            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        private int GetSwiftDay(string text)
        {
            var trimmedText = this.config.Normalize(text.Trim());
            var swift = 0;

            var match = this.config.RelativeDayRegex.Match(text);

            // The sequence here is important
            // As suffix "day before yesterday" should be matched before suffix "day before" or "yesterday"
            if (config.SameDayTerms.Contains(trimmedText))
            {
                swift = 0;
            }
            else if (EndsWithTerms(trimmedText, config.PlusTwoDayTerms))
            {
                swift = 2;
            }
            else if (EndsWithTerms(trimmedText, config.MinusTwoDayTerms))
            {
                swift = -2;
            }
            else if (EndsWithTerms(trimmedText, config.PlusOneDayTerms))
            {
                swift = 1;
            }
            else if (EndsWithTerms(trimmedText, config.MinusOneDayTerms))
            {
                swift = -1;
            }
            else if (match.Success)
            {
                swift = GetSwift(text);
            }

            return swift;
        }

        private int GetSwift(string text)
        {
            var trimmedText = text.Trim();

            var swift = 0;
            if (this.config.NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }
            else if (this.config.PreviousPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }

            return swift;
        }

        private void GetYearInAffix(string affix, ref int year, ref bool ambiguous, out bool success)
        {
            var matchYear = this.config.YearSuffix.Match(affix);
            success = matchYear.Success;
            if (success)
            {
                year = ((BaseDateExtractor)this.config.DateExtractor).GetYearFromText(matchYear);
                if (year != Constants.InvalidYear)
                {
                    ambiguous = false;
                }
            }
        }
    }
}