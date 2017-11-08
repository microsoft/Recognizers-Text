using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.Number.Chinese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class HolidayParserChs : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; //"Date";

        private static readonly IExtractor IntegerExtractor = new IntegerExtractor();

        private static readonly IParser IntegerParser = new ChineseNumberParser(new ChineseNumberParserConfiguration());

        //@TODO Move to definitions
        public static readonly Dictionary<string, DateObject> FixedHolidaysDict = new Dictionary<string, DateObject>
        {
            
            #region Fixed Date Holidays

            {"元旦", TimexConstants.yuandan},
            {"元旦节", TimexConstants.yuandan},
            {"教师节", TimexConstants.teacherday},
            {"青年节", TimexConstants.youthday},
            {"儿童节", TimexConstants.childrenday},
            {"妇女节", TimexConstants.femaleday},
            {"植树节", TimexConstants.treeplantday},
            {"情人节", TimexConstants.loverday},
            {"圣诞节", TimexConstants.christmasday},
            {"新年", TimexConstants.yuandan},
            {"愚人节", TimexConstants.foolday},
            {"五一", TimexConstants.laborday},
            {"劳动节", TimexConstants.laborday},
            {"万圣节", TimexConstants.halloweenday},
            {"中秋节", TimexConstants.midautumnday},
            {"中秋", TimexConstants.midautumnday},
            {"春节", TimexConstants.springday},
            {"除夕", TimexConstants.chuxiday},
            {"元宵节", TimexConstants.lanternday},
            {"清明节", TimexConstants.qingmingday},
            {"清明", TimexConstants.qingmingday},
            {"端午节", TimexConstants.dragonboatday},
            {"端午", TimexConstants.dragonboatday},
            {"国庆节", TimexConstants.chsnationalday},
            {"建军节", TimexConstants.chsmilbuildday},
            {"女生节", TimexConstants.girlsday},
            {"光棍节", TimexConstants.singlesday},
            {"双十一", TimexConstants.singlesday},
            {"重阳节", TimexConstants.chongyangday}

            #endregion

        };

        public static readonly Dictionary<string, Func<int, DateObject>> HolidayFuncDict = new Dictionary
            <string, Func<int, DateObject>>
        {
            
            #region Holiday Functions

            {"父亲节", GetFathersDayOfYear},
            {"母亲节", GetMothersDayOfYear},
            {"感恩节", GetThanksgivingDayOfYear}
            
            #endregion

        };

        public static readonly Dictionary<string, string> NoFixedTimex = DateTimeDefinitions.HolidayNoFixedTimex;

        private readonly IFullDateTimeParserConfiguration config;

        public HolidayParserChs(IFullDateTimeParserConfiguration configuration)
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
                var innerResult = ParseHolidayRegexMatch(er.Text, referenceDate);

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

                    innerResult.IsLunar = IsLunarCalendar(er.Text);
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
                TimexStr = value == null ? "" : ((DateTimeResolutionResult) value).Timex,
                ResolutionStr = ""
            };
            return ret;
        }

        // parse if lunar contains
        private bool IsLunarCalendar(string text)
        {
            var trimedText = text.Trim();
            var match = ChineseHolidayExtractorConfiguration.LunarHolidayRegex.Match(trimedText);
            return match.Success;
        }

        private static DateTimeResolutionResult ParseHolidayRegexMatch(string text, DateObject referenceDate)
        {
            var trimedText = text.Trim();
            foreach (var regex in ChineseHolidayExtractorConfiguration.HolidayRegexList)
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

            return new DateTimeResolutionResult();
        }

        private static DateTimeResolutionResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var holidayStr = match.Groups["holiday"].Value.ToLower();

            var year = referenceDate.Year;
            var hasYear = false;
            var yearNum = match.Groups["year"].Value;
            var yearChs = match.Groups["yearchs"].Value;
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
            else if (!string.IsNullOrEmpty(yearChs))
            {
                hasYear = true;
                if (yearChs.EndsWith("年"))
                {
                    yearChs = yearChs.Substring(0, yearChs.Length - 1);
                }
                year = ConvertChineseToInteger(yearChs);
            }
            else if (!string.IsNullOrEmpty(yearRel))
            {
                hasYear = true;
                if (yearRel.EndsWith("去年"))
                {
                    year--;
                }
                else if (yearRel.EndsWith("明年"))
                {
                    year++;
                }
            }

            if (year < 100 && year >= 90)
            {
                year += 1900;
            }
            else if (year < 20)
            {
                year += 2000;
            }

            if (!string.IsNullOrEmpty(holidayStr))
            {
                DateObject value;
                string timexStr;
                if (FixedHolidaysDict.ContainsKey(holidayStr))
                {
                    value = FixedHolidaysDict[holidayStr];
                    timexStr = $"-{value.Month:D2}-{value.Day:D2}";
                }
                else
                {
                    if (HolidayFuncDict.ContainsKey(holidayStr))
                    {
                        value = HolidayFuncDict[holidayStr](year);
                        timexStr = NoFixedTimex[holidayStr];
                    }
                    else
                    {
                        return ret;
                    }
                }

                if (hasYear)
                {
                    ret.Timex = year.ToString("D4") + timexStr;
                    ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(year, value.Month, value.Day);
                    ret.Success = true;
                    return ret;
                }

                ret.Timex = "XXXX" + timexStr;
                ret.FutureValue = GetFutureValue(value, referenceDate, holidayStr);
                ret.PastValue = GetPastValue(value, referenceDate, holidayStr);
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        private static DateObject GetFutureValue(DateObject value, DateObject referenceDate, string holiday)
        {
            if (value < referenceDate)
            {
                if (FixedHolidaysDict.ContainsKey(holiday))
                {
                    return value.AddYears(1);
                }

                if (HolidayFuncDict.ContainsKey(holiday))
                {
                    value = HolidayFuncDict[holiday](referenceDate.Year + 1);
                }
            }

            return value;
        }

        private static DateObject GetPastValue(DateObject value, DateObject referenceDate, string holiday)
        {
            if (value >= referenceDate)
            {
                if (FixedHolidaysDict.ContainsKey(holiday))
                {
                    return value.AddYears(-1);
                }

                if (HolidayFuncDict.ContainsKey(holiday))
                {
                    value = HolidayFuncDict[holiday](referenceDate.Year - 1);
                }
            }

            return value;
        }

        private static DateObject GetMothersDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 5, (from day in Enumerable.Range(1, 31)
                where DateObject.MinValue.SafeCreateFromValue(year, 5, day).DayOfWeek == DayOfWeek.Sunday
                select day).ElementAt(1));
        }

        private static DateObject GetFathersDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 6, (from day in Enumerable.Range(1, 30)
                where DateObject.MinValue.SafeCreateFromValue(year, 6, day).DayOfWeek == DayOfWeek.Sunday
                select day).ElementAt(2));
        }

        private static DateObject GetMartinLutherKingDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 1, (from day in Enumerable.Range(1, 31)
                where DateObject.MinValue.SafeCreateFromValue(year, 1, day).DayOfWeek == DayOfWeek.Monday
                select day).ElementAt(2));
        }

        private static DateObject GetWashingtonsBirthdayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 2, (from day in Enumerable.Range(1, 29)
                where DateObject.MinValue.SafeCreateFromValue(year, 2, day).DayOfWeek == DayOfWeek.Monday
                select day).ElementAt(2));
        }

        private static DateObject GetCanberraDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 3, (from day in Enumerable.Range(1, 31)
                where DateObject.MinValue.SafeCreateFromValue(year, 3, day).DayOfWeek == DayOfWeek.Monday
                select day).ElementAt(0));
        }

        private static DateObject GetMemorialDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 5, (from day in Enumerable.Range(1, 31)
                where DateObject.MinValue.SafeCreateFromValue(year, 5, day).DayOfWeek == DayOfWeek.Monday
                select day).Last());
        }

        private static DateObject GetLabourDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 9, (from day in Enumerable.Range(1, 30)
                where DateObject.MinValue.SafeCreateFromValue(year, 9, day).DayOfWeek == DayOfWeek.Monday
                select day).ElementAt(0));
        }

        private static DateObject GetColumbusDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 10, (from day in Enumerable.Range(1, 31)
                where DateObject.MinValue.SafeCreateFromValue(year, 10, day).DayOfWeek == DayOfWeek.Monday
                select day).ElementAt(1));
        }

        private static DateObject GetThanksgivingDayOfYear(int year)
        {
            return DateObject.MinValue.SafeCreateFromValue(year, 11, (from day in Enumerable.Range(1, 30)
                where DateObject.MinValue.SafeCreateFromValue(year, 11, day).DayOfWeek == DayOfWeek.Thursday
                select day).ElementAt(3));
        }

        private static int ConvertChineseToInteger(string yearChsStr)
        {
            var year = 0;
            var num = 0;

            var er = IntegerExtractor.Extract(yearChsStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                {
                    num = Convert.ToInt32((double) (IntegerParser.Parse(er[0]).Value ?? 0));
                }
            }

            if (num < 10)
            {
                num = 0;
                foreach (var ch in yearChsStr)
                {
                    num *= 10;
                    er = IntegerExtractor.Extract(ch.ToString());
                    if (er.Count != 0)
                    {
                        if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                        {
                            num += Convert.ToInt32((double) (IntegerParser.Parse(er[0]).Value ?? 0));
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
    }

    #region Holiday Timex Constants

    internal static class TimexConstants
    {
        internal static readonly DateObject now = DateObject.Now;
        internal static readonly DateObject yuandan = new DateObject(now.Year, 1, 1);
        internal static readonly DateObject chsnationalday = new DateObject(now.Year, 10, 1);
        internal static readonly DateObject laborday = new DateObject(now.Year, 5, 1);
        internal static readonly DateObject christmasday = new DateObject(now.Year, 12, 25);
        internal static readonly DateObject loverday = new DateObject(now.Year, 2, 14);
        internal static readonly DateObject chsmilbuildday = new DateObject(now.Year, 8, 1);
        internal static readonly DateObject foolday = new DateObject(now.Year, 4, 1);
        internal static readonly DateObject girlsday = new DateObject(now.Year, 3, 7);
        internal static readonly DateObject treeplantday = new DateObject(now.Year, 3, 12);
        internal static readonly DateObject femaleday = new DateObject(now.Year, 3, 8);
        internal static readonly DateObject childrenday = new DateObject(now.Year, 6, 1);
        internal static readonly DateObject youthday = new DateObject(now.Year, 5, 4);
        internal static readonly DateObject teacherday = new DateObject(now.Year, 9, 10);
        internal static readonly DateObject singlesday = new DateObject(now.Year, 11, 11);

        internal static readonly DateObject halloweenday = new DateObject(now.Year, 10, 31);

        internal static readonly DateObject midautumnday = new DateObject(now.Year, 8, 15);
        internal static readonly DateObject springday = new DateObject(now.Year, 1, 1);
        internal static readonly DateObject chuxiday = new DateObject(now.Year, 1, 1).AddDays(-1);

        internal static readonly DateObject lanternday = new DateObject(now.Year, 1, 15);
        internal static readonly DateObject qingmingday = new DateObject(now.Year, 4, 4);
        internal static readonly DateObject dragonboatday = new DateObject(now.Year, 5, 5);
        internal static readonly DateObject chongyangday = new DateObject(now.Year, 9, 9);
    }

    #endregion

}