using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDateParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateParserConfiguration
    {
        public static readonly Regex PlusOneDayRegex = new Regex(DateTimeDefinitions.PlusOneDayRegex, RegexFlags);
        public static readonly Regex MinusOneDayRegex = new Regex(DateTimeDefinitions.MinusOneDayRegex, RegexFlags);
        public static readonly Regex PlusTwoDayRegex = new Regex(DateTimeDefinitions.PlusTwoDayRegex, RegexFlags);
        public static readonly Regex MinusTwoDayRegex = new Regex(DateTimeDefinitions.MinusTwoDayRegex, RegexFlags);
        public static readonly Regex PlusThreeDayRegex = new Regex(DateTimeDefinitions.PlusThreeDayRegex, RegexFlags);
        public static readonly Regex MinusThreeDayRegex = new Regex(DateTimeDefinitions.MinusThreeDayRegex, RegexFlags);
        public static readonly Regex PlusFourDayRegex = new Regex(DateTimeDefinitions.PlusFourDayRegex, RegexFlags);

        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date";

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseDateParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
             : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;

            NumberParser = config.NumberParser;

            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;

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
            WeekDayAndDayRegex = JapaneseDateExtractorConfiguration.WeekDayAndDayRegex;
            DurationRelativeDurationUnitRegex = JapaneseDateExtractorConfiguration.DurationRelativeDurationUnitRegex;
            SpecialDayWithNumRegex = JapaneseDateExtractorConfiguration.SpecialDayWithNumRegex;

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
            var swift = 0;

            if (PlusOneDayRegex.MatchBegin(text, trim: true).Success)
            {
                swift = 1;
            }
            else if (MinusOneDayRegex.MatchBegin(text, trim: true).Success)
            {
                swift = -1;
            }

            if (PlusOneDayRegex.IsExactMatch(text, trim: false))
            {
                swift = 1;
            }
            else if (PlusThreeDayRegex.IsExactMatch(text, trim: false))
            {
                swift = 3;
            }
            else if (PlusFourDayRegex.IsExactMatch(text, trim: false))
            {
                swift = 4;
            }
            else if (MinusThreeDayRegex.IsExactMatch(text, trim: false))
            {
                swift = -3;
            }
            else if (MinusOneDayRegex.IsExactMatch(text, trim: false))
            {
                swift = -1;

            }
            else if (PlusTwoDayRegex.IsExactMatch(text, trim: false))
            {
                swift = 2;

            }
            else if (MinusTwoDayRegex.IsExactMatch(text, trim: false))
            {
                swift = -2;

            }

            return swift;
        }
    }
}
