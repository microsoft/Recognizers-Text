// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDatePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATEPERIOD; // "DatePeriod";

        private static bool inclusiveEndPeriod = false;

        private static readonly Calendar Cal = DateTimeFormatInfo.InvariantInfo.Calendar;

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
                var innerResult = ParseBaseDatePeriod(er.Text, refDate);

                if (!innerResult.Success)
                {
                    innerResult = ParseComplexDatePeriod(er.Text, refDate);
                }

                if (innerResult.Success)
                {
                    if (innerResult.Mod == Constants.BEFORE_MOD)
                    {
                        innerResult.FutureResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.END_DATE,
                                DateTimeFormatUtil.FormatDate((DateObject)innerResult.FutureValue)
                            },
                        };

                        innerResult.PastResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.END_DATE,
                                DateTimeFormatUtil.FormatDate((DateObject)innerResult.PastValue)
                            },
                        };
                    }
                    else if (innerResult.Mod == Constants.AFTER_MOD)
                    {
                        innerResult.FutureResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATE,
                                DateTimeFormatUtil.FormatDate((DateObject)innerResult.FutureValue)
                            },
                        };

                        innerResult.PastResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATE,
                                DateTimeFormatUtil.FormatDate((DateObject)innerResult.PastValue)
                            },
                        };
                    }
                    else if (innerResult.FutureValue != null && innerResult.PastValue != null)
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

            var yearStr = match.Groups[Constants.YearGroupName].Value;
            var writtenYearStr = match.Groups[Constants.FullYearGroupName].Value;

            if (!string.IsNullOrEmpty(yearStr) && !yearStr.Equals(writtenYearStr, StringComparison.Ordinal))
            {
                year = ConvertCJKToInteger(yearStr);
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
                var firstTwoYearNumStr = match.Groups[Constants.FirstTwoYearGroupName].Value;
                if (!string.IsNullOrEmpty(firstTwoYearNumStr))
                {
                    var er = new ExtractResult
                    {
                        Text = firstTwoYearNumStr,
                        Start = match.Groups[Constants.FirstTwoYearGroupName].Index,
                        Length = match.Groups[Constants.FirstTwoYearGroupName].Length,
                    };

                    var firstTwoYearNum = Convert.ToInt32((double)(this.config.NumberParser.Parse(er).Value ?? 0));

                    var lastTwoYearNum = 0;
                    var lastTwoYearNumStr = match.Groups[Constants.LastTwoYearGroupName].Value;
                    if (!string.IsNullOrEmpty(lastTwoYearNumStr))
                    {
                        er.Text = lastTwoYearNumStr;
                        er.Start = match.Groups[Constants.LastTwoYearGroupName].Index;
                        er.Length = match.Groups[Constants.LastTwoYearGroupName].Length;

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
                            Start = match.Groups[Constants.FullYearGroupName].Index,
                            Length = match.Groups[Constants.FullYearGroupName].Length,
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

        private static DateObject ShiftResolution(Tuple<DateObject, DateObject> date, Match match, bool start)
        {
            DateObject result;
            result = date.Item1;

            return result;
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

        private DateTimeResolutionResult ParseSingleTimePoint(string text, DateObject referenceDate, DateContext dateContext = null)
        {
            var ret = new DateTimeResolutionResult();
            var er = this.config.DateExtractor.Extract(text, referenceDate).FirstOrDefault();

            if (er != null)
            {
                var pr = this.config.DateParser.Parse(er, referenceDate);

                if (pr != null)
                {
                    ret.Timex = $"({pr.TimexStr}";
                    ret.FutureValue = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
                    ret.PastValue = (DateObject)((DateTimeResolutionResult)pr.Value).PastValue;
                    ret.Success = true;
                }
            }

            return ret;
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

            if (!match.Success)
            {
                match = this.config.MonthDayRange.MatchExact(text, trim: true);
            }

            if (match.Success)
            {
                var days = match.Groups[Constants.DayGroupName];
                beginDay = this.config.DayOfMonth[days.Captures[0].Value];
                endDay = this.config.DayOfMonth[days.Captures[1].Value];

                var monthStr = match.Groups[Constants.MonthGroupName].Value;
                var yearStr = match.Groups[Constants.YearGroupName].Value;
                if (!string.IsNullOrEmpty(yearStr))
                {
                    year = ConvertCJKToInteger(yearStr);
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
                    monthStr = match.Groups[Constants.RelMonthGroupName].Value.Trim();
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
                    var yearFrom = yearMatch[0].Groups[Constants.YearGroupName].Value;
                    var yearTo = yearMatch[1].Groups[Constants.YearGroupName].Value;
                    beginYear = ConvertCJKToInteger(yearFrom);
                    endYear = ConvertCJKToInteger(yearTo);
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
                        var yearFrom = yearMatch[0].Groups[Constants.YearGroupName].Value;
                        var yearTo = yearInCJKMatch[0].Groups[Constants.YearCJKGroupName].Value;
                        beginYear = int.Parse(yearFrom, CultureInfo.InvariantCulture);
                        endYear = ConvertCJKToInteger(yearTo);
                    }
                    else
                    {
                        var yearFrom = yearInCJKMatch[0].Groups[Constants.YearCJKGroupName].Value;
                        var yearTo = yearMatch[0].Groups[Constants.YearGroupName].Value;
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
                    var monthFrom = monthMatch[0].Groups[Constants.MonthGroupName].Value;
                    var monthTo = monthMatch[1].Groups[Constants.MonthGroupName].Value;
                    beginMonth = this.config.ToMonthNumber(monthFrom);
                    endMonth = this.config.ToMonthNumber(monthTo);
                }
                else if (match.Groups[Constants.MonthFromGroupName].Success && match.Groups[Constants.MonthToGroupName].Success)
                {
                    var monthFrom = match.Groups[Constants.MonthFromGroupName].Value;
                    var monthTo = match.Groups[Constants.MonthToGroupName].Value;
                    beginMonth = this.config.ToMonthNumber(monthFrom);
                    endMonth = this.config.ToMonthNumber(monthTo);
                }

                var yearMatch = this.config.YearRegex.Matches(text);
                var hasYear = false;
                var beginYear = 0;
                var endYear = 0;
                if (yearMatch.Count > 0 && match.Groups[Constants.YearGroupName].Success)
                {
                    hasYear = true;
                    if (yearMatch.Count == 2)
                    {
                        var yearFrom = yearMatch[0].Groups[Constants.YearGroupName].Value;
                        var yearTo = yearMatch[1].Groups[Constants.YearGroupName].Value;
                        beginYear = ParseNumYear(yearFrom);
                        endYear = ParseNumYear(yearTo);
                    }
                    else
                    {
                        var year = yearMatch[0].Groups[Constants.YearGroupName].Value;
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
                    var dayMatch = this.config.DayRegexForPeriod.Matches(text);

                    // handle cases like 2019年2月1日から3月まで
                    if (dayMatch.Count > 0 && match.Groups[Constants.DayGroupName].Success)
                    {
                        ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDateForFutureResolution, endDateForFutureResolution, DatePeriodTimexType.ByDay, beginDateForPastResolution, endDateForPastResolution, hasYear);
                    }

                    // If the year is not specified, the combined range timex will use fuzzy years.
                    else
                    {
                        ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDateForFutureResolution, endDateForFutureResolution, DatePeriodTimexType.ByMonth, beginDateForPastResolution, endDateForPastResolution, hasYear);
                    }

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
            var ret = new DateTimeResolutionResult();
            var match = this.config.DayToDay.Match(text);

            if (match.Success)
            {
                var dayMatchMatch = this.config.DayRegexForPeriod.Matches(text);
                var beginDay = 0;
                var endDay = 0;

                if (dayMatchMatch.Count == 2)
                {
                    var dayFrom = dayMatchMatch[0].Groups[Constants.DayGroupName].Value;
                    var dayTo = dayMatchMatch[1].Groups[Constants.DayGroupName].Value;
                    beginDay = this.config.DayOfMonth[dayFrom];
                    endDay = this.config.DayOfMonth[dayTo];
                }
                else if (match.Groups[Constants.HalfGroupName].Success)
                {
                    var er = this.config.DurationExtractor.Extract(match.Groups[Constants.HalfGroupName].Value, referenceDate);
                    var pr = this.config.DurationParser.Parse(er[0], referenceDate);

                    float number = TimexUtility.ParseNumberFromDurationTimex(pr.TimexStr);

                    DateObject beginDay1 = referenceDate;
                    DateObject endDay1 = referenceDate.AddDays(number);

                    ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDay1, endDay1, DatePeriodTimexType.ByDay);
                    ret.PastValue = ret.FutureValue = new Tuple<DateObject, DateObject>(beginDay1, endDay1);
                    ret.Success = true;
                    return ret;

                }

                var beginYearForPastResolution = referenceDate.Year;
                var endYearForPastResolution = referenceDate.Year;
                var beginYearForFutureResolution = referenceDate.Year;
                var endYearForFutureResolution = referenceDate.Year;
                var currentMonth = referenceDate.Month;
                var currentDay = referenceDate.Day;
                var durationDays = 0;

                var relativeMonth = this.config.RelativeMonthRegex.Match(text);
                currentMonth += this.config.GetSwiftMonth(relativeMonth.Value);

                var beginMonthForPastResolution = currentMonth;
                var endMonthForPastResolution = currentMonth;
                var beginMonthForFutureResolution = currentMonth;
                var endMonthForFutureResolution = currentMonth;

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

                    ret.Timex = relativeMonth.Success ? TimexUtility.GenerateDatePeriodTimex(beginDateForFutureResolution, endDateForFutureResolution, DatePeriodTimexType.ByDay) :
                            TimexUtility.GenerateDatePeriodTimex(beginDateForFutureResolution, endDateForFutureResolution, DatePeriodTimexType.ByDay, UnspecificDateTimeTerms.NonspecificYear | UnspecificDateTimeTerms.NonspecificMonth);

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
            var yearNum = match.Groups[Constants.YearGroupName].Value;
            var yearCJK = match.Groups[Constants.YearCJKGroupName].Value;
            var yearRel = match.Groups[Constants.YearRelGroupName].Value;
            var cardinalStr = match.Groups[Constants.CardinalGroupName].Value;
            if (!string.IsNullOrEmpty(yearNum))
            {
                if (this.config.IsYearOnly(yearNum))
                {
                    yearNum = yearNum.Substring(0, yearNum.Length - 1);
                }

                year = ConvertCJKToInteger(yearNum);
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

            var monthStr = match.Groups[Constants.MonthGroupName].Value;
            var month = match.Groups[Constants.MonthGroupName].Success ? this.config.ToMonthNumber(monthStr) :
                    config.WoMLastRegex.IsExactMatch(cardinalStr, trim: true) ? 12 : this.config.CardinalMap[cardinalStr];
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

            var isReferenceDatePeriod = false;

            var trimmedText = text.Trim();
            var match = this.config.OneWordPeriodRegex.MatchExact(trimmedText, trim: true);

            // For cases "that week|month|year"
            if (!match.Success)
            {
                match = this.config.ReferenceDatePeriodRegex.MatchExact(trimmedText, trim: true);
                if (match.Success)
                {
                    isReferenceDatePeriod = true;
                    ret.Mod = Constants.REF_UNDEF_MOD;
                }
            }

            if (!match.Success)
            {
                match = this.config.LaterEarlyPeriodRegex.MatchExact(trimmedText, trim: true);

                if (match.Success)
                {
                    return ParseLaterEarlyPeriod(text, referenceDate);
                }
            }

            if (match.Success)
            {
                var monthStr = match.Groups[Constants.MonthGroupName].Value;
                if (this.config.IsThisYear(trimmedText))
                {
                    ret.Timex = TimexUtility.GenerateYearTimex(referenceDate);
                    ret.FutureValue =
                        ret.PastValue =
                            new Tuple<DateObject, DateObject>(DateObject.MinValue.SafeCreateFromValue(referenceDate.Year, 1, 1), referenceDate);
                    ret.Success = true;
                    return ret;
                }

                if (this.config.IsYearToDate(trimmedText))
                {
                    ret.Timex = TimexUtility.GenerateYearTimex(referenceDate.Year);
                    ret.FutureValue =
                        ret.PastValue =
                            new Tuple<DateObject, DateObject>(DateObject.MinValue.SafeCreateFromValue(referenceDate.Year, 1, 1), referenceDate);
                    ret.Success = true;
                    return ret;
                }

                // In Chinese, "下" means next, "下下周" means next next week, "下下周末" means next next weekend, need to check whether the text match "下下"
                ChineseDatePeriodParserConfiguration config = this.config as ChineseDatePeriodParserConfiguration;
                bool nextNextMatch = config == null ? false : config.NextNextRegex.Match(trimmedText).Success;

                var nextMatch = this.config.NextRegex.Match(trimmedText);
                var lastMatch = this.config.LastRegex.Match(trimmedText);

                if (!string.IsNullOrEmpty(monthStr))
                {
                    var swift = -10;

                    var yearRel = match.Groups[Constants.YearRelGroupName].Value;

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
                    if (nextNextMatch)
                    {
                        // If it is Chinese "下下周" (next next week), "下下周末" (next next weekend), then swift is 2
                        swift = 2;
                    }
                    else if (nextMatch.Success)
                    {
                        if (nextMatch.Groups[Constants.AfterGroupName].Success)
                        {
                            swift = 2;
                        }
                        else
                        {
                            swift = 1;
                        }
                    }
                    else if (lastMatch.Success)
                    {
                        swift = -1;
                    }

                    // Handle cases with "(上|下)半" like "上半月"、 "下半年"
                    if (!string.IsNullOrEmpty(match.Groups[Constants.HalfTagGroupName].Value))
                    {
                        return HandleWithHalfTag(trimmedText, referenceDate, ret, swift);
                    }

                    if (this.config.IsWeekOnly(trimmedText))
                    {
                        var monday = referenceDate.This(DayOfWeek.Monday).AddDays(7 * swift);
                        ret.Timex = isReferenceDatePeriod ? TimexUtility.GenerateWeekTimex() : TimexUtility.GenerateWeekTimex(monday);
                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(
                                    referenceDate.This(DayOfWeek.Monday).AddDays(7 * swift),
                                    referenceDate.This(DayOfWeek.Sunday).AddDays(7 * swift).AddDays(1));
                        ret.Success = true;
                        return ret;
                    }
                    else if (!string.IsNullOrEmpty(match.Groups[Constants.RestOfGroupName].Value))
                    {
                        var durationStr = match.Groups[Constants.DurationGroupName].Value;
                        var durationUnit = this.config.UnitMap[durationStr];
                        DateObject beginDate;
                        DateObject endDate = beginDate = referenceDate;

                        ret.Timex = TimexUtility.GenerateDatePeriodTimexWithDiff(beginDate, ref endDate, durationUnit);
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }

                    if (this.config.IsWeekend(trimmedText))
                    {
                        var beginDate = referenceDate.This(DayOfWeek.Saturday).AddDays(7 * swift);
                        var endDate = referenceDate.This(DayOfWeek.Sunday).AddDays(7 * swift);
                        match = this.config.ReferenceDatePeriodRegex.MatchExact(trimmedText, trim: true);

                        if (match.Success)
                        {
                            isReferenceDatePeriod = true;
                            ret.Mod = Constants.REF_UNDEF_MOD;
                        }

                        ret.Timex = isReferenceDatePeriod ? TimexUtility.GenerateWeekendTimex() : TimexUtility.GenerateWeekendTimex(beginDate);
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
                        ret.Timex = isReferenceDatePeriod ? TimexUtility.GenerateMonthTimex() : DateTimeFormatUtil.LuisDate(year, month);
                        futureYear = pastYear = year;
                    }
                    else if (this.config.IsYearOnly(trimmedText))
                    {
                        // Handle like "今年上半年"，"明年下半年"
                        swift = 0;
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

                        return HandleYearResult(ret, hasHalf, isFirstHalf, isReferenceDatePeriod, year);
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

        private DateTimeResolutionResult ParseLaterEarlyPeriod(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int futureYear = year, pastYear = year;
            var earlyPrefix = false;
            var latePrefix = false;
            var midPrefix = false;
            var earlierPrefix = false;
            var laterPrefix = false;
            var isReferenceDatePeriod = false;

            var trimmedText = text.Trim();

            var match = this.config.LaterEarlyPeriodRegex.MatchExact(trimmedText, trim: true);

            if (match.Success)
            {
                if (match.Groups[Constants.EarlyPrefixGroupName].Success)
                {
                    earlyPrefix = true;
                    trimmedText = match.Groups[Constants.SuffixGroupName].ToString();
                    ret.Mod = Constants.EARLY_MOD;
                }
                else if (match.Groups[Constants.LatePrefixGroupName].Success)
                {
                    latePrefix = true;
                    trimmedText = match.Groups[Constants.SuffixGroupName].ToString();
                    ret.Mod = Constants.LATE_MOD;
                }
                else if (match.Groups[Constants.MidPrefixGroupName].Success)
                {
                    midPrefix = true;
                    trimmedText = match.Groups[Constants.SuffixGroupName].ToString();
                    ret.Mod = Constants.MID_MOD;
                }

                var swift = 0;
                if (!string.IsNullOrEmpty(match.Groups[Constants.MonthGroupName].Value))
                {
                    swift = this.config.GetSwiftYear(trimmedText);
                }
                else
                {
                    if (match.Groups[Constants.NextGroupName].Success)
                    {
                        swift = 1;
                    }
                    else if (match.Groups[Constants.LastGroupName].Success)
                    {
                        swift = -1;
                    }
                }

                if (match.Groups[Constants.RelEarlyGroupName].Success)
                {
                    earlierPrefix = true;
                    if (BaseDatePeriodParser.IsPresent(swift))
                    {
                        ret.Mod = null;
                    }
                }
                else if (match.Groups[Constants.RelLateGroupName].Success)
                {
                    laterPrefix = true;
                    if (BaseDatePeriodParser.IsPresent(swift))
                    {
                        ret.Mod = null;
                    }
                }

                var monthStr = match.Groups[Constants.MonthGroupName].Value;

                // Parse expressions "till date", "to date"
                if (match.Groups[Constants.ToDateGroupName].Success)
                {
                    ret.Timex = "PRESENT_REF";
                    ret.FutureValue = ret.PastValue = referenceDate;
                    ret.Mod = Constants.BEFORE_MOD;
                    ret.Success = true;
                    return ret;
                }

                if (!string.IsNullOrEmpty(monthStr))
                {
                    swift = this.config.GetSwiftYear(trimmedText);

                    month = this.config.MonthOfYear[monthStr];

                    if (swift >= -1)
                    {
                        ret.Timex = (referenceDate.Year + swift).ToString("D4", CultureInfo.InvariantCulture) + "-" + month.ToString("D2", CultureInfo.InvariantCulture);
                        year = year + swift;
                        futureYear = pastYear = year;
                    }
                    else
                    {
                        ret.Timex = "XXXX-" + month.ToString("D2", CultureInfo.InvariantCulture);
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
                    if (match.Groups[Constants.NextGroupName].Success)
                    {
                        swift = 1;
                    }
                    else if (match.Groups[Constants.LastGroupName].Success)
                    {
                        swift = -1;
                    }

                    var isWorkingWeek = match.Groups[Constants.BusinessDayGroupName].Success;

                    var isWeekOnly = this.config.IsWeekOnly(trimmedText);

                    if (isWorkingWeek || isWeekOnly)
                    {
                        var delta = Constants.WeekDayCount * swift;
                        var endDelta = delta;

                        var monday = referenceDate.This(DayOfWeek.Monday).AddDays(delta);
                        var endDay = isWorkingWeek ? DayOfWeek.Friday : DayOfWeek.Sunday;

                        var beginDate = referenceDate.This(DayOfWeek.Monday).AddDays(delta);
                        var endDate = inclusiveEndPeriod ?
                                      referenceDate.This(endDay).AddDays(endDelta) :
                                      referenceDate.This(endDay).AddDays(endDelta).AddDays(1);

                        if (earlyPrefix)
                        {
                            endDate = inclusiveEndPeriod ?
                                      referenceDate.This(DayOfWeek.Wednesday).AddDays(endDelta) :
                                      referenceDate.This(DayOfWeek.Wednesday).AddDays(endDelta).AddDays(1);
                        }
                        else if (midPrefix)
                        {
                            beginDate = referenceDate.This(DayOfWeek.Tuesday).AddDays(delta);
                            endDate = inclusiveEndPeriod ?
                                      referenceDate.This(DayOfWeek.Friday).AddDays(endDelta) :
                                      referenceDate.This(DayOfWeek.Friday).AddDays(endDelta).AddDays(1);
                        }
                        else if (latePrefix)
                        {
                            beginDate = referenceDate.This(DayOfWeek.Thursday).AddDays(delta);
                        }

                        if (earlierPrefix && swift == 0)
                        {
                            if (endDate > referenceDate)
                            {
                                endDate = referenceDate;
                            }
                        }
                        else if (laterPrefix && swift == 0)
                        {
                            if (beginDate < referenceDate)
                            {
                                beginDate = referenceDate;
                            }
                        }

                        ret.Timex = isReferenceDatePeriod ? TimexUtility.GenerateWeekTimex() : TimexUtility.GenerateWeekTimex(monday);

                        if (latePrefix && swift != 0)
                        {
                            ret.Mod = Constants.LATE_MOD;
                        }

                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(beginDate, endDate);

                        ret.Success = true;

                        return ret;
                    }

                    if (this.config.IsWeekend(trimmedText))
                    {
                        var beginDate = referenceDate.This(DayOfWeek.Saturday).AddDays(Constants.WeekDayCount * swift);
                        var endDate = referenceDate.This(DayOfWeek.Sunday).AddDays(Constants.WeekDayCount * swift);
                        ret.Timex = isReferenceDatePeriod ? TimexUtility.GenerateWeekendTimex() : TimexUtility.GenerateWeekendTimex(beginDate);
                        endDate = inclusiveEndPeriod ? endDate : endDate.AddDays(1);
                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }

                    if (this.config.IsMonthOnly(trimmedText))
                    {
                        var date = referenceDate.AddMonths(swift);
                        month = date.Month;
                        year = date.Year;
                        ret.Timex = isReferenceDatePeriod ? TimexUtility.GenerateMonthTimex() : TimexUtility.GenerateMonthTimex(date);
                        futureYear = pastYear = year;
                    }
                    else if (this.config.IsYearOnly(trimmedText))
                    {
                        var date = referenceDate.AddYears(swift);
                        year = date.Year;

                        if (!string.IsNullOrEmpty(match.Groups[Constants.SpecialGroupName].Value))
                        {
                            swift = this.config.GetSwiftYear(trimmedText);
                            date = swift < -1 ? Constants.InvalidDate : date;
                            ret.Timex = TimexUtility.GenerateYearTimex(date, null);
                            ret.Success = true;
                            return ret;
                        }

                        var beginDate = DateObject.MinValue.SafeCreateFromValue(year, 1, 1);
                        var endDate = inclusiveEndPeriod ?
                            DateObject.MinValue.SafeCreateFromValue(year, 12, 31) :
                            DateObject.MinValue.SafeCreateFromValue(year, 12, 31).AddDays(1);

                        if (earlyPrefix)
                        {
                            endDate = inclusiveEndPeriod ?
                                DateObject.MinValue.SafeCreateFromValue(year, 6, 30) :
                                DateObject.MinValue.SafeCreateFromValue(year, 6, 30).AddDays(1);
                        }
                        else if (midPrefix)
                        {
                            beginDate = DateObject.MinValue.SafeCreateFromValue(year, 4, 1);
                            endDate = inclusiveEndPeriod ?
                                DateObject.MinValue.SafeCreateFromValue(year, 9, 30) :
                                DateObject.MinValue.SafeCreateFromValue(year, 9, 30).AddDays(1);
                        }
                        else if (latePrefix)
                        {
                            beginDate = DateObject.MinValue.SafeCreateFromValue(year, Constants.WeekDayCount, 1);
                        }

                        if (earlierPrefix && swift == 0)
                        {
                            if (endDate > referenceDate)
                            {
                                endDate = referenceDate;
                            }
                        }
                        else if (laterPrefix && swift == 0)
                        {
                            if (beginDate < referenceDate)
                            {
                                beginDate = referenceDate;
                            }
                        }

                        ret.Timex = isReferenceDatePeriod ? TimexUtility.GenerateYearTimex() : TimexUtility.GenerateYearTimex(date);
                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }

                    // Early/mid/late are resolved in this policy to 4 month ranges at the start/middle/end of the year.
                    else if (!string.IsNullOrEmpty(match.Groups[Constants.FourDigitYearGroupName].Value))
                    {
                        var date = referenceDate.AddYears(swift);
                        year = int.Parse(match.Groups[Constants.FourDigitYearGroupName].Value, CultureInfo.InvariantCulture);

                        var beginDate = DateObject.MinValue.SafeCreateFromValue(year, 1, 1);
                        var endDate = inclusiveEndPeriod ?
                            DateObject.MinValue.SafeCreateFromValue(year, 12, 31) :
                            DateObject.MinValue.SafeCreateFromValue(year, 12, 31).AddDays(1);

                        if (earlyPrefix)
                        {
                            endDate = inclusiveEndPeriod ?
                                DateObject.MinValue.SafeCreateFromValue(year, 4, 30) :
                                DateObject.MinValue.SafeCreateFromValue(year, 4, 30).AddDays(1);
                        }
                        else if (midPrefix)
                        {
                            beginDate = DateObject.MinValue.SafeCreateFromValue(year, 5, 1);
                            endDate = inclusiveEndPeriod ?
                                DateObject.MinValue.SafeCreateFromValue(year, 8, 31) :
                                DateObject.MinValue.SafeCreateFromValue(year, 8, 31).AddDays(1);
                        }
                        else if (latePrefix)
                        {
                            beginDate = DateObject.MinValue.SafeCreateFromValue(year, 9, 1);
                        }

                        ret.Timex = isReferenceDatePeriod ? TimexUtility.GenerateYearTimex() : TimexUtility.GenerateYearTimex(beginDate);
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
            var futureEnd = inclusiveEndPeriod ?
                    DateObject.MinValue.SafeCreateFromValue(futureYear, month, 1).AddMonths(1).AddDays(-1) :
                    DateObject.MinValue.SafeCreateFromValue(futureYear, month, 1).AddMonths(1);

            var pastStart = DateObject.MinValue.SafeCreateFromValue(pastYear, month, 1);
            var pastEnd = inclusiveEndPeriod ?
                DateObject.MinValue.SafeCreateFromValue(pastYear, month, 1).AddMonths(1).AddDays(-1) :
                DateObject.MinValue.SafeCreateFromValue(pastYear, month, 1).AddMonths(1);

            if (earlyPrefix)
            {
                futureEnd = inclusiveEndPeriod ?
                    DateObject.MinValue.SafeCreateFromValue(futureYear, month, 15) :
                    DateObject.MinValue.SafeCreateFromValue(futureYear, month, 15).AddDays(1);

                pastEnd = inclusiveEndPeriod ?
                    DateObject.MinValue.SafeCreateFromValue(pastYear, month, 15) :
                    DateObject.MinValue.SafeCreateFromValue(pastYear, month, 15).AddDays(1);
            }
            else if (midPrefix)
            {
                futureStart = DateObject.MinValue.SafeCreateFromValue(futureYear, month, 10);
                pastStart = DateObject.MinValue.SafeCreateFromValue(pastYear, month, 10);
                futureEnd = inclusiveEndPeriod ?
                    DateObject.MinValue.SafeCreateFromValue(futureYear, month, 20) :
                    DateObject.MinValue.SafeCreateFromValue(futureYear, month, 20).AddDays(1);

                pastEnd = inclusiveEndPeriod ?
                    DateObject.MinValue.SafeCreateFromValue(pastYear, month, 20) :
                    DateObject.MinValue.SafeCreateFromValue(pastYear, month, 20).AddDays(1);
            }
            else if (latePrefix)
            {
                futureStart = DateObject.MinValue.SafeCreateFromValue(futureYear, month, 16);
                pastStart = DateObject.MinValue.SafeCreateFromValue(pastYear, month, 16);
            }

            if (earlierPrefix && futureEnd == pastEnd)
            {
                if (futureEnd > referenceDate)
                {
                    futureEnd = pastEnd = referenceDate;
                }
            }
            else if (laterPrefix && futureStart == pastStart)
            {
                if (futureStart < referenceDate)
                {
                    futureStart = pastStart = referenceDate;
                }
            }

            ret.FutureValue = new Tuple<DateObject, DateObject>(futureStart, futureEnd);
            ret.PastValue = new Tuple<DateObject, DateObject>(pastStart, pastEnd);
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

                var year = ConvertCJKToInteger(yearStr);

                return HandleYearResult(ret, hasHalf, isFirstHalf, false, year);
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

                return HandleYearResult(ret, hasHalf, isFirstHalf, false, year);
            }

            return ret;
        }

        private string HandleWithHalfYear(ConditionalMatch match, string text, out bool hasHalf, out bool isFirstHalf)
        {
            var firstHalf = match.Groups[Constants.FirstHalfGroupName].Value;
            var secondHalf = match.Groups[Constants.SecondHalfGroupName].Value;

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

        private DateTimeResolutionResult HandleYearResult(DateTimeResolutionResult ret, bool hasHalf, bool isFirstHalf, bool isReferenceDatePeriod, int year)
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

            ret.Timex = isReferenceDatePeriod ? TimexUtility.GenerateYearTimex() : DateTimeFormatUtil.LuisDate(year);

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
                var match = this.config.WeekWithWeekDayRangeRegex.Match(text);

                if (match.Success)
                {
                    var weekPrefix = match.Groups[Constants.WeekGroupName].ToString();

                    // Check if weekPrefix is already included in the extractions otherwise include it
                    if (!string.IsNullOrEmpty(weekPrefix))
                    {
                        if (!er[0].Text.Contains(weekPrefix))
                        {
                            er[0].Text = weekPrefix + er[0].Text;
                        }

                        if (!er[1].Text.Contains(weekPrefix))
                        {
                            er[1].Text = weekPrefix + er[1].Text;
                        }
                    }

                    pr1 = this.config.DateParser.Parse(er[0], referenceDate);
                    pr2 = this.config.DateParser.Parse(er[1], referenceDate);

                }
                else
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
                var srcUnit = match.Groups[Constants.UnitGroupName].Value;
                var beforeStr = text.Substring(0, match.Index);

                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];
                    numStr = match.Groups[Constants.NumGroupName].Value;

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

            return ret;
        }

        // Analogous to the same method in BaseDatePeriodParser, it deals with date periods that involve durations
        // e.g. "past 2 years", "within 2 days", "first 2 weeks of 2018".
        private DateTimeResolutionResult ParseDuration(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            // For cases like 'first 2 weeks of 2021' (2021年的前2周), 'past 2 years' (前两年), 'next 3 years' (后三年)
            var durationRes = this.config.DurationExtractor.Extract(text, referenceDate);

            var matchHalf = this.config.OneWordPeriodRegex.MatchExact(text, trim: true);

            // halfTag cases are processed in ParseOneWordPeriod
            if (!string.IsNullOrEmpty(matchHalf.Groups[Constants.HalfTagGroupName].Value))
            {
                ret.Success = false;
                return ret;
            }

            if (durationRes.Count > 0)
            {
                var beforeStr = text.Substring(0, (int)durationRes[0].Start);
                var afterStr = text.Substring((int)durationRes[0].Start + (int)durationRes[0].Length).Trim();
                var matches = this.config.UnitRegex.Matches(durationRes[0].Text);
                var matchBusinessDays = this.config.DurationUnitRegex.MatchExact(text, trim: true);

                // handle duration cases like "5 years 1 month 21 days" and "multiple business days"
                if ((matches.Count > 1 && matches.Count <= 3) ||
                    matchBusinessDays.Groups[Constants.BusinessDayGroupName].Success)
                {
                    ret = ParseMultipleDatesDuration(text, referenceDate);
                    return ret;
                }
                else if (matches.Count == 1 && matches[0].Success)
                {
                    var srcUnit = matches[0].Groups[Constants.UnitGroupName].Value;

                    var numberStr = durationRes[0].Text.Substring(0, matches[0].Index).Trim();
                    var matchFew = this.config.DurationRelativeDurationUnitRegex.Match(text);
                    var number = numberStr.Equals(matchFew.Groups[Constants.FewGroupName].Value, StringComparison.Ordinal) ? 3 : ConvertCJKToNum(numberStr);

                    if (this.config.UnitMap.ContainsKey(srcUnit))
                    {
                        var beginDate = referenceDate;
                        var endDate = referenceDate;

                        var unitStr = this.config.UnitMap[srcUnit];

                        // Get prefix
                        var prefixMatch = new ConditionalMatch(Match.Empty, false);
                        if (this.config.UnitRegex.Match(srcUnit).Groups[Constants.UnitOfYearGroupName].Success)
                        {
                            // Patterns like 'first 2 weeks of 2018' are considered only if the unit is compatible
                            prefixMatch = this.config.FirstLastOfYearRegex.MatchExact(beforeStr, trim: true);
                        }

                        var isOfYearMatch = prefixMatch.Success;
                        var isPastMatch = prefixMatch.Groups[Constants.LastGroupName].Success;
                        var isFuture = false;

                        if (!prefixMatch.Success)
                        {
                            prefixMatch = this.config.PastRegex.MatchExact(beforeStr, trim: true);
                            isPastMatch = prefixMatch.Success;
                        }

                        if (!prefixMatch.Success)
                        {
                            prefixMatch = this.config.FutureRegex.MatchExact(beforeStr, trim: true);
                            isFuture = prefixMatch.Success;
                        }

                        if (!prefixMatch.Success)
                        {
                            prefixMatch = this.config.FutureRegex.MatchExact(afterStr, trim: true);
                            isFuture = prefixMatch.Success;
                        }

                        if (isFuture && !this.config.FutureRegex.MatchExact(afterStr, trim: true).Groups[Constants.WithinGroupName].Success)
                        {
                            // for the "within" case it should start from the current day
                            beginDate = beginDate.AddDays(1);
                            endDate = endDate.AddDays(1);
                        }

                        // Shift by year (if present)
                        if (isOfYearMatch)
                        {
                            // Get year
                            var year = GetYearFromText(prefixMatch.Match);
                            if (year == Constants.InvalidYear)
                            {
                                var swift = 0;
                                var yearRel = prefixMatch.Groups[Constants.YearRelGroupName].Value;
                                if (this.config.IsLastYear(yearRel))
                                {
                                    swift = -1;
                                }
                                else if (this.config.IsNextYear(yearRel))
                                {
                                    swift = 1;
                                }

                                year = referenceDate.Year + swift;
                            }

                            // Get begin/end dates for year
                            if (unitStr == Constants.TimexWeek)
                            {
                                // First/last week of the year is calculated according to ISO definition
                                beginDate = DateObjectExtension.GetFirstThursday(year).This(DayOfWeek.Monday);
                                endDate = DateObjectExtension.GetLastThursday(year).This(DayOfWeek.Monday).AddDays(7);
                            }
                            else
                            {
                                beginDate = DateObject.MinValue.SafeCreateFromValue(year, 1, 1);
                                endDate = DateObject.MinValue.SafeCreateFromValue(year, 12, 31).AddDays(1);
                            }
                        }

                        // Shift begin/end dates by duration span
                        if (prefixMatch.Success)
                        {
                            if (isPastMatch)
                            {
                                beginDate = endDate;
                                switch (unitStr)
                                {
                                    case Constants.TimexDay:
                                        beginDate = beginDate.AddDays(-number);
                                        break;
                                    case Constants.TimexWeek:
                                        beginDate = beginDate.AddDays(-7 * number);
                                        break;
                                    case Constants.TimexMonthFull:
                                        beginDate = beginDate.AddMonths(-number);
                                        break;
                                    case Constants.TimexYear:
                                        beginDate = beginDate.AddYears(-number);
                                        break;
                                    default:
                                        return ret;
                                }
                            }
                            else
                            {
                                endDate = beginDate;
                                switch (unitStr)
                                {
                                    case Constants.TimexDay:
                                        endDate = endDate.AddDays(number);
                                        break;
                                    case Constants.TimexWeek:
                                        endDate = endDate.AddDays(7 * number);
                                        break;
                                    case Constants.TimexMonthFull:
                                        endDate = endDate.AddMonths(number);
                                        break;
                                    case Constants.TimexYear:
                                        endDate = endDate.AddYears(number);
                                        break;
                                    default:
                                        return ret;
                                }

                            }

                            ret.Timex = $"({DateTimeFormatUtil.LuisDate(beginDate)},{DateTimeFormatUtil.LuisDate(endDate)},P{number}{unitStr[0]})";
                            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                            ret.Success = true;
                            return ret;
                        }
                    }
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParseMultipleDatesDuration(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            DateObject beginDate;
            DateObject endDate = beginDate = referenceDate;
            var durationTimex = string.Empty;
            var restNowSunday = false;

            var durationErs = config.DurationExtractor.Extract(text, referenceDate);

            if (durationErs.Count > 0)
            {
                var durationPr = config.DurationParser.Parse(durationErs[0]);
                var beforeStr = text.Substring(0, durationPr.Start ?? 0).Trim();
                var afterStr = text.Substring((durationPr.Start ?? 0) + (durationPr.Length ?? 0)).Trim();

                ModAndDateResult modAndDateResult = new ModAndDateResult(beginDate, endDate);

                if (durationPr.Value != null)
                {
                    var durationResult = (DateTimeResolutionResult)durationPr.Value;

                    if (string.IsNullOrEmpty(durationResult.Timex))
                    {
                        return ret;
                    }

                    if (config.PastRegex.IsMatch(beforeStr) || config.PastRegex.IsMatch(afterStr))
                    {
                        modAndDateResult = ModAndDateResult.GetModAndDate(beginDate, endDate, referenceDate, durationResult.Timex, false);
                        beginDate = modAndDateResult.BeginDate;
                    }

                    if ((config.FutureRegex.IsExactMatch(beforeStr, trim: true) || config.FutureRegex.IsExactMatch(afterStr, trim: true)) &&
                        DurationParsingUtil.IsDateDuration(durationResult.Timex))
                    {
                        modAndDateResult = ModAndDateResult.GetModAndDate(beginDate, endDate, referenceDate, durationResult.Timex, true);

                        beginDate = modAndDateResult.BeginDate;
                        endDate = modAndDateResult.EndDate;

                        // In GetModAndDate, this "future" resolution will add one day to beginDate/endDate,
                        // but for the "within" case it should start from the current day.
                        if (this.config.FutureRegex.MatchExact(afterStr, trim: true).Groups[Constants.WithinGroupName].Success)
                        {
                            beginDate = beginDate.AddDays(-1);
                            endDate = endDate.AddDays(-1);
                        }
                    }

                    if (!string.IsNullOrEmpty(modAndDateResult.Mod))
                    {
                        ((DateTimeResolutionResult)durationPr.Value).Mod = modAndDateResult.Mod;
                    }

                    durationTimex = durationResult.Timex;
                    ret.SubDateTimeEntities = new List<object> { durationPr };
                    if (modAndDateResult.DateList != null)
                    {
                        ret.List = modAndDateResult.DateList.Cast<object>().ToList();
                    }
                }

                if (!beginDate.Equals(endDate) || restNowSunday)
                {
                    endDate = inclusiveEndPeriod ? endDate.AddDays(-1) : endDate;

                    // TODO: analyse upper code and use GenerateDatePeriodTimex to create this Timex.
                    ret.Timex = $"({DateTimeFormatUtil.LuisDate(beginDate)},{DateTimeFormatUtil.LuisDate(endDate)},{durationTimex})";
                    ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                    ret.Success = true;
                    return ret;
                }
            }

            ret.Success = false;
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

            var cardinalStr = match.Groups[Constants.CardinalGroupName].Value;
            var monthStr = match.Groups[Constants.MonthGroupName].Value;
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
                year = GetYearFromText(match);

                if (year == Constants.InvalidYear)
                {
                    year = referenceDate.Year;
                    noYear = true;
                }

            }

            ret = GetWeekOfMonth(cardinalStr, month, year, referenceDate, noYear);

            return ret;
        }

        private DateTimeResolutionResult ParseWeekOfDate(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = config.WeekOfDateRegex.Match(text);
            var dateErs = config.DateExtractor.Extract(text, referenceDate);

            // Cases like 'week of september 16th' (9月16日の週)
            if (match.Success && dateErs.Count == 1)
            {
                var pr = (DateTimeResolutionResult)config.DateParser.Parse(dateErs[0], referenceDate).Value;
                if ((config.Options & DateTimeOptions.CalendarMode) != 0)
                {
                    var monday = ((DateObject)pr.FutureValue).This(DayOfWeek.Monday);
                    ret.Timex = DateTimeFormatUtil.ToIsoWeekTimex(monday);
                }
                else
                {
                    ret.Timex = pr.Timex;
                }

                ret.Comment = Constants.Comment_WeekOf;
                ret.FutureValue = BaseDatePeriodParser.GetWeekRangeFromDate((DateObject)pr.FutureValue);
                ret.PastValue = BaseDatePeriodParser.GetWeekRangeFromDate((DateObject)pr.PastValue);
                ret.Success = true;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseMonthOfDate(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = config.MonthOfDateRegex.Match(text);
            var ex = config.DateExtractor.Extract(text, referenceDate);

            // Cases like 'month of september 16th' (9月16日の月)
            if (match.Success && ex.Count == 1)
            {
                var pr = (DateTimeResolutionResult)config.DateParser.Parse(ex[0], referenceDate).Value;
                ret.Timex = pr.Timex;
                ret.Comment = Constants.Comment_MonthOf;
                ret.FutureValue = BaseDatePeriodParser.GetMonthRangeFromDate((DateObject)pr.FutureValue);
                ret.PastValue = BaseDatePeriodParser.GetMonthRangeFromDate((DateObject)pr.PastValue);
                ret.Success = true;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseWhichWeek(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.WhichWeekRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var num = int.Parse(match.Groups[Constants.NumberGroupName].ToString(), CultureInfo.InvariantCulture);
                if (num == 0)
                {
                    return ret;
                }

                var year = referenceDate.Year;
                ret.Timex = year.ToString("D4", CultureInfo.InvariantCulture) + "-W" + num.ToString("D2", CultureInfo.InvariantCulture);

                var firstDay = DateObject.MinValue.SafeCreateFromValue(year, 1, 1);
                var firstThursday = firstDay.AddDays(DayOfWeek.Thursday - firstDay.DayOfWeek);
                var firstWeek = Cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                if (firstWeek == 1)
                {
                    num -= 1;
                }

                var value = firstThursday.AddDays((num * 7) - 3);
                var futureDate = value;
                var pastDate = value;

                ret.FutureValue = new Tuple<DateObject, DateObject>(futureDate, futureDate.AddDays(Constants.WeekDayCount));
                ret.PastValue = new Tuple<DateObject, DateObject>(pastDate, pastDate.AddDays(Constants.WeekDayCount));
                ret.Success = true;
            }

            return ret;
        }

        private DateTimeResolutionResult GetWeekOfMonth(string cardinalStr, int month, int year, DateObject referenceDate, bool noYear)
        {
            var ret = new DateTimeResolutionResult();
            var targetMonday = GetMondayOfTargetWeek(cardinalStr, month, year);

            var futureDate = targetMonday;
            var pastDate = targetMonday;

            if (noYear && futureDate < referenceDate)
            {
                futureDate = GetMondayOfTargetWeek(cardinalStr, month, year + 1);
            }

            if (noYear && pastDate >= referenceDate)
            {
                pastDate = GetMondayOfTargetWeek(cardinalStr, month, year - 1);
            }

            if (noYear)
            {
                year = Constants.InvalidYear;
            }

            // Note that if the cardinalStr equals to "last", the weekNumber would be fixed at "5"
            // This may lead to some inconsistency between Timex and Resolution
            // the StartDate and EndDate of the resolution would always be correct (following ISO week definition)
            // But week number for "last week" might be inconsistent with the resolution as we only have one Timex, but we may have past and future resolutions which may have different week numbers
            var weekNum = GetWeekNumberForMonth(cardinalStr);
            ret.Timex = TimexUtility.GenerateWeekOfMonthTimex(year, month, weekNum);

            ret.FutureValue = inclusiveEndPeriod
                ? new Tuple<DateObject, DateObject>(futureDate, futureDate.AddDays(Constants.WeekDayCount - 1))
                : new Tuple<DateObject, DateObject>(futureDate, futureDate.AddDays(Constants.WeekDayCount));

            ret.PastValue = inclusiveEndPeriod
                ? new Tuple<DateObject, DateObject>(pastDate, pastDate.AddDays(Constants.WeekDayCount - 1))
                : new Tuple<DateObject, DateObject>(pastDate, pastDate.AddDays(Constants.WeekDayCount));

            ret.Success = true;

            return ret;
        }

        private DateObject GetMondayOfTargetWeek(string cardinalStr, int month, int year)
        {
            DateObject result;
            if (config.WoMLastRegex.IsExactMatch(cardinalStr, trim: true))
            {
                var lastThursday = DateObjectExtension.GetLastThursday(year, month);
                result = lastThursday.This(DayOfWeek.Monday);
            }
            else
            {
                int cardinal = GetWeekNumberForMonth(cardinalStr);
                var firstThursday = DateObjectExtension.GetFirstThursday(year, month);

                result = firstThursday.This(DayOfWeek.Monday)
                    .AddDays(Constants.WeekDayCount * (cardinal - 1));
            }

            return result;
        }

        private int GetWeekNumberForMonth(string cardinalStr)
        {
            // "last week of month" might not be "5th week of month"
            // Sometimes it can also be "4th week of month" depends on specific year and month
            // But as we only have one Timex, we use "5" to indicate it's the "last week"
            int cardinal;
            if (config.WoMLastRegex.IsExactMatch(cardinalStr, trim: true))
            {
                cardinal = 5;
            }
            else
            {
                cardinal = this.config.CardinalMap[cardinalStr];
            }

            return cardinal;
        }

        // Cases like 'second week of 2021' (2021年的第二周)
        private DateTimeResolutionResult ParseWeekOfYear(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var trimmedText = text.Trim();

            var match = this.config.WeekOfYearRegex.Match(text);
            if (!match.Success)
            {
                return ret;
            }

            var cardinalStr = match.Groups[Constants.CardinalGroupName].Value;
            var orderStr = match.Groups[Constants.OrderGroupName].Value;

            var year = GetYearFromText(match);
            if (year == Constants.InvalidYear)
            {
                var swift = this.config.GetSwiftYear(orderStr);
                if (swift < -1)
                {
                    return ret;
                }

                year = referenceDate.Year + swift;
            }

            DateObject targetWeekMonday;

            if (config.WoMLastRegex.IsExactMatch(cardinalStr, trim: true))
            {
                targetWeekMonday = DateObjectExtension.GetLastThursday(year).This(DayOfWeek.Monday);

                ret.Timex = TimexUtility.GenerateWeekTimex(targetWeekMonday);
            }
            else
            {
                var weekNum = this.config.CardinalMap[cardinalStr];
                targetWeekMonday = DateObjectExtension.GetFirstThursday(year).This(DayOfWeek.Monday)
                    .AddDays(Constants.WeekDayCount * (weekNum - 1));

                ret.Timex = TimexUtility.GenerateWeekOfYearTimex(year, weekNum);
            }

            ret.FutureValue = inclusiveEndPeriod ?
                new Tuple<DateObject, DateObject>(targetWeekMonday, targetWeekMonday.AddDays(Constants.WeekDayCount - 1)) :
                new Tuple<DateObject, DateObject>(targetWeekMonday, targetWeekMonday.AddDays(Constants.WeekDayCount));

            ret.PastValue = inclusiveEndPeriod ?
                new Tuple<DateObject, DateObject>(targetWeekMonday, targetWeekMonday.AddDays(Constants.WeekDayCount - 1)) :
                new Tuple<DateObject, DateObject>(targetWeekMonday, targetWeekMonday.AddDays(Constants.WeekDayCount));

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
                var yearNum = match.Groups[Constants.YearGroupName].Value;
                var yearCJK = match.Groups[Constants.YearCJKGroupName].Value;
                var yearRel = match.Groups[Constants.YearRelGroupName].Value;

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

                // handle cases like "this summer" 今夏
                if (!hasYear && config.ThisRegex.MatchBegin(text, trim: true).Success)
                {
                    hasYear = true;
                    year = referenceDate.Year;
                }
                else if (!hasYear && config.NextRegex.MatchBegin(text, trim: true).Success)
                {
                    hasYear = true;
                    year = referenceDate.Year + 1;
                }
                else if (!hasYear && config.LastRegex.MatchBegin(text, trim: true).Success)
                {
                    hasYear = true;
                    year = referenceDate.Year - 1;
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
                var seasonStr = match.Groups[Constants.SeasonGroupName].Value;

                if (match.Groups[Constants.EarlyPrefixGroupName].Success)
                {
                    ret.Mod = Constants.EARLY_MOD;
                }
                else if (match.Groups[Constants.MidPrefixGroupName].Success)
                {
                    ret.Mod = Constants.MID_MOD;
                }
                else if (match.Groups[Constants.LatePrefixGroupName].Success)
                {
                    ret.Mod = Constants.LATE_MOD;
                }

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
            var yearNum = match.Groups[Constants.YearGroupName].Value;
            var yearCJK = match.Groups[Constants.YearCJKGroupName].Value;
            var yearRel = match.Groups[Constants.YearRelGroupName].Value;
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
            var cardinalStr = match.Groups[Constants.CardinalGroupName].Value;
            var quarterNum = this.config.CardinalMap[cardinalStr];

            if (!string.IsNullOrEmpty(yearNum) || !string.IsNullOrEmpty(yearRel))
            {
                var beginDate = DateObject.MinValue.SafeCreateFromValue(year, ((quarterNum - 1) * Constants.TrimesterMonthCount) + 1, 1);
                var endDate = DateObject.MinValue.SafeCreateFromValue(year, quarterNum * Constants.TrimesterMonthCount, 1).AddMonths(1);
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDate, endDate, DatePeriodTimexType.ByMonth);
                ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDate, endDate, DatePeriodTimexType.ByMonth);
                ret.Success = true;
            }
            else
            {
                var beginDate = DateObject.MinValue.SafeCreateFromValue(year, ((quarterNum - 1) * Constants.TrimesterMonthCount) + 1, 1);
                var endDate = DateObject.MinValue.SafeCreateFromValue(year, quarterNum * Constants.TrimesterMonthCount, 1).AddMonths(1);
                ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDate, endDate, DatePeriodTimexType.ByMonth);
                beginDate = DateObject.MinValue.SafeCreateFromValue(year + 1, ((quarterNum - 1) * Constants.TrimesterMonthCount) + 1, 1);
                endDate = DateObject.MinValue.SafeCreateFromValue(year + 1, quarterNum * Constants.TrimesterMonthCount, 1).AddMonths(1);
                ret.FutureValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
            }

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

            if (match.Success)
            {
                var decadeStr = match.Groups[Constants.DecadeGroupName].Value;
                if (!int.TryParse(decadeStr, out decade))
                {
                    decade = ConvertCJKToNum(decadeStr);
                }

                var centuryStr = match.Groups[Constants.CenturyGroupName].Value;
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
                    centuryStr = match.Groups[Constants.RelCenturyGroupName].Value;

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
            var firstTwoNumOfYear = match.Groups[Constants.FirstTwoYearGroupName].Value;

            // handle cases like "2000年代"
            if (!string.IsNullOrEmpty(firstTwoNumOfYear))
            {
                beginYear = (ConvertCJKToInteger(firstTwoNumOfYear) * 100) + decade;
            }

            ret.Timex = TimexUtility.GenerateDecadeTimex(beginYear, decadeLastYear, decade, inputCentury);

            int futureYear = beginYear, pastYear = beginYear;
            var startDate = DateObject.MinValue.SafeCreateFromValue(beginYear, 1, 1);
            if (!inputCentury && startDate < referenceDate && string.IsNullOrEmpty(firstTwoNumOfYear))
            {
                futureYear += 100;
            }

            if (!inputCentury && startDate >= referenceDate && string.IsNullOrEmpty(firstTwoNumOfYear))
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

        private DateTimeResolutionResult ParseCentury(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            int century = (referenceDate.Year / 100) + 1;

            var match = this.config.CenturyRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var centuryStr = match.Groups[Constants.CenturyGroupName].Value;
                if (!string.IsNullOrEmpty(centuryStr))
                {
                    if (!int.TryParse(centuryStr, out century))
                    {
                        century = ConvertCJKToNum(centuryStr);
                    }
                }

                var beginYear = (century - 1) * Constants.CenturyYearsCount;
                var endYear = beginYear + Constants.CenturyYearsCount;

                var startDate = new DateObject(beginYear, 1, 1);
                var endDate = new DateObject(endYear, 1, 1);

                ret.Timex = TimexUtility.GenerateDatePeriodTimex(startDate, endDate, DatePeriodTimexType.ByYear);
                ret.FutureValue = new Tuple<DateObject, DateObject>(startDate, endDate);
                ret.PastValue = new Tuple<DateObject, DateObject>(startDate, endDate);
                ret.Success = true;
            }

            return ret;
        }

        // Only handle cases like "within/less than/more than x weeks from/before/after today"
        private DateTimeResolutionResult ParseDatePointWithAgoAndLater(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var er = this.config.DateExtractor.Extract(text, referenceDate).FirstOrDefault();
            var trimmedText = text.Trim();
            var match = this.config.DatePointWithAgoAndLater.MatchExact(trimmedText, trim: true);

            if (er != null && match.Success)
            {
                var isAgo = match.Groups[Constants.AgoGroupName].Success;
                var isWithin = match.Groups[Constants.WithinGroupName].Success;
                var isMoreThan = match.Groups[Constants.MoreGroupName].Success;

                if (match.Groups[Constants.YesterdayGroupName].Success)
                {
                    referenceDate = referenceDate.AddDays(-1);
                }

                er.Text = text;
                var pr = this.config.DateParser.Parse(er, referenceDate);
                var durationExtractionResult = this.config.DurationExtractor.Extract(er.Text, referenceDate).FirstOrDefault();

                if (durationExtractionResult != null)
                {
                    var duration = this.config.DurationParser.Parse(durationExtractionResult);
                    var durationInSeconds = (double)((DateTimeResolutionResult)duration.Value).PastValue;

                    if (isWithin)
                    {
                        DateObject startDate;
                        DateObject endDate;

                        if (isAgo)
                        {
                            startDate = (DateObject)((DateTimeResolutionResult)pr.Value).PastValue;
                            endDate = startDate.AddSeconds(durationInSeconds);
                        }
                        else
                        {
                            endDate = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
                            startDate = endDate.AddSeconds(-durationInSeconds);
                        }

                        if (startDate != DateObject.MinValue)
                        {
                            var durationTimex = ((DateTimeResolutionResult)duration.Value).Timex;

                            ret.Timex = TimexUtility.GenerateDatePeriodTimexWithDuration(startDate, endDate, durationTimex);
                            ret.FutureValue = new Tuple<DateObject, DateObject>(startDate, endDate);
                            ret.PastValue = new Tuple<DateObject, DateObject>(startDate, endDate);
                            ret.Success = true;
                            return ret;
                        }
                    }
                    else if (isMoreThan)
                    {
                        ret.Mod = isAgo ? Constants.BEFORE_MOD : Constants.AFTER_MOD;
                        ret.Timex = pr.TimexStr;
                        ret.FutureValue = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
                        ret.PastValue = (DateObject)((DateTimeResolutionResult)pr.Value).PastValue;
                        ret.Success = true;
                        return ret;
                    }
                }
            }

            ret.Success = false;
            return ret;
        }

        private DateTimeResolutionResult ParseComplexDatePeriod(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = this.config.ComplexDatePeriodRegex.Match(text);

            if (match.Success)
            {
                var futureBegin = DateObject.MinValue;
                var futureEnd = DateObject.MinValue;
                var pastBegin = DateObject.MinValue;
                var pastEnd = DateObject.MinValue;
                var isSpecificDate = false;
                var isStartByWeek = false;
                var isEndByWeek = false;
                bool isAmbiguousStart = false, isAmbiguousEnd = false;
                var ambiguousRes = new DateTimeResolutionResult();
                var dateContext = GetYearContext(match.Groups[Constants.StartGroupName].Value.Trim(), match.Groups[Constants.EndGroupName].Value.Trim(), text);

                var startResolution = ParseSingleTimePoint(match.Groups[Constants.StartGroupName].Value.Trim(), referenceDate, dateContext);

                if (startResolution.Success)
                {
                    futureBegin = (DateObject)startResolution.FutureValue;
                    pastBegin = (DateObject)startResolution.PastValue;
                    isSpecificDate = true;
                }

                if (!startResolution.Success)
                {
                    startResolution = ParseBaseDatePeriod(match.Groups[Constants.StartGroupName].Value.Trim(), referenceDate, dateContext);

                    if (startResolution.Success)
                    {
                        futureBegin = ShiftResolution((Tuple<DateObject, DateObject>)startResolution.FutureValue, match, start: true);
                        pastBegin = ShiftResolution((Tuple<DateObject, DateObject>)startResolution.PastValue, match, start: true);

                        if (startResolution.Timex.Contains("-W"))
                        {
                            isStartByWeek = true;
                        }
                    }
                }

                if (startResolution.Success)
                {
                    var endResolution = ParseSingleTimePoint(match.Groups[Constants.EndGroupName].Value.Trim(), referenceDate, dateContext);

                    if (endResolution.Success)
                    {

                        futureEnd = (DateObject)endResolution.FutureValue;
                        pastEnd = (DateObject)endResolution.PastValue;
                        isSpecificDate = true;
                    }

                    if (!endResolution.Success || isAmbiguousEnd)
                    {
                        endResolution = ParseBaseDatePeriod(match.Groups[Constants.EndGroupName].Value.Trim(), referenceDate, dateContext);

                        if (endResolution.Success)
                        {
                            // When the end group contains modifiers such as 'end of', 'middle of', the end resolution must be updated accordingly.
                            futureEnd = ShiftResolution((Tuple<DateObject, DateObject>)endResolution.FutureValue, match, start: false);
                            pastEnd = ShiftResolution((Tuple<DateObject, DateObject>)endResolution.PastValue, match, start: false);

                            if (endResolution.Timex.Contains("-W"))
                            {
                                isEndByWeek = true;
                            }
                        }
                    }

                    if (endResolution.Success)
                    {
                        // When start or end is ambiguous it is better to resolve it to the type of the unambiguous extraction.
                        // In Spanish, for example, 'de lunes a mar' (from Monday to Tuesday) or 'de enero a mar' (from January to March).
                        // In the first case 'mar' is resolved as Date (weekday), in the second case it is resolved as DatePeriod (month).
                        if (isAmbiguousStart && isSpecificDate)
                        {
                            startResolution = ambiguousRes;
                            futureBegin = (DateObject)startResolution.FutureValue;
                            pastBegin = (DateObject)startResolution.PastValue;
                        }
                        else if (isAmbiguousEnd && isSpecificDate)
                        {
                            endResolution = ambiguousRes;
                            futureEnd = (DateObject)endResolution.FutureValue;
                            pastEnd = (DateObject)endResolution.PastValue;
                        }

                        if (futureBegin > futureEnd)
                        {
                            if (dateContext == null || dateContext.IsEmpty())
                            {
                                futureBegin = pastBegin;
                            }
                            else
                            {
                                futureBegin = DateContext.SwiftDateObject(futureBegin, futureEnd);
                            }
                        }

                        if (pastEnd < pastBegin)
                        {
                            if (dateContext == null || dateContext.IsEmpty())
                            {
                                pastEnd = futureEnd;
                            }
                            else
                            {
                                pastBegin = DateContext.SwiftDateObject(pastBegin, pastEnd);
                            }
                        }

                        // If both begin/end are date ranges in "Month", the Timex should be ByMonth
                        // The year period case should already be handled in Basic Cases
                        var datePeriodTimexType = DatePeriodTimexType.ByMonth;

                        if (isSpecificDate)
                        {
                            // If at least one of the begin/end is specific date, the Timex should be    ByDay
                            datePeriodTimexType = DatePeriodTimexType.ByDay;
                        }
                        else if (isStartByWeek && isEndByWeek)
                        {
                            // If both begin/end are date ranges in "Week", the Timex should be ByWeek
                            datePeriodTimexType = DatePeriodTimexType.ByWeek;
                        }

                        var hasYear = !startResolution.Timex.StartsWith(Constants.TimexFuzzyYear, StringComparison.Ordinal) ||
                            !endResolution.Timex.StartsWith(Constants.TimexFuzzyYear, StringComparison.Ordinal);

                        // If the year is not specified, the combined range timex will use fuzzy years.
                        ret.Timex = TimexUtility.GenerateDatePeriodTimex(futureBegin, futureEnd, datePeriodTimexType, pastBegin, pastEnd, hasYear);
                        ret.FutureValue = new Tuple<DateObject, DateObject>(futureBegin, futureEnd);
                        ret.PastValue = new Tuple<DateObject, DateObject>(pastBegin, pastEnd);
                        ret.Success = true;
                    }
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParseBaseDatePeriod(string text, DateObject referenceDate, DateContext dateContext = null)
        {
            var innerResult = ParseSimpleCases(text, referenceDate);

            if (!innerResult.Success)
            {
                innerResult = ParseDuration(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseOneWordPeriod(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseNumberWithUnit(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseDayToDay(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = MergeTwoTimePoints(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseYearAndMonth(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseYearToYear(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseMonthToMonth(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseYear(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseWeekOfMonth(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseWeekOfYear(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseWeekOfDate(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseMonthOfDate(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseWhichWeek(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseSeason(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseQuarter(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseDecade(text, referenceDate);
            }

            // Cases like "21st century"
            if (!innerResult.Success)
            {
                innerResult = ParseCentury(text, referenceDate);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseDatePointWithAgoAndLater(text, referenceDate);
            }

            if (innerResult.Success && dateContext != null)
            {
                innerResult = dateContext.ProcessDatePeriodEntityResolution(innerResult);
            }

            return innerResult;
        }

    }

    }
