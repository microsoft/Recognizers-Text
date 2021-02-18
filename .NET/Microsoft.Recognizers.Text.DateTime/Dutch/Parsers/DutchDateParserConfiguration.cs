using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchDateParserConfiguration : BaseDateTimeOptionsConfiguration, IDateParserConfiguration
    {
        private IImmutableList<string> lastCardinalTerms = DateTimeDefinitions.LastCardinalTerms.ToImmutableList();

        public DutchDateParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DateExtractor = config.DateExtractor;
            DurationParser = config.DurationParser;
            HolidayParser = new BaseHolidayParser(new DutchHolidayParserConfiguration(this));
            DateRegexes = new DutchDateExtractorConfiguration(this).DateRegexList;
            OnRegex = DutchDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = DutchDateExtractorConfiguration.SpecialDayRegex;
            SpecialDayWithNumRegex = DutchDateExtractorConfiguration.SpecialDayWithNumRegex;
            NextRegex = DutchDateExtractorConfiguration.NextDateRegex;
            ThisRegex = DutchDateExtractorConfiguration.ThisRegex;
            LastRegex = DutchDateExtractorConfiguration.LastDateRegex;
            UnitRegex = DutchDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = DutchDateExtractorConfiguration.WeekDayRegex;
            MonthRegex = DutchDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = DutchDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = DutchDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = DutchDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            WeekDayAndDayRegex = DutchDateExtractorConfiguration.WeekDayAndDayRegex;
            RelativeMonthRegex = DutchDateExtractorConfiguration.RelativeMonthRegex;
            StrictRelativeRegex = DutchDateExtractorConfiguration.StrictRelativeRegex;
            YearSuffix = DutchDateExtractorConfiguration.YearSuffix;
            RelativeWeekDayRegex = DutchDateExtractorConfiguration.RelativeWeekDayRegex;
            BeforeAfterRegex = DutchDateExtractorConfiguration.BeforeAfterRegex;
            RelativeDayRegex = RegexCache.Get(DateTimeDefinitions.RelativeDayRegex, RegexOptions.Singleline);
            NextPrefixRegex = RegexCache.Get(DateTimeDefinitions.NextPrefixRegex, RegexOptions.Singleline);
            PreviousPrefixRegex = RegexCache.Get(DateTimeDefinitions.PreviousPrefixRegex, RegexOptions.Singleline);
            UpcomingPrefixRegex = RegexCache.Get(DateTimeDefinitions.UpcomingPrefixRegex, RegexOptions.Singleline);
            PastPrefixRegex = RegexCache.Get(DateTimeDefinitions.PastPrefixRegex, RegexOptions.Singleline);
            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;
            CardinalMap = config.CardinalMap;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
            SameDayTerms = DateTimeDefinitions.SameDayTerms.ToImmutableList();
            PlusOneDayTerms = DateTimeDefinitions.PlusOneDayTerms.ToImmutableList();
            PlusTwoDayTerms = DateTimeDefinitions.PlusTwoDayTerms.ToImmutableList();
            MinusOneDayTerms = DateTimeDefinitions.MinusOneDayTerms.ToImmutableList();
            MinusTwoDayTerms = DateTimeDefinitions.MinusTwoDayTerms.ToImmutableList();
        }

        public string DateTokenPrefix { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public IEnumerable<Regex> DateRegexes { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public Regex OnRegex { get; }

        public Regex SpecialDayRegex { get; }

        public Regex SpecialDayWithNumRegex { get; }

        public Regex NextRegex { get; }

        public Regex ThisRegex { get; }

        public Regex LastRegex { get; }

        public Regex UnitRegex { get; }

        public Regex WeekDayRegex { get; }

        public Regex MonthRegex { get; }

        public Regex WeekDayOfMonthRegex { get; }

        public Regex ForTheRegex { get; }

        public Regex WeekDayAndDayOfMothRegex { get; }

        public Regex WeekDayAndDayRegex { get; }

        public Regex RelativeMonthRegex { get; }

        public Regex StrictRelativeRegex { get; }

        public Regex YearSuffix { get; }

        public Regex RelativeWeekDayRegex { get; }

        public Regex RelativeDayRegex { get; }

        public Regex NextPrefixRegex { get; }

        public Regex PreviousPrefixRegex { get; }

        public Regex UpcomingPrefixRegex { get; }

        public Regex PastPrefixRegex { get; }

        public Regex BeforeAfterRegex { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableList<string> SameDayTerms { get; }

        public IImmutableList<string> PlusOneDayTerms { get; }

        public IImmutableList<string> MinusOneDayTerms { get; }

        public IImmutableList<string> PlusTwoDayTerms { get; }

        public IImmutableList<string> MinusTwoDayTerms { get; }

        bool IDateParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public int GetSwiftMonthOrYear(string text)
        {
            var trimmedText = text.Trim();
            var swift = 0;

            if (NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }

            if (PreviousPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }

            return swift;
        }

        public bool IsCardinalLast(string text)
        {
            var trimmedText = text.Trim();
            return lastCardinalTerms.Contains(trimmedText);
        }

        public string Normalize(string text)
        {
            return text;
        }
    }
}
