using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.English;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; //"Date";

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
            if (er.Type.Equals(ParserName))
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
                        {TimeTypeConstants.DATE, FormatUtil.FormatDate((DateObject) innerResult.FutureValue)}
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATE, FormatUtil.FormatDate((DateObject) innerResult.PastValue)}
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
                TimexStr = value == null ? "" : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = ""
            };

            return ret;
        }

        // parse basic patterns in DateRegexList
        private DateTimeResolutionResult ParseBasicRegexMatch(string text, DateObject referenceDate)
        {
            var trimedText = text.Trim();
            foreach (var regex in this.config.DateRegexes)
            {
                var offset = 0;
                var match = regex.Match(trimedText);
                if (!match.Success)
                {
                    match = regex.Match(this.config.DateTokenPrefix + trimedText);
                    offset = this.config.DateTokenPrefix.Length;
                }

                if (match.Success && match.Index == offset && match.Length == trimedText.Length)
                {
                    // LUIS value string will be set in Match2Date method
                    var ret = Match2Date(match, referenceDate);
                    return ret;
                }
            }
            return new DateTimeResolutionResult();
        }

        // match several other cases
        // including 'today', 'the day after tomorrow', 'on 13'
        private DateTimeResolutionResult ParseImplicitDate(string text, DateObject referenceDate)
        {
            var trimedText = text.Trim();

            var ret = new DateTimeResolutionResult();

            // handle "on 12"
            var match = this.config.OnRegex.Match(this.config.DateTokenPrefix + trimedText);
            if (match.Success && match.Index == 3 && match.Length == trimedText.Length)
            {
                int day = 0, month = referenceDate.Month, year = referenceDate.Year;
                var dayStr = match.Groups["day"].Value.ToLower();
                day = this.config.DayOfMonth[dayStr];

                ret.Timex = FormatUtil.LuisDate(-1, -1, day);

                DateObject futureDate, pastDate, temp;
                var tryStr = FormatUtil.LuisDate(year, month, day);
                if (DateObject.TryParse(tryStr, out temp))
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

            // handle "today", "the day before yesterday"
            match = this.config.SpecialDayRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var swift = this.config.GetSwiftDay(match.Value);

                var value = referenceDate.AddDays(swift);

                ret.Timex = FormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "next Sunday"
            match = this.config.NextRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var weekdayStr = match.Groups["weekday"].Value.ToLower();
                var value = referenceDate.Next((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                ret.Timex = FormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "this Friday"
            match = this.config.ThisRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var weekdayStr = match.Groups["weekday"].Value.ToLower();
                var value = referenceDate.This((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                ret.Timex = FormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "last Friday", "last mon"
            match = this.config.LastRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var weekdayStr = match.Groups["weekday"].Value.ToLower();
                var value = referenceDate.Last((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                ret.Timex = FormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "Friday"
            match = this.config.WeekDayRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var weekdayStr = match.Groups["weekday"].Value.ToLower();
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

                ret.FutureValue = futureDate;
                ret.PastValue = pastDate;
                ret.Success = true;

                return ret;
            }

            // handle "for the 27th."
            match = this.config.ForTheRegex.Match(text);
            if (match.Success)
            {
                int day = 0, month = referenceDate.Month, year = referenceDate.Year;
                var dayStr = match.Groups["DayOfMonth"].Value.ToLower();

                // create a extract result which content ordinal string of text
                ExtractResult er = new ExtractResult();
                er.Text = dayStr;
                er.Start = match.Groups["DayOfMonth"].Index;
                er.Length = match.Groups["DayOfMonth"].Length;

                day = Convert.ToInt32((double)(this.config.NumberParser.Parse(er).Value ?? 0));

                ret.Timex = FormatUtil.LuisDate(-1, -1, day);

                DateObject futureDate, pastDate, temp;
                var tryStr = FormatUtil.LuisDate(year, month, day);
                if (DateObject.TryParse(tryStr, out temp))
                {
                    futureDate = new DateObject(year, month, day);
                }
                else
                {
                    futureDate = new DateObject(year, month + 1, day);
                }

                ret.FutureValue = futureDate;
                ret.PastValue = ret.FutureValue;
                ret.Success = true;

                return ret;
            }

            // handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
            match = this.config.WeekDayAndDayOfMothRegex.Match(text);
            if (match.Success)
            {
                // create a extract result which content ordinal string of text
                ExtractResult erTmp = new ExtractResult();
                erTmp.Text = match.Groups["DayOfMonth"].Value.ToString();
                erTmp.Start = match.Groups["DayOfMonth"].Index;
                erTmp.Length = match.Groups["DayOfMonth"].Length;

                // parse the day in text into number, and get week of day of this number which regarded as a day in this month
                var day = Convert.ToInt32((double)(this.config.NumberParser.Parse(erTmp).Value ?? 0));
                var date = new DateObject(referenceDate.Year, referenceDate.Month, day);
                var date2weekdayStr = date.DayOfWeek.ToString().ToLower();

                // get week day from text directly, compare it with the weekday parse above
                // to see whether they refer to a same week day
                var extractedWeekDayStr = match.Groups["weekday"].Value.ToString().ToLower();
                if (this.config.DayOfWeek[date2weekdayStr] == this.config.DayOfWeek[extractedWeekDayStr])
                {
                    int month = referenceDate.Month, year = referenceDate.Year;

                    ret.Timex = FormatUtil.LuisDate(year, month, day);
                    ret.FutureValue = new DateObject(year, month, day); ;
                    ret.PastValue = new DateObject(year, month, day); ;
                    ret.Success = true;

                    return ret;
                }
            }

            return ret;
        }

        // handle cases like "January first", "twenty-two of August"
        private DateTimeResolutionResult ParseNumberWithMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimedText = text.Trim().ToLower();
            int month = 0, day = 0, year = referenceDate.Year;

            var match = this.config.MonthRegex.Match(trimedText);
            if (!match.Success)
            {
                return ret;
            }

            month = this.config.MonthOfYear[match.Value.Trim()];

            var er = this.config.OrdinalExtractor.Extract(trimedText);
            if (er.Count == 0)
            {
                er = this.config.IntegerExtractor.Extract(trimedText);
            }

            if (er.Count == 0)
            {
                return ret;
            }

            day = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));

            // for LUIS format value string
            ret.Timex = FormatUtil.LuisDate(-1, month, day);
            var futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

            if (futureDate < referenceDate)
            {
                futureDate = futureDate.AddYears(+1);
            }

            if (pastDate >= referenceDate)
            {
                pastDate = pastDate.AddYears(-1);
            }

            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        // handle cases like "the 27th". In the extractor, only the unmatched weekday and date will output this date.
        private DateTimeResolutionResult ParseSingleNumber(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimedText = text.Trim().ToLower();
            int month = referenceDate.Month, day = 0, year = referenceDate.Year;

            var er = this.config.OrdinalExtractor.Extract(trimedText);
            if (er.Count == 0)
            {
                er = this.config.IntegerExtractor.Extract(trimedText);
            }

            if (er.Count == 0)
            {
                return ret;
            }

            day = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));

            // for LUIS format value string
            ret.Timex = FormatUtil.LuisDate(-1, -1, day);
            var futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

            if (futureDate < referenceDate)
            {
                futureDate = futureDate.AddMonths(1);
            }

            if (pastDate >= referenceDate)
            {
                pastDate = pastDate.AddMonths(-1);
            }

            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        // handle like "two days ago" 
        private DateTimeResolutionResult ParseDurationWithAgoAndLater(string text, DateObject referenceDate)
        {
            return AgoLaterUtil.ParseDurationWithAgoAndLater(
                text,
                referenceDate,
                config.DurationExtractor,
                config.DurationParser,
                config.UnitMap,
                config.UnitRegex,
                config.UtilityConfiguration,
                AgoLaterUtil.AgoLaterMode.Date
                );
        }

        // parse a regex match which includes 'day', 'month' and 'year' (optional) group
        private DateTimeResolutionResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var monthStr = match.Groups["month"].Value.ToLower();
            var dayStr = match.Groups["day"].Value.ToLower();
            var yearStr = match.Groups["year"].Value.ToLower();
            int month = 0, day = 0, year = 0;

            if (this.config.MonthOfYear.ContainsKey(monthStr) && this.config.DayOfMonth.ContainsKey(dayStr))
            {
                month = this.config.MonthOfYear[monthStr];
                day = this.config.DayOfMonth[dayStr];
                if (!string.IsNullOrEmpty(yearStr))
                {
                    year = int.Parse(yearStr);
                    if (year < 100 && year >= 90)
                    {
                        year += 1900;
                    }
                    else if (year < 100 && year < 20)
                    {
                        year += 2000;
                    }
                }
            }

            var noYear = false;
            if (year == 0)
            {
                year = referenceDate.Year;
                ret.Timex = FormatUtil.LuisDate(-1, month, day);
                noYear = true;
            }
            else
            {
                ret.Timex = FormatUtil.LuisDate(year, month, day);
            }

            var futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

            if (noYear && futureDate < referenceDate)
            {
                futureDate = futureDate.AddYears(+1);
            }

            if (noYear && pastDate >= referenceDate)
            {
                pastDate = pastDate.AddYears(-1);
            }

            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        private DateTimeResolutionResult ParseWeekdayOfMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimedText = text.Trim().ToLowerInvariant();
            var match = this.config.WeekDayOfMonthRegex.Match(trimedText);
            if (!match.Success)
            {
                return ret;
            }

            var cardinalStr = match.Groups["cardinal"].Value;
            var weekdayStr = match.Groups["weekday"].Value;
            var monthStr = match.Groups["month"].Value;
            var noYear = false;
            int year;

            int cardinal;
            if (this.config.IsCardinalLast(cardinalStr))
            {
                cardinal = 5;
            }
            else
            {
                cardinal = this.config.CardinalMap[cardinalStr];
            }

            var weekday = this.config.DayOfWeek[weekdayStr];
            int month;
            if (string.IsNullOrEmpty(monthStr))
            {
                var swift = this.config.GetSwiftMonth(trimedText);

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

            // here is a very special case, timeX followe future date
            ret.Timex = $@"XXXX-{month.ToString("D2")}-WXX-{weekday}-#{cardinal}";
            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        private static DateObject ComputeDate(int cardinal, int weekday, int month, int year)
        {
            var firstDay = DateObject.MinValue.SafeCreateFromValue(year, month, 1);
            var firstWeekday = firstDay.This((DayOfWeek)weekday);

            if (weekday == 0)
            {
                weekday = 7;
            }

            if (weekday < (int)firstDay.DayOfWeek)
            {
                firstWeekday = firstDay.Next((DayOfWeek)weekday);
            }

            return firstWeekday.AddDays(7 * (cardinal - 1));
        }
    }
}