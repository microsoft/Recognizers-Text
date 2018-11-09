using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDateParserConfiguration : BaseOptionsConfiguration, IDateParserConfiguration
    {
        public string DateTokenPrefix { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeParser DurationParser { get; }

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

        public Regex RelativeMonthRegex { get; }

        public Regex YearSuffix { get; }

        public Regex RelativeWeekDayRegex { get; }

        public static readonly Regex RelativeDayRegex= new Regex(
                DateTimeDefinitions.RelativeDayRegex,
                RegexOptions.Singleline);

        public static readonly Regex NextPrefixRegex = new Regex(
                DateTimeDefinitions.NextPrefixRegex,
                RegexOptions.Singleline);

        public static readonly Regex PastPrefixRegex = new Regex(
                DateTimeDefinitions.PastPrefixRegex,
                RegexOptions.Singleline);

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public GermanDateParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DateExtractor = config.DateExtractor;
            DurationParser = config.DurationParser;
            DateRegexes = new GermanDateExtractorConfiguration(this).DateRegexList;
            OnRegex = GermanDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = GermanDateExtractorConfiguration.SpecialDayRegex;
            SpecialDayWithNumRegex = GermanDateExtractorConfiguration.SpecialDayWithNumRegex;
            NextRegex = GermanDateExtractorConfiguration.NextDateRegex;
            ThisRegex = GermanDateExtractorConfiguration.ThisRegex;
            LastRegex = GermanDateExtractorConfiguration.LastDateRegex;
            UnitRegex = GermanDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = GermanDateExtractorConfiguration.WeekDayRegex;
            MonthRegex = GermanDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = GermanDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = GermanDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = GermanDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            RelativeMonthRegex = GermanDateExtractorConfiguration.RelativeMonthRegex;
            YearSuffix = GermanDateExtractorConfiguration.YearSuffix;
            RelativeWeekDayRegex = GermanDateExtractorConfiguration.RelativeWeekDayRegex;
            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;
            CardinalMap = config.CardinalMap;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public int GetSwiftDay(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            var swift = 0;

            var match = RelativeDayRegex.Match(text);

            if (trimmedText.Equals("heute"))
            {
                swift = 0;
            }
            else if (trimmedText.Equals("morgen") || trimmedText.Equals("tmr"))
            {
                swift = 1;
            }
            else if (trimmedText.Equals("gestern"))
            {
                swift = -1;
            }
            else if (trimmedText.EndsWith("übermorgen"))
            {
                swift = 2;
            }
            else if (trimmedText.EndsWith("vorgestern"))
            {
                swift = -2;
            }
            else if (match.Success)
            {
                swift = GetSwift(text);
            }
            return swift;
        }

        public int GetSwiftMonth(string text)
        {
            return GetSwift(text);
        }

        public int GetSwift(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            var swift = 0;
            if (NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }
            else if (PastPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }
            return swift;
        }

        public bool IsCardinalLast(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return trimmedText.Equals("letzten");
        }
    }
}
