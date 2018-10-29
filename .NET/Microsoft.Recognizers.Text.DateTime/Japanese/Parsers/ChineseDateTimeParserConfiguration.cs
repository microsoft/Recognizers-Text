using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDateTimeParserConfiguration : BaseOptionsConfiguration, IFullDateTimeParserConfiguration
    {
        public JapaneseDateTimeParserConfiguration(DateTimeOptions options = DateTimeOptions.None) : base(options)
        {
            DateParser = new DateParser(this);
            TimeParser = new TimeParserJap(this);
            DateTimeParser = new DateTimeParserJap(this);
            DatePeriodParser = new DatePeriodParserJap(this);
            TimePeriodParser = new TimePeriodParserJap(this);
            DateTimePeriodParser = new DateTimePeriodParserJap(this);
            DurationParser = new DurationParserJap(this);
            GetParser = new SetParserJap(this);
            HolidayParser = new HolidayParserJap(this);
            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.ParserConfigurationUnitValueMap.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.ParserConfigurationSeasonMap.ToImmutableDictionary();
            SeasonValueMap = DateTimeDefinitions.ParserConfigurationSeasonValueMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.ParserConfigurationCardinalMap.ToImmutableDictionary();
            DayOfMonth = DateTimeDefinitions.ParserConfigurationDayOfMonth.ToImmutableDictionary();
            DayOfWeek = DateTimeDefinitions.ParserConfigurationDayOfWeek.ToImmutableDictionary();
            MonthOfYear = DateTimeDefinitions.ParserConfigurationMonthOfYear.ToImmutableDictionary();
            Numbers = InitNumbers();
            DateRegexList = DateExtractorJap.DateRegexList;
            NextRegex = DateExtractorJap.NextRegex;
            ThisRegex = DateExtractorJap.ThisRegex;
            LastRegex = DateExtractorJap.LastRegex;
            StrictWeekDayRegex = DateExtractorJap.WeekDayRegex;
            WeekDayOfMonthRegex = DateExtractorJap.WeekDayOfMonthRegex;
            BeforeRegex = MergedExtractorJap.BeforeRegex;
            AfterRegex = MergedExtractorJap.AfterRegex;
            UntilRegex = MergedExtractorJap.UntilRegex;
            SincePrefixRegex = MergedExtractorJap.SincePrefixRegex;
            SinceSuffixRegex = MergedExtractorJap.SinceSuffixRegex;
        }

        public int TwoNumYear => int.Parse(DateTimeDefinitions.TwoNumYear);

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

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex UntilRegex { get; }

        public Regex SincePrefixRegex { get; }

        public Regex SinceSuffixRegex { get; }

        #endregion

        private static ImmutableDictionary<string, int> InitNumbers()
        {
            return new Dictionary<string, int>
            {
            }.ToImmutableDictionary();
        }

        public int GetSwiftDay(string text)
        {
            var value = 0;

            if (text.Equals("今天") || text.Equals("今日") || text.Equals("最近"))
            {
                value = 0;
            }
            else if (text.StartsWith("明"))
            {
                value = 1;
            }
            else if (text.StartsWith("昨"))
            {
                value = -1;
            }
            else if (text.Equals("大后天") || text.Equals("大後天"))
            {
                value = 3;
            }
            else if (text.Equals("大前天"))
            {
                value = -3;
            }
            else if (text.Equals("后天") || text.Equals("後天"))
            {
                value = 2;
            }
            else if (text.Equals("前天"))
            {
                value = -2;
            }

            return value;
        }
    }
}
