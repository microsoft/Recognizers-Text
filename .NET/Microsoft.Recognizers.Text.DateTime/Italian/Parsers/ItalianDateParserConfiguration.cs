using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianDateParserConfiguration : BaseOptionsConfiguration, IDateParserConfiguration
    {
        public ItalianDateParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config.Options)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DateExtractor = config.DateExtractor;
            DurationParser = config.DurationParser;
            DateRegexes = new ItalianDateExtractorConfiguration(this).DateRegexList;
            OnRegex = ItalianDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = ItalianDateExtractorConfiguration.SpecialDayRegex;
            SpecialDayWithNumRegex = ItalianDateExtractorConfiguration.SpecialDayWithNumRegex;
            NextRegex = ItalianDateExtractorConfiguration.NextRegex;
            ThisRegex = ItalianDateExtractorConfiguration.ThisRegex;
            LastRegex = ItalianDateExtractorConfiguration.LastRegex;
            UnitRegex = ItalianDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = ItalianDateExtractorConfiguration.WeekDayRegex;
            StrictWeekDay = ItalianDateExtractorConfiguration.StrictWeekDay;
            MonthRegex = ItalianDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = ItalianDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = ItalianDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = ItalianDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            WeekDayAndDayRegex = ItalianDateExtractorConfiguration.WeekDayAndDayRegex;
            RelativeMonthRegex = ItalianDateExtractorConfiguration.RelativeMonthRegex;
            YearSuffix = ItalianDateExtractorConfiguration.YearSuffix;
            RelativeWeekDayRegex = ItalianDateExtractorConfiguration.RelativeWeekDayRegex;
            RelativeDayRegex = new Regex(DateTimeDefinitions.RelativeDayRegex, RegexOptions.Singleline);
            NextPrefixRegex = new Regex(DateTimeDefinitions.NextPrefixRegex, RegexOptions.Singleline);
            PreviousPrefixRegex = new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexOptions.Singleline);
            UpcomingPrefixRegex = new Regex(DateTimeDefinitions.UpcomingPrefixRegex, RegexOptions.Singleline);
            PastPrefixRegex = new Regex(DateTimeDefinitions.PastPrefixRegex, RegexOptions.Singleline);
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

        public Regex StrictWeekDay { get; }

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
            if (trimmedText.EndsWith("prochaine") || trimmedText.EndsWith("prochain"))
            {
                swift = 1;
            }
            else if (trimmedText.Equals("dernière") || trimmedText.Equals("dernières") ||
                    trimmedText.Equals("derniere") || trimmedText.Equals("dernieres"))
            {
                swift = -1;
            }

            return swift;
        }

        public bool IsCardinalLast(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return trimmedText.Equals("dernière") || trimmedText.Equals("dernières") ||
                    trimmedText.Equals("derniere") || trimmedText.Equals("dernieres");
        }

        public string Normalize(string text)
        {
            return text;
        }
    }
}
