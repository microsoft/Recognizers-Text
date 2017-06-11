using System;
using System.Collections.Generic;
using System.Globalization;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDatePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATEPERIOD; //"DatePeriod";
        
        private static readonly Calendar _cal = DateTimeFormatInfo.InvariantInfo.Calendar;

        private readonly IDatePeriodParserConfiguration config;

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
            var luisStr = string.Empty;
            var valueStr = string.Empty;
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
                    innerResult = ParseNumberWithUnit(er.Text, referenceDate);
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


                if (innerResult.Success)
                {
                    if (innerResult.FutureValue != null && innerResult.PastValue != null)
                    {
                        innerResult.FutureResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATE,
                                Util.FormatDate(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item1)
                            },
                            {
                                TimeTypeConstants.END_DATE,
                                Util.FormatDate(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item2)
                            }
                        };
                        innerResult.PastResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATE,
                                Util.FormatDate(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item1)
                            },
                            {
                                TimeTypeConstants.END_DATE,
                                Util.FormatDate(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item2)
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
                TimexStr = value == null ? "" : ((DTParseResult) value).Timex,
                ResolutionStr = ""
            };
            return ret;
        }

        private DTParseResult ParseSimpleCases(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int beginDay = referenceDate.Day, endDay = referenceDate.Day;
            var noYear = false;

            var trimedText = text.Trim();
            var match = this.config.MonthFrontBetweenRegex.Match(trimedText);
            string beginLuisStr = string.Empty, endLuisStr = string.Empty;
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
                    var swiftMonth = this.config.GetSwiftMonth(monthStr);
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
                    beginLuisStr = Util.LuisDate(year, month, beginDay);
                    endLuisStr = Util.LuisDate(year, month, endDay);
                }
                else
                {
                    beginLuisStr = Util.LuisDate(-1, month, beginDay);
                    endLuisStr = Util.LuisDate(-1, month, endDay);
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
            var startDate = new DateObject(year, month, beginDay);
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
                new DateObject(futureYear, month, beginDay),
                new DateObject(futureYear, month, endDay));
            ret.PastValue = new Tuple<DateObject, DateObject>(
                new DateObject(pastYear, month, beginDay),
                new DateObject(pastYear, month, endDay));
            ret.Success = true;

            return ret;
        }

        private DTParseResult ParseOneWordPeriod(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int futureYear = year, pastYear = year;

            var trimedText = text.Trim().ToLower();
            var match = this.config.OneWordPeriodRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var monthStr = match.Groups["month"].Value;
                if (this.config.IsYearToDate(trimedText))
                {
                    ret.Timex = referenceDate.Year.ToString("D4");
                    ret.FutureValue =
                        ret.PastValue =
                            new Tuple<DateObject, DateObject>(new DateObject(referenceDate.Year, 1, 1), referenceDate);
                    ret.Success = true;
                    return ret;
                }
                if (this.config.IsMonthToDate(trimedText))
                {
                    ret.Timex = referenceDate.Year.ToString("D4") + "-" + referenceDate.Month.ToString("D2");
                    ret.FutureValue =
                        ret.PastValue =
                            new Tuple<DateObject, DateObject>(
                                new DateObject(referenceDate.Year, referenceDate.Month, 1), referenceDate);
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
                    var swift = this.config.GetSwiftDay(trimedText);

                    if (this.config.IsWeekOnly(trimedText))
                    {
                        var monday = referenceDate.This(DayOfWeek.Monday).AddDays(7*swift);
                        ret.Timex = monday.Year.ToString("D4") + "-W" +
                                    _cal.GetWeekOfYear(monday, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                                        .ToString("D2");
                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(
                                    referenceDate.This(DayOfWeek.Monday).AddDays(7*swift),
                                    referenceDate.This(DayOfWeek.Sunday).AddDays(7*swift).AddDays(1));
                        ret.Success = true;
                        return ret;
                    }
                    if (this.config.IsWeekend(trimedText))
                    {
                        DateObject beginDate, endDate;
                        beginDate = referenceDate.This(DayOfWeek.Saturday).AddDays(7*swift);
                        endDate = referenceDate.This(DayOfWeek.Sunday).AddDays(7*swift);

                        ret.Timex = beginDate.Year.ToString("D4") + "-W" +
                                    _cal.GetWeekOfYear(beginDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                                        .ToString("D2") + "-WE";
                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate.AddDays(1));
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
                        ret.Timex = year.ToString("D4");
                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(new DateObject(year, 1, 1),
                                    new DateObject(year, 12, 31).AddDays(1));
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
            ret.FutureValue = new Tuple<DateObject, DateObject>(
                new DateObject(futureYear, month, 1),
                new DateObject(futureYear, month, 1).AddMonths(1));
            ret.PastValue = new Tuple<DateObject, DateObject>(
                new DateObject(pastYear, month, 1),
                new DateObject(pastYear, month, 1).AddMonths(1));
            ret.Success = true;
            return ret;
        }

        private  DTParseResult ParseMonthWithYear(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
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
                    new DateObject(year, month, 1),
                    new DateObject(year, month, 1).AddMonths(1));
                ret.Timex = year.ToString("D4") + "-" + month.ToString("D2");
                ret.Success = true;
            }
            return ret;
        }

        private DTParseResult ParseYear(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var match = this.config.YearRegex.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                var year = int.Parse(match.Value);
                var beginDay = new DateObject(year, 1, 1);
                var endDay = new DateObject(year + 1, 1, 1);
                ret.Timex = year.ToString("D4");
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        // parse entities that made up by two time points
        private DTParseResult MergeTwoTimePoints(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var er = this.config.DateExtractor.Extract(text);
            if (er.Count < 2)
            {
                er = this.config.DateExtractor.Extract(this.config.TokenBeforeDate + text);
                if (er.Count < 2)
                {
                    return ret;
                }
                er[0].Start -= this.config.TokenBeforeDate.Length;
                er[1].Start -= this.config.TokenBeforeDate.Length;
            }

            var pr1 = this.config.DateParser.Parse(er[0], referenceDate);
            var pr2 = this.config.DateParser.Parse(er[1], referenceDate);
            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            DateObject futureBegin = (DateObject) ((DTParseResult) pr1.Value).FutureValue,
                futureEnd = (DateObject) ((DTParseResult) pr2.Value).FutureValue;
            DateObject pastBegin = (DateObject) ((DTParseResult) pr1.Value).PastValue,
                pastEnd = (DateObject) ((DTParseResult) pr2.Value).PastValue;
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

        private DTParseResult ParseNumberWithUnit(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();

            var numStr = string.Empty;
            var unitStr = string.Empty;

            // if there are spaces between nubmer and unit
            var ers = this.config.CardinalExtractor.Extract(text);
            if (ers.Count == 1)
            {
                var pr = this.config.NumberParser.Parse(ers[0]);
                var srcUnit = text.Substring(ers[0].Start + ers[0].Length ?? 0).Trim().ToLowerInvariant();
                var beforeStr = text.Substring(0, ers[0].Start ?? 0).Trim().ToLowerInvariant();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    numStr = pr.ResolutionStr;
                    unitStr = this.config.UnitMap[srcUnit];
                    var prefixMatch = this.config.PastRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "D":
                                beginDate = referenceDate.AddDays(-(double) pr.Value);
                                endDate = referenceDate;
                                break;
                            case "W":
                                beginDate = referenceDate.AddDays(-7*(double) pr.Value);
                                endDate = referenceDate;
                                break;
                            case "MON":
                                beginDate = referenceDate.AddMonths(-Convert.ToInt32((double) pr.Value));
                                endDate = referenceDate;
                                break;
                            case "Y":
                                beginDate = referenceDate.AddYears(-Convert.ToInt32((double) pr.Value));
                                endDate = referenceDate;
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex = $"({Util.LuisDate(beginDate)},{Util.LuisDate(endDate)},P{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }
                    prefixMatch = this.config.FutureRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "D":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddDays((double) pr.Value);
                                break;
                            case "W":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddDays(7*(double) pr.Value);
                                break;
                            case "MON":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddMonths(Convert.ToInt32((double) pr.Value));
                                break;
                            case "Y":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddYears(Convert.ToInt32((double) pr.Value));
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({Util.LuisDate(beginDate.AddDays(1))},{Util.LuisDate(endDate.AddDays(1))},P{numStr}{unitStr[0]})";
                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate.AddDays(1), endDate.AddDays(1));
                        ret.Success = true;
                        return ret;
                    }
                }
            }

            // if there are NO spaces between number and unit
            var match = this.config.NumberCombinedWithUnit.Match(text);
            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value.ToLowerInvariant();
                var beforeStr = text.Substring(0, match.Index).Trim().ToLowerInvariant();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];
                    numStr = match.Groups["num"].Value;
                    var prefixMatch = this.config.PastRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "D":
                                beginDate = referenceDate.AddDays(-double.Parse(numStr));
                                endDate = referenceDate;
                                break;
                            case "W":
                                beginDate = referenceDate.AddDays(-7*double.Parse(numStr));
                                endDate = referenceDate;
                                break;
                            case "MON":
                                beginDate = referenceDate.AddMonths(-Convert.ToInt32(double.Parse(numStr)));
                                endDate = referenceDate;
                                break;
                            case "Y":
                                beginDate = referenceDate.AddYears(-Convert.ToInt32(double.Parse(numStr)));
                                endDate = referenceDate;
                                break;
                            default:
                                return ret;
                        }
                        ret.Timex = $"({Util.LuisDate(beginDate)},{Util.LuisDate(endDate)},P{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }
                    prefixMatch = this.config.FutureRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "D":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddDays(double.Parse(numStr));
                                break;
                            case "W":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddDays(7*double.Parse(numStr));
                                break;
                            case "MON":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddMonths(Convert.ToInt32(double.Parse(numStr)));
                                break;
                            case "Y":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddYears(Convert.ToInt32(double.Parse(numStr)));
                                break;
                            default:
                                return ret;
                        }
                        ret.Timex =
                            $"({Util.LuisDate(beginDate.AddDays(1))},{Util.LuisDate(endDate.AddDays(1))},P{numStr}{unitStr[0]})";
                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate.AddDays(1), endDate.AddDays(1));
                        ret.Success = true;
                        return ret;
                    }
                }
            }

            return ret;
        }

        private DTParseResult ParseWeekOfMonth(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
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

            ret = GetWeekOfMonth(cardinal, month, year, referenceDate, noYear);

            return ret;
        }

        private DTParseResult ParseWeekOfYear(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
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

            int cardinal;
            if (this.config.IsLastCardinal(cardinalStr))
            {
                ret = GetWeekOfMonth(5, 12, year, referenceDate, false);
            }
            else
            {
                cardinal = this.config.CardinalMap[cardinalStr];
                ret = GetWeekOfMonth(cardinal, 1, year, referenceDate, false);
            }

            return ret;
        }

        private DTParseResult ParseQuarter(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
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
            var beginDate = new DateObject(year, quarterNum*3 - 2, 1);
            var endDate = new DateObject(year, quarterNum*3 + 1, 1);
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
            ret.Timex = $"({Util.LuisDate(beginDate)},{Util.LuisDate(endDate)},P3M)";
            ret.Success = true;

            return ret;
        }

        private DTParseResult ParseSeason(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var match = this.config.SeasonRegex.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                var swift = this.config.GetSwiftYear(text);

                var yearStr = match.Groups["year"].Value;
                var year = referenceDate.Year;
                var hasYear = false;
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

        private static DTParseResult GetWeekOfMonth(int cardinal, int month, int year, DateObject referenceDate,
            bool noYear)
        {
            var ret = new DTParseResult();
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
            ret.FutureValue = new Tuple<DateObject, DateObject>(futureDate, futureDate.AddDays(7));
            ret.PastValue = new Tuple<DateObject, DateObject>(pastDate, pastDate.AddDays(7));
            ret.Success = true;

            return ret;
        }

        private static DateObject ComputeDate(int cadinal, int weekday, int month, int year)
        {
            var firstDay = new DateObject(year, month, 1);
            var firstWeekday = firstDay.This((DayOfWeek) weekday);
            if (weekday == 0)
            {
                weekday = 7;
            }
            var firstDayOfWeek = firstDay.DayOfWeek != 0 ? (int) firstDay.DayOfWeek : 7;
            if (weekday < firstDayOfWeek)
            {
                firstWeekday = firstDay.Next((DayOfWeek) weekday);
            }
            return firstWeekday.AddDays(7*(cadinal - 1));
        }
    }
}