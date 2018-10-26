using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchTimePeriodParserConfiguration : BaseOptionsConfiguration, ITimePeriodParserConfiguration
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

        public FrenchTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config)
        {
            TimeExtractor = config.TimeExtractor;
            IntegerExtractor = config.IntegerExtractor;
            TimeParser = config.TimeParser;
            PureNumberFromToRegex = FrenchTimePeriodExtractorConfiguration.PureNumFromTo; 
            PureNumberBetweenAndRegex = FrenchTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeFromToRegex = FrenchTimePeriodExtractorConfiguration.SpecificTimeFromTo;
            SpecificTimeBetweenAndRegex = FrenchTimePeriodExtractorConfiguration.SpecificTimeBetweenAnd;
            TimeOfDayRegex = FrenchTimePeriodExtractorConfiguration.TimeOfDayRegex;
            GeneralEndingRegex = FrenchTimePeriodExtractorConfiguration.GeneralEndingRegex;
            TillRegex = FrenchTimePeriodExtractorConfiguration.TillRegex;
            Numbers = config.Numbers;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public bool GetMatchedTimexRange(string text, out string timex, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            if (trimmedText.EndsWith("s"))
            {
                trimmedText = trimmedText.Substring(0, trimmedText.Length - 1);
            }

            beginHour = 0;
            endHour = 0;
            endMin = 0;

            var timeOfDay = "";
            if (trimmedText.EndsWith(DateTimeDefinitions.MorningTerm1) || trimmedText.EndsWith(DateTimeDefinitions.MorningTerm2)
                                                                       || trimmedText.EndsWith(DateTimeDefinitions.MorningTerm3))
            {
                timeOfDay = Constants.Morning;
            }
            else if (trimmedText.EndsWith(DateTimeDefinitions.AfternoonTerm1) || trimmedText.EndsWith(DateTimeDefinitions.AfternoonTerm2)
                || trimmedText.EndsWith(DateTimeDefinitions.AfternoonTerm3) || trimmedText.EndsWith(DateTimeDefinitions.AfternoonTerm4))
            {
                timeOfDay = Constants.Afternoon;
            }
            else if (trimmedText.EndsWith(DateTimeDefinitions.EveningTerm1) || trimmedText.EndsWith(DateTimeDefinitions.EveningTerm2)
                                                                            || trimmedText.EndsWith(DateTimeDefinitions.EveningTerm3))
            {
                timeOfDay = Constants.Evening;
            }    
            else if (trimmedText.Equals(DateTimeDefinitions.DaytimeTerm1) || trimmedText.EndsWith(DateTimeDefinitions.DaytimeTerm2)
                                                                          || trimmedText.EndsWith(DateTimeDefinitions.DaytimeTerm3))
            {
                timeOfDay = Constants.Daytime;
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
