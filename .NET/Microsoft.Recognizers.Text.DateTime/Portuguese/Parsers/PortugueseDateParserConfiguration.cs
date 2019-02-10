using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDateParserConfiguration : BaseOptionsConfiguration, IDateParserConfiguration
    {
        public PortugueseDateParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;
            DateRegexes = new PortugueseDateExtractorConfiguration(this).DateRegexList;
            OnRegex = PortugueseDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = PortugueseDateExtractorConfiguration.SpecialDayRegex;
            SpecialDayWithNumRegex = PortugueseDateExtractorConfiguration.SpecialDayWithNumRegex;
            NextRegex = PortugueseDateExtractorConfiguration.NextDateRegex;
            ThisRegex = PortugueseDateExtractorConfiguration.ThisRegex;
            LastRegex = PortugueseDateExtractorConfiguration.LastDateRegex;
            UnitRegex = PortugueseDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = PortugueseDateExtractorConfiguration.WeekDayRegex;
            MonthRegex = PortugueseDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = PortugueseDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = PortugueseDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = PortugueseDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            WeekDayAndDayRegex = PortugueseDateExtractorConfiguration.WeekDayAndDayRegex;
            RelativeMonthRegex = PortugueseDateExtractorConfiguration.RelativeMonthRegex;
            YearSuffix = PortugueseDateExtractorConfiguration.YearSuffix;
            RelativeWeekDayRegex = PortugueseDateExtractorConfiguration.RelativeWeekDayRegex;
            RelativeDayRegex = new Regex(DateTimeDefinitions.RelativeDayRegex, RegexOptions.Singleline);
            NextPrefixRegex = new Regex(DateTimeDefinitions.NextPrefixRegex, RegexOptions.Singleline);
            PreviousPrefixRegex = new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexOptions.Singleline);
            UpcomingPrefixRegex = new Regex(DateTimeDefinitions.UpcomingPrefixRegex, RegexOptions.Singleline);
            PastPrefixRegex = new Regex(DateTimeDefinitions.PastPrefixRegex, RegexOptions.Singleline);
            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;
            CardinalMap = config.CardinalMap;
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DateExtractor = config.DateExtractor;
            DurationParser = config.DurationParser;
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

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IEnumerable<Regex> DateRegexes { get; }

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

        public Regex YearSuffix { get; }

        public Regex RelativeWeekDayRegex { get; }

        public Regex RelativeDayRegex { get; }

        public Regex NextPrefixRegex { get; }

        public Regex PreviousPrefixRegex { get; }

        public Regex UpcomingPrefixRegex { get; }

        public Regex PastPrefixRegex { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableList<string> SameDayTerms { get; }

        public IImmutableList<string> PlusOneDayTerms { get; }

        public IImmutableList<string> MinusOneDayTerms { get; }

        public IImmutableList<string> PlusTwoDayTerms { get; }

        public IImmutableList<string> MinusTwoDayTerms { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public int GetSwiftMonth(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
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
            var trimmedText = text.Trim().ToLowerInvariant();
            return PreviousPrefixRegex.IsMatch(trimmedText);
        }

        public string Normalize(string text)
        {
            return text.Normalized(DateTimeDefinitions.SpecialCharactersEquivalent);
        }
    }
}
