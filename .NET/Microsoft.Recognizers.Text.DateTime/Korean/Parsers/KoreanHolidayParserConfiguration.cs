using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Korean;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanHolidayParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKHolidayParserConfiguration
    {
        // @TODO Move dictionaries and hardcoded terms to resource file
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
            { "平安夜", ChristmasEve },
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
            { "国庆节", ChsNationalDay },
            { "建军节", ChsMilBuildDay },
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

        public KoreanHolidayParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            NumberParser = config.NumberParser;

            HolidayRegexList = KoreanHolidayExtractorConfiguration.HolidayRegexList;
            LunarHolidayRegex = KoreanHolidayExtractorConfiguration.LunarHolidayRegex;
        }

        public IExtractor IntegerExtractor { get; }

        public IParser NumberParser { get; }

        Dictionary<string, Func<int, DateObject>> ICJKHolidayParserConfiguration.FixedHolidaysDict => FixedHolidaysDict;

        Dictionary<string, Func<int, DateObject>> ICJKHolidayParserConfiguration.HolidayFuncDict => HolidayFuncDict;

        Dictionary<string, string> ICJKHolidayParserConfiguration.NoFixedTimex => NoFixedTimex;

        public IEnumerable<Regex> HolidayRegexList { get; }

        public Regex LunarHolidayRegex { get; }

        public int GetSwiftYear(string text)
        {
            // @TODO move hardcoded values to resource file
            var trimmedText = text.Trim();
            var swift = -10;

            if (text.EndsWith("去年", StringComparison.Ordinal))
            {
                swift = -1;
            }
            else if (text.EndsWith("明年", StringComparison.Ordinal))
            {
                swift = +1;
            }

            return swift;
        }

        public string SanitizeYearToken(string yearStr)
        {
            // @TODO move hardcoded values to resource file
            if (yearStr.EndsWith("年", StringComparison.Ordinal))
            {
                yearStr = yearStr.Substring(0, yearStr.Length - 1);
            }

            return yearStr;
        }

        private static DateObject NewYear(int year) => new DateObject(year, 1, 1);

        private static DateObject TeacherDay(int year) => new DateObject(year, 9, 10);

        private static DateObject YouthDay(int year) => new DateObject(year, 5, 4);

        private static DateObject ChildrenDay(int year) => new DateObject(year, 6, 1);

        private static DateObject FemaleDay(int year) => new DateObject(year, 3, 8);

        private static DateObject TreePlantDay(int year) => new DateObject(year, 3, 12);

        private static DateObject LoverDay(int year) => new DateObject(year, 2, 14);

        private static DateObject ChristmasEve(int year) => new DateObject(year, 12, 24);

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

        private static DateObject ChsNationalDay(int year) => new DateObject(year, 10, 1);

        private static DateObject ChsMilBuildDay(int year) => new DateObject(year, 8, 1);

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
    }
}