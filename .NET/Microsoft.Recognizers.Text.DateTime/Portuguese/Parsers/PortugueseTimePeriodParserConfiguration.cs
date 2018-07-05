using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseTimePeriodParserConfiguration : BaseOptionsConfiguration, ITimePeriodParserConfiguration
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

        public PortugueseTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            TimeExtractor = config.TimeExtractor;
            IntegerExtractor = config.IntegerExtractor;
            TimeParser = config.TimeParser;
            PureNumberFromToRegex = PortugueseTimePeriodExtractorConfiguration.PureNumFromTo;
            PureNumberBetweenAndRegex = PortugueseTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeFromToRegex = PortugueseTimePeriodExtractorConfiguration.SpecificTimeFromTo;
            SpecificTimeBetweenAndRegex = PortugueseTimePeriodExtractorConfiguration.SpecificTimeBetweenAnd;
            TimeOfDayRegex = PortugueseTimePeriodExtractorConfiguration.TimeOfDayRegex;
            GeneralEndingRegex = PortugueseTimePeriodExtractorConfiguration.GeneralEndingRegex;
            TillRegex = PortugueseTimePeriodExtractorConfiguration.TillRegex;
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
            else if (trimedText.EndsWith("manha") || trimedText.EndsWith("manhã"))
            {
                timex = "TMO";
                beginHour = 8;
                endHour = Constants.HalfDayHourCount;
            }
            else if (trimedText.Contains("passado o meio dia") || trimedText.Contains("depois do meio dia"))
            {
                timex = "TAF";
                beginHour = Constants.HalfDayHourCount;
                endHour = 16;
            }
            else if (trimedText.EndsWith("tarde"))
            {
                timex = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (trimedText.EndsWith("noite"))
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
