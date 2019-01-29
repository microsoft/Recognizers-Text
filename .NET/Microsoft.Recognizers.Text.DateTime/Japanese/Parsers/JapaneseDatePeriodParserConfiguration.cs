using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Japanese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDatePeriodParserConfiguration : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATEPERIOD; // "DatePeriod";

        private const int LastMonthOfYear = 12;

        private static readonly IDateTimeExtractor SingleDateExtractor = new JapaneseDateExtractorConfiguration();

        private static readonly IExtractor IntegerExtractor = new IntegerExtractor();

        private static readonly IParser IntegerParser = new BaseCJKNumberParser(new JapaneseNumberParserConfiguration());

        private static readonly IDateTimeExtractor Durationextractor = new JapaneseDurationExtractorConfiguration();

        private static readonly Calendar Cal = DateTimeFormatInfo.InvariantInfo.Calendar;

        private readonly IFullDateTimeParserConfiguration config;

        public JapaneseDatePeriodParserConfiguration(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceDate = refDate;

            object value = null;

            if (er.Type.Equals(ParserName))
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

        // convert Japanese Number to Integer
        private static int ConvertJapaneseToNum(string numStr)
        {
            var num = -1;
            var er = IntegerExtractor.Extract(numStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                {
                    num = Convert.ToInt32((double)(IntegerParser.Parse(er[0]).Value ?? 0));
                }
            }

            return num;
        }

        // convert Japanese Year to Integer
        private static int ConvertJapaneseToInteger(string yearJapStr)
        {
            var year = 0;
            var num = 0;

            var er = IntegerExtractor.Extract(yearJapStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                {
                    num = Convert.ToInt32((double)(IntegerParser.Parse(er[0]).Value ?? 0));
                }
            }

            if (num < 10)
            {
                num = 0;
                foreach (var ch in yearJapStr)
                {
                    num *= 10;
                    er = IntegerExtractor.Extract(ch.ToString());
                    if (er.Count != 0)
                    {
                        if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                        {
                            num += Convert.ToInt32((double)(IntegerParser.Parse(er[0]).Value ?? 0));
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

        private static DateObject ComputeDate(int cadinal, int weekday, int month, int year)
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

            return firstWeekday.AddDays(7 * (cadinal - 1));
        }

        private DateTimeResolutionResult ParseSimpleCases(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int beginDay, endDay;
            var noYear = false;
            var inputYear = false;

            var match = JapaneseDatePeriodExtractorConfiguration.SimpleCasesRegex.MatchExact(text, trim: true);
            string beginLuisStr, endLuisStr;

            if (match.Success)
            {
                var days = match.Groups["day"];
                beginDay = this.config.DayOfMonth[days.Captures[0].Value.ToLower()];
                endDay = this.config.DayOfMonth[days.Captures[1].Value.ToLower()];

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
                    month = ToMonthNumber(monthStr.ToLower());
                }
                else
                {
                    monthStr = match.Groups["relmonth"].Value.Trim().ToLower();
                    var thismatch = JapaneseDatePeriodExtractorConfiguration.ThisRegex.Match(monthStr);
                    var nextmatch = JapaneseDatePeriodExtractorConfiguration.NextRegex.Match(monthStr);
                    var lastmatch = JapaneseDatePeriodExtractorConfiguration.LastRegex.Match(monthStr);

                    if (thismatch.Success)
                    {
                        // do nothing
                    }
                    else if (nextmatch.Success)
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

                if (inputYear || JapaneseDatePeriodExtractorConfiguration.ThisRegex.Match(monthStr).Success ||
                    JapaneseDatePeriodExtractorConfiguration.NextRegex.Match(monthStr).Success)
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

        // handle like "2008年から2012年まで
        private DateTimeResolutionResult ParseYearToYear(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = JapaneseDatePeriodExtractorConfiguration.YearToYear.Match(text);

            if (match.Success)
            {
                var yearMatch = JapaneseDatePeriodExtractorConfiguration.YearRegex.Matches(text);
                var yearInJapaneseMatch = JapaneseDatePeriodExtractorConfiguration.YearInJapaneseRegex.Matches(text);
                var beginYear = 0;
                var endYear = 0;

                if (yearMatch.Count == 2)
                {
                    var yearFrom = yearMatch[0].Groups["year"].Value;
                    var yearTo = yearMatch[1].Groups["year"].Value;
                    beginYear = int.Parse(yearFrom);
                    endYear = int.Parse(yearTo);
                }
                else if (yearInJapaneseMatch.Count == 2)
                {
                    var yearFrom = yearInJapaneseMatch[0].Groups["yearJap"].Value;
                    var yearTo = yearInJapaneseMatch[1].Groups["yearJap"].Value;
                    beginYear = ConvertJapaneseToInteger(yearFrom);
                    endYear = ConvertJapaneseToInteger(yearTo);
                }
                else if (yearInJapaneseMatch.Count == 1 && yearMatch.Count == 1)
                {
                    if (yearMatch[0].Index < yearInJapaneseMatch[0].Index)
                    {
                        var yearFrom = yearMatch[0].Groups["year"].Value;
                        var yearTo = yearInJapaneseMatch[0].Groups["yearch"].Value;
                        beginYear = int.Parse(yearFrom);
                        endYear = ConvertJapaneseToInteger(yearTo);
                    }
                    else
                    {
                        var yearFrom = yearInJapaneseMatch[0].Groups["yearch"].Value;
                        var yearTo = yearMatch[0].Groups["year"].Value;
                        beginYear = ConvertJapaneseToInteger(yearFrom);
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

                var beginDate = DateObject.MinValue.SafeCreateFromValue(beginYear, 1, 1);
                var endDate = DateObject.MinValue.SafeCreateFromValue(endYear, 1, 1);
                var beginTimex = DateTimeFormatUtil.LuisDate(beginYear, 1, 1);
                var endTimex = DateTimeFormatUtil.LuisDate(endYear, 1, 1);
                ret.Timex = $"({beginTimex},{endTimex},P{endYear - beginYear}Y)";
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        // handle like "08月から12月まで
        private DateTimeResolutionResult ParseMonthToMonth(string text, DateObject referenceDate)
        {
            int undefinedValue = -1;

            var ret = new DateTimeResolutionResult();
            var match = JapaneseDatePeriodExtractorConfiguration.MonthToMonth.Match(text);

            if (match.Success)
            {
                var monthMatch = JapaneseDatePeriodExtractorConfiguration.MonthRegex.Matches(text);
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

                    var beginTimex = DateTimeFormatUtil.LuisDate(undefinedValue, beginMonth, 1);
                    var endTimex = DateTimeFormatUtil.LuisDate(undefinedValue, endMonth, 1);
                    ret.Timex = $"({beginTimex},{endTimex},P{durationMonths}M)";
                    ret.PastValue = new Tuple<DateObject, DateObject>(beginDateForPastResolution, endDateForPastResolution);
                    ret.FutureValue = new Tuple<DateObject, DateObject>(beginDateForFutureResolution, endDateForFutureResolution);
                    ret.Success = true;
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParseDayToDay(string text, DateObject referenceDate)
        {
            int undefinedValue = -1;
            var ret = new DateTimeResolutionResult();
            var match = JapaneseDatePeriodExtractorConfiguration.DayToDay.Match(text);

            if (match.Success)
            {
                var dayMatchMatch = JapaneseDatePeriodExtractorConfiguration.DayRegexForPeriod.Matches(text);
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
                            beginMonthForPastResolution = endMonthForPastResolution = LastMonthOfYear;
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
                        if (currentMonth == LastMonthOfYear)
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
                        if (currentMonth == LastMonthOfYear)
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
                        if (currentMonth == LastMonthOfYear)
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

        // case like "今年三月" "这个周末" "五月"
        private DateTimeResolutionResult ParseOneWordPeriod(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int futureYear = year, pastYear = year;

            var trimmedText = text.Trim().ToLower();
            var match = JapaneseDatePeriodExtractorConfiguration.OneWordPeriodRegex.MatchExact(trimmedText, trim: true);

            if (match.Success)
            {
                var monthStr = match.Groups["month"].Value;
                if (trimmedText.Equals("今年"))
                {
                    ret.Timex = referenceDate.Year.ToString("D4");
                    ret.FutureValue =
                        ret.PastValue =
                            new Tuple<DateObject, DateObject>(DateObject.MinValue.SafeCreateFromValue(referenceDate.Year, 1, 1), referenceDate);
                    ret.Success = true;
                    return ret;
                }

                var thismatch = JapaneseDatePeriodExtractorConfiguration.ThisRegex.Match(trimmedText);
                var nextmatch = JapaneseDatePeriodExtractorConfiguration.NextRegex.Match(trimmedText);
                var lastmatch = JapaneseDatePeriodExtractorConfiguration.LastRegex.Match(trimmedText);

                if (!string.IsNullOrEmpty(monthStr))
                {
                    var swift = -10;

                    if (trimmedText.StartsWith("来年") || trimmedText.StartsWith("先年"))
                    {
                        swift = 1;
                    }
                    else if (trimmedText.StartsWith("前年"))
                    {
                        swift = -1;
                    }
                    else if (trimmedText.StartsWith("今年"))
                    {
                        swift = 0;
                    }

                    month = ToMonthNumber(monthStr.ToLower());

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
                    var swift = 0;
                    if (nextmatch.Success)
                    {
                        swift = 1;
                    }
                    else if (lastmatch.Success)
                    {
                        swift = -1;
                    }

                    if (trimmedText.EndsWith("周") | trimmedText.EndsWith("星期"))
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

                    if (trimmedText.EndsWith("周末"))
                    {
                        var beginDate = referenceDate.This(DayOfWeek.Saturday).AddDays(7 * swift);
                        var endDate = referenceDate.This(DayOfWeek.Sunday).AddDays(7 * swift);

                        ret.Timex = beginDate.Year.ToString("D4") + "-W" +
                                    Cal.GetWeekOfYear(beginDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
                                        .ToString("D2") + "-WE";

                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate.AddDays(1));

                        ret.Success = true;

                        return ret;
                    }

                    if (trimmedText.EndsWith("月"))
                    {
                        month = referenceDate.AddMonths(swift).Month;
                        year = referenceDate.AddMonths(swift).Year;
                        ret.Timex = year.ToString("D4") + "-" + month.ToString("D2");
                        futureYear = pastYear = year;
                    }
                    else if (trimmedText.EndsWith("年"))
                    {
                        year = referenceDate.AddYears(swift).Year;
                        if (trimmedText.EndsWith("前年") || trimmedText.EndsWith("先年"))
                        {
                            year--;
                        }
                        else if (trimmedText.EndsWith("来年"))
                        {
                            year++;
                        }
                        else if (trimmedText.EndsWith("前年"))
                        {
                            year -= 2;
                        }
                        else if (trimmedText.EndsWith("后年"))
                        {
                            year += 2;
                        }

                        ret.Timex = year.ToString("D4");
                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(
                                    DateObject.MinValue.SafeCreateFromValue(year, 1, 1),
                                    DateObject.MinValue.SafeCreateFromValue(year, 12, 31).AddDays(1));
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
                DateObject.MinValue.SafeCreateFromValue(futureYear, month, 1),
                DateObject.MinValue.SafeCreateFromValue(futureYear, month, 1).AddMonths(1));

            ret.PastValue = new Tuple<DateObject, DateObject>(
                DateObject.MinValue.SafeCreateFromValue(pastYear, month, 1),
                DateObject.MinValue.SafeCreateFromValue(pastYear, month, 1).AddMonths(1));

            ret.Success = true;

            return ret;
        }

        // only contains year like "2016年"
        private DateTimeResolutionResult ParseYear(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var match = JapaneseDatePeriodExtractorConfiguration.YearRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var tmp = match.Value;

                // Trim() to handle extra whitespaces like '07 年'
                if (tmp.EndsWith("年"))
                {
                    tmp = tmp.Substring(0, tmp.Length - 1).Trim();
                }

                var num = 0;
                var year = 0;
                if (tmp.Length == 2)
                {
                    num = int.Parse(tmp);
                    if (num < 100 && num >= 30)
                    {
                        num += 1900;
                    }
                    else if (num < 30)
                    {
                        num += 2000;
                    }

                    year = num;
                }
                else
                {
                    year = int.Parse(tmp);
                }

                var beginDay = DateObject.MinValue.SafeCreateFromValue(year, 1, 1);
                var endDay = DateObject.MinValue.SafeCreateFromValue(year + 1, 1, 1);

                ret.Timex = year.ToString("D4");
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
                ret.Success = true;

                return ret;
            }

            match = JapaneseDatePeriodExtractorConfiguration.YearInJapaneseRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var tmp = match.Value;
                if (tmp.EndsWith("年"))
                {
                    tmp = tmp.Substring(0, tmp.Length - 1);
                }

                if (tmp.Length == 1)
                {
                    return ret;
                }

                var re = ConvertJapaneseToInteger(tmp);
                var year = re;

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
            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
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

            if ((JapaneseDatePeriodExtractorConfiguration.YearAndMonth.IsMatch(pr1.Text) && JapaneseDatePeriodExtractorConfiguration.YearAndMonth.IsMatch(pr2.Text)) ||
                (JapaneseDatePeriodExtractorConfiguration.SimpleYearAndMonth.IsMatch(pr1.Text) && JapaneseDatePeriodExtractorConfiguration.SimpleYearAndMonth.IsMatch(pr2.Text)))
            {
                ret.Timex = $"({pr1.TimexStr},{pr2.TimexStr},P{(int)(futureEnd - futureBegin).TotalDays / 30}M)";
            }
            else
            {
                ret.Timex = $"({pr1.TimexStr},{pr2.TimexStr},P{(futureEnd - futureBegin).TotalDays}D)";
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
            var match = JapaneseDatePeriodExtractorConfiguration.NumberCombinedWithUnit.Match(text);
            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value.ToLowerInvariant();
                var beforeStr = text.Substring(0, match.Index).ToLowerInvariant();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];
                    numStr = match.Groups["num"].Value;

                    var prefixMatch = JapaneseDatePeriodExtractorConfiguration.PastRegex.MatchExact(beforeStr, trim: true);

                    if (prefixMatch.Success)
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

                    prefixMatch = JapaneseDatePeriodExtractorConfiguration.FutureRegex.MatchExact(beforeStr, trim: true);

                    if (prefixMatch.Success)
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
            var durationRes = Durationextractor.Extract(text, referenceDate);
            if (durationRes.Count > 0)
            {
                var beforeStr = text.Substring(0, (int)durationRes[0].Start).ToLowerInvariant();
                match = JapaneseDatePeriodExtractorConfiguration.UnitRegex.Match(durationRes[0].Text);
                if (match.Success)
                {
                    var srcUnit = match.Groups["unit"].Value.ToLowerInvariant();
                    var numberStr = durationRes[0].Text.Substring(0, match.Index).Trim().ToLowerInvariant();
                    var number = ConvertJapaneseToNum(numberStr);
                    if (this.config.UnitMap.ContainsKey(srcUnit))
                    {
                        unitStr = this.config.UnitMap[srcUnit];
                        numStr = number.ToString();
                        var prefixMatch = JapaneseDatePeriodExtractorConfiguration.PastRegex.MatchExact(beforeStr, trim: true);

                        if (prefixMatch.Success)
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

                        prefixMatch = JapaneseDatePeriodExtractorConfiguration.FutureRegex.MatchExact(beforeStr, trim: true);

                        if (prefixMatch.Success)
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
            var trimmedText = text.Trim().ToLowerInvariant();
            var match = JapaneseDatePeriodExtractorConfiguration.WeekOfMonthRegex.Match(text);
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
                ret.Timex = referenceDate.Year.ToString("D4") + "-" + month.ToString("D2");
            }
            else
            {
                month = ToMonthNumber(monthStr);
                ret.Timex = "XXXX" + "-" + month.ToString("D2");
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
            var match = JapaneseDatePeriodExtractorConfiguration.SeasonWithYear.MatchExact(text, trim: true);

            if (match.Success)
            {
                // parse year
                var year = referenceDate.Year;
                var hasYear = false;
                var yearNum = match.Groups["year"].Value;
                var yearJap = match.Groups["yearJap"].Value;
                var yearRel = match.Groups["yearrel"].Value;

                if (!string.IsNullOrEmpty(yearNum))
                {
                    hasYear = true;
                    if (yearNum.EndsWith("年"))
                    {
                        yearNum = yearNum.Substring(0, yearNum.Length - 1);
                    }

                    year = int.Parse(yearNum);
                }
                else if (!string.IsNullOrEmpty(yearJap))
                {
                    hasYear = true;
                    if (yearJap.EndsWith("年"))
                    {
                        yearJap = yearJap.Substring(0, yearJap.Length - 1);
                    }

                    year = ConvertJapaneseToInteger(yearJap);
                }
                else if (!string.IsNullOrEmpty(yearRel))
                {
                    hasYear = true;
                    if (yearRel.EndsWith("前年") || yearRel.EndsWith("先年"))
                    {
                        year--;
                    }
                    else if (yearRel.EndsWith("来年"))
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
            var match = JapaneseDatePeriodExtractorConfiguration.QuarterRegex.MatchExact(text, trim: true);

            if (!match.Success)
            {
                return ret;
            }

            // pare year
            var year = referenceDate.Year;
            var yearNum = match.Groups["year"].Value;
            var yearJap = match.Groups["yearJap"].Value;
            var yearRel = match.Groups["yearrel"].Value;
            if (!string.IsNullOrEmpty(yearNum))
            {
                if (yearNum.EndsWith("年"))
                {
                    yearNum = yearNum.Substring(0, yearNum.Length - 1);
                }

                year = int.Parse(yearNum);
            }
            else if (!string.IsNullOrEmpty(yearJap))
            {
                if (yearJap.EndsWith("年"))
                {
                    yearJap = yearJap.Substring(0, yearJap.Length - 1);
                }

                year = ConvertJapaneseToInteger(yearJap);
            }
            else if (!string.IsNullOrEmpty(yearRel))
            {
                if (yearRel.EndsWith("前年") || yearRel.EndsWith("先年"))
                {
                    year--;
                }
                else if (yearRel.EndsWith("来年"))
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
            ret.Timex = $"({DateTimeFormatUtil.LuisDate(beginDate)},{DateTimeFormatUtil.LuisDate(endDate)},P3M)";
            ret.Success = true;

            return ret;
        }

        private DateTimeResolutionResult ParseDecade(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            int century = (referenceDate.Year / 100) + 1;
            int decade;
            int beginYear, endYear;
            int decadeLastYear = 10;
            var inputCentury = false;

            var match = JapaneseDatePeriodExtractorConfiguration.DecadeRegex.MatchExact(text, trim: true);
            string beginLuisStr, endLuisStr;

            if (match.Success)
            {
                var decadeStr = match.Groups["decade"].Value;
                if (!int.TryParse(decadeStr, out decade))
                {
                    decade = ConvertJapaneseToNum(decadeStr);
                }

                var centuryStr = match.Groups["century"].Value;
                if (!string.IsNullOrEmpty(centuryStr))
                {
                    if (!int.TryParse(centuryStr, out century))
                    {
                        century = ConvertJapaneseToNum(centuryStr);
                    }

                    inputCentury = true;
                }
                else
                {
                    centuryStr = match.Groups["relcentury"].Value;

                    if (!string.IsNullOrEmpty(centuryStr))
                    {
                        centuryStr = centuryStr.Trim().ToLower();
                        var thismatch = JapaneseDatePeriodExtractorConfiguration.ThisRegex.Match(centuryStr);
                        var nextmatch = JapaneseDatePeriodExtractorConfiguration.NextRegex.Match(centuryStr);
                        var lastmatch = JapaneseDatePeriodExtractorConfiguration.LastRegex.Match(centuryStr);

                        if (thismatch.Success)
                        {
                            // do nothing
                        }
                        else if (nextmatch.Success)
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

            beginYear = ((century - 1) * 100) + decade;
            endYear = beginYear + decadeLastYear;

            if (inputCentury)
            {
                beginLuisStr = DateTimeFormatUtil.LuisDate(beginYear, 1, 1);
                endLuisStr = DateTimeFormatUtil.LuisDate(endYear, 1, 1);
            }
            else
            {
                var beginYearStr = "XX" + decade.ToString();
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
    }
}