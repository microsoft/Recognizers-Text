using System;
using System.Collections.Generic;
using System.Globalization;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDatePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATEPERIOD; //"DatePeriod";
        
        private static readonly Calendar Cal = DateTimeFormatInfo.InvariantInfo.Calendar;

        private readonly IDatePeriodParserConfiguration config;

        private static bool InclusiveEndPeriod = false;

        private const string WeekOfComment="WeekOf";

        private const string MonthOfComment = "MonthOf";

        public BaseDatePeriodParser(IDatePeriodParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceDate = refDate;

            object value = null;
            
            if (er.Type.Equals(ParserName))
            {
                var innerResult = ParseMonthWithYear(er.Text, referenceDate);
                if (!innerResult.Success)
                {
                    innerResult = ParseSimpleCases(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseOneWordPeriod(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = MergeTwoTimePoints(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseYear(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseWeekOfMonth(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseWeekOfYear(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseQuarter(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseSeason(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseWhichWeek(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseWeekOfDate(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseMonthOfDate(er.Text, referenceDate);
                }

                // parse duration should be at the end since it will extract "the last week" from "the last week of July"
                if (!innerResult.Success)
                {
                    innerResult = ParseDuration(er.Text, referenceDate);
                }

                if (innerResult.Success)
                {
                    if (innerResult.FutureValue != null && innerResult.PastValue != null)
                    {
                        innerResult.FutureResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATE,
                                FormatUtil.FormatDate(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item1)
                            },
                            {
                                TimeTypeConstants.END_DATE,
                                FormatUtil.FormatDate(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item2)
                            }
                        };

                        innerResult.PastResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATE,
                                FormatUtil.FormatDate(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item1)
                            },
                            {
                                TimeTypeConstants.END_DATE,
                                FormatUtil.FormatDate(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item2)
                            }
                        };
                    }
                    else
                    {
                        innerResult.FutureResolution = innerResult.PastResolution = new Dictionary<string, string>();
                    }
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

        private DateTimeResolutionResult ParseSimpleCases(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int beginDay, endDay;
            var noYear = false;

            var trimedText = text.Trim();
            var match = this.config.MonthFrontBetweenRegex.Match(trimedText);
            string beginLuisStr, endLuisStr;

            if (!match.Success)
            {
                match = this.config.BetweenRegex.Match(trimedText);
            }

            if (!match.Success)
            {
                match = this.config.MonthFrontSimpleCasesRegex.Match(trimedText);
            }

            if (!match.Success)
            {
                match = this.config.SimpleCasesRegex.Match(trimedText);
            }

            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var days = match.Groups["day"];
                beginDay = this.config.DayOfMonth[days.Captures[0].Value.ToLower()];
                endDay = this.config.DayOfMonth[days.Captures[1].Value.ToLower()];

                var monthStr = match.Groups["month"].Value;
                if (!string.IsNullOrEmpty(monthStr))
                {
                    month = this.config.MonthOfYear[monthStr.ToLower()];
                    noYear = true;
                }
                else
                {
                    monthStr = match.Groups["relmonth"].Value.Trim().ToLower();
                    var swiftMonth = this.config.GetSwiftDayOrMonth(monthStr);
                    switch (swiftMonth)
                    {
                        case 1:
                            if (month != 12)
                            {
                                month += 1;
                            }
                            else
                            {
                                month = 1;
                                year += 1;
                            }
                            break;
                        case -1:
                            if (month != 1)
                            {
                                month -= 1;
                            }
                            else
                            {
                                month = 12;
                                year -= 1;
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (this.config.IsFuture(monthStr))
                {
                    beginLuisStr = FormatUtil.LuisDate(year, month, beginDay);
                    endLuisStr = FormatUtil.LuisDate(year, month, endDay);
                }
                else
                {
                    beginLuisStr = FormatUtil.LuisDate(-1, month, beginDay);
                    endLuisStr = FormatUtil.LuisDate(-1, month, endDay);
                }
            }
            else
            {
                return ret;
            }

            // parse year
            var yearStr = match.Groups["year"].Value;
            if (!string.IsNullOrEmpty(yearStr))
            {
                year = int.Parse(yearStr);
                noYear = false;
            }

            int futureYear = year, pastYear = year;
            var startDate = DateObject.MinValue.SafeCreateFromValue(year, month, beginDay);

            if (noYear && startDate < referenceDate)
            {
                futureYear++;
            }

            if (noYear && startDate >= referenceDate)
            {
                pastYear--;
            }

            ret.Timex = $"({beginLuisStr},{endLuisStr},P{endDay - beginDay}D)";
            ret.FutureValue = new Tuple<DateObject, DateObject>(
                DateObject.MinValue.SafeCreateFromValue(futureYear, month, beginDay),
                DateObject.MinValue.SafeCreateFromValue(futureYear, month, endDay));
            ret.PastValue = new Tuple<DateObject, DateObject>(
                DateObject.MinValue.SafeCreateFromValue(pastYear, month, beginDay),
                DateObject.MinValue.SafeCreateFromValue(pastYear, month, endDay));
            ret.Success = true;

            return ret;
        }

        private DateTimeResolutionResult ParseOneWordPeriod(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int futureYear = year, pastYear = year;
            var earlyPrefix = false;
            var latePrefix = false;

            var trimedText = text.Trim().ToLower();
            var match = this.config.OneWordPeriodRegex.Match(trimedText);

            if (!(match.Success && match.Index == 0 && match.Length == trimedText.Length))
            {
                match = this.config.LaterEarlyPeriodRegex.Match(trimedText);
            }

            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                if (match.Groups["EarlyPrefix"].Success)
                {
                    earlyPrefix = true;
                    trimedText = match.Groups["suffix"].ToString();
                }

                if (match.Groups["LatePrefix"].Success)
                {
                    latePrefix = true;
                    trimedText = match.Groups["suffix"].ToString();
                }

                var monthStr = match.Groups["month"].Value;
                if (this.config.IsYearToDate(trimedText))
                {
                    ret.Timex = referenceDate.Year.ToString("D4");
                    ret.FutureValue =
                        ret.PastValue =
                            new Tuple<DateObject, DateObject>(DateObject.MinValue.SafeCreateFromValue(referenceDate.Year, 1, 1), referenceDate);
                    ret.Success = true;
                    return ret;
                }

                if (this.config.IsMonthToDate(trimedText))
                {
                    ret.Timex = referenceDate.Year.ToString("D4") + "-" + referenceDate.Month.ToString("D2");
                    ret.FutureValue =
                        ret.PastValue =
                            new Tuple<DateObject, DateObject>(
                                DateObject.MinValue.SafeCreateFromValue(referenceDate.Year, referenceDate.Month, 1), referenceDate);
                    ret.Success = true;
                    return ret;
                }

                if (!string.IsNullOrEmpty(monthStr))
                {
                    var swift = this.config.GetSwiftYear(trimedText);

                    month = this.config.MonthOfYear[monthStr.ToLower()];

                    if (swift >= -1)
                    {
                        ret.Timex = (referenceDate.Year + swift).ToString("D4") + "-" + month.ToString("D2");
                        year = year + swift;
                        futureYear = pastYear = year;
                    }
                    else
                    {
                        ret.Timex = "XXXX-" + month.ToString("D2");
                        if (month < referenceDate.Month)
                        {
                            futureYear++;
                        }

                        if (month >= referenceDate.Month)
                        {
                            pastYear--;
                        }
                    }
                }
                else
                {
                    var swift = this.config.GetSwiftDayOrMonth(trimedText);

                    if (this.config.IsWeekOnly(trimedText))
                    {
                        var monday = referenceDate.This(DayOfWeek.Monday).AddDays(7 * swift);

                        ret.Timex = monday.Year.ToString("D4") + "-W" +
                                    Cal.GetWeekOfYear(monday, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                                        .ToString("D2");

                        var beginDate = referenceDate.This(DayOfWeek.Monday).AddDays(7 * swift);
                        var endDate = InclusiveEndPeriod
                                        ? referenceDate.This(DayOfWeek.Sunday).AddDays(7 * swift)
                                        : referenceDate.This(DayOfWeek.Sunday).AddDays(7 * swift).AddDays(1);
                        
                        if (earlyPrefix)
                        {
                            endDate = InclusiveEndPeriod
                                        ? referenceDate.This(DayOfWeek.Wednesday).AddDays(7 * swift)
                                        : referenceDate.This(DayOfWeek.Wednesday).AddDays(7 * swift).AddDays(1);
                        }
                        if (latePrefix)
                        {
                            beginDate = referenceDate.This(DayOfWeek.Thursday).AddDays(7 * swift);
                        }

                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(beginDate, endDate);

                        ret.Success = true;
                        return ret;
                    }

                    if (this.config.IsWeekend(trimedText))
                    {
                        var beginDate = referenceDate.This(DayOfWeek.Saturday).AddDays(7 * swift);
                        var endDate = referenceDate.This(DayOfWeek.Sunday).AddDays(7 * swift);

                        ret.Timex = beginDate.Year.ToString("D4") + "-W" +
                                    Cal.GetWeekOfYear(beginDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                                        .ToString("D2") + "-WE";

                        endDate = InclusiveEndPeriod ? endDate : endDate.AddDays(1);

                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);

                        ret.Success = true;
                        return ret;
                    }

                    if (this.config.IsMonthOnly(trimedText))
                    {
                        month = referenceDate.AddMonths(swift).Month;
                        year = referenceDate.AddMonths(swift).Year;
                        ret.Timex = year.ToString("D4") + "-" + month.ToString("D2");
                        futureYear = pastYear = year;
                    }
                    else if (this.config.IsYearOnly(trimedText))
                    {
                        year = referenceDate.AddYears(swift).Year;

                        var beginDate = DateObject.MinValue.SafeCreateFromValue(year, 1, 1);
                        var endDate = InclusiveEndPeriod
                                    ? DateObject.MinValue.SafeCreateFromValue(year, 12, 31)
                                    : DateObject.MinValue.SafeCreateFromValue(year, 12, 31).AddDays(1);
                        if (earlyPrefix)
                        {
                            endDate = InclusiveEndPeriod
                                    ? DateObject.MinValue.SafeCreateFromValue(year, 6, 30)
                                    : DateObject.MinValue.SafeCreateFromValue(year, 6, 30).AddDays(1);
                        }
                        if (latePrefix)
                        {
                            beginDate = DateObject.MinValue.SafeCreateFromValue(year, 7, 1);
                        }

                        ret.Timex = year.ToString("D4");

                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(beginDate, endDate);

                        ret.Success = true;
                        return ret;
                    }
                }
            }
            else
            {
                return ret;
            }

            // only "month" will come to here
            var futureStart = DateObject.MinValue.SafeCreateFromValue(futureYear, month, 1);
            var futureEnd = InclusiveEndPeriod
                ? DateObject.MinValue.SafeCreateFromValue(futureYear, month, 1).AddMonths(1).AddDays(-1)
                : DateObject.MinValue.SafeCreateFromValue(futureYear, month, 1).AddMonths(1);
            var pastStart = DateObject.MinValue.SafeCreateFromValue(pastYear, month, 1);
            var pastEnd = InclusiveEndPeriod
                ? DateObject.MinValue.SafeCreateFromValue(pastYear, month, 1).AddMonths(1).AddDays(-1)
                : DateObject.MinValue.SafeCreateFromValue(pastYear, month, 1).AddMonths(1);
            if (earlyPrefix)
            {
                futureEnd = InclusiveEndPeriod
                    ? DateObject.MinValue.SafeCreateFromValue(futureYear, month, 15)
                    : DateObject.MinValue.SafeCreateFromValue(futureYear, month, 15).AddDays(1);
                pastEnd = InclusiveEndPeriod
                    ? DateObject.MinValue.SafeCreateFromValue(pastYear, month, 15)
                    : DateObject.MinValue.SafeCreateFromValue(pastYear, month, 15).AddDays(1);
            }
            else if (latePrefix)
            {
                futureStart = DateObject.MinValue.SafeCreateFromValue(futureYear, month, 16);
                pastStart = DateObject.MinValue.SafeCreateFromValue(pastYear, month, 16);
            }

            ret.FutureValue = new Tuple<DateObject, DateObject>(futureStart, futureEnd);

            ret.PastValue = new Tuple<DateObject, DateObject>(pastStart, pastEnd);

            ret.Success = true;

            return ret;
        }

        private DateTimeResolutionResult ParseMonthWithYear(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var match = this.config.MonthWithYear.Match(text);
            if (!match.Success)
            {
                match = this.config.MonthNumWithYear.Match(text);
            }

            if (match.Success && match.Length == text.Length)
            {
                var monthStr = match.Groups["month"].Value.ToLower();
                var yearStr = match.Groups["year"].Value.ToLower();
                var orderStr = match.Groups["order"].Value.ToLower();

                var month = this.config.MonthOfYear[monthStr.ToLower()];

                int year;
                if (!string.IsNullOrEmpty(yearStr))
                {
                    year = int.Parse(yearStr);
                }
                else
                {
                    var swift = this.config.GetSwiftYear(orderStr);
                    if (swift < -1)
                    {
                        return ret;
                    }
                    year = referenceDate.Year + swift;
                }

                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(
                    DateObject.MinValue.SafeCreateFromValue(year, month, 1),
                    InclusiveEndPeriod
                        ? DateObject.MinValue.SafeCreateFromValue(year, month, 1).AddMonths(1).AddDays(-1)
                        : DateObject.MinValue.SafeCreateFromValue(year, month, 1).AddMonths(1));

                ret.Timex = year.ToString("D4") + "-" + month.ToString("D2");

                ret.Success = true;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseYear(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var match = this.config.YearRegex.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                var year = int.Parse(match.Value);

                var beginDay = DateObject.MinValue.SafeCreateFromValue(year, 1, 1);

                var endDay = InclusiveEndPeriod
                        ? DateObject.MinValue.SafeCreateFromValue(year + 1, 1, 1).AddDays(-1)
                        : DateObject.MinValue.SafeCreateFromValue(year + 1, 1, 1);

                ret.Timex = year.ToString("D4");
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
                ret.Success = true;

                return ret;
            }

            return ret;
        }

        // parse entities that made up by two time points
        private DateTimeResolutionResult MergeTwoTimePoints(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var er = this.config.DateExtractor.Extract(text, referenceDate);
            if (er.Count < 2)
            {
                er = this.config.DateExtractor.Extract(this.config.TokenBeforeDate + text, referenceDate);
                if (er.Count < 2)
                {
                    return ret;
                }
                er[0].Start -= this.config.TokenBeforeDate.Length;
                er[1].Start -= this.config.TokenBeforeDate.Length;
            }

            var match = this.config.WeekWithWeekDayRangeRegex.Match(text);
            string weekPrefix = null;
            if (match.Success)
            {
                weekPrefix = match.Groups["week"].ToString();
            }

            if (!string.IsNullOrEmpty(weekPrefix))
            {
                er[0].Text = weekPrefix + " " + er[0].Text;
                er[1].Text = weekPrefix + " " + er[1].Text;
            }

            var pr1 = this.config.DateParser.Parse(er[0], referenceDate);
            var pr2 = this.config.DateParser.Parse(er[1], referenceDate);
            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            ret.SubDateTimeEntities= new List<object> { pr1, pr2 };

            DateObject futureBegin = (DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue,
                futureEnd = (DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue;

            DateObject pastBegin = (DateObject)((DateTimeResolutionResult)pr1.Value).PastValue,
                pastEnd = (DateObject)((DateTimeResolutionResult)pr2.Value).PastValue;

            if (futureBegin > futureEnd)
            {
                futureBegin = pastBegin;
            }

            if (pastEnd < pastBegin)
            {
                pastEnd = futureEnd;
            }
            
            ret.Timex = $"({pr1.TimexStr},{pr2.TimexStr},P{(futureEnd - futureBegin).TotalDays}D)";
            ret.FutureValue = new Tuple<DateObject, DateObject>(futureBegin, futureEnd);
            ret.PastValue = new Tuple<DateObject, DateObject>(pastBegin, pastEnd);
            ret.Success = true;

            return ret;
        }

        private DateTimeResolutionResult ParseDuration(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            DateObject beginDate;
            var endDate = beginDate = referenceDate;
            string timex = string.Empty;
            bool restNowSunday = false;

            var ers = config.DurationExtractor.Extract(text, referenceDate);
            if (ers.Count == 1)
            {
                var pr = config.DurationParser.Parse(ers[0]);
                var beforeStr = text.Substring(0, pr.Start ?? 0).Trim().ToLowerInvariant();
                var mod = "";
                if (pr.Value != null)
                {
                    var durationResult = (DateTimeResolutionResult) pr.Value;

                    if (string.IsNullOrEmpty(durationResult.Timex))
                    {
                        return ret;
                    }

                    var prefixMatch = config.PastRegex.Match(beforeStr);
                    if (prefixMatch.Success)
                    {
                        mod = TimeTypeConstants.beforeMod;
                        beginDate = GetSwiftDate(endDate, durationResult.Timex, false);
                    }

                    prefixMatch = config.FutureRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        mod = TimeTypeConstants.afterMod;
            
                        //for future the beginDate should add 1 first
                        beginDate = referenceDate.AddDays(1);
                        endDate= GetSwiftDate(beginDate, durationResult.Timex, true);
                    }

                    //handle the "in two weeks" case which means the second week
                    prefixMatch = config.InConnectorRegex.Match(beforeStr);
                    if(prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        mod = TimeTypeConstants.afterMod;

                        beginDate = referenceDate.AddDays(1);
                        endDate = GetSwiftDate(beginDate, durationResult.Timex, true);

                        //change the duration value and the beginDate
                        var unit = durationResult.Timex.Substring(durationResult.Timex.Length - 1);

                        durationResult.Timex = "P1" + unit;
                        beginDate = GetSwiftDate(endDate, durationResult.Timex, false);
                    }

                    if (!string.IsNullOrEmpty(mod))
                    {
                        ((DateTimeResolutionResult) pr.Value).Mod = mod;
                    }

                    timex = durationResult.Timex;

                    ret.SubDateTimeEntities = new List<object> { pr };
                }
            }
            
            //parse rest of
            var match = this.config.RestOfDateRegex.Match(text);
            if (match.Success)
            {
                var durationStr = match.Groups["duration"].Value;
                var durationUnit = this.config.UnitMap[durationStr];
                switch (durationUnit)
                {
                    case "W":
                        var diff = 7 - (((int)beginDate.DayOfWeek) == 0? 7: (int)beginDate.DayOfWeek);
                        endDate = beginDate.AddDays(diff);
                        timex = "P" + diff + "D";
                        if (diff == 0)
                        {
                            restNowSunday = true;
                        }
                        break;
                    case "MON":
                        endDate = DateObject.MinValue.SafeCreateFromValue(beginDate.Year, beginDate.Month, 1);
                        endDate = endDate.AddMonths(1).AddDays(-1);
                        diff = endDate.Day - beginDate.Day + 1;
                        timex = "P" + diff + "D";
                        break;
                    case "Y":
                        endDate = DateObject.MinValue.SafeCreateFromValue(beginDate.Year, 12, 1);
                        endDate = endDate.AddMonths(1).AddDays(-1);
                        diff = endDate.DayOfYear - beginDate.DayOfYear + 1;
                        timex = "P" + diff + "D";
                        break;
                }
            }

            if (!beginDate.Equals(endDate) || restNowSunday)
            {
                endDate = InclusiveEndPeriod ? endDate.AddDays(-1) : endDate;

                ret.Timex =
                    $"({FormatUtil.LuisDate(beginDate)},{FormatUtil.LuisDate(endDate)},{timex})";
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                ret.Success = true;

                return ret;
            }

            return ret;
        }

        private DateObject GetSwiftDate(DateObject date, string dateTimex, bool positiveSwift)
        {
            var numberString = dateTimex.Replace("P", "").Substring(0, dateTimex.Length - 2);
            string unit = dateTimex.Substring(dateTimex.Length - 1);

            Double.TryParse(numberString, out double swiftValue);

            if (swiftValue == 0)
            {
                return date;
            }

            if (!positiveSwift)
            {
                swiftValue = swiftValue*(-1);
            }

            switch (unit)
            {
                case "D":
                    date = date.AddDays(swiftValue);
                    break;
                case "W":
                    date = date.AddDays(7 * swiftValue);
                    break;
                case "M":
                    date = date.AddMonths((int)swiftValue);
                    break;
                case "Y":
                    date = date.AddYears((int)swiftValue);
                    break;
            }

            return date;
        }

        private DateTimeResolutionResult ParseWeekOfMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimedText = text.Trim().ToLowerInvariant();
            var match = this.config.WeekOfMonthRegex.Match(trimedText);
            if (!(match.Success && match.Length == text.Length))
            {
                return ret;
            }

            var cardinalStr = match.Groups["cardinal"].Value;
            var monthStr = match.Groups["month"].Value;
            var noYear = false;
            int year;

            int cardinal;
            if (this.config.IsLastCardinal(cardinalStr))
            {
                cardinal = 5;
            }
            else
            {
                cardinal = this.config.CardinalMap[cardinalStr];
            }

            int month;
            if (string.IsNullOrEmpty(monthStr))
            {
                var swift = this.config.GetSwiftDayOrMonth(trimedText);

                month = referenceDate.AddMonths(swift).Month;
                year = referenceDate.AddMonths(swift).Year;
            }
            else
            {
                month = this.config.MonthOfYear[monthStr];
                year = referenceDate.Year;
                noYear = true;
            }

            ret = GetWeekOfMonth(cardinal, month, year, referenceDate, noYear);

            return ret;
        }

        private DateTimeResolutionResult ParseWeekOfYear(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var trimedText = text.Trim().ToLowerInvariant();
            var match = this.config.WeekOfYearRegex.Match(trimedText);
            if (!(match.Success && match.Length == text.Length))
            {
                return ret;
            }

            var cardinalStr = match.Groups["cardinal"].Value;
            var yearStr = match.Groups["year"].Value.ToLower();
            var orderStr = match.Groups["order"].Value.ToLower();

            int year;
            if (!string.IsNullOrEmpty(yearStr))
            {
                year = int.Parse(yearStr);
            }
            else
            {
                var swift = this.config.GetSwiftYear(orderStr);
                if (swift < -1)
                {
                    return ret;
                }
                year = referenceDate.Year + swift;
            }

            if (this.config.IsLastCardinal(cardinalStr))
            {
                ret = GetWeekOfMonth(5, 12, year, referenceDate, false);
            }
            else {
                var cardinal = this.config.CardinalMap[cardinalStr];
                ret = GetWeekOfMonth(cardinal, 1, year, referenceDate, false);
            }

            return ret;
        }

        private DateTimeResolutionResult ParseQuarter(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.QuarterRegex.Match(text);

            if (!(match.Success && match.Length == text.Length))
            {
                match = this.config.QuarterRegexYearFront.Match(text);
            }

            if (!(match.Success && match.Length == text.Length))
            {
                return ret;
            }

            var cardinalStr = match.Groups["cardinal"].Value.ToLower();
            var yearStr = match.Groups["year"].Value.ToLower();
            var orderStr = match.Groups["order"].Value.ToLower();

            int year;
            if (!string.IsNullOrEmpty(yearStr))
            {
                year = int.Parse(yearStr);
            }
            else
            {
                var swift = this.config.GetSwiftYear(orderStr);
                if (swift < -1)
                {
                    return ret;
                }
                year = referenceDate.Year + swift;
            }

            var quarterNum = this.config.CardinalMap[cardinalStr];
            var beginDate = DateObject.MinValue.SafeCreateFromValue(year, quarterNum * 3 - 2, 1);
            var endDate = DateObject.MinValue.SafeCreateFromValue(year, quarterNum * 3 + 1, 1);
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
            ret.Timex = $"({FormatUtil.LuisDate(beginDate)},{FormatUtil.LuisDate(endDate)},P3M)";
            ret.Success = true;

            return ret;
        }

        private DateTimeResolutionResult ParseSeason(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.SeasonRegex.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                var swift = this.config.GetSwiftYear(text);

                var yearStr = match.Groups["year"].Value;
                var year = referenceDate.Year;
                var seasonStr = this.config.SeasonMap[match.Groups["seas"].Value.ToLowerInvariant()];
                if (swift >= -1 || !string.IsNullOrEmpty(yearStr))
                {
                    if (string.IsNullOrEmpty(yearStr))
                    {
                        yearStr = (referenceDate.Year + swift).ToString("D4");
                    }
                    ret.Timex = yearStr + "-" + seasonStr;
                    year = int.Parse(yearStr);
                }
                else
                {
                    ret.Timex = seasonStr;
                }

                ret.Success = true;
                return ret;
            }
            return ret;
        }

        private DateTimeResolutionResult ParseWeekOfDate(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = config.WeekOfRegex.Match(text);
            var ex = config.DateExtractor.Extract(text, referenceDate);
            if (match.Success && ex.Count==1)
            {
                var pr= (DateTimeResolutionResult)config.DateParser.Parse(ex[0], referenceDate).Value;
                ret.Timex = pr.Timex;
                ret.Comment = WeekOfComment;
                ret.FutureValue= GetWeekRangeFromDate((DateObject)pr.FutureValue);
                ret.PastValue= GetWeekRangeFromDate((DateObject)pr.PastValue);
                ret.Success = true;
            }
            return ret;
        }

        private DateTimeResolutionResult ParseMonthOfDate(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = config.MonthOfRegex.Match(text);
            var ex = config.DateExtractor.Extract(text, referenceDate);
            if (match.Success && ex.Count == 1)
            {
                var pr = (DateTimeResolutionResult)config.DateParser.Parse(ex[0], referenceDate).Value;
                ret.Timex = pr.Timex;
                ret.Comment = MonthOfComment;
                ret.FutureValue = GetMonthRangeFromDate((DateObject)pr.FutureValue);
                ret.PastValue = GetMonthRangeFromDate((DateObject)pr.PastValue);
                ret.Success = true;
            }
            return ret;
        }

        private Tuple<DateObject, DateObject> GetWeekRangeFromDate(DateObject date)
        {
            var startDate = date.This(DayOfWeek.Monday);
            var endDate = InclusiveEndPeriod ? startDate.AddDays(6) : startDate.AddDays(7);
            return new Tuple<DateObject, DateObject>(startDate, endDate);
        }

        private Tuple<DateObject, DateObject> GetMonthRangeFromDate(DateObject date)
        {
            var startDate = DateObject.MinValue.SafeCreateFromValue(date.Year, date.Month, 1);
            DateObject endDate;
            if (date.Month < 12)
            {
                endDate = DateObject.MinValue.SafeCreateFromValue(date.Year, date.Month + 1, 1);
            }
            else
            {
                endDate = DateObject.MinValue.SafeCreateFromValue(date.Year + 1, 1, 1);
            }
            endDate = InclusiveEndPeriod ? endDate.AddDays(-1) : endDate;
            return new Tuple<DateObject, DateObject>(startDate, endDate);
        }

        private DateTimeResolutionResult ParseWhichWeek(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.WhichWeekRegex.Match(text);
            if (match.Success)
            {
                var num = int.Parse(match.Groups["number"].ToString());
                int year = referenceDate.Year;
                ret.Timex = year.ToString("D4");
                var firstDay = DateObject.MinValue.SafeCreateFromValue(year, 1, 1);
                var firstWeekday = firstDay.This((DayOfWeek)1);
                var value = firstWeekday.AddDays(7 * num);
                var futureDate = value;
                var pastDate = value;
                ret.Timex += "-W" + num.ToString("D2");
                ret.FutureValue = new Tuple<DateObject, DateObject>(futureDate, futureDate.AddDays(7));
                ret.PastValue = new Tuple<DateObject, DateObject>(pastDate, pastDate.AddDays(7));
                ret.Success = true;
            }
            return ret;
        }
        
        private static DateTimeResolutionResult GetWeekOfMonth(int cardinal, int month, int year, DateObject referenceDate, bool noYear)
        {
            var ret = new DateTimeResolutionResult();
            var value = ComputeDate(cardinal, 1, month, year);
            if (value.Month != month)
            {
                cardinal -= 1;
                value = value.AddDays(-7);
            }

            var futureDate = value;
            var pastDate = value;
            if (noYear && futureDate < referenceDate)
            {
                futureDate = ComputeDate(cardinal, 1, month, year + 1);
                if (futureDate.Month != month)
                {
                    futureDate = futureDate.AddDays(-7);
                }
            }

            if (noYear && pastDate >= referenceDate)
            {
                pastDate = ComputeDate(cardinal, 1, month, year - 1);
                if (pastDate.Month != month)
                {
                    pastDate = pastDate.AddDays(-7);
                }
            }

            if (noYear)
            {
                ret.Timex = "XXXX" + "-" + month.ToString("D2");
            }
            else
            {
                ret.Timex = year.ToString("D4") + "-" + month.ToString("D2");
            }

            ret.Timex += "-W" + cardinal.ToString("D2");

            ret.FutureValue = InclusiveEndPeriod
                ? new Tuple<DateObject, DateObject>(futureDate, futureDate.AddDays(6))
                : new Tuple<DateObject, DateObject>(futureDate, futureDate.AddDays(7));

            ret.PastValue = InclusiveEndPeriod
                ? new Tuple<DateObject, DateObject>(pastDate, pastDate.AddDays(6))
                : new Tuple<DateObject, DateObject>(pastDate, pastDate.AddDays(7));

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

            var firstDayOfWeek = firstDay.DayOfWeek != 0 ? (int)firstDay.DayOfWeek : 7;

            if (weekday < firstDayOfWeek)
            {
                firstWeekday = firstDay.Next((DayOfWeek)weekday);
            }

            return firstWeekday.AddDays(7 * (cardinal - 1));
        }

        public bool GetInclusiveEndPeriodFlag()
        {
            return InclusiveEndPeriod;
        }
    }

    public enum CalculateRangeMode
    {
        Week,
        Month
    }
}