using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateParserConfiguration : IDateParserConfiguration
    {
        public string DateTokenPrefix { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IEnumerable<Regex> DateRegexes { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public Regex OnRegex { get; }

        public Regex SpecialDayRegex { get; }

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

        //The following three regexes only used in this configuration
        //They are not used in the base parser, therefore they are not extracted
        //If the spanish date parser need the same regexes, they should be extracted
        public static readonly Regex RelativeDayRegex= new Regex(
                DateTimeDefinitions.RelativeDayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextPrefixRegex = new Regex(
                DateTimeDefinitions.NextPrefixRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastPrefixRegex = new Regex(
                DateTimeDefinitions.PastPrefixRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public EnglishDateParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            DateRegexes = EnglishDateExtractorConfiguration.DateRegexList;
            OnRegex = EnglishDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = EnglishDateExtractorConfiguration.SpecialDayRegex;
            NextRegex = EnglishDateExtractorConfiguration.NextDateRegex;
            ThisRegex = EnglishDateExtractorConfiguration.ThisRegex;
            LastRegex = EnglishDateExtractorConfiguration.LastDateRegex;
            UnitRegex = EnglishDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = EnglishDateExtractorConfiguration.WeekDayRegex;
            MonthRegex = EnglishDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = EnglishDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = EnglishDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = EnglishDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            RelativeMonthRegex = EnglishDateExtractorConfiguration.RelativeMonthRegex;
            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;
            CardinalMap = config.CardinalMap;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public int GetSwiftDay(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;

            var match = RelativeDayRegex.Match(text);

            if (trimedText.Equals("today"))
            {
                swift = 0;
            }
            else if (trimedText.Equals("tomorrow") || trimedText.Equals("tmr"))
            {
                swift = 1;
            }
            else if (trimedText.Equals("yesterday"))
            {
                swift = -1;
            }
            else if (trimedText.EndsWith("day after tomorrow") ||
                     trimedText.EndsWith("day after tmr"))
            {
                swift = 2;
            }
            else if (trimedText.EndsWith("day before yesterday"))
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
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;
            if (NextPrefixRegex.IsMatch(trimedText))
            {
                swift = 1;
            }
            else if (PastPrefixRegex.IsMatch(trimedText))
            {
                swift = -1;
            }
            return swift;
        }

        public bool IsCardinalLast(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.Equals("last");
        }
    }
}
