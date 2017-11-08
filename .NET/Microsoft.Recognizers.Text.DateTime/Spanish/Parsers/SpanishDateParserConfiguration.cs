using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateParserConfiguration : IDateParserConfiguration
    {
        public string DateTokenPrefix { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IEnumerable<Regex> DateRegexes { get; }

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

        //TODO: implement the relative day regex if needed. If yes, they should be abstracted
        public static readonly Regex RelativeDayRegex = new Regex("", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextPrefixRegex = new Regex(DateTimeDefinitions.NextPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastPrefixRegex = new Regex(DateTimeDefinitions.PastPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public SpanishDateParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;
            DateRegexes = SpanishDateExtractorConfiguration.DateRegexList;
            OnRegex = SpanishDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = SpanishDateExtractorConfiguration.SpecialDayRegex;
            NextRegex = SpanishDateExtractorConfiguration.NextDateRegex;
            ThisRegex = SpanishDateExtractorConfiguration.ThisRegex;
            LastRegex = SpanishDateExtractorConfiguration.LastDateRegex;
            UnitRegex = SpanishDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = SpanishDateExtractorConfiguration.WeekDayRegex;
            MonthRegex = SpanishDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = SpanishDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = SpanishDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = SpanishDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            RelativeMonthRegex = SpanishDateExtractorConfiguration.RelativeMonthRegex;
            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;
            CardinalMap = config.CardinalMap;
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public int GetSwiftDay(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant().Normalized();
            var swift = 0;

            //TODO: add the relative day logic if needed. If yes, the whole method should be abstracted.
            if (trimedText.Equals("hoy") || trimedText.Equals("el dia"))
            {
                swift = 0;
            }
            else if (trimedText.Equals("mañana") ||
                     trimedText.EndsWith("dia siguiente") ||
                     trimedText.EndsWith("el dia de mañana") ||
                     trimedText.EndsWith("proximo dia"))
            {
                swift = 1;
            }
            else if (trimedText.Equals("ayer"))
            {
                swift = -1;
            }
            else if (trimedText.EndsWith("pasado mañana") ||
                     trimedText.EndsWith("dia despues de mañana"))
            {
                swift = 2;
            }
            else if (trimedText.EndsWith("anteayer") ||
                     trimedText.EndsWith("dia antes de ayer"))
            {
                swift = -2;
            }
            else if (trimedText.EndsWith("ultimo dia"))
            {
                swift = -1;
            }

            return swift;
        }

        public int GetSwiftMonth(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;

            if (NextPrefixRegex.IsMatch(trimedText))
            {
                swift = 1;
            }

            if (PastPrefixRegex.IsMatch(trimedText))
            {
                swift = -1;
            }

            return swift;
        }

        public bool IsCardinalLast(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return PastPrefixRegex.IsMatch(trimedText);
        }
    }

    public static class StringExtension
    {
        public static string Normalized(this string text)
        {
            return text
                .Replace('á', 'a')
                .Replace('é', 'e')
                .Replace('í', 'i')
                .Replace('ó', 'o')
                .Replace('ú', 'u');
        }
    }
}
