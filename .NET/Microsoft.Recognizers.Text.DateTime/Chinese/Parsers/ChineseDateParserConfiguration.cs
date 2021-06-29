using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDateParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateParserConfiguration
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date";

        public ChineseDateParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
             : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;

            NumberParser = config.NumberParser;

            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;

            DateRegexList = new ChineseDateExtractorConfiguration(this).DateRegexList;
            SpecialDate = ChineseDateExtractorConfiguration.SpecialDate;
            NextRe = ChineseDateExtractorConfiguration.NextRe;
            LastRe = ChineseDateExtractorConfiguration.LastRe;
            SpecialDayRegex = ChineseDateExtractorConfiguration.SpecialDayRegex;
            StrictWeekDayRegex = ChineseDateExtractorConfiguration.WeekDayRegex;
            LunarRegex = ChineseDateExtractorConfiguration.LunarRegex;
            UnitRegex = ChineseDateExtractorConfiguration.UnitRegex;
            BeforeRegex = ChineseDateExtractorConfiguration.BeforeRegex;
            AfterRegex = ChineseDateExtractorConfiguration.AfterRegex;
            DynastyYearRegex = ChineseDateExtractorConfiguration.DynastyYearRegex;
            DynastyStartYear = ChineseDateExtractorConfiguration.DynastyStartYear;
            DynastyYearMap = ChineseDateExtractorConfiguration.DynastyYearMap;
            NextRegex = ChineseDateExtractorConfiguration.NextRegex;
            ThisRegex = ChineseDateExtractorConfiguration.ThisRegex;
            LastRegex = ChineseDateExtractorConfiguration.LastRegex;
            WeekDayOfMonthRegex = ChineseDateExtractorConfiguration.WeekDayOfMonthRegex;
            WeekDayAndDayRegex = ChineseDateExtractorConfiguration.WeekDayAndDayRegex;
            DurationRelativeDurationUnitRegex = ChineseDateExtractorConfiguration.DurationRelativeDurationUnitRegex;
            SpecialDayWithNumRegex = ChineseDateExtractorConfiguration.SpecialDayWithNumRegex;

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

        public IDateTimeParser DurationParser { get; }

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

        public Regex WeekDayAndDayRegex { get; }

        public Regex DurationRelativeDurationUnitRegex { get; }

        public Regex SpecialDayWithNumRegex { get; }

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

        public int GetSwiftDay(string text)
        {
            var value = 0;

            // @TODO move hardcoded values to resources file
            if (text.Equals("今天", StringComparison.Ordinal) ||
                text.Equals("今日", StringComparison.Ordinal) ||
                text.Equals("最近", StringComparison.Ordinal))
            {
                value = 0;
            }
            else if (text.StartsWith("明", StringComparison.Ordinal))
            {
                value = 1;
            }
            else if (text.StartsWith("昨", StringComparison.Ordinal))
            {
                value = -1;
            }
            else if (text.Equals("大后天", StringComparison.Ordinal) ||
                     text.Equals("大後天", StringComparison.Ordinal))
            {
                value = 3;
            }
            else if (text.Equals("大前天", StringComparison.Ordinal))
            {
                value = -3;
            }
            else if (text.Equals("后天", StringComparison.Ordinal) ||
                     text.Equals("後天", StringComparison.Ordinal))
            {
                value = 2;
            }
            else if (text.Equals("前天", StringComparison.Ordinal))
            {
                value = -2;
            }

            return value;
        }
    }
}
