using Microsoft.Recognizers.Text.DateTime.Parsers;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Spanish.Extractors;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Parsers
{
    public class SpanishDateTimePeriodParserConfiguration : IDateTimePeriodParserConfiguration
    {
        public IExtractor DateExtractor { get; }

        public IExtractor TimeExtractor { get; }

        public IExtractor DateTimeExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public Regex SpecificNightRegex { get; }

        public Regex NightRegex { get; }

        public Regex PastRegex { get; }

        public Regex FutureRegex { get; }

        public Regex NumberCombinedWithUnitRegex { get; }

        public Regex UnitRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public SpanishDateTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateTimeExtractor = config.DateTimeExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            DateTimeParser = config.DateTimeParser;
            PureNumberFromToRegex = SpanishTimePeriodExtractorConfiguration.PureNumFromTo;
            PureNumberBetweenAndRegex = SpanishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificNightRegex = SpanishDateTimeExtractorConfiguration.SpecificNightRegex;
            NightRegex = SpanishDateTimeExtractorConfiguration.NightRegex;
            PastRegex = SpanishDatePeriodExtractorConfiguration.PastRegex;
            FutureRegex = SpanishDatePeriodExtractorConfiguration.FutureRegex;
            NumberCombinedWithUnitRegex = SpanishDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit;
            UnitRegex = SpanishTimePeriodExtractorConfiguration.UnitRegex;
            UnitMap = config.UnitMap;
            Numbers = config.Numbers;
        }

        public bool GetMatchedTimeRange(string text, out string timeStr, out int beginHour, out int endHour, out int endMin)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            beginHour = 0;
            endHour = 0;
            endMin = 0;
            if (trimedText.EndsWith("madrugada"))
            {
                timeStr = "TDA";
                beginHour = 4;
                endHour = 8;
            }
            else if (trimedText.EndsWith("mañana"))
            {
                timeStr = "TMO";
                beginHour = 8;
                endHour = 12;
            }
            else if (trimedText.Contains("pasado mediodia") || trimedText.Contains("pasado el mediodia"))
            {
                timeStr = "TAF";
                beginHour = 12;
                endHour = 16;
            }
            else if (trimedText.EndsWith("tarde"))
            {
                timeStr = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (trimedText.EndsWith("noche"))
            {
                timeStr = "TNI";
                beginHour = 20;
                endHour = 23;
                endMin = 59;
            }
            else
            {
                timeStr = null;
                return false;
            }
            return true;
        }

        public int GetSwiftPrefix(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;

            //TODO: Replace with a regex
            if (trimedText.StartsWith("ultimo") || trimedText.StartsWith("último") ||
                trimedText.StartsWith("ultima") || trimedText.StartsWith("última") ||
                trimedText.Equals("anoche"))
            {
                swift = -1;
            }

            //TODO: Replace with a regex
            else if (trimedText.StartsWith("proximo") || trimedText.StartsWith("próximo") ||
                     trimedText.StartsWith("proxima") || trimedText.StartsWith("próxima") ||
                     trimedText.StartsWith("siguiente"))
            {
                swift = 1;
            }
            return swift;
        }
    }
}
