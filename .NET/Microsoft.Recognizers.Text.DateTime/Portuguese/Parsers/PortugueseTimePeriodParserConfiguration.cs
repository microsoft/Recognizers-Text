using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;
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

        public PortugueseTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config)
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
            var trimmedText = text.Trim().ToLowerInvariant();

            beginHour = 0;
            endHour = 0;
            endMin = 0;

            var timeOfDay = "";
            if (trimmedText.EndsWith(DateTimeDefinitions.EarlyMorningTerm))
            {
                timeOfDay = Constants.EarlyMorning;
            }
            else if (trimmedText.EndsWith(DateTimeDefinitions.MorningTerm1) || trimmedText.EndsWith(DateTimeDefinitions.MorningTerm2))
            {
                timeOfDay = Constants.Morning;
            }
            else if (trimmedText.EndsWith(DateTimeDefinitions.AfternoonTerm1) || trimmedText.EndsWith(DateTimeDefinitions.AfternoonTerm2))
            {
                timeOfDay = Constants.Afternoon;
            }
            else if (trimmedText.EndsWith(DateTimeDefinitions.EveningTerm))
            {
                timeOfDay = Constants.Evening;
            }
            else if (trimmedText.EndsWith(DateTimeDefinitions.NightTerm))
            {
                timeOfDay = Constants.Night;
            }
            else
            {
                timex = null;
                return false;
            }

            var parseResult = TimexUtility.ParseTimeOfDay(timeOfDay);
            timex = parseResult.Timex;
            beginHour = parseResult.BeginHour;
            endHour = parseResult.EndHour;
            endMin = parseResult.EndMin;

            return true;
        }
    }
}
