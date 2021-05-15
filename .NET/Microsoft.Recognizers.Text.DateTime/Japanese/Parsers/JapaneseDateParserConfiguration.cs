using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDateParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateParserConfiguration
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date";

        public static readonly List<int> MonthMaxDays = new List<int> { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        public JapaneseDateParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
             : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;

            NumberParser = config.NumberParser;

            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;

            DateRegexList = new JapaneseDateExtractorConfiguration(this).DateRegexList;
            SpecialDate = JapaneseDateExtractorConfiguration.SpecialDate;
            NextRe = JapaneseDateExtractorConfiguration.NextRe;
            LastRe = JapaneseDateExtractorConfiguration.LastRe;
            SpecialDayRegex = JapaneseDateExtractorConfiguration.SpecialDayRegex;
            StrictWeekDayRegex = JapaneseDateExtractorConfiguration.WeekDayRegex;
            LunarRegex = JapaneseDateExtractorConfiguration.LunarRegex;
            UnitRegex = JapaneseDateExtractorConfiguration.UnitRegex;
            BeforeRegex = JapaneseDateExtractorConfiguration.BeforeRegex;
            AfterRegex = JapaneseDateExtractorConfiguration.AfterRegex;
            DynastyYearRegex = JapaneseDateExtractorConfiguration.DynastyYearRegex;
            DynastyStartYear = JapaneseDateExtractorConfiguration.DynastyStartYear;
            DynastyYearMap = JapaneseDateExtractorConfiguration.DynastyYearMap;
            NextRegex = JapaneseDateExtractorConfiguration.NextRegex;
            ThisRegex = JapaneseDateExtractorConfiguration.ThisRegex;
            LastRegex = JapaneseDateExtractorConfiguration.LastRegex;
            WeekDayOfMonthRegex = JapaneseDateExtractorConfiguration.WeekDayOfMonthRegex;

            CardinalMap = config.CardinalMap;
            UnitMap = config.UnitMap;
            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;

        }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IEnumerable<Regex> DateRegexList { get; }

        public Regex SpecialDate { get; }

        public Regex NextRe { get; }

        public Regex LastRe { get; }

        public Regex SpecialDayRegex { get; }

        public Regex StrictWeekDayRegex { get; }

        public Regex LunarRegex { get; }

        public Regex UnitRegex { get; }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex NextRegex { get; }

        public Regex ThisRegex { get; }

        public Regex LastRegex { get; }

        public Regex WeekDayOfMonthRegex { get; }

        public Regex DynastyYearRegex { get; }

        public ImmutableDictionary<string, int> DynastyYearMap { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public string DynastyStartYear { get; }

        public string LastWeekDayToken => DateTimeDefinitions.ParserConfigurationLastWeekDayToken;

        public string NextMonthToken => DateTimeDefinitions.ParserConfigurationNextMonthToken;

        public string LastMonthToken => DateTimeDefinitions.ParserConfigurationLastMonthToken;

        List<int> ICJKDateParserConfiguration.MonthMaxDays => MonthMaxDays;

        public int GetSwiftDay(string text)
        {
            // Today: 今天, 今日, 最近, きょう, この日
            var value = 0;

            // @TODO move hardcoded values to resources file

            if (text.StartsWith("来", StringComparison.Ordinal) ||
                text is "あす" or "あした" or "明日")
            {
                value = 1;
            }
            else if (text.StartsWith("昨", StringComparison.Ordinal) ||
                     text is "きのう" or "前日")
            {
                value = -1;
            }
            else if (text is "大后天" or "大後天")
            {
                value = 3;
            }
            else if (text is "大前天")
            {
                value = -3;
            }
            else if (text is "后天" or "後天" or "明後日" or "あさって")
            {
                value = 2;
            }
            else if (text is "前天" or "一昨日" or "二日前" or "おととい")
            {
                value = -2;
            }

            return value;
        }
    }
}
