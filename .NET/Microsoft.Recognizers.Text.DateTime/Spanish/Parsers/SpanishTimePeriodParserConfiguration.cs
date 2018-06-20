using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishTimePeriodParserConfiguration : BaseOptionsConfiguration, ITimePeriodParserConfiguration
    {
        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeParser TimeParser { get; }

        public IExtractor IntegerExtractor { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public Regex SpecificTimeFromToRegex { get; }

        public Regex SpecificTimeBetweenAndRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex GeneralEndingRegex { get; }

        public Regex TillRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public SpanishTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            TimeExtractor = config.TimeExtractor;
            IntegerExtractor = config.IntegerExtractor;
            TimeParser = config.TimeParser;
            PureNumberFromToRegex = SpanishTimePeriodExtractorConfiguration.PureNumFromTo;
            PureNumberBetweenAndRegex = SpanishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeFromToRegex = SpanishTimePeriodExtractorConfiguration.SpecificTimeFromTo;
            SpecificTimeBetweenAndRegex = SpanishTimePeriodExtractorConfiguration.SpecificTimeBetweenAnd;
            TimeOfDayRegex = SpanishTimePeriodExtractorConfiguration.TimeOfDayRegex;
            GeneralEndingRegex = SpanishTimePeriodExtractorConfiguration.GeneralEndingRegex;
            TillRegex = SpanishTimePeriodExtractorConfiguration.TillRegex;
            Numbers = config.Numbers;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public bool GetMatchedTimexRange(string text, out string timex, out int beginHour, out int endHour, out int endMin)
        {
            var trimedText = text.Trim().ToLowerInvariant();

            beginHour = 0;
            endHour = 0;
            endMin = 0;

            if (trimedText.EndsWith("madrugada"))
            {
                timex = "TDA";
                beginHour = 4;
                endHour = 8;
            }
            else if (trimedText.EndsWith("mañana"))
            {
                timex = "TMO";
                beginHour = 8;
                endHour = 12;
            }
            else if (trimedText.Contains("pasado mediodia") || trimedText.Contains("pasado el mediodia"))
            {
                timex = "TAF";
                beginHour = 12;
                endHour = 16;
            }
            else if (trimedText.EndsWith("tarde"))
            {
                timex = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (trimedText.EndsWith("noche"))
            {
                timex = "TNI";
                beginHour = 20;
                endHour = 23;
                endMin = 59;
            }
            else
            {
                timex = null;
                return false;
            }

            return true;
        }
    }
}
