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
            UnitMap = DateTimeDefinitions.ParserConfiguration_UnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.ParserConfiguration_UnitValueMap.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.ParserConfiguration_SeasonMap.ToImmutableDictionary();
            SeasonValueMap = DateTimeDefinitions.ParserConfiguration_SeasonValueMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.ParserConfiguration_CardinalMap.ToImmutableDictionary();
            DayOfMonth = DateTimeDefinitions.ParserConfiguration_DayOfMonth.ToImmutableDictionary();
            DayOfWeek = DateTimeDefinitions.ParserConfiguration_DayOfWeek.ToImmutableDictionary();
            MonthOfYear = DateTimeDefinitions.ParserConfiguration_MonthOfYear.ToImmutableDictionary();
            Numbers = InitNumbers();
            DateRegexList = DateExtractorChs.DateRegexList;
            NextRegex = DateExtractorChs.NextRegex;
            ThisRegex = DateExtractorChs.ThisRegex;
            LastRegex = DateExtractorChs.LastRegex;
            StrictWeekDayRegex = DateExtractorChs.WeekDayRegex;
            WeekDayOfMonthRegex = DateExtractorChs.WeekDayOfMonthRegex;
        }
        public string Before => DateTimeDefinitions.ParserConfiguration_Before;

        public string After => DateTimeDefinitions.ParserConfiguration_After;

        public string LastWeekDayToken => DateTimeDefinitions.ParserConfiguration_LastWeekDayToken;

        public string NextMonthToken => DateTimeDefinitions.ParserConfiguration_NextMonthToken;

        public string LastMonthToken => DateTimeDefinitions.ParserConfiguration_LastMonthToken;

        public string DatePrefix => DateTimeDefinitions.ParserConfiguration_DatePrefix;

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
