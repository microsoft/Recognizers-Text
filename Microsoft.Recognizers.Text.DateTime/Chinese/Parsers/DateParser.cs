using Microsoft.Recognizers.Text.DateTime.Chinese.Extractors;
using Microsoft.Recognizers.Text.DateTime.Parsers;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number.Chinese.Extractors;
using Microsoft.Recognizers.Text.Number.Chinese.Parsers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Parsers
{
    public class DateParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; //"Date";

        private readonly IFullDateTimeParserConfiguration config;
        private IExtractor integerExtractor;
        private IExtractor ordinalExtractor;
        private IParser numberParser;

        private IExtractor durationExtractor;

        public DateParser(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
            integerExtractor = new IntegerExtractor();
            ordinalExtractor = new OrdinalExtractor();
            durationExtractor = new DurationExtractorChs();
            numberParser = new ChineseNumberParser(new ChineseNumberParserConfiguration());
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
                TimexStr = value == null ? "" : ((DTParseResult)value).Timex,
                ResolutionStr = ""
            };
            return ret;
        }

        protected DTParseResult InnerParser(string text, DateObject reference)
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
                        {TimeTypeConstants.DATE, Util.FormatDate((DateObject) innerResult.FutureValue)}
                    };
                innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATE, Util.FormatDate((DateObject) innerResult.PastValue)}
                    };
                innerResult.IsLunar = IsLunarCalendar(text);
                return innerResult;
            }
            return null;
        }

        // parse if lunar contains
        private bool IsLunarCalendar(string text)
        {
            var trimedText = text.Trim();
            var match = DateExtractorChs.LunarRegex.Match(trimedText);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        // parse basic patterns in DateRegexList
        protected DTParseResult ParseBasicRegexMatch(string text, DateObject referenceDate)
        {
            var trimedText = text.Trim();
            foreach (var regex in DateExtractorChs.DateRegexList)
            {
                var offset = 0;
                var match = regex.Match(trimedText);

                if (match.Success && match.Index == offset && match.Length == trimedText.Length)
                {
                    // LUIS value string will be set in Match2Date method
                    var ret = Match2Date(match, referenceDate);
                    return ret;
                }
            }
            return new DTParseResult();
        }


        // match several other cases
        // including '今天', '后天', '十三日'
        protected DTParseResult ParseImplicitDate(string text, DateObject referenceDate)
        {
            var trimedText = text.Trim();

            var ret = new DTParseResult();

            int[] ContainsDay = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            // handle "十二日" "明年这个月三日" "本月十一日"
            var match = DateExtractorChs.SpecialDate.Match(trimedText);
            if (match.Success && match.Length == trimedText.Length)
            {
                int day = 0, month = referenceDate.Month, year = referenceDate.Year;
                var yearStr = match.Groups["thisyear"].Value.ToLower();
                var monthStr = match.Groups["thismonth"].Value.ToLower();
                var dayStr = match.Groups["day"].Value.ToLower();
                day = this.config.DayOfMonth[dayStr];
                bool hasYear = false, hasMonth = false;

                if (!string.IsNullOrEmpty(monthStr))
                {
                    hasMonth = true;
                    if (DateExtractorChs.NextRe.Match(monthStr).Success)
                    {
                        month++;
                        if (month == 13)
                        {
                            month = 1;
                            year++;
                        }
                    }
                    else if (DateExtractorChs.LastRe.Match(monthStr).Success)
                    {
                        month--;
                        if (month == 0)
                        {
                            month = 12;
                            year--;
                        }
                    }
                    if (!string.IsNullOrEmpty(yearStr))
                    {
                        hasYear = true;
                        if (DateExtractorChs.NextRe.Match(yearStr).Success)
                        {
                            ++year;
                        }
                        else if (DateExtractorChs.LastRe.Match(yearStr).Success)
                        {
                            --year;
                        }
                    }
                }

                ret.Timex = Util.LuisDate(hasYear ? year : -1, hasMonth ? month : -1, day);

                var futureDate = DateObject.MinValue;
                var pastDate = DateObject.MinValue;
                if (day > ContainsDay[month - 1])
                {
                    futureDate = new DateObject(year, month + 1, day);
                    pastDate = new DateObject(year, month - 1, day);
                }
                else
                {
                    futureDate = new DateObject(year, month, day);
                    pastDate = new DateObject(year, month, day);
                    if (!hasMonth)
                    {
                        if (futureDate < referenceDate)
                        {
                            futureDate = futureDate.AddMonths(+1);
                        }
                        if (pastDate >= referenceDate)
                        {
                            pastDate = pastDate.AddMonths(-1);
                        }
                    }
                    else if (hasMonth && !hasYear)
                    {
                        if (futureDate < referenceDate)
                        {
                            futureDate = futureDate.AddYears(+1);
                        }
                        if (pastDate >= referenceDate)
                        {
                            pastDate = pastDate.AddYears(-1);
                        }
                    }
                }
                ret.FutureValue = futureDate;
                ret.PastValue = pastDate;
                ret.Success = true;


                return ret;
            }

            // handle "today", "the day before yesterday"
            match = DateExtractorChs.SpecialDayRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var value = referenceDate;
                if (match.Value.ToLower().Equals("今天") || match.Value.ToLower().Equals("今日") ||
                    match.Value.ToLower().Equals("最近"))
                {
                    value = referenceDate;
                }
                else if (match.Value.ToLower().Equals("明天") || match.Value.ToLower().Equals("明日"))
                {
                    value = referenceDate.AddDays(1);
                }
                else if (match.Value.ToLower().Equals("昨天"))
                {
                    value = referenceDate.AddDays(-1);
                }
                else if (match.Value.ToLower().EndsWith("后天"))
                {
                    value = referenceDate.AddDays(2);
                }
                else if (match.Value.ToLower().EndsWith("前天"))
                {
                    value = referenceDate.AddDays(-2);
                }

                ret.Timex = Util.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            if(!ret.Success)
            {
                ret = MatchThisWeekday(text, referenceDate);
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

        protected DTParseResult MatchNextWeekday(string text, DateObject reference)
        {
            var result = new DTParseResult();
            var match = this.config.NextRegex.Match(text);
            if (match.Success && match.Index == 0 && match.Length == text.Length)
            {
                var weekdayKey = match.Groups["weekday"].Value.ToLowerInvariant();
                var value = reference.Next((DayOfWeek)this.config.DayOfWeek[weekdayKey]);

                result.Timex = Util.LuisDate(value);
                result.FutureValue = result.PastValue = value;
                result.Success = true;
            }
            return result;
        }

        protected DTParseResult MatchThisWeekday(string text, DateObject reference)
        {
            var result = new DTParseResult();
            var match = this.config.ThisRegex.Match(text);
            if (match.Success && match.Index == 0 && match.Length == text.Length)
            {
                var weekdayKey = match.Groups["weekday"].Value.ToLowerInvariant();
                var value = reference.This((DayOfWeek)this.config.DayOfWeek[weekdayKey]);

                result.Timex = Util.LuisDate(value);
                result.FutureValue = result.PastValue = value;
                result.Success = true;
            }
            return result;
        }

        protected DTParseResult MatchLastWeekday(string text, DateObject reference)
        {
            var result = new DTParseResult();
            var match = this.config.LastRegex.Match(text);
            if (match.Success && match.Index == 0 && match.Length == text.Length)
            {
                var weekdayKey = match.Groups["weekday"].Value.ToLowerInvariant();
                var value = reference.Last((DayOfWeek)this.config.DayOfWeek[weekdayKey]);

                result.Timex = Util.LuisDate(value);
                result.FutureValue = result.PastValue = value;
                result.Success = true;
            }
            return result;
        }

        protected DTParseResult MatchWeekdayAlone(string text, DateObject reference)
        {
            var result = new DTParseResult();
            var match = this.config.StrictWeekDayRegex.Match(text);
            if (match.Success && match.Index == 0 && match.Length == text.Length)
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

        protected virtual DTParseResult ParseWeekdayOfMonth(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();

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
                if (trimedText.StartsWith(this.config.NextMonthToken))
                {
                    swift = 1;
                }
                else if (trimedText.StartsWith(this.config.LastMonthToken))
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
            ret.Timex = $@"XXXX-{month.ToString("D2")}-WXX-{weekday}-#{cardinal}";
            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        private DateObject ComputeDate(int cadinal, int weekday, int month, int year)
        {
            var firstDay = new DateObject(year, month, 1);
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

        // handle like "三天前" 
        private DTParseResult ParserDurationWithBeforeAndAfter(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var duration_res = durationExtractor.Extract(text);
            var numStr = string.Empty;
            var unitStr = string.Empty;
            if (duration_res.Count > 0)
            {
                var match = DateExtractorChs.UnitRegex.Match(text);
                if (match.Success)
                {
                    var AfterStr =
                        text.Substring((int)duration_res[0].Start + (int)duration_res[0].Length, 1)
                            .Trim()
                            .ToLowerInvariant();
                    var srcUnit = match.Groups["unit"].Value.ToLowerInvariant();
                    var numberStr =
                        text.Substring((int)duration_res[0].Start, match.Index - (int)duration_res[0].Start)
                            .Trim()
                            .ToLowerInvariant();
                    var number = ConvertChineseToNum(numberStr);
                    if (this.config.UnitMap.ContainsKey(srcUnit))
                    {
                        unitStr = this.config.UnitMap[srcUnit];
                        numStr = number.ToString();
                        if (AfterStr.Equals("前"))
                        {
                            DateObject Date;
                            switch (unitStr)
                            {
                                case "D":
                                    Date = referenceDate.AddDays(-double.Parse(numStr));
                                    break;
                                case "W":
                                    Date = referenceDate.AddDays(-7 * double.Parse(numStr));
                                    break;
                                case "MON":
                                    Date = referenceDate.AddMonths(-Convert.ToInt32(double.Parse(numStr)));
                                    break;
                                case "Y":
                                    Date = referenceDate.AddYears(-Convert.ToInt32(double.Parse(numStr)));
                                    break;
                                default:
                                    return ret;
                            }
                            ret.Timex = $"{Util.LuisDate(Date)}";
                            ret.FutureValue = ret.PastValue = Date;
                            ret.Success = true;
                            return ret;
                        }
                        if (AfterStr.Equals("后"))
                        {
                            DateObject Date;
                            switch (unitStr)
                            {
                                case "D":
                                    Date = referenceDate.AddDays(double.Parse(numStr));
                                    break;
                                case "W":
                                    Date = referenceDate.AddDays(7 * double.Parse(numStr));
                                    break;
                                case "MON":
                                    Date = referenceDate.AddMonths(Convert.ToInt32(double.Parse(numStr)));
                                    break;
                                case "Y":
                                    Date = referenceDate.AddYears(Convert.ToInt32(double.Parse(numStr)));
                                    break;
                                default:
                                    return ret;
                            }
                            ret.Timex =
                                $"{Util.LuisDate(Date)}";
                            ret.FutureValue =
                                ret.PastValue = Date;
                            ret.Success = true;
                            return ret;
                        }
                    }
                }
            }
            return ret;
        }

        // parse a regex match which includes 'day', 'month' and 'year' (optional) group
        protected DTParseResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DTParseResult();

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
                ret.Timex = Util.LuisDate(-1, month, day);
                noYear = true;
            }
            else
            {
                ret.Timex = Util.LuisDate(year, month, day);
            }

            var futureDate = new DateObject(year, month, day);
            var pastDate = new DateObject(year, month, day);
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

        // concert Chinese Number to Integer
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
