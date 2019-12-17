using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDatePeriodParserConfiguration : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATEPERIOD; // "DatePeriod";

        private static readonly IDateTimeExtractor SingleDateExtractor = new ChineseDateExtractorConfiguration();

        private static readonly IDateTimeExtractor DurationExtractor = new ChineseDurationExtractorConfiguration();

        private readonly IExtractor integerExtractor;

        private readonly IParser integerParser;

        private readonly IFullDateTimeParserConfiguration config;

        public ChineseDatePeriodParserConfiguration(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;

            var numOptions = NumberOptions.None;

            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            integerExtractor = new IntegerExtractor(numConfig);
            integerParser = new BaseCJKNumberParser(new ChineseNumberParserConfiguration(numConfig));

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

        // convert Chinese Number to Integer
        private int ConvertChineseToNum(string numStr)
        {
            var num = -1;
            var er = integerExtractor.Extract(numStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal))
                {
                    num = Convert.ToInt32((double)(integerParser.Parse(er[0]).Value ?? 0));
                }
            }

            return num;
        }

        // convert Chinese Year to Integer
        private int ConvertChineseToInteger(string yearChsStr)
        {
            var year = 0;
            var num = 0;

            var er = integerExtractor.Extract(yearChsStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal))
                {
                    num = Convert.ToInt32((double)(integerParser.Parse(er[0]).Value ?? 0));
                }
            }

            if (num < 10)
            {
                num = 0;
                foreach (var ch in yearChsStr)
                {
                    num *= 10;
                    er = integerExtractor.Extract(ch.ToString());
                    if (er.Count != 0)
                    {
                        if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal))
                        {
                            num += Convert.ToInt32((double)(integerParser.Parse(er[0]).Value ?? 0));
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

            var match = ChineseDatePeriodExtractorConfiguration.SimpleCasesRegex.MatchExact(text, trim: true);
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
                    year = int.Parse(yearStr);
                    if (year < 100 && year >= this.config.TwoNumYear)
                    {
                        year += 1900;
                    }
                    else if (year < 100 && year < this.config.TwoNumYear)
                    {
                        year += 2000;
                    }

                    inputYear = true;
                }
                else
                {
                    noYear = true;
                }

                if (!string.IsNullOrEmpty(monthStr))
                {
                    month = ToMonthNumber(monthStr);
                }
                else
                {
                    monthStr = match.Groups["relmonth"].Value.Trim();
                    var thisMatch = ChineseDatePeriodExtractorConfiguration.ThisRegex.Match(monthStr);
                    var nextMatch = ChineseDatePeriodExtractorConfiguration.NextRegex.Match(monthStr);
                    var lastMatch = ChineseDatePeriodExtractorConfiguration.LastRegex.Match(monthStr);

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

                if (inputYear || ChineseDatePeriodExtractorConfiguration.ThisRegex.Match(monthStr).Success ||
                    ChineseDatePeriodExtractorConfiguration.NextRegex.Match(monthStr).Success)
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
                return ret;
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

        // handle like "2016年到2017年", "2016年和2017年之间"
        private DateTimeResolutionResult ParseYearToYear(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = ChineseDatePeriodExtractorConfiguration.YearToYear.Match(text);

            if (!match.Success)
            {
                match = ChineseDatePeriodExtractorConfiguration.YearToYearSuffixRequired.Match(text);
            }

            if (match.Success)
            {
                var yearMatch = ChineseDatePeriodExtractorConfiguration.YearRegex.Matches(text);
                var yearInChineseMatch = ChineseDatePeriodExtractorConfiguration.YearInChineseRegex.Matches(text);
                var beginYear = 0;
                var endYear = 0;

                if (yearMatch.Count == 2)
                {
                    var yearFrom = yearMatch[0].Groups["year"].Value;
                    var yearTo = yearMatch[1].Groups["year"].Value;
                    beginYear = int.Parse(yearFrom);
                    endYear = int.Parse(yearTo);
                }
                else if (yearInChineseMatch.Count == 2)
                {
                    var yearFrom = yearInChineseMatch[0].Groups["yearchs"].Value;
                    var yearTo = yearInChineseMatch[1].Groups["yearchs"].Value;
                    beginYear = ConvertChineseToInteger(yearFrom);
                    endYear = ConvertChineseToInteger(yearTo);
                }
                else if (yearInChineseMatch.Count == 1 && yearMatch.Count == 1)
                {
                    if (yearMatch[0].Index < yearInChineseMatch[0].Index)
                    {
                        var yearFrom = yearMatch[0].Groups["year"].Value;
                        var yearTo = yearInChineseMatch[0].Groups["yearch"].Value;
                        beginYear = int.Parse(yearFrom);
                        endYear = ConvertChineseToInteger(yearTo);
                    }
                    else
                    {
                        var yearFrom = yearInChineseMatch[0].Groups["yearch"].Value;
                        var yearTo = yearMatch[0].Groups["year"].Value;
                        beginYear = ConvertChineseToInteger(yearFrom);
                        endYear = int.Parse(yearTo);
                    }
                }

                if (beginYear < 100 && beginYear >= this.config.TwoNumYear)
                {
                    beginYear += 1900;
                }
                else if (beginYear < 100 && beginYear < this.config.TwoNumYear)
                {
                    beginYear += 2000;
                }

                if (endYear < 100 && endYear >= this.config.TwoNumYear)
                {
                    endYear += 1900;
                }
                else if (endYear < 100 && endYear < this.config.TwoNumYear)
                {
                    endYear += 2000;
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
            var match = ChineseDatePeriodExtractorConfiguration.MonthToMonth.Match(text);

            if (!match.Success)
            {
                match = ChineseDatePeriodExtractorConfiguration.MonthToMonthSuffixRequired.Match(text);
            }

            if (match.Success)
            {
                var monthMatch = ChineseDatePeriodExtractorConfiguration.MonthRegex.Matches(text);
                var beginMonth = 0;
                var endMonth = 0;

                if (monthMatch.Count == 2)
                {
                    var monthFrom = monthMatch[0].Groups["month"].Value;
                    var monthTo = monthMatch[1].Groups["month"].Value;
                    beginMonth = ToMonthNumber(monthFrom);
                    endMonth = ToMonthNumber(monthTo);
                }

                var currentYear = referenceDate.Year;
                var currentMonth = referenceDate.Month;
                var beginYearForPastResolution = currentYear;
                var endYearForPastResolution = currentYear;
                var beginYearForFutureResolution = currentYear;
                var endYearForFutureResolution = currentYear;
                var durationMonths = 0;

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

                if (durationMonths != 0)
                {
                    var beginDateForPastResolution = DateObject.MinValue.SafeCreateFromValue(beginYearForPastResolution, beginMonth, 1);
                    var endDateForPastResolution = DateObject.MinValue.SafeCreateFromValue(endYearForPastResolution, endMonth, 1);
                    var beginDateForFutureResolution = DateObject.MinValue.SafeCreateFromValue(beginYearForFutureResolution, beginMonth, 1);
                    var endDateForFutureResolution = DateObject.MinValue.SafeCreateFromValue(endYearForFutureResolution, endMonth, 1);

                    var beginTimex = DateTimeFormatUtil.LuisDate(beginDateForPastResolution, beginDateForFutureResolution);
                    var endTimex = DateTimeFormatUtil.LuisDate(endDateForPastResolution, endDateForFutureResolution);
                    ret.Timex = $"({beginTimex},{endTimex},P{durationMonths}M)";
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
            var match = ChineseDatePeriodExtractorConfiguration.YearAndMonth.MatchExact(text, trim: true);

            if (!match.Success)
            {
                match = ChineseDatePeriodExtractorConfiguration.PureNumYearAndMonth.MatchExact(text, trim: true);
            }

            if (!match.Success)
            {
                return ret;
            }

            // parse year
            var year = referenceDate.Year;
            var yearNum = match.Groups["year"].Value;
            var yearChs = match.Groups["yearchs"].Value;
            var yearRel = match.Groups["yearrel"].Value;
            if (!string.IsNullOrEmpty(yearNum))
            {
                if (IsYearOnly(yearNum))
                {
                    yearNum = yearNum.Substring(0, yearNum.Length - 1);
                }

                year = int.Parse(yearNum);
            }
            else if (!string.IsNullOrEmpty(yearChs))
            {
                if (IsYearOnly(yearChs))
                {
                    yearChs = yearChs.Substring(0, yearChs.Length - 1);
                }

                year = ConvertChineseToInteger(yearChs);
            }
            else if (!string.IsNullOrEmpty(yearRel))
            {
                if (IsLastYear(yearRel))
                {
                    year--;
                }
                else if (IsNextYear(yearRel))
                {
                    year++;
                }
            }

            if (year < 100 && year >= this.config.TwoNumYear)
            {
                year += 1900;
            }
            else if (year < this.config.TwoNumYear)
            {
                year += 2000;
            }

            var monthStr = match.Groups["month"].Value;
            var month = ToMonthNumber(monthStr);
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
            var match = ChineseDatePeriodExtractorConfiguration.OneWordPeriodRegex.MatchExact(trimmedText, trim: true);

            if (match.Success)
            {
                var monthStr = match.Groups["month"].Value;
                if (IsThisYear(trimmedText))
                {
                    ret.Timex = TimexUtility.GenerateYearTimex(referenceDate);
                    ret.FutureValue =
                        ret.PastValue =
                            new Tuple<DateObject, DateObject>(DateObject.MinValue.SafeCreateFromValue(referenceDate.Year, 1, 1), referenceDate);
                    ret.Success = true;
                    return ret;
                }

                var thisMatch = ChineseDatePeriodExtractorConfiguration.ThisRegex.Match(trimmedText);
                var nextMatch = ChineseDatePeriodExtractorConfiguration.NextRegex.Match(trimmedText);
                var lastMatch = ChineseDatePeriodExtractorConfiguration.LastRegex.Match(trimmedText);

                if (!string.IsNullOrEmpty(monthStr))
                {
                    var swift = -10;
                    var yearRel = match.Groups["yearrel"].Value;
                    if (!string.IsNullOrEmpty(yearRel))
                    {
                        if (IsNextYear(yearRel))
                        {
                            swift = 1;
                        }
                        else if (IsLastYear(yearRel))
                        {
                            swift = -1;
                        }
                        else if (IsThisYear(yearRel))
                        {
                            swift = 0;
                        }
                    }

                    month = ToMonthNumber(monthStr);

                    if (swift >= -1)
                    {
                        year = year + swift;
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

                    if (IsWeekOnly(trimmedText))
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

                    if (IsWeekend(trimmedText))
                    {
                        var beginDate = referenceDate.This(DayOfWeek.Saturday).AddDays(7 * swift);
                        var endDate = referenceDate.This(DayOfWeek.Sunday).AddDays(7 * swift);

                        ret.Timex = TimexUtility.GenerateWeekendTimex(beginDate);

                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate.AddDays(1));

                        ret.Success = true;

                        return ret;
                    }

                    if (IsMonthOnly(trimmedText))
                    {
                        month = referenceDate.AddMonths(swift).Month;
                        year = referenceDate.AddMonths(swift).Year;
                        ret.Timex = DateTimeFormatUtil.LuisDate(year, month);
                        futureYear = pastYear = year;
                    }
                    else if (IsYearOnly(trimmedText))
                    {
                        // Handle like "今年上半年"，"明年下半年"
                        trimmedText = HandleWithHalfYear(match, trimmedText, out bool hasHalf, out bool isFirstHalf);
                        swift = hasHalf ? 0 : swift;

                        year = referenceDate.AddYears(swift).Year;
                        if (IsLastYear(trimmedText))
                        {
                            year--;
                        }
                        else if (IsNextYear(trimmedText))
                        {
                            year++;
                        }
                        else if (IsYearBeforeLast(trimmedText))
                        {
                            year -= 2;
                        }
                        else if (IsYearAfterNext(trimmedText))
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

            if (IsWeekOnly(text))
            {
                // Handle like "上半周"，"下半周"
                beginDay = swift == -1 ? referenceDate.This(DayOfWeek.Monday) : referenceDate.This(DayOfWeek.Thursday);
                endDay = swift == -1 ? referenceDate.This(DayOfWeek.Thursday) : referenceDate.This(DayOfWeek.Sunday).AddDays(1);
                ret.Timex = TimexUtility.GenerateDatePeriodTimex(beginDay, endDay, DatePeriodTimexType.ByDay);
            }
            else if (IsMonthOnly(text))
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
            var match = ChineseDatePeriodExtractorConfiguration.YearRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var yearStr = match.Value;

                // Handle like "2016年上半年"，"2017年下半年"
                yearStr = HandleWithHalfYear(match, yearStr, out bool hasHalf, out bool isFirstHalf);

                // Trim() to handle extra whitespaces like '07 年'
                if (IsYearOnly(yearStr))
                {
                    yearStr = yearStr.Substring(0, yearStr.Length - 1).Trim();
                }

                var year = int.Parse(yearStr);

                return HandleYearResult(ret, hasHalf, isFirstHalf, year);
            }

            match = ChineseDatePeriodExtractorConfiguration.YearInChineseRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var yearStr = match.Value;

                // Handle like "二零一七年上半年"，"二零一七年下半年"
                yearStr = HandleWithHalfYear(match, yearStr, out bool hasHalf, out bool isFirstHalf);

                if (IsYearOnly(yearStr))
                {
                    yearStr = yearStr.Substring(0, yearStr.Length - 1);
                }

                if (yearStr.Length == 1)
                {
                    return ret;
                }

                var year = ConvertChineseToInteger(yearStr);

                return HandleYearResult(ret, hasHalf, isFirstHalf, year);
            }

            return ret;
        }

        private string HandleWithHalfYear(ConditionalMatch match, string text, out bool hasHalf, out bool isFirstHalf)
        {
            var firstHalf = match.Groups["firstHalf"].Value;
            var secondHalf = match.Groups["secondHalf"].Value;
            hasHalf = false;
            isFirstHalf = !string.IsNullOrEmpty(firstHalf) ? true : false;

            if (isFirstHalf || !string.IsNullOrEmpty(secondHalf))
            {
                var halfText = isFirstHalf ? firstHalf : secondHalf;
                text = text.Substring(0, text.Length - halfText.Length);
                hasHalf = true;
            }

            return text;
        }

        private DateTimeResolutionResult HandleYearResult(DateTimeResolutionResult ret, bool hasHalf, bool isFirstHalf, int year)
        {
            if (year < 100 && year >= this.config.TwoNumYear)
            {
                year += 1900;
            }
            else if (year < 100 && year < this.config.TwoNumYear)
            {
                year += 2000;
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

        // parse entities that made up by two time points
        private DateTimeResolutionResult MergeTwoTimePoints(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var er = SingleDateExtractor.Extract(text, referenceDate);
            if (er.Count < 2)
            {
                er = SingleDateExtractor.Extract("on " + text, referenceDate);
                if (er.Count < 2)
                {
                    return ret;
                }

                er[0].Start -= 3;
                er[1].Start -= 3;
            }

            var pr1 = this.config.DateParser.Parse(er[0], referenceDate);
            var pr2 = this.config.DateParser.Parse(er[1], referenceDate);

            if (er.Count >= 2)
            {
                // @TODO Refactor code to remove the cycle between BaseDatePeriodParser and its config.
                var dateContext = BaseDatePeriodParser.GetYearContext(this.config, er[0].Text, er[1].Text, text);

                if (pr1.Value == null || pr2.Value == null)
                {
                    return ret;
                }

                pr1 = dateContext.ProcessDateEntityParsingResult(pr1);
                pr2 = dateContext.ProcessDateEntityParsingResult(pr2);
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

            ret.Timex = $"({pr1.TimexStr},{pr2.TimexStr},P{(futureEnd - futureBegin).TotalDays}D)";
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
            var match = ChineseDatePeriodExtractorConfiguration.NumberCombinedWithUnit.Match(text);

            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value;
                var beforeStr = text.Substring(0, match.Index);

                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];
                    numStr = match.Groups["num"].Value;

                    if (ChineseDatePeriodExtractorConfiguration.PastRegex.IsExactMatch(beforeStr, trim: true))
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case Constants.TimexDay:
                                beginDate = referenceDate.AddDays(-double.Parse(numStr));
                                endDate = referenceDate;
                                break;
                            case Constants.TimexWeek:
                                beginDate = referenceDate.AddDays(-7 * double.Parse(numStr));
                                endDate = referenceDate;
                                break;
                            case Constants.TimexMonthFull:
                                beginDate = referenceDate.AddMonths(-Convert.ToInt32(double.Parse(numStr)));
                                endDate = referenceDate;
                                break;
                            case Constants.TimexYear:
                                beginDate = referenceDate.AddYears(-Convert.ToInt32(double.Parse(numStr)));
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

                    if (ChineseDatePeriodExtractorConfiguration.FutureRegex.IsExactMatch(beforeStr, trim: true))
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
            var durationRes = DurationExtractor.Extract(text, referenceDate);
            if (durationRes.Count > 0)
            {
                var beforeStr = text.Substring(0, (int)durationRes[0].Start);
                match = ChineseDatePeriodExtractorConfiguration.UnitRegex.Match(durationRes[0].Text);
                if (match.Success)
                {
                    var srcUnit = match.Groups["unit"].Value;

                    var numberStr = durationRes[0].Text.Substring(0, match.Index).Trim();
                    var number = ConvertChineseToNum(numberStr);

                    if (this.config.UnitMap.ContainsKey(srcUnit))
                    {
                        unitStr = this.config.UnitMap[srcUnit];

                        var prefixMatch = ChineseDatePeriodExtractorConfiguration.PastRegex.MatchExact(beforeStr, trim: true);

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

                        prefixMatch = ChineseDatePeriodExtractorConfiguration.FutureRegex.MatchExact(beforeStr, trim: true);

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

            var match = ChineseDatePeriodExtractorConfiguration.WeekOfMonthRegex.Match(text);
            if (!match.Success)
            {
                return ret;
            }

            var cardinalStr = match.Groups["cardinal"].Value;
            var monthStr = match.Groups["month"].Value;
            var noYear = false;
            int year;

            int cardinal;
            if (cardinalStr.Equals("最后一"))
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
                if (trimmedText.StartsWith("下个"))
                {
                    swift = 1;
                }
                else if (trimmedText.StartsWith("上个"))
                {
                    swift = -1;
                }

                month = referenceDate.AddMonths(swift).Month;
                year = referenceDate.AddMonths(swift).Year;
                ret.Timex = DateTimeFormatUtil.LuisDate(referenceDate.Year, month);
            }
            else
            {
                month = ToMonthNumber(monthStr);
                ret.Timex = DateTimeFormatUtil.LuisDate(Constants.InvalidYear, month);
                year = referenceDate.Year;
                noYear = true;
            }

            var value = ComputeDate(cardinal, 1, month, year);

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

            ret.Timex += "-W" + cardinal.ToString("D2");
            ret.FutureValue = new Tuple<DateObject, DateObject>(futureDate, futureDate.AddDays(7));
            ret.PastValue = new Tuple<DateObject, DateObject>(pastDate, pastDate.AddDays(7));
            ret.Success = true;

            return ret;
        }

        // parse "今年夏天"
        private DateTimeResolutionResult ParseSeason(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = ChineseDatePeriodExtractorConfiguration.SeasonWithYear.MatchExact(text, trim: true);

            if (match.Success)
            {
                // parse year
                var year = referenceDate.Year;
                var hasYear = false;
                var yearNum = match.Groups["year"].Value;
                var yearChs = match.Groups["yearchs"].Value;
                var yearRel = match.Groups["yearrel"].Value;

                if (!string.IsNullOrEmpty(yearNum))
                {
                    hasYear = true;
                    if (IsYearOnly(yearNum))
                    {
                        yearNum = yearNum.Substring(0, yearNum.Length - 1);
                    }

                    year = int.Parse(yearNum);
                }
                else if (!string.IsNullOrEmpty(yearChs))
                {
                    hasYear = true;
                    if (IsYearOnly(yearChs))
                    {
                        yearChs = yearChs.Substring(0, yearChs.Length - 1);
                    }

                    year = ConvertChineseToInteger(yearChs);
                }
                else if (!string.IsNullOrEmpty(yearRel))
                {
                    hasYear = true;
                    if (IsLastYear(yearRel))
                    {
                        year--;
                    }
                    else if (IsNextYear(yearRel))
                    {
                        year++;
                    }
                }

                if (year < 100 && year >= this.config.TwoNumYear)
                {
                    year += 1900;
                }
                else if (year < 100 && year < this.config.TwoNumYear)
                {
                    year += 2000;
                }

                // parse season
                var seasonStr = match.Groups["season"].Value;
                ret.Timex = this.config.SeasonMap[seasonStr];
                if (hasYear)
                {
                    ret.Timex = year.ToString("D4") + "-" + ret.Timex;
                }

                ret.Success = true;
                return ret;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseQuarter(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = ChineseDatePeriodExtractorConfiguration.QuarterRegex.MatchExact(text, trim: true);

            if (!match.Success)
            {
                return ret;
            }

            // parse year
            var year = referenceDate.Year;
            var yearNum = match.Groups["year"].Value;
            var yearChs = match.Groups["yearchs"].Value;
            var yearRel = match.Groups["yearrel"].Value;
            if (!string.IsNullOrEmpty(yearNum))
            {
                if (IsYearOnly(yearNum))
                {
                    yearNum = yearNum.Substring(0, yearNum.Length - 1);
                }

                year = int.Parse(yearNum);
            }
            else if (!string.IsNullOrEmpty(yearChs))
            {
                if (IsYearOnly(yearChs))
                {
                    yearChs = yearChs.Substring(0, yearChs.Length - 1);
                }

                year = ConvertChineseToInteger(yearChs);
            }
            else if (!string.IsNullOrEmpty(yearRel))
            {
                if (IsLastYear(yearRel))
                {
                    year--;
                }
                else if (IsNextYear(yearRel))
                {
                    year++;
                }
            }

            if (year < 100 && year >= this.config.TwoNumYear)
            {
                year += 1900;
            }
            else if (year < 100 && year < this.config.TwoNumYear)
            {
                year += 2000;
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

            var match = ChineseDatePeriodExtractorConfiguration.DecadeRegex.MatchExact(text, trim: true);

            string beginLuisStr, endLuisStr;

            if (match.Success)
            {
                var decadeStr = match.Groups["decade"].Value;
                if (!int.TryParse(decadeStr, out decade))
                {
                    decade = ConvertChineseToNum(decadeStr);
                }

                var centuryStr = match.Groups["century"].Value;
                if (!string.IsNullOrEmpty(centuryStr))
                {
                    if (!int.TryParse(centuryStr, out century))
                    {
                        century = ConvertChineseToNum(centuryStr);
                    }

                    inputCentury = true;
                }
                else
                {
                    centuryStr = match.Groups["relcentury"].Value;

                    if (!string.IsNullOrEmpty(centuryStr))
                    {
                        centuryStr = centuryStr.Trim();
                        var thisMatch = ChineseDatePeriodExtractorConfiguration.ThisRegex.Match(centuryStr);
                        var nextMatch = ChineseDatePeriodExtractorConfiguration.NextRegex.Match(centuryStr);
                        var lastMatch = ChineseDatePeriodExtractorConfiguration.LastRegex.Match(centuryStr);

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

                var endYearStr = "XX" + (endYear % 100).ToString("D2");
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

        private int ToMonthNumber(string monthStr)
        {
            return this.config.MonthOfYear[monthStr] > 12 ? this.config.MonthOfYear[monthStr] % 12 : this.config.MonthOfYear[monthStr];
        }

        private bool IsMonthOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.MonthTerms.Any(o => trimmedText.EndsWith(o));
        }

        private bool IsWeekend(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.WeekendTerms.Any(o => trimmedText.EndsWith(o));
        }

        private bool IsWeekOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.WeekTerms.Any(o => trimmedText.EndsWith(o));
        }

        private bool IsYearOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearTerms.Any(o => trimmedText.EndsWith(o));
        }

        private bool IsThisYear(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.ThisYearTerms.Any(o => trimmedText.Equals(o));
        }

        private bool IsLastYear(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.LastYearTerms.Any(o => trimmedText.Equals(o));
        }

        private bool IsNextYear(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.NextYearTerms.Any(o => trimmedText.Equals(o));
        }

        private bool IsYearAfterNext(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearAfterNextTerms.Any(o => trimmedText.Equals(o));
        }

        private bool IsYearBeforeLast(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearBeforeLastTerms.Any(o => trimmedText.Equals(o));
        }
    }
}