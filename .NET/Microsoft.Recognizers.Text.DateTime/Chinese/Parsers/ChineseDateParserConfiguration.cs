using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Chinese;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDateParserConfiguration : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date";

        private static readonly int[] MonthMaxDays = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private readonly ChineseDateTimeParserConfiguration config;

        private readonly IExtractor integerExtractor;
        private readonly IExtractor ordinalExtractor;
        private readonly IParser numberParser;
        private readonly IDateTimeExtractor durationExtractor;

        public ChineseDateParserConfiguration(ChineseDateTimeParserConfiguration configuration)
        {
            config = configuration;
            integerExtractor = new IntegerExtractor();
            ordinalExtractor = new OrdinalExtractor();
            durationExtractor = new ChineseDurationExtractorConfiguration();
            numberParser = new BaseCJKNumberParser(new ChineseNumberParserConfiguration());
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public virtual DateTimeParseResult Parse(ExtractResult er, DateObject referenceDate)
        {
            object value = null;

            if (er.Type.Equals(ParserName))
            {
                value = InnerParser(er.Text, referenceDate);
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

        protected DateTimeResolutionResult InnerParser(string text, DateObject reference)
        {
            var innerResult = ParseBasicRegexMatch(text, reference);

            if (!innerResult.Success)
            {
                innerResult = ParseImplicitDate(text, reference);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseWeekdayOfMonth(text, reference);
            }

            if (!innerResult.Success)
            {
                innerResult = ParserDurationWithBeforeAndAfter(text, reference);
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

                innerResult.IsLunar = IsLunarCalendar(text);

                return innerResult;
            }

            return null;
        }

        // parse basic patterns in DateRegexList
        protected DateTimeResolutionResult ParseBasicRegexMatch(string text, DateObject referenceDate)
        {
            foreach (var regex in ChineseDateExtractorConfiguration.DateRegexList)
            {
                var match = regex.MatchExact(text, trim: true);

                if (match.Success)
                {
                    // LUIS value string will be set in Match2Date method
                    var ret = Match2Date(match.Match, referenceDate);
                    return ret;
                }
            }

            return new DateTimeResolutionResult();
        }

        // match several other cases
        // including '今天', '后天', '十三日'
        protected DateTimeResolutionResult ParseImplicitDate(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            // handle "十二日" "明年这个月三日" "本月十一日"
            var match = ChineseDateExtractorConfiguration.SpecialDate.MatchExact(text, trim: true);
            if (match.Success)
            {
                var yearStr = match.Groups["thisyear"].Value.ToLower();
                var monthStr = match.Groups["thismonth"].Value.ToLower();
                var dayStr = match.Groups["day"].Value.ToLower();

                int month = referenceDate.Month, year = referenceDate.Year;
                var day = this.config.DayOfMonth[dayStr];

                bool hasYear = false, hasMonth = false;

                if (!string.IsNullOrEmpty(monthStr))
                {
                    hasMonth = true;
                    if (ChineseDateExtractorConfiguration.NextRe.Match(monthStr).Success)
                    {
                        month++;
                        if (month == Constants.MaxMonth + 1)
                        {
                            month = Constants.MinMonth;
                            year++;
                        }
                    }
                    else if (ChineseDateExtractorConfiguration.LastRe.Match(monthStr).Success)
                    {
                        month--;
                        if (month == Constants.MinMonth - 1)
                        {
                            month = Constants.MaxMonth;
                            year--;
                        }
                    }

                    if (!string.IsNullOrEmpty(yearStr))
                    {
                        hasYear = true;
                        if (ChineseDateExtractorConfiguration.NextRe.Match(yearStr).Success)
                        {
                            ++year;
                        }
                        else if (ChineseDateExtractorConfiguration.LastRe.Match(yearStr).Success)
                        {
                            --year;
                        }
                    }
                }

                ret.Timex = DateTimeFormatUtil.LuisDate(hasYear ? year : -1, hasMonth ? month : -1, day);

                DateObject futureDate, pastDate;

                if (day > GetMonthMaxDay(year, month))
                {
                    var futureMonth = month + 1;
                    var pastMonth = month - 1;
                    var futureYear = year;
                    var pastYear = year;

                    if (futureMonth == Constants.MaxMonth + 1)
                    {
                        futureMonth = Constants.MinMonth;
                        futureYear = year++;
                    }

                    if (pastMonth == Constants.MinMonth - 1)
                    {
                        pastMonth = Constants.MaxMonth;
                        pastYear = year--;
                    }

                    var isFutureValid = DateObjectExtension.IsValidDate(futureYear, futureMonth, day);
                    var isPastValid = DateObjectExtension.IsValidDate(pastYear, pastMonth, day);

                    if (isFutureValid && isPastValid)
                    {
                        futureDate = DateObject.MinValue.SafeCreateFromValue(futureYear, futureMonth, day);
                        pastDate = DateObject.MinValue.SafeCreateFromValue(pastYear, pastMonth, day);
                    }
                    else if (isFutureValid && !isPastValid)
                    {
                        futureDate = pastDate = DateObject.MinValue.SafeCreateFromValue(futureYear, futureMonth, day);
                    }
                    else if (!isFutureValid && !isPastValid)
                    {
                        futureDate = pastDate = DateObject.MinValue.SafeCreateFromValue(pastYear, pastMonth, day);
                    }
                    else
                    {
                        // Fall back to normal cases, might lead to resolution failure
                        // TODO: Ideally, this failure should be filtered out in extract phase
                        futureDate = pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
                    }
                }
                else
                {
                    futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
                    pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

                    if (!hasMonth)
                    {
                        if (futureDate < referenceDate)
                        {
                            if (IsValidDate(year, month + 1, day))
                            {
                                futureDate = futureDate.AddMonths(1);
                            }
                        }

                        if (pastDate >= referenceDate)
                        {
                            if (IsValidDate(year, month - 1, day))
                            {
                                pastDate = pastDate.AddMonths(-1);
                            }
                            else if (IsNonleapYearFeb29th(year, month - 1, day))
                            {
                                pastDate = pastDate.AddMonths(-2);
                            }
                        }
                    }
                    else if (!hasYear)
                    {
                        if (futureDate < referenceDate)
                        {
                            if (IsValidDate(year + 1, month, day))
                            {
                                futureDate = futureDate.AddYears(1);
                            }
                        }

                        if (pastDate >= referenceDate)
                        {
                            if (IsValidDate(year - 1, month, day))
                            {
                                pastDate = pastDate.AddYears(-1);
                            }
                        }
                    }
                }

                ret.FutureValue = futureDate;
                ret.PastValue = pastDate;
                ret.Success = true;

                return ret;
            }

            // handle cases like "昨日", "明日", "大后天"
            match = ChineseDateExtractorConfiguration.SpecialDayRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var value = referenceDate.AddDays(ChineseDateTimeParserConfiguration.GetSwiftDay(match.Value.ToLower()));
                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            if (!ret.Success)
            {
                ret = MatchThisWeekday(text, referenceDate);
            }

            if (!ret.Success)
            {
                ret = MatchNextWeekday(text, referenceDate);
            }

            if (!ret.Success)
            {
                ret = MatchLastWeekday(text, referenceDate);
            }

            if (!ret.Success)
            {
                ret = MatchWeekdayAlone(text, referenceDate);
            }

            return ret;
        }

        protected DateTimeResolutionResult MatchNextWeekday(string text, DateObject reference)
        {
            var result = new DateTimeResolutionResult();
            var match = this.config.NextRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var weekdayKey = match.Groups["weekday"].Value.ToLowerInvariant();
                var value = reference.Next((DayOfWeek)this.config.DayOfWeek[weekdayKey]);

                result.Timex = DateTimeFormatUtil.LuisDate(value);
                result.FutureValue = result.PastValue = value;
                result.Success = true;
            }

            return result;
        }

        protected DateTimeResolutionResult MatchThisWeekday(string text, DateObject reference)
        {
            var result = new DateTimeResolutionResult();
            var match = this.config.ThisRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var weekdayKey = match.Groups["weekday"].Value.ToLowerInvariant();
                var value = reference.This((DayOfWeek)this.config.DayOfWeek[weekdayKey]);

                result.Timex = DateTimeFormatUtil.LuisDate(value);
                result.FutureValue = result.PastValue = value;
                result.Success = true;
            }

            return result;
        }

        protected DateTimeResolutionResult MatchLastWeekday(string text, DateObject reference)
        {
            var result = new DateTimeResolutionResult();
            var match = this.config.LastRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var weekdayKey = match.Groups["weekday"].Value.ToLowerInvariant();
                var value = reference.Last((DayOfWeek)this.config.DayOfWeek[weekdayKey]);

                result.Timex = DateTimeFormatUtil.LuisDate(value);
                result.FutureValue = result.PastValue = value;
                result.Success = true;
            }

            return result;
        }

        protected DateTimeResolutionResult MatchWeekdayAlone(string text, DateObject reference)
        {
            var result = new DateTimeResolutionResult();
            var match = this.config.StrictWeekDayRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var weekdayKey = match.Groups["weekday"].Value.ToLower();
                var weekday = this.config.DayOfWeek[weekdayKey];
                var value = reference.This((DayOfWeek)weekday);

                if (weekday == 0)
                {
                    weekday = 7;
                }

                if (weekday < (int)reference.DayOfWeek)
                {
                    value = reference.Next((DayOfWeek)weekday);
                }

                result.Timex = "XXXX-WXX-" + weekday;
                var futureDate = value;
                var pastDate = value;
                if (futureDate < reference)
                {
                    futureDate = futureDate.AddDays(7);
                }

                if (pastDate >= reference)
                {
                    pastDate = pastDate.AddDays(-7);
                }

                result.FutureValue = futureDate;
                result.PastValue = pastDate;
                result.Success = true;
            }

            return result;
        }

        protected virtual DateTimeResolutionResult ParseWeekdayOfMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimmedText = text.Trim().ToLowerInvariant();
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

            int cardinal;
            if (cardinalStr.Equals(this.config.LastWeekDayToken))
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
                var swift = 0;
                if (trimmedText.StartsWith(this.config.NextMonthToken))
                {
                    swift = 1;
                }
                else if (trimmedText.StartsWith(this.config.LastMonthToken))
                {
                    swift = -1;
                }

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
            ret.Timex = $@"XXXX-{month:D2}-WXX-{weekday}-#{cardinal}";
            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        // parse a regex match which includes 'day', 'month' and 'year' (optional) group
        protected DateTimeResolutionResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var monthStr = match.Groups["month"].Value.ToLower();
            var dayStr = match.Groups["day"].Value.ToLower();
            var yearStr = match.Groups["year"].Value.ToLower();
            var yearChsStr = match.Groups["yearchs"].Value.ToLower();
            int month = 0, day = 0, year = 0;

            var tmp = ConvertChineseYearToInteger(yearChsStr);
            year = tmp == -1 ? 0 : tmp;

            if (this.config.MonthOfYear.ContainsKey(monthStr) && this.config.DayOfMonth.ContainsKey(dayStr))
            {
                month = this.config.MonthOfYear[monthStr] > 12 ? this.config.MonthOfYear[monthStr] % 12 : this.config.MonthOfYear[monthStr];
                day = this.config.DayOfMonth[dayStr] > 31 ? this.config.DayOfMonth[dayStr] % 31 : this.config.DayOfMonth[dayStr];
                if (!string.IsNullOrEmpty(yearStr))
                {
                    year = int.Parse(yearStr);
                    if (year < 100 && year >= Constants.MinTwoDigitYearPastNum)
                    {
                        year += 1900;
                    }
                    else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum)
                    {
                        year += 2000;
                    }
                }
            }

            var noYear = false;
            if (year == 0)
            {
                year = referenceDate.Year;
                ret.Timex = DateTimeFormatUtil.LuisDate(-1, month, day);
                noYear = true;
            }
            else
            {
                ret.Timex = DateTimeFormatUtil.LuisDate(year, month, day);
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

        // parse if lunar contains
        private static bool IsLunarCalendar(string text)
        {
            var trimmedText = text.Trim();
            var match = ChineseDateExtractorConfiguration.LunarRegex.Match(trimmedText);

            return match.Success;
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

        private static int GetMonthMaxDay(int year, int month)
        {
            var maxDay = MonthMaxDays[month - 1];

            if (!DateObject.IsLeapYear(year) && month == 2)
            {
                maxDay -= 1;
            }

            return maxDay;
        }

        // Judge if a date is valid
        private static bool IsValidDate(int year, int month, int day)
        {
            if (month < Constants.MinMonth)
            {
                year--;
                month = Constants.MaxMonth;
            }

            if (month > Constants.MaxMonth)
            {
                year++;
                month = Constants.MinMonth;
            }

            return DateObjectExtension.IsValidDate(year, month, day);
        }

        // Judge the date is non-leap year Feb 29th
        private static bool IsNonleapYearFeb29th(int year, int month, int day)
        {
            return !DateObject.IsLeapYear(year) && month == 2 && day == 29;
        }

        // Handle cases like "三天前"
        private DateTimeResolutionResult ParserDurationWithBeforeAndAfter(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var durationRes = durationExtractor.Extract(text, referenceDate);
            var numStr = string.Empty;
            var unitStr = string.Empty;
            if (durationRes.Count > 0)
            {
                var match = ChineseDateExtractorConfiguration.UnitRegex.Match(text);
                if (match.Success)
                {
                    var suffix =
                        text.Substring((int)durationRes[0].Start + (int)durationRes[0].Length)
                            .Trim()
                            .ToLowerInvariant();
                    var srcUnit = match.Groups["unit"].Value.ToLowerInvariant();
                    var numberStr =
                        text.Substring((int)durationRes[0].Start, match.Index - (int)durationRes[0].Start)
                            .Trim()
                            .ToLowerInvariant();
                    var number = ConvertChineseToNum(numberStr);
                    if (this.config.UnitMap.ContainsKey(srcUnit))
                    {
                        unitStr = this.config.UnitMap[srcUnit];
                        numStr = number.ToString();

                        var beforeMatch = ChineseDateExtractorConfiguration.BeforeRegex.Match(suffix);
                        if (beforeMatch.Success && suffix.StartsWith(beforeMatch.Value))
                        {
                            DateObject date;
                            switch (unitStr)
                            {
                                case Constants.TimexDay:
                                    date = referenceDate.AddDays(-double.Parse(numStr));
                                    break;
                                case Constants.TimexWeek:
                                    date = referenceDate.AddDays(-7 * double.Parse(numStr));
                                    break;
                                case Constants.TimexMonthFull:
                                    date = referenceDate.AddMonths(-Convert.ToInt32(double.Parse(numStr)));
                                    break;
                                case Constants.TimexYear:
                                    date = referenceDate.AddYears(-Convert.ToInt32(double.Parse(numStr)));
                                    break;
                                default:
                                    return ret;
                            }

                            ret.Timex = $"{DateTimeFormatUtil.LuisDate(date)}";
                            ret.FutureValue = ret.PastValue = date;
                            ret.Success = true;
                            return ret;
                        }

                        var afterMatch = ChineseDateExtractorConfiguration.AfterRegex.Match(suffix);
                        if (afterMatch.Success && suffix.StartsWith(afterMatch.Value))
                        {
                            DateObject date;
                            switch (unitStr)
                            {
                                case Constants.TimexDay:
                                    date = referenceDate.AddDays(double.Parse(numStr));
                                    break;
                                case Constants.TimexWeek:
                                    date = referenceDate.AddDays(7 * double.Parse(numStr));
                                    break;
                                case Constants.TimexMonthFull:
                                    date = referenceDate.AddMonths(Convert.ToInt32(double.Parse(numStr)));
                                    break;
                                case Constants.TimexYear:
                                    date = referenceDate.AddYears(Convert.ToInt32(double.Parse(numStr)));
                                    break;
                                default:
                                    return ret;
                            }

                            ret.Timex = $"{DateTimeFormatUtil.LuisDate(date)}";
                            ret.FutureValue = ret.PastValue = date;
                            ret.Success = true;
                            return ret;
                        }
                    }
                }
            }

            return ret;
        }

        // Convert Chinese Number to Integer
        private int ConvertChineseToNum(string numStr)
        {
            var num = -1;
            var er = integerExtractor.Extract(numStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                {
                    num = Convert.ToInt32((double)(numberParser.Parse(er[0]).Value ?? 0));
                }
            }

            return num;
        }

        // convert Chinese Year to Integer
        private int ConvertChineseYearToInteger(string yearChsStr)
        {
            var year = 0;
            var num = 0;

            var er = integerExtractor.Extract(yearChsStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                {
                    num = Convert.ToInt32((double)(numberParser.Parse(er[0]).Value ?? 0));
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
                        if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                        {
                            num += Convert.ToInt32((double)(numberParser.Parse(er[0]).Value ?? 0));
                        }
                    }
                }
            }

            year = num;

            return year < 10 ? -1 : year;
        }
    }
}
