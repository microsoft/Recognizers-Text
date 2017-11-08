using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDateTimeParserConfiguration : IFullDateTimeParserConfiguration
    {
        public ChineseDateTimeParserConfiguration()
        {
            DateParser = new DateParser(this);
            TimeParser = new TimeParserChs(this);
            DateTimeParser = new DateTimeParserChs(this);
            DatePeriodParser = new DatePeriodParserChs(this);
            TimePeriodParser = new TimePeriodParserChs(this);
            DateTimePeriodParser = new DateTimePeriodParserChs(this);
            DurationParser = new DurationParserChs(this);
            GetParser = new SetParserChs(this);
            HolidayParser = new HolidayParserChs(this);
            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.ParserConfigurationUnitValueMap.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.ParserConfigurationSeasonMap.ToImmutableDictionary();
            SeasonValueMap = DateTimeDefinitions.ParserConfigurationSeasonValueMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.ParserConfigurationCardinalMap.ToImmutableDictionary();
            DayOfMonth = DateTimeDefinitions.ParserConfigurationDayOfMonth.ToImmutableDictionary();
            DayOfWeek = DateTimeDefinitions.ParserConfigurationDayOfWeek.ToImmutableDictionary();
            MonthOfYear = DateTimeDefinitions.ParserConfigurationMonthOfYear.ToImmutableDictionary();
            Numbers = InitNumbers();
            DateRegexList = DateExtractorChs.DateRegexList;
            NextRegex = DateExtractorChs.NextRegex;
            ThisRegex = DateExtractorChs.ThisRegex;
            LastRegex = DateExtractorChs.LastRegex;
            StrictWeekDayRegex = DateExtractorChs.WeekDayRegex;
            WeekDayOfMonthRegex = DateExtractorChs.WeekDayOfMonthRegex;
        }
        public string Before => DateTimeDefinitions.ParserConfigurationBefore;

        public string After => DateTimeDefinitions.ParserConfigurationAfter;

        public string LastWeekDayToken => DateTimeDefinitions.ParserConfigurationLastWeekDayToken;

        public string NextMonthToken => DateTimeDefinitions.ParserConfigurationNextMonthToken;

        public string LastMonthToken => DateTimeDefinitions.ParserConfigurationLastMonthToken;

        public string DatePrefix => DateTimeDefinitions.ParserConfigurationDatePrefix;

        #region internalParsers

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser DatePeriodParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DateTimePeriodParser { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser GetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        #endregion

        #region Dictionaries

        public ImmutableDictionary<string, string> UnitMap { get; }

        public ImmutableDictionary<string, long> UnitValueMap { get; }

        public ImmutableDictionary<string, string> SeasonMap { get; }

        public ImmutableDictionary<string, int> SeasonValueMap { get; }

        public ImmutableDictionary<string, int> CardinalMap { get; }

        public ImmutableDictionary<string, int> DayOfMonth { get; }

        public ImmutableDictionary<string, int> DayOfWeek { get; }

        public ImmutableDictionary<string, int> MonthOfYear { get; }

        public ImmutableDictionary<string, int> Numbers { get; }

        #endregion

        #region Regexes

        public IEnumerable<Regex> DateRegexList { get; }

        public Regex NextRegex { get; }

        public Regex ThisRegex { get; }

        public Regex LastRegex { get; }

        public Regex StrictWeekDayRegex { get; }

        public Regex WeekDayOfMonthRegex { get; }

        #endregion

        private static ImmutableDictionary<string, int> InitNumbers()
        {
            return new Dictionary<string, int>
            {
            }.ToImmutableDictionary();
        }
    }
}
