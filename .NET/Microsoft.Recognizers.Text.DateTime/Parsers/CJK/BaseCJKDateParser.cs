using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDateParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date";

        public static readonly DateObject NoDate = DateObject.MinValue.SafeCreateFromValue(0, 0, 0);

        private readonly ICJKDateParserConfiguration config;

        public BaseCJKDateParser(ICJKDateParserConfiguration config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public virtual DateTimeParseResult Parse(ExtractResult er, DateObject referenceDate)
        {
            object value = null;

            if (er.Type.Equals(ParserName, StringComparison.Ordinal))
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
                innerResult = ParserDurationWithAgoAndLater(text, reference);
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
            foreach (var regex in this.config.DateRegexList)
            {
                var match = regex.MatchExact(text, trim: true);

                if (match.Success)
                {
                    // Value string will be set in Match2Date method
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
            var match = this.config.SpecialDate.MatchExact(text, trim: true);
            if (match.Success)
            {
                var yearStr = match.Groups["thisyear"].Value;
                var monthStr = match.Groups["thismonth"].Value;
                var dayStr = match.Groups["day"].Value;

                int month = referenceDate.Month, year = referenceDate.Year;
                var day = this.config.DayOfMonth[dayStr];

                bool hasYear = false, hasMonth = false;

                if (!string.IsNullOrEmpty(monthStr))
                {
                    hasMonth = true;
                    if (this.config.NextRe.Match(monthStr).Success)
                    {
                        month++;
                        if (month == Constants.MaxMonth + 1)
                        {
                            month = Constants.MinMonth;
                            year++;
                        }
                    }
                    else if (this.config.LastRe.Match(monthStr).Success)
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
                        if (this.config.NextRe.Match(yearStr).Success)
                        {
                            ++year;
                        }
                        else if (this.config.LastRe.Match(yearStr).Success)
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
                            else if (DateContext.IsFeb29th(year, month - 1, day))
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
            match = this.config.SpecialDayRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var value = referenceDate.AddDays(this.config.GetSwiftDay(match.Value));
                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(value.Year, value.Month, value.Day);
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
                var weekdayKey = match.Groups["weekday"].Value;
                var value = reference.Next((DayOfWeek)this.config.DayOfWeek[weekdayKey]);

                result.Timex = DateTimeFormatUtil.LuisDate(value);
                result.FutureValue = result.PastValue = DateObject.MinValue.SafeCreateFromValue(value.Year, value.Month, value.Day);
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
                var weekdayKey = match.Groups["weekday"].Value;
                var value = reference.This((DayOfWeek)this.config.DayOfWeek[weekdayKey]);

                result.Timex = DateTimeFormatUtil.LuisDate(value);
                result.FutureValue = result.PastValue = DateObject.MinValue.SafeCreateFromValue(value.Year, value.Month, value.Day);
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
                var weekdayKey = match.Groups["weekday"].Value;
                var value = reference.Last((DayOfWeek)this.config.DayOfWeek[weekdayKey]);

                result.Timex = DateTimeFormatUtil.LuisDate(value);
                result.FutureValue = result.PastValue = DateObject.MinValue.SafeCreateFromValue(value.Year, value.Month, value.Day);
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
                var weekdayKey = match.Groups["weekday"].Value;
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

            int cardinal;
            if (cardinalStr.Equals(this.config.LastWeekDayToken, StringComparison.Ordinal))
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
                if (trimmedText.StartsWith(this.config.NextMonthToken, StringComparison.Ordinal))
                {
                    swift = 1;
                }
                else if (trimmedText.StartsWith(this.config.LastMonthToken, StringComparison.Ordinal))
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

            // here is a very special case, timeX follows future date
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

            var monthStr = match.Groups["month"].Value;
            var dayStr = match.Groups["day"].Value;
            var yearStr = match.Groups["year"].Value;
            var yearCJKStr = match.Groups[Constants.YearCJKGroupName].Value;
            int month = 0, day = 0, year = 0;

            var tmp = ConvertCJKYearToInteger(yearCJKStr);
            year = tmp == -1 ? 0 : tmp;

            if (this.config.MonthOfYear.ContainsKey(monthStr) && this.config.DayOfMonth.ContainsKey(dayStr))
            {
                month = this.config.MonthOfYear[monthStr] > 12 ? this.config.MonthOfYear[monthStr] % 12 : this.config.MonthOfYear[monthStr];
                day = this.config.DayOfMonth[dayStr] > 31 ? this.config.DayOfMonth[dayStr] % 31 : this.config.DayOfMonth[dayStr];
                if (!string.IsNullOrEmpty(yearStr))
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

            var futurePastDates = DateContext.GenerateDates(noYear, referenceDate, year, month, day);

            ret.FutureValue = futurePastDates.future;
            ret.PastValue = futurePastDates.past;
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

        // parse if lunar contains
        private bool IsLunarCalendar(string text)
        {
            var trimmedText = text.Trim();
            var match = this.config.LunarRegex.Match(trimmedText);

            return match.Success;
        }

        private int GetMonthMaxDay(int year, int month)
        {
            var maxDay = this.config.MonthMaxDays[month - 1];

            if (!DateObject.IsLeapYear(year) && month == 2)
            {
                maxDay -= 1;
            }

            return maxDay;
        }

        // Handle cases like "三天前"
        private DateTimeResolutionResult ParserDurationWithAgoAndLater(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var numStr = string.Empty;
            var unitStr = string.Empty;

            var durationRes = this.config.DurationExtractor.Extract(text, referenceDate);

            if (durationRes.Count > 0)
            {
                var match = this.config.UnitRegex.Match(text);
                if (match.Success)
                {
                    var suffix = text.Substring((int)durationRes[0].Start + (int)durationRes[0].Length).Trim();
                    var srcUnit = match.Groups["unit"].Value;

                    var numberStr = text.Substring((int)durationRes[0].Start, match.Index - (int)durationRes[0].Start).Trim();
                    var number = ConvertCJKToNum(numberStr);

                    if (this.config.UnitMap.ContainsKey(srcUnit))
                    {
                        unitStr = this.config.UnitMap[srcUnit];

                        var beforeMatch = this.config.BeforeRegex.Match(suffix);
                        if (beforeMatch.Success && suffix.StartsWith(beforeMatch.Value, StringComparison.Ordinal))
                        {
                            DateObject date;
                            switch (unitStr)
                            {
                                case Constants.TimexDay:
                                    date = referenceDate.AddDays(-number);
                                    break;
                                case Constants.TimexWeek:
                                    date = referenceDate.AddDays(-7 * number);
                                    break;
                                case Constants.TimexMonthFull:
                                    date = referenceDate.AddMonths(-number);
                                    break;
                                case Constants.TimexYear:
                                    date = referenceDate.AddYears(-number);
                                    break;
                                default:
                                    return ret;
                            }

                            ret.Timex = $"{DateTimeFormatUtil.LuisDate(date)}";
                            ret.FutureValue = ret.PastValue = date;
                            ret.Success = true;
                            return ret;
                        }

                        var afterMatch = this.config.AfterRegex.Match(suffix);
                        if (afterMatch.Success && suffix.StartsWith(afterMatch.Value, StringComparison.Ordinal))
                        {
                            DateObject date;
                            switch (unitStr)
                            {
                                case Constants.TimexDay:
                                    date = referenceDate.AddDays(number);
                                    break;
                                case Constants.TimexWeek:
                                    date = referenceDate.AddDays(7 * number);
                                    break;
                                case Constants.TimexMonthFull:
                                    date = referenceDate.AddMonths(number);
                                    break;
                                case Constants.TimexYear:
                                    date = referenceDate.AddYears(number);
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

        // Convert CJK Number to Integer
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
        private int ConvertCJKYearToInteger(string yearCJKStr)
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
            }

            year = num;

            return year < 10 ? -1 : year;
        }
    }
}