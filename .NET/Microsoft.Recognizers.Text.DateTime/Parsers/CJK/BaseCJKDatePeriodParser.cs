using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDatePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATEPERIOD; // "DatePeriod";

        private static bool inclusiveEndPeriod = false;

        private readonly ICJKDatePeriodParserConfiguration config;

        public BaseCJKDatePeriodParser(ICJKDatePeriodParserConfiguration configuration)
        {
            config = configuration;
        }

        // @TODO Refactor code to remove the cycle between BaseDatePeriodParser and its config.
        public DateContext GetYearContext(string startDateStr, string endDateStr, string text)
        {
            var isEndDatePureYear = false;
            var isDateRelative = false;
            int contextYear = Constants.InvalidYear;

            var yearMatchForEndDate = this.config.YearRegex.Match(endDateStr);

            if (yearMatchForEndDate.Success && yearMatchForEndDate.Length == endDateStr.Length)
            {
                isEndDatePureYear = true;
            }

            var relativeMatchForStartDate = this.config.RelativeRegex.Match(startDateStr);
            var relativeMatchForEndDate = this.config.RelativeRegex.Match(endDateStr);
            isDateRelative = relativeMatchForStartDate.Success || relativeMatchForEndDate.Success;

            if (!isEndDatePureYear && !isDateRelative)
            {
                foreach (Match match in this.config.YearRegex.Matches(text))
                {
                    var year = GetYearFromText(match);

                    if (year != Constants.InvalidYear)
                    {
                        if (contextYear == Constants.InvalidYear)
                        {
                            contextYear = year;
                        }
                        else
                        {
                            // This indicates that the text has two different year value, no common context year
                            if (contextYear != year)
                            {
                                contextYear = Constants.InvalidYear;
                                break;
                            }
                        }
                    }
                }
            }

            return new DateContext() { Year = contextYear };
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceDate = refDate;

            object value = null;

            if (er.Type.Equals(ParserName, StringComparison.Ordinal))
            {
                var innerResult = ParseSimpleCases(er.Text, referenceDate);
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
                    innerResult = ParseNumberWithUnit(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseYearAndMonth(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseYearToYear(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseMonthToMonth(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseDayToDay(er.Text, referenceDate);
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
                    innerResult = ParseSeason(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseQuarter(er.Text, referenceDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseDecade(er.Text, referenceDate);
                }

                if (innerResult.Success)
                {
                    if (innerResult.FutureValue != null && innerResult.PastValue != null)
                    {
                        innerResult.FutureResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATE,
                                DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)innerResult.FutureValue).Item1)
                            },
                            {
                                TimeTypeConstants.END_DATE,
                                DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)innerResult.FutureValue).Item2)
                            },
                        };

                        innerResult.PastResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATE,
                                DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)innerResult.PastValue).Item1)
                            },
                            {
                                TimeTypeConstants.END_DATE,
                                DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)innerResult.PastValue).Item2)
                            },
                        };
                    }
                    else
                    {
                        innerResult.PastResolution = innerResult.FutureResolution = new Dictionary<string, string>();
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
                TimexStr = value == null ? string.Empty : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        // @TODO use the method defined in AbstractYearExtractor
        public int GetYearFromText(Match match)
        {
            int year = Constants.InvalidYear;

            var yearStr = match.Groups["year"].Value;
            var writtenYearStr = match.Groups["fullyear"].Value;

            if (!string.IsNullOrEmpty(yearStr) && !yearStr.Equals(writtenYearStr, StringComparison.Ordinal))
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
            }
            else
            {
                var firstTwoYearNumStr = match.Groups["firsttwoyearnum"].Value;
                if (!string.IsNullOrEmpty(firstTwoYearNumStr))
                {
                    var er = new ExtractResult
                    {
                        Text = firstTwoYearNumStr,
                        Start = match.Groups["firsttwoyearnum"].Index,
                        Length = match.Groups["firsttwoyearnum"].Length,
                    };

                    var firstTwoYearNum = Convert.ToInt32((double)(this.config.NumberParser.Parse(er).Value ?? 0));

                    var lastTwoYearNum = 0;
                    var lastTwoYearNumStr = match.Groups["lasttwoyearnum"].Value;
                    if (!string.IsNullOrEmpty(lastTwoYearNumStr))
                    {
                        er.Text = lastTwoYearNumStr;
                        er.Start = match.Groups["lasttwoyearnum"].Index;
                        er.Length = match.Groups["lasttwoyearnum"].Length;

                        lastTwoYearNum = Convert.ToInt32((double)(this.config.NumberParser.Parse(er).Value ?? 0));
                    }

                    // Exclude pure number like "nineteen", "twenty four"
                    if ((firstTwoYearNum < 100 && lastTwoYearNum == 0) ||
                        (firstTwoYearNum < 100 && firstTwoYearNum % 10 == 0 && lastTwoYearNumStr.Trim().Split(' ').Length == 1))
                    {
                        year = Constants.InvalidYear;
                        return year;
                    }

                    if (firstTwoYearNum >= 100)
                    {
                        year = firstTwoYearNum + lastTwoYearNum;
                    }
                    else
                    {
                        year = (firstTwoYearNum * 100) + lastTwoYearNum;
                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(writtenYearStr))
                    {
                        var er = new ExtractResult
                        {
                            Text = writtenYearStr,
                            Start = match.Groups["fullyear"].Index,
                            Length = match.Groups["fullyear"].Length,
                        };

                        year = Convert.ToInt32((double)(this.config.NumberParser.Parse(er).Value ?? 0));

                        if (year < 100 && year >= Constants.MinTwoDigitYearPastNum)
                        {
                            year += Constants.BASE_YEAR_PAST_CENTURY;
                        }
                        else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum)
                        {
                            year += Constants.BASE_YEAR_CURRENT_CENTURY;
                        }
                    }
                }
            }

            return year;
        }

        // convert CJK Number to Integer
        private int ConvertCJKToNum(string numStr)
        {
            var num = -1;
            var er = this.config.IntegerExtractor.Extract(numStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal))
                {
                    num = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));
                }
            }

            return num;
        }

        // convert CJK Year to Integer
        private int ConvertCJKToInteger(string yearCJKStr)
        {
            var year = 0;
            var num = 0;

            int dynastyYear = DateTimeFormatUtil.ParseDynastyYear(yearCJKStr,
                                                                  this.config.DynastyYearRegex,
                                                                  this.config.DynastyStartYear,
                                                                  this.config.DynastyYearMap,
                                                                  this.config.IntegerExtractor,
                                                                  this.config.NumberParser);
            if (dynastyYear > 0)
            {
                return dynastyYear;
            }

            var er = this.config.IntegerExtractor.Extract(yearCJKStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal))
                {
                    num = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));
                }
            }

            if (num < 10)
            {
                num = 0;
                foreach (var ch in yearCJKStr)
                {
                    num *= 10;

                    er = this.config.IntegerExtractor.Extract(ch.ToString(CultureInfo.InvariantCulture));

                    if (er.Count != 0)
                    {
                        if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal))
                        {
                            num += Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));
                        }
                    }
                }

                year = num;
            }
            else
            {
                year = num;
            }

            return year == 0 ? -1 : year;
        }

        private DateTimeResolutionResult ParseSimpleCases(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int beginDay, endDay;
            var noYear = false;
            var inputYear = false;

            var match = this.config.SimpleCasesRegex.MatchExact(text, trim: true);
            string beginLuisStr, endLuisStr;

            if (match.Success)
            {
                var days = match.Groups["day"];
                beginDay = this.config.DayOfMonth[days.Captures[0].Value];
                endDay = this.config.DayOfMonth[days.Captures[1].Value];

                var monthStr = match.Groups["month"].Value;
                var yearStr = match.Groups["year"].Value;
                if (!string.IsNullOrEmpty(yearStr))
                {
                    year = int.Parse(yearStr, CultureInfo.InvariantCulture);
                    if (year < 100 && year >= this.config.TwoNumYear)
                    {
                        year += Constants.BASE_YEAR_PAST_CENTURY;
                    }
                    else if (year < 100 && year < this.config.TwoNumYear)
                    {
                        year += Constants.BASE_YEAR_CURRENT_CENTURY;
                    }

                    inputYear = true;
                }
                else
                {
                    noYear = true;
                }

                if (!string.IsNullOrEmpty(monthStr))
                {
                    month = this.config.ToMonthNumber(monthStr);
                }
                else
                {
                    monthStr = match.Groups["relmonth"].Value.Trim();
                    var thisMatch = this.config.ThisRegex.Match(monthStr);
                    var nextMatch = this.config.NextRegex.Match(monthStr);
                    var lastMatch = this.config.LastRegex.Match(monthStr);

                    if (thisMatch.Success)
                    {
                        // do nothing
                    }
                    else if (nextMatch.Success)
                    {
                        if (month != 12)
                        {
                            month += 1;
                        }
                        else
                        {
                            month = 1;
                            year += 1;
                        }
                    }
                    else
                    {
                        if (month != 1)
                        {
                            month -= 1;
                        }
                        else
                        {
                            month = 12;
                            year -= 1;
                        }
                    }
                }

                if (inputYear || this.config.ThisRegex.Match(monthStr).Success ||
                    this.config.NextRegex.Match(monthStr).Success)
                {
                    beginLuisStr = DateTimeFormatUtil.LuisDate(year, month, beginDay);
                    endLuisStr = DateTimeFormatUtil.LuisDate(year, month, endDay);
                }
                else
                {
                    beginLuisStr = DateTimeFormatUtil.LuisDate(-1, month, beginDay);
                    endLuisStr = DateTimeFormatUtil.LuisDate(-1, month, endDay);
                }
            }
            else
            {
                match = this.config.SpecialMonthRegex.MatchExact(text, trim: true);

                if (match.Success)
                {
                    var value = referenceDate.AddMonths(this.config.GetSwiftMonth(match.Value));
                    ret.Timex = DateTimeFormatUtil.LuisDate(value);
                    ret.FutureValue = ret.PastValue = value;
                    ret.Success = true;

                    return ret;
                }

                match = this.config.SpecialYearRegex.MatchExact(text, trim: true);

                if (match.Success)
                {
                    var value = referenceDate.AddYears(this.config.GetSwiftYear(match.Value));
                    ret.Timex = DateTimeFormatUtil.LuisDate(value);
                    ret.FutureValue = ret.PastValue = value;
                    ret.Success = true;

                    return ret;
                }

                return ret;
            }

            var futurePastBeginDates = DateContext.GenerateDates(noYear, referenceDate, year, month, beginDay);
            var futurePastEndDates = DateContext.GenerateDates(noYear, referenceDate, year, month, endDay);

            ret.Timex = $"({beginLuisStr},{endLuisStr},P{endDay - beginDay}D)";
            ret.FutureValue = new Tuple<DateObject, DateObject>(futurePastBeginDates.future, futurePastEndDates.future);
            ret.PastValue = new Tuple<DateObject, DateObject>(futurePastBeginDates.past, futurePastEndDates.past);
            ret.Success = true;

            return ret;
        }

        // handle like "2016年到2017年", "2016年和2017年之间"
        private DateTimeResolutionResult ParseYearToYear(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.YearToYear.Match(text);

            if (!match.Success)
            {
                match = this.config.YearToYearSuffixRequired.Match(text);
            }

            if (match.Success)
            {
                var yearMatch = this.config.YearRegex.Matches(text);
                var yearInCJKMatch = this.config.YearInCJKRegex.Matches(text);
                var beginYear = 0;
                var endYear = 0;

                if (yearMatch.Count == 2)
                {
                    var yearFrom = yearMatch[0].Groups["year"].Value;
                    var yearTo = yearMatch[1].Groups["year"].Value;
                    beginYear = int.Parse(yearFrom, CultureInfo.InvariantCulture);
                    endYear = int.Parse(yearTo, CultureInfo.InvariantCulture);
                }
                else if (yearInCJKMatch.Count == 2)
                {
                    var yearFrom = yearInCJKMatch[0].Groups[Constants.YearCJKGroupName].Value;
                    var yearTo = yearInCJKMatch[1].Groups[Constants.YearCJKGroupName].Value;
                    beginYear = ConvertCJKToInteger(yearFrom);
                    endYear = ConvertCJKToInteger(yearTo);
                }
                else if (yearInCJKMatch.Count == 1 && yearMatch.Count == 1)
                {
                    if (yearMatch[0].Index < yearInCJKMatch[0].Index)
                    {
                        var yearFrom = yearMatch[0].Groups["year"].Value;
                        var yearTo = yearInCJKMatch[0].Groups["yearch"].Value;
                        beginYear = int.Parse(yearFrom, CultureInfo.InvariantCulture);
                        endYear = ConvertCJKToInteger(yearTo);
                    }
                    else
                    {
                        var yearFrom = yearInCJKMatch[0].Groups["yearch"].Value;
                        var yearTo = yearMatch[0].Groups["year"].Value;
                        beginYear = ConvertCJKToInteger(yearFrom);
                        endYear = int.Parse(yearTo, CultureInfo.InvariantCulture);
                    }
                }

                if (beginYear < 100 && beginYear >= this.config.TwoNumYear)
                {
                    beginYear += Constants.BASE_YEAR_PAST_CENTURY;
                }
                else if (beginYear < 100 && beginYear < this.config.TwoNumYear)
                {
                    beginYear += Constants.BASE_YEAR_CURRENT_CENTURY;
                }

                if (endYear < 100 && endYear >= this.config.TwoNumYear)
                {
                    endYear += Constants.BASE_YEAR_PAST_CENTURY;
                }
                else if (endYear < 100 && endYear < this.config.TwoNumYear)
                {
                    endYear += Constants.BASE_YEAR_CURRENT_CENTURY;
                }

                var beginDay = DateObject.MinValue.SafeCreateFromValue(beginYear, 1, 1);
                var endDay = DateObject.MinValue.SafeCreateFromValue(endYear, 1, 1);
                ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDay, endDay, DatePeriodTimexType.ByYear);
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        // handle like "3月到5月", "3月和5月之间"
        private DateTimeResolutionResult ParseMonthToMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.MonthToMonth.Match(text);

            if (!match.Success)
            {
                match = this.config.MonthToMonthSuffixRequired.Match(text);
            }

            if (match.Success)
            {
                var monthMatch = this.config.MonthRegex.Matches(text);
                var beginMonth = 0;
                var endMonth = 0;

                if (monthMatch.Count == 2)
                {
                    var monthFrom = monthMatch[0].Groups["month"].Value;
                    var monthTo = monthMatch[1].Groups["month"].Value;
                    beginMonth = this.config.ToMonthNumber(monthFrom);
                    endMonth = this.config.ToMonthNumber(monthTo);
                }
                else if (match.Groups["monthFrom"].Success && match.Groups["monthTo"].Success)
                {
                    var monthFrom = match.Groups["monthFrom"].Value;
                    var monthTo = match.Groups["monthTo"].Value;
                    beginMonth = this.config.ToMonthNumber(monthFrom);
                    endMonth = this.config.ToMonthNumber(monthTo);
                }

                var yearMatch = this.config.YearRegex.Matches(text);
                var hasYear = false;
                var beginYear = 0;
                var endYear = 0;
                if (yearMatch.Count > 0 && match.Groups["year"].Success)
                {
                    hasYear = true;
                    if (yearMatch.Count == 2)
                    {
                        var yearFrom = yearMatch[0].Groups["year"].Value;
                        var yearTo = yearMatch[1].Groups["year"].Value;
                        beginYear = ParseNumYear(yearFrom);
                        endYear = ParseNumYear(yearTo);
                    }
                    else
                    {
                        var year = yearMatch[0].Groups["year"].Value;
                        beginYear = endYear = ParseNumYear(year);
                    }
                }
                else
                {
                    beginYear = endYear = referenceDate.Year;
                }

                var currentYear = referenceDate.Year;
                var currentMonth = referenceDate.Month;
                var beginYearForPastResolution = beginYear;
                var endYearForPastResolution = endYear;
                var beginYearForFutureResolution = beginYear;
                var endYearForFutureResolution = endYear;
                var durationMonths = 0;

                if (hasYear)
                {
                    var diffmoths = endMonth - beginMonth;
                    var diffyear = endYear - beginYear;
                    durationMonths = (diffyear * 12) + diffmoths;
                }
                else
                {
                    if (beginMonth < endMonth)
                    {
                        // For this case, FutureValue and PastValue share the same resolution
                        if (beginMonth < currentMonth && endMonth >= currentMonth)
                        {
                            // Keep the beginYear and endYear equal to currentYear
                        }
                        else if (beginMonth >= currentMonth)
                        {
                            beginYearForPastResolution = endYearForPastResolution = currentYear - 1;
                        }
                        else if (endMonth < currentMonth)
                        {
                            beginYearForFutureResolution = endYearForFutureResolution = currentYear + 1;
                        }

                        durationMonths = endMonth - beginMonth;
                    }
                    else if (beginMonth > endMonth)
                    {
                        // For this case, FutureValue and PastValue share the same resolution
                        if (beginMonth < currentMonth)
                        {
                            endYearForPastResolution = endYearForFutureResolution = currentYear + 1;
                        }
                        else
                        {
                            beginYearForPastResolution = currentYear - 1;
                            endYearForFutureResolution = currentYear + 1;
                        }

                        durationMonths = beginMonth - endMonth;
                    }
                }

                if (durationMonths != 0)
                {
                    var beginDateForPastResolution = DateObject.MinValue.SafeCreateFromValue(beginYearForPastResolution, beginMonth, 1);
                    var endDateForPastResolution = DateObject.MinValue.SafeCreateFromValue(endYearForPastResolution, endMonth, 1);
                    var beginDateForFutureResolution = DateObject.MinValue.SafeCreateFromValue(beginYearForFutureResolution, beginMonth, 1);
                    var endDateForFutureResolution = DateObject.MinValue.SafeCreateFromValue(endYearForFutureResolution, endMonth, 1);

                    /*var beginTimex = hasYear || beginYearForPastResolution == endYearForFutureResolution ? DateTimeFormatUtil.LuisDate(beginDateForPastResolution, beginDateForFutureResolution) :
                        DateTimeFormatUtil.LuisDate(-1, beginMonth, 1);
                    var endTimex = hasYear || beginYearForPastResolution == endYearForFutureResolution ? DateTimeFormatUtil.LuisDate(endDateForPastResolution, endDateForFutureResolution) :
                        DateTimeFormatUtil.LuisDate(-1, endMonth, 1);*/

                    // If the year is not specified, the combined range timex will use fuzzy years.
                    ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDateForFutureResolution, endDateForFutureResolution, DatePeriodTimexType.ByMonth, beginDateForPastResolution, endDateForPastResolution, hasYear);
                    ret.PastValue = new Tuple<DateObject, DateObject>(beginDateForPastResolution, endDateForPastResolution);
                    ret.FutureValue = new Tuple<DateObject, DateObject>(beginDateForFutureResolution, endDateForFutureResolution);
                    ret.Success = true;
                }
            }

            return ret;
        }

        private int ParseNumYear(string yearNum)
        {
            int year = int.Parse(yearNum, CultureInfo.InvariantCulture);

            if (year < 100 && year >= this.config.TwoNumYear)
            {
                year += Constants.BASE_YEAR_PAST_CENTURY;
            }
            else if (year < 100 && year < this.config.TwoNumYear)
            {
                year += Constants.BASE_YEAR_CURRENT_CENTURY;
            }

            return year;
        }

        private DateTimeResolutionResult ParseDayToDay(string text, DateObject referenceDate)
        {
            int undefinedValue = -1;
            var ret = new DateTimeResolutionResult();
            var match = this.config.DayToDay.Match(text);

            if (match.Success)
            {
                var dayMatchMatch = this.config.DayRegexForPeriod.Matches(text);
                var beginDay = 0;
                var endDay = 0;

                if (dayMatchMatch.Count == 2)
                {
                    var dayFrom = dayMatchMatch[0].Groups["day"].Value;
                    var dayTo = dayMatchMatch[1].Groups["day"].Value;
                    beginDay = this.config.DayOfMonth[dayFrom];
                    endDay = this.config.DayOfMonth[dayTo];
                }

                var beginYearForPastResolution = referenceDate.Year;
                var endYearForPastResolution = referenceDate.Year;
                var beginYearForFutureResolution = referenceDate.Year;
                var endYearForFutureResolution = referenceDate.Year;
                var currentMonth = referenceDate.Month;
                var currentDay = referenceDate.Day;
                var beginMonthForPastResolution = currentMonth;
                var endMonthForPastResolution = currentMonth;
                var beginMonthForFutureResolution = currentMonth;
                var endMonthForFutureResolution = currentMonth;
                var durationDays = 0;

                if (beginDay < endDay)
                {
                    // For this case, FutureValue and PastValue share the same resolution
                    if (beginDay < currentDay && endDay >= currentDay)
                    {
                        // Keep the beginMonth and endMonth equal to currentMonth
                    }
                    else if (beginDay >= currentDay)
                    {
                        if (currentMonth == 1)
                        {
                            beginMonthForPastResolution = endMonthForPastResolution = Constants.MaxMonth;
                            beginYearForPastResolution--;
                            endYearForPastResolution--;
                        }
                        else
                        {
                            beginMonthForPastResolution = endMonthForPastResolution = currentMonth - 1;
                        }
                    }
                    else if (endDay < currentDay)
                    {
                        if (currentMonth == Constants.MaxMonth)
                        {
                            beginMonthForFutureResolution = endMonthForFutureResolution = 1;
                            beginYearForFutureResolution++;
                            endYearForFutureResolution++;
                        }
                        else
                        {
                            beginMonthForFutureResolution = endMonthForFutureResolution = currentMonth + 1;
                        }
                    }

                    durationDays = endDay - beginDay;
                }
                else if (beginDay > endDay)
                {
                    // For this case, FutureValue and PastValue share the same resolution
                    if (beginDay < currentDay)
                    {
                        if (currentMonth == Constants.MaxMonth)
                        {
                            endMonthForPastResolution = endMonthForFutureResolution = 1;
                            endYearForPastResolution++;
                            endYearForFutureResolution++;
                        }
                        else
                        {
                            endMonthForPastResolution = endMonthForFutureResolution = currentMonth + 1;
                        }
                    }
                    else
                    {
                        if (currentMonth == Constants.MaxMonth)
                        {
                            beginMonthForPastResolution = currentMonth - 1;
                            endMonthForFutureResolution = 1;
                            endYearForFutureResolution++;
                        }
                        else if (currentMonth == 1)
                        {
                            beginMonthForPastResolution = 12;
                            beginYearForPastResolution--;
                            endMonthForFutureResolution = currentMonth + 1;
                        }
                        else
                        {
                            beginMonthForPastResolution = currentMonth - 1;
                            endMonthForFutureResolution = currentMonth + 1;
                        }
                    }

                    durationDays = beginDay - endDay;
                }

                if (durationDays != 0)
                {
                    var beginDateForPastResolution = DateObject.MinValue.SafeCreateFromValue(beginYearForPastResolution, beginMonthForPastResolution, beginDay);
                    var endDateForPastResolution = DateObject.MinValue.SafeCreateFromValue(endYearForPastResolution, endMonthForPastResolution, endDay);
                    var beginDateForFutureResolution = DateObject.MinValue.SafeCreateFromValue(beginYearForFutureResolution, beginMonthForFutureResolution, beginDay);
                    var endDateForFutureResolution = DateObject.MinValue.SafeCreateFromValue(endYearForFutureResolution, endMonthForFutureResolution, endDay);
                    var beginTimex = DateTimeFormatUtil.LuisDate(undefinedValue, undefinedValue, beginDay);
                    var endTimex = DateTimeFormatUtil.LuisDate(undefinedValue, undefinedValue, endDay);

                    ret.Timex = $"({beginTimex},{endTimex},P{durationDays}D)";
                    ret.PastValue = new Tuple<DateObject, DateObject>(beginDateForPastResolution, endDateForPastResolution);
                    ret.FutureValue = new Tuple<DateObject, DateObject>(beginDateForFutureResolution, endDateForFutureResolution);
                    ret.Success = true;
                }
            }

            return ret;
        }

        // for case "2016年5月"
        private DateTimeResolutionResult ParseYearAndMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.YearAndMonth.MatchExact(text, trim: true);

            if (!match.Success)
            {
                match = this.config.PureNumYearAndMonth.MatchExact(text, trim: true);
            }

            if (!match.Success)
            {
                return ret;
            }

            // parse year
            var year = referenceDate.Year;
            var yearNum = match.Groups["year"].Value;
            var yearCJK = match.Groups[Constants.YearCJKGroupName].Value;
            var yearRel = match.Groups["yearrel"].Value;
            if (!string.IsNullOrEmpty(yearNum))
            {
                if (this.config.IsYearOnly(yearNum))
                {
                    yearNum = yearNum.Substring(0, yearNum.Length - 1);
                }

                year = int.Parse(yearNum, CultureInfo.InvariantCulture);
            }
            else if (!string.IsNullOrEmpty(yearCJK))
            {
                if (this.config.IsYearOnly(yearCJK))
                {
                    yearCJK = yearCJK.Substring(0, yearCJK.Length - 1);
                }

                year = ConvertCJKToInteger(yearCJK);
            }
            else if (!string.IsNullOrEmpty(yearRel))
            {
                if (this.config.IsLastYear(yearRel))
                {
                    year--;
                }
                else if (this.config.IsNextYear(yearRel))
                {
                    year++;
                }
            }

            if (year < 100 && year >= this.config.TwoNumYear)
            {
                year += Constants.BASE_YEAR_PAST_CENTURY;
            }
            else if (year < this.config.TwoNumYear)
            {
                year += Constants.BASE_YEAR_CURRENT_CENTURY;
            }

            var monthStr = match.Groups["month"].Value;
            var month = this.config.ToMonthNumber(monthStr);
            var beginDay = DateObject.MinValue.SafeCreateFromValue(year, month, 1);
            DateObject endDay;

            if (month == 12)
            {
                endDay = DateObject.MinValue.SafeCreateFromValue(year + 1, 1, 1);
            }
            else
            {
                endDay = DateObject.MinValue.SafeCreateFromValue(year, month + 1, 1);
            }

            ret.Timex = DateTimeFormatUtil.LuisDate(year, month);
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
            ret.Success = true;
            return ret;
        }

        // case like "今年三月" "这个周末" "五月"
        private DateTimeResolutionResult ParseOneWordPeriod(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int futureYear = year, pastYear = year;

            var trimmedText = text.Trim();
            var match = this.config.OneWordPeriodRegex.MatchExact(trimmedText, trim: true);

            if (match.Success)
            {
                var monthStr = match.Groups["month"].Value;
                if (this.config.IsThisYear(trimmedText))
                {
                    ret.Timex = TimexUtility.GenerateYearTimex(referenceDate);
                    ret.FutureValue =
                        ret.PastValue =
                            new Tuple<DateObject, DateObject>(DateObject.MinValue.SafeCreateFromValue(referenceDate.Year, 1, 1), referenceDate);
                    ret.Success = true;
                    return ret;
                }

                var thisMatch = this.config.ThisRegex.Match(trimmedText);
                var nextMatch = this.config.NextRegex.Match(trimmedText);
                var lastMatch = this.config.LastRegex.Match(trimmedText);

                if (!string.IsNullOrEmpty(monthStr))
                {
                    var swift = -10;

                    var yearRel = match.Groups["yearrel"].Value;

                    if (!string.IsNullOrEmpty(yearRel))
                    {
                        if (this.config.IsNextYear(yearRel))
                        {
                            swift = 1;
                        }
                        else if (this.config.IsLastYear(yearRel))
                        {
                            swift = -1;
                        }
                        else if (this.config.IsThisYear(yearRel))
                        {
                            swift = 0;
                        }
                    }

                    month = this.config.ToMonthNumber(monthStr);

                    if (swift >= -1)
                    {
                        year += swift;
                        ret.Timex = DateTimeFormatUtil.LuisDate(year, month);
                        futureYear = pastYear = year;
                    }
                    else
                    {
                        ret.Timex = DateTimeFormatUtil.LuisDate(Constants.InvalidYear, month);
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
                    var swift = 0;
                    if (nextMatch.Success)
                    {
                        swift = 1;
                    }
                    else if (lastMatch.Success)
                    {
                        swift = -1;
                    }

                    // Handle cases with "(上|下)半" like "上半月"、 "下半年"
                    if (!string.IsNullOrEmpty(match.Groups["halfTag"].Value))
                    {
                        return HandleWithHalfTag(trimmedText, referenceDate, ret, swift);
                    }

                    if (this.config.IsWeekOnly(trimmedText))
                    {
                        var monday = referenceDate.This(DayOfWeek.Monday).AddDays(7 * swift);
                        ret.Timex = DateTimeFormatUtil.ToIsoWeekTimex(monday);
                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(
                                    referenceDate.This(DayOfWeek.Monday).AddDays(7 * swift),
                                    referenceDate.This(DayOfWeek.Sunday).AddDays(7 * swift).AddDays(1));
                        ret.Success = true;
                        return ret;
                    }

                    if (this.config.IsWeekend(trimmedText))
                    {
                        var beginDate = referenceDate.This(DayOfWeek.Saturday).AddDays(7 * swift);
                        var endDate = referenceDate.This(DayOfWeek.Sunday).AddDays(7 * swift);

                        ret.Timex = TimexUtility.GenerateWeekendTimex(beginDate);

                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate.AddDays(1));

                        ret.Success = true;

                        return ret;
                    }

                    if (this.config.IsMonthOnly(trimmedText))
                    {
                        month = referenceDate.AddMonths(swift).Month;
                        year = referenceDate.AddMonths(swift).Year;
                        ret.Timex = DateTimeFormatUtil.LuisDate(year, month);
                        futureYear = pastYear = year;
                    }
                    else if (this.config.IsYearOnly(trimmedText))
                    {
                        // Handle like "今年上半年"，"明年下半年"
                        trimmedText = HandleWithHalfYear(match, trimmedText, out bool hasHalf, out bool isFirstHalf);
                        swift = hasHalf ? 0 : swift;

                        year = referenceDate.AddYears(swift).Year;
                        if (this.config.IsLastYear(trimmedText))
                        {
                            year--;
                        }
                        else if (this.config.IsNextYear(trimmedText))
                        {
                            year++;
                        }
                        else if (this.config.IsYearBeforeLast(trimmedText))
                        {
                            year -= 2;
                        }
                        else if (this.config.IsYearAfterNext(trimmedText))
                        {
                            year += 2;
                        }

                        return HandleYearResult(ret, hasHalf, isFirstHalf, year);
                    }
                }
            }
            else
            {
                return ret;
            }

            // only "month" will come to here
            ret.FutureValue = new Tuple<DateObject, DateObject>(
                DateObject.MinValue.SafeCreateFromValue(futureYear, month, 1),
                DateObject.MinValue.SafeCreateFromValue(futureYear, month, 1).AddMonths(1));

            ret.PastValue = new Tuple<DateObject, DateObject>(
                DateObject.MinValue.SafeCreateFromValue(pastYear, month, 1),
                DateObject.MinValue.SafeCreateFromValue(pastYear, month, 1).AddMonths(1));

            ret.Success = true;

            return ret;
        }

        private DateTimeResolutionResult HandleWithHalfTag(string text, DateObject referenceDate, DateTimeResolutionResult ret, int swift)
        {
            DateObject beginDay, endDay;
            int year = referenceDate.Year, month = referenceDate.Month;

            if (this.config.IsWeekOnly(text))
            {
                // Handle like "上半周"，"下半周"
                beginDay = swift == -1 ? referenceDate.This(DayOfWeek.Monday) : referenceDate.This(DayOfWeek.Thursday);
                endDay = swift == -1 ? referenceDate.This(DayOfWeek.Thursday) : referenceDate.This(DayOfWeek.Sunday).AddDays(1);
                ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDay, endDay, DatePeriodTimexType.ByDay);
            }
            else if (this.config.IsMonthOnly(text))
            {
                // Handle like "上半月"，"下半月"
                var monthStartDay = DateObject.MinValue.SafeCreateFromValue(year, month, 1);
                var monthEndDay = DateObject.MinValue.SafeCreateFromValue(year, month + 1, 1);
                var halfMonthDay = (int)((monthEndDay - monthStartDay).TotalDays / 2);

                beginDay = swift == -1 ? monthStartDay : monthStartDay.AddDays(halfMonthDay);
                endDay = swift == -1 ? monthStartDay.AddDays(halfMonthDay) : monthEndDay;
                ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDay, endDay, DatePeriodTimexType.ByDay);
            }
            else
            {
                // Handle like "上(个)半年"，"下(个)半年"
                beginDay = swift == -1 ? DateObject.MinValue.SafeCreateFromValue(year, 1, 1) : DateObject.MinValue.SafeCreateFromValue(year, 7, 1);
                endDay = swift == -1 ? DateObject.MinValue.SafeCreateFromValue(year, 7, 1) : DateObject.MinValue.SafeCreateFromValue(year + 1, 1, 1);
                ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDay, endDay, DatePeriodTimexType.ByMonth);
            }

            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
            ret.Success = true;
            return ret;
        }

        // only contains year like "2016年" or "2016年上半年"
        private DateTimeResolutionResult ParseYear(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.YearRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var yearStr = match.Value;

                // Handle like "2016年上半年"，"2017年下半年"
                yearStr = HandleWithHalfYear(match, yearStr, out bool hasHalf, out bool isFirstHalf);

                // Trim() to handle extra whitespaces like '07 年'
                if (this.config.IsYearOnly(yearStr))
                {
                    yearStr = yearStr.Substring(0, yearStr.Length - 1).Trim();
                }

                var year = int.Parse(yearStr, CultureInfo.InvariantCulture);

                return HandleYearResult(ret, hasHalf, isFirstHalf, year);
            }

            match = this.config.YearInCJKRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var yearStr = match.Value;

                // Handle like "二零一七年上半年"，"二零一七年下半年"
                yearStr = HandleWithHalfYear(match, yearStr, out bool hasHalf, out bool isFirstHalf);

                if (this.config.IsYearOnly(yearStr))
                {
                    yearStr = yearStr.Substring(0, yearStr.Length - 1);
                }

                if (yearStr.Length == 1)
                {
                    return ret;
                }

                var year = ConvertCJKToInteger(yearStr);

                return HandleYearResult(ret, hasHalf, isFirstHalf, year);
            }

            return ret;
        }

        private string HandleWithHalfYear(ConditionalMatch match, string text, out bool hasHalf, out bool isFirstHalf)
        {
            var firstHalf = match.Groups["firstHalf"].Value;
            var secondHalf = match.Groups["secondHalf"].Value;

            hasHalf = false;
            isFirstHalf = !string.IsNullOrEmpty(firstHalf);

            if (isFirstHalf || !string.IsNullOrEmpty(secondHalf))
            {
                var halfText = isFirstHalf ? firstHalf : secondHalf;
                text = text.Substring(0, text.Length - halfText.Length);
                hasHalf = true;
            }

            return text.Trim();
        }

        private DateTimeResolutionResult HandleYearResult(DateTimeResolutionResult ret, bool hasHalf, bool isFirstHalf, int year)
        {
            if (year < 100 && year >= this.config.TwoNumYear)
            {
                year += Constants.BASE_YEAR_PAST_CENTURY;
            }
            else if (year < 100 && year < this.config.TwoNumYear)
            {
                year += Constants.BASE_YEAR_CURRENT_CENTURY;
            }

            var beginDay = DateObject.MinValue.SafeCreateFromValue(year, 1, 1);
            var endDay = DateObject.MinValue.SafeCreateFromValue(year + 1, 1, 1);

            ret.Timex = DateTimeFormatUtil.LuisDate(year);

            if (hasHalf)
            {
                if (isFirstHalf)
                {
                    endDay = DateObject.MinValue.SafeCreateFromValue(year, 7, 1);
                }
                else
                {
                    beginDay = DateObject.MinValue.SafeCreateFromValue(year, 7, 1);
                }

                ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDay, endDay, DatePeriodTimexType.ByMonth);
            }

            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
            ret.Success = true;

            return ret;
        }

        // @TODO Unify this method with its counterpart in BaseDatePeriodParser (if possible) and move it to Utilities
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

            var pr1 = this.config.DateParser.Parse(er[0], referenceDate);
            var pr2 = this.config.DateParser.Parse(er[1], referenceDate);

            if (er.Count >= 2)
            {
                // @TODO Refactor code to remove the cycle between BaseDatePeriodParser and its config.
                var dateContext = GetYearContext(er[0].Text, er[1].Text, text);

                if (pr1.Value == null || pr2.Value == null)
                {
                    return ret;
                }

                pr1 = dateContext.ProcessDateEntityParsingResult(pr1);
                pr2 = dateContext.ProcessDateEntityParsingResult(pr2);

                // When the case has no specified year, we should sync the future/past year due to invalid date Feb 29th.
                if (dateContext.IsEmpty() && (DateContext.IsFeb29th((DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue)
                                              || DateContext.IsFeb29th((DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue)))
                {
                    (pr1, pr2) = dateContext.SyncYear(pr1, pr2);
                }
            }

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

            ret.Timex = TimexUtility.GenerateDatePeriodTimex(futureBegin, futureEnd, DatePeriodTimexType.ByDay, pr1.TimexStr, pr2.TimexStr);

            if (pr1.TimexStr.StartsWith(Constants.TimexFuzzyYear, StringComparison.Ordinal) &&
                futureBegin.CompareTo(DateObject.MinValue.SafeCreateFromValue(futureBegin.Year, 2, 28)) <= 0 &&
                futureEnd.CompareTo(DateObject.MinValue.SafeCreateFromValue(futureBegin.Year, 3, 1)) >= 0)
            {
                // Handle cases like "2月28日到3月1日".
                // There may be different timexes for FutureValue and PastValue due to the different validity of Feb 29th.
                ret.Comment = Constants.Comment_DoubleTimex;
                var pastTimex = TimexUtility.GenerateDatePeriodTimex(pastBegin, pastEnd, DatePeriodTimexType.ByDay, pr1.TimexStr, pr2.TimexStr);
                ret.Timex = TimexUtility.MergeTimexAlternatives(ret.Timex, pastTimex);
            }

            ret.FutureValue = new Tuple<DateObject, DateObject>(futureBegin, futureEnd);
            ret.PastValue = new Tuple<DateObject, DateObject>(pastBegin, pastEnd);
            ret.Success = true;

            return ret;
        }

        // handle like "前两年" "前三个月"
        private DateTimeResolutionResult ParseNumberWithUnit(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            string numStr, unitStr;

            // if there are NO spaces between number and unit
            var match = this.config.NumberCombinedWithUnit.Match(text);

            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value;
                var beforeStr = text.Substring(0, match.Index);

                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];
                    numStr = match.Groups["num"].Value;

                    if (this.config.PastRegex.IsExactMatch(beforeStr, trim: true))
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case Constants.TimexDay:
                                beginDate = referenceDate.AddDays(-double.Parse(numStr, CultureInfo.InvariantCulture));
                                endDate = referenceDate;
                                break;
                            case Constants.TimexWeek:
                                beginDate = referenceDate.AddDays(-7 * double.Parse(numStr, CultureInfo.InvariantCulture));
                                endDate = referenceDate;
                                break;
                            case Constants.TimexMonthFull:
                                beginDate = referenceDate.AddMonths(-Convert.ToInt32(double.Parse(numStr, CultureInfo.InvariantCulture)));
                                endDate = referenceDate;
                                break;
                            case Constants.TimexYear:
                                beginDate = referenceDate.AddYears(-Convert.ToInt32(double.Parse(numStr, CultureInfo.InvariantCulture)));
                                endDate = referenceDate;
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex = $"({DateTimeFormatUtil.LuisDate(beginDate)},{DateTimeFormatUtil.LuisDate(endDate)},P{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }

                    if (this.config.FutureRegex.IsExactMatch(beforeStr, trim: true))
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case Constants.TimexDay:
                                beginDate = referenceDate;
                                endDate = referenceDate.AddDays(double.Parse(numStr));
                                break;
                            case Constants.TimexWeek:
                                beginDate = referenceDate;
                                endDate = referenceDate.AddDays(7 * double.Parse(numStr));
                                break;
                            case Constants.TimexMonthFull:
                                beginDate = referenceDate;
                                endDate = referenceDate.AddMonths(Convert.ToInt32(double.Parse(numStr)));
                                break;
                            case Constants.TimexYear:
                                beginDate = referenceDate;
                                endDate = referenceDate.AddYears(Convert.ToInt32(double.Parse(numStr)));
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({DateTimeFormatUtil.LuisDate(beginDate.AddDays(1))},{DateTimeFormatUtil.LuisDate(endDate.AddDays(1))},P{numStr}{unitStr[0]})";
                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate.AddDays(1), endDate.AddDays(1));
                        ret.Success = true;
                        return ret;
                    }
                }
            }

            // for case "前两年" "后三年"
            var durationRes = this.config.DurationExtractor.Extract(text, referenceDate);
            if (durationRes.Count > 0)
            {
                var beforeStr = text.Substring(0, (int)durationRes[0].Start);
                match = this.config.UnitRegex.Match(durationRes[0].Text);
                if (match.Success)
                {
                    var srcUnit = match.Groups["unit"].Value;

                    var numberStr = durationRes[0].Text.Substring(0, match.Index).Trim();
                    var number = ConvertCJKToNum(numberStr);

                    if (this.config.UnitMap.ContainsKey(srcUnit))
                    {
                        unitStr = this.config.UnitMap[srcUnit];

                        var prefixMatch = this.config.PastRegex.MatchExact(beforeStr, trim: true);

                        if (prefixMatch.Success)
                        {
                            DateObject beginDate, endDate;
                            switch (unitStr)
                            {
                                case Constants.TimexDay:
                                    beginDate = referenceDate.AddDays(-number);
                                    endDate = referenceDate;
                                    break;
                                case Constants.TimexWeek:
                                    beginDate = referenceDate.AddDays(-7 * number);
                                    endDate = referenceDate;
                                    break;
                                case Constants.TimexMonthFull:
                                    beginDate = referenceDate.AddMonths(-number);
                                    endDate = referenceDate;
                                    break;
                                case Constants.TimexYear:
                                    beginDate = referenceDate.AddYears(-number);
                                    endDate = referenceDate;
                                    break;
                                default:
                                    return ret;
                            }

                            ret.Timex = $"({DateTimeFormatUtil.LuisDate(beginDate)},{DateTimeFormatUtil.LuisDate(endDate)},P{number}{unitStr[0]})";
                            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                            ret.Success = true;
                            return ret;
                        }

                        prefixMatch = this.config.FutureRegex.MatchExact(beforeStr, trim: true);

                        if (prefixMatch.Success)
                        {
                            DateObject beginDate, endDate;
                            switch (unitStr)
                            {
                                case Constants.TimexDay:
                                    beginDate = referenceDate;
                                    endDate = referenceDate.AddDays(number);
                                    break;
                                case Constants.TimexWeek:
                                    beginDate = referenceDate;
                                    endDate = referenceDate.AddDays(7 * number);
                                    break;
                                case Constants.TimexMonthFull:
                                    beginDate = referenceDate;
                                    endDate = referenceDate.AddMonths(number);
                                    break;
                                case Constants.TimexYear:
                                    beginDate = referenceDate;
                                    endDate = referenceDate.AddYears(number);
                                    break;
                                default:
                                    return ret;
                            }

                            ret.Timex =
                                $"({DateTimeFormatUtil.LuisDate(beginDate.AddDays(1))},{DateTimeFormatUtil.LuisDate(endDate.AddDays(1))},P{number}{unitStr[0]})";
                            ret.FutureValue =
                                ret.PastValue =
                                    new Tuple<DateObject, DateObject>(beginDate.AddDays(1), endDate.AddDays(1));
                            ret.Success = true;
                            return ret;
                        }
                    }
                }
            }

            return ret;
        }

        // case like "三月的第一周"
        private DateTimeResolutionResult ParseWeekOfMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var trimmedText = text.Trim();

            var match = this.config.WeekOfMonthRegex.Match(text);
            if (!match.Success)
            {
                return ret;
            }

            var cardinalStr = match.Groups["cardinal"].Value;
            var monthStr = match.Groups["month"].Value;
            var noYear = false;
            int year;

            int cardinal;
            if (config.WoMLastRegex.IsExactMatch(cardinalStr, trim: true))
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
                var swift = 0;
                if (config.WoMNextRegex.MatchBegin(trimmedText, trim: true).Success)
                {
                    swift = 1;
                }
                else if (config.WoMPreviousRegex.MatchBegin(trimmedText, trim: true).Success)
                {
                    swift = -1;
                }

                month = referenceDate.AddMonths(swift).Month;
                year = referenceDate.AddMonths(swift).Year;
                ret.Timex = DateTimeFormatUtil.LuisDate(referenceDate.Year, month);
            }
            else
            {
                month = this.config.ToMonthNumber(monthStr);
                ret.Timex = DateTimeFormatUtil.LuisDate(Constants.InvalidYear, month);
                year = referenceDate.Year;
                noYear = true;
            }

            var value = DateContext.ComputeDate(cardinal, 1, month, year);

            var futureDate = value;
            var pastDate = value;
            if (noYear && futureDate < referenceDate)
            {
                futureDate = DateContext.ComputeDate(cardinal, 1, month, year + 1);
                if (futureDate.Month != month)
                {
                    futureDate = futureDate.AddDays(-7);
                }
            }

            if (noYear && pastDate >= referenceDate)
            {
                pastDate = DateContext.ComputeDate(cardinal, 1, month, year - 1);
                if (pastDate.Month != month)
                {
                    pastDate = pastDate.AddDays(-7);
                }
            }

            ret.Timex += "-W" + cardinal.ToString("D2", CultureInfo.InvariantCulture);
            ret.FutureValue = new Tuple<DateObject, DateObject>(futureDate, futureDate.AddDays(7));
            ret.PastValue = new Tuple<DateObject, DateObject>(pastDate, pastDate.AddDays(7));
            ret.Success = true;

            return ret;
        }

        // parse "今年夏天"
        private DateTimeResolutionResult ParseSeason(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.SeasonWithYear.MatchExact(text, trim: true);

            if (match.Success)
            {
                // parse year
                var year = referenceDate.Year;
                var hasYear = false;
                var yearNum = match.Groups["year"].Value;
                var yearCJK = match.Groups[Constants.YearCJKGroupName].Value;
                var yearRel = match.Groups["yearrel"].Value;

                if (!string.IsNullOrEmpty(yearNum))
                {
                    hasYear = true;
                    if (this.config.IsYearOnly(yearNum))
                    {
                        yearNum = yearNum.Substring(0, yearNum.Length - 1);
                    }

                    year = int.Parse(yearNum, CultureInfo.InvariantCulture);
                }
                else if (!string.IsNullOrEmpty(yearCJK))
                {
                    hasYear = true;
                    if (this.config.IsYearOnly(yearCJK))
                    {
                        yearCJK = yearCJK.Substring(0, yearCJK.Length - 1);
                    }

                    year = ConvertCJKToInteger(yearCJK);
                }
                else if (!string.IsNullOrEmpty(yearRel))
                {
                    hasYear = true;
                    if (this.config.IsLastYear(yearRel))
                    {
                        year--;
                    }
                    else if (this.config.IsNextYear(yearRel))
                    {
                        year++;
                    }
                }

                if (year < 100 && year >= this.config.TwoNumYear)
                {
                    year += Constants.BASE_YEAR_PAST_CENTURY;
                }
                else if (year < 100 && year < this.config.TwoNumYear)
                {
                    year += Constants.BASE_YEAR_CURRENT_CENTURY;
                }

                // parse season
                var seasonStr = match.Groups["season"].Value;
                ret.Timex = this.config.SeasonMap[seasonStr];
                if (hasYear)
                {
                    ret.Timex = year.ToString("D4", CultureInfo.InvariantCulture) + "-" + ret.Timex;
                }

                ret.Success = true;
                return ret;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseQuarter(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.QuarterRegex.MatchExact(text, trim: true);

            if (!match.Success)
            {
                return ret;
            }

            // parse year
            var year = referenceDate.Year;
            var yearNum = match.Groups["year"].Value;
            var yearCJK = match.Groups[Constants.YearCJKGroupName].Value;
            var yearRel = match.Groups["yearrel"].Value;
            if (!string.IsNullOrEmpty(yearNum))
            {
                if (this.config.IsYearOnly(yearNum))
                {
                    yearNum = yearNum.Substring(0, yearNum.Length - 1);
                }

                year = int.Parse(yearNum, CultureInfo.InvariantCulture);
            }
            else if (!string.IsNullOrEmpty(yearCJK))
            {
                if (this.config.IsYearOnly(yearCJK))
                {
                    yearCJK = yearCJK.Substring(0, yearCJK.Length - 1);
                }

                year = ConvertCJKToInteger(yearCJK);
            }
            else if (!string.IsNullOrEmpty(yearRel))
            {
                if (this.config.IsLastYear(yearRel))
                {
                    year--;
                }
                else if (this.config.IsNextYear(yearRel))
                {
                    year++;
                }
            }

            if (year < 100 && year >= this.config.TwoNumYear)
            {
                year += Constants.BASE_YEAR_PAST_CENTURY;
            }
            else if (year < 100 && year < this.config.TwoNumYear)
            {
                year += Constants.BASE_YEAR_CURRENT_CENTURY;
            }

            // parse quarterNum
            var cardinalStr = match.Groups["cardinal"].Value;
            var quarterNum = this.config.CardinalMap[cardinalStr];

            var beginDate = DateObject.MinValue.SafeCreateFromValue(year, (quarterNum * 3) - 2, 1);
            var endDate = DateObject.MinValue.SafeCreateFromValue(year, (quarterNum * 3) + 1, 1);
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
            ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDate, endDate, DatePeriodTimexType.ByMonth);
            ret.Success = true;

            return ret;
        }

        private DateTimeResolutionResult ParseDecade(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            int century = (referenceDate.Year / 100) + 1;
            int decade;
            int decadeLastYear = 10;
            var inputCentury = false;

            var match = this.config.DecadeRegex.MatchExact(text, trim: true);

            string beginLuisStr, endLuisStr;

            if (match.Success)
            {
                var decadeStr = match.Groups["decade"].Value;
                if (!int.TryParse(decadeStr, out decade))
                {
                    decade = ConvertCJKToNum(decadeStr);
                }

                var centuryStr = match.Groups["century"].Value;
                if (!string.IsNullOrEmpty(centuryStr))
                {
                    if (!int.TryParse(centuryStr, out century))
                    {
                        century = ConvertCJKToNum(centuryStr);
                    }

                    inputCentury = true;
                }
                else
                {
                    centuryStr = match.Groups["relcentury"].Value;

                    if (!string.IsNullOrEmpty(centuryStr))
                    {
                        centuryStr = centuryStr.Trim();
                        var thisMatch = this.config.ThisRegex.Match(centuryStr);
                        var nextMatch = this.config.NextRegex.Match(centuryStr);
                        var lastMatch = this.config.LastRegex.Match(centuryStr);

                        if (thisMatch.Success)
                        {
                            // do nothing
                        }
                        else if (nextMatch.Success)
                        {
                            century++;
                        }
                        else
                        {
                            century--;
                        }

                        inputCentury = true;
                    }
                }
            }
            else
            {
                return ret;
            }

            var beginYear = ((century - 1) * 100) + decade;
            var endYear = beginYear + decadeLastYear;

            if (inputCentury)
            {
                beginLuisStr = DateTimeFormatUtil.LuisDate(beginYear, 1, 1);
                endLuisStr = DateTimeFormatUtil.LuisDate(endYear, 1, 1);
            }
            else
            {
                var beginYearStr = "XX" + decade;
                beginLuisStr = DateTimeFormatUtil.LuisDate(-1, 1, 1);
                beginLuisStr = beginLuisStr.Replace("XXXX", beginYearStr);

                var endYearStr = "XX" + (endYear % 100).ToString("D2", CultureInfo.InvariantCulture);
                endLuisStr = DateTimeFormatUtil.LuisDate(-1, 1, 1);
                endLuisStr = endLuisStr.Replace("XXXX", endYearStr);
            }

            ret.Timex = $"({beginLuisStr},{endLuisStr},P10Y)";

            int futureYear = beginYear, pastYear = beginYear;
            var startDate = DateObject.MinValue.SafeCreateFromValue(beginYear, 1, 1);
            if (!inputCentury && startDate < referenceDate)
            {
                futureYear += 100;
            }

            if (!inputCentury && startDate >= referenceDate)
            {
                pastYear -= 100;
            }

            ret.FutureValue = new Tuple<DateObject, DateObject>(
                DateObject.MinValue.SafeCreateFromValue(futureYear, 1, 1),
                DateObject.MinValue.SafeCreateFromValue(futureYear + decadeLastYear, 1, 1));

            ret.PastValue = new Tuple<DateObject, DateObject>(
                DateObject.MinValue.SafeCreateFromValue(pastYear, 1, 1),
                DateObject.MinValue.SafeCreateFromValue(pastYear + decadeLastYear, 1, 1));

            ret.Success = true;

            return ret;
        }
    }
}
