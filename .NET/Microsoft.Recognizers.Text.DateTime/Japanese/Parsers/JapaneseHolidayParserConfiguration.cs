using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Japanese;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseHolidayParserConfiguration : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date";

        public static readonly Dictionary<string, Func<int, DateObject>> FixedHolidaysDict = new Dictionary<string, Func<int, DateObject>>
        {
            { "元旦", NewYear },
            { "元旦节", NewYear },
            { "教师节", TeacherDay },
            { "青年节", YouthDay },
            { "儿童节", ChildrenDay },
            { "妇女节", FemaleDay },
            { "植树节", TreePlantDay },
            { "情人节", LoverDay },
            { "圣诞节", ChristmasDay },
            { "新年", NewYear },
            { "愚人节", FoolDay },
            { "五一", LaborDay },
            { "劳动节", LaborDay },
            { "万圣节", HalloweenDay },
            { "中秋节", MidautumnDay },
            { "中秋", MidautumnDay },
            { "春节", SpringDay },
            { "除夕", NewYearEve },
            { "元宵节", LanternDay },
            { "清明节", QingMingDay },
            { "清明", QingMingDay },
            { "端午节", DragonBoatDay },
            { "端午", DragonBoatDay },
            { "国庆节", JapNationalDay },
            { "建军节", JapMilBuildDay },
            { "女生节", GirlsDay },
            { "光棍节", SinglesDay },
            { "双十一", SinglesDay },
            { "重阳节", ChongYangDay },
        };

        public static readonly Dictionary<string, Func<int, DateObject>> HolidayFuncDict = new Dictionary
            <string, Func<int, DateObject>>
        {
            { "父亲节", GetFathersDayOfYear },
            { "母亲节", GetMothersDayOfYear },
            { "感恩节", GetThanksgivingDayOfYear },
        };

        public static readonly Dictionary<string, string> NoFixedTimex = DateTimeDefinitions.HolidayNoFixedTimex;

        private static readonly IExtractor IntegerExtractor = new IntegerExtractor();

        private static readonly IParser IntegerParser = new BaseCJKNumberParser(new JapaneseNumberParserConfiguration());

        private readonly IFullDateTimeParserConfiguration config;

        public JapaneseHolidayParserConfiguration(IFullDateTimeParserConfiguration configuration)
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
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)innerResult.FutureValue) },
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)innerResult.PastValue) },
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
                TimexStr = value == null ? string.Empty : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private static DateTimeResolutionResult ParseHolidayRegexMatch(string text, DateObject referenceDate)
        {
            foreach (var regex in JapaneseHolidayExtractorConfiguration.HolidayRegexList)
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

        private static DateTimeResolutionResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var holidayStr = match.Groups["holiday"].Value.ToLower();

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
                    value = FixedHolidaysDict[holidayStr](year);
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

        private static DateObject NewYear(int year) => new DateObject(year, 1, 1);

        private static DateObject TeacherDay(int year) => new DateObject(year, 9, 10);

        private static DateObject YouthDay(int year) => new DateObject(year, 5, 4);

        private static DateObject ChildrenDay(int year) => new DateObject(year, 6, 1);

        private static DateObject FemaleDay(int year) => new DateObject(year, 3, 8);

        private static DateObject TreePlantDay(int year) => new DateObject(year, 3, 12);

        private static DateObject LoverDay(int year) => new DateObject(year, 2, 14);

        private static DateObject ChristmasDay(int year) => new DateObject(year, 12, 25);

        private static DateObject FoolDay(int year) => new DateObject(year, 4, 1);

        private static DateObject LaborDay(int year) => new DateObject(year, 5, 1);

        private static DateObject HalloweenDay(int year) => new DateObject(year, 10, 31);

        private static DateObject MidautumnDay(int year) => new DateObject(year, 8, 15);

        private static DateObject SpringDay(int year) => new DateObject(year, 1, 1);

        private static DateObject NewYearEve(int year) => new DateObject(year, 1, 1).AddDays(-1);

        private static DateObject LanternDay(int year) => new DateObject(year, 1, 15);

        private static DateObject QingMingDay(int year) => new DateObject(year, 4, 4);

        private static DateObject DragonBoatDay(int year) => new DateObject(year, 5, 5);

        private static DateObject JapNationalDay(int year) => new DateObject(year, 10, 1);

        private static DateObject JapMilBuildDay(int year) => new DateObject(year, 8, 1);

        private static DateObject GirlsDay(int year) => new DateObject(year, 3, 7);

        private static DateObject SinglesDay(int year) => new DateObject(year, 11, 11);

        private static DateObject ChongYangDay(int year) => new DateObject(year, 9, 9);

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

        // parse if lunar contains
        private bool IsLunarCalendar(string text)
        {
            var trimmedText = text.Trim();
            var match = JapaneseHolidayExtractorConfiguration.LunarHolidayRegex.Match(trimmedText);
            return match.Success;
        }
    }
}