using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishTimePeriodParserConfiguration : BaseOptionsConfiguration, ITimePeriodParserConfiguration
    {
        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeParser TimeParser { get; }

        public IExtractor IntegerExtractor { get; }

        public Regex SpecificTimeFromToRegex { get; }

        public Regex SpecificTimeBetweenAndRegex { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex GeneralEndingRegex { get; }

        public Regex TillRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public EnglishTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config)
        {
            TimeExtractor = config.TimeExtractor;
            IntegerExtractor = config.IntegerExtractor;
            TimeParser = config.TimeParser;
            PureNumberFromToRegex = EnglishTimePeriodExtractorConfiguration.PureNumFromTo;
            PureNumberBetweenAndRegex = EnglishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeFromToRegex = EnglishTimePeriodExtractorConfiguration.SpecificTimeFromTo;
            SpecificTimeBetweenAndRegex = EnglishTimePeriodExtractorConfiguration.SpecificTimeBetweenAnd;
            TimeOfDayRegex = EnglishTimePeriodExtractorConfiguration.TimeOfDayRegex;
            GeneralEndingRegex = EnglishTimePeriodExtractorConfiguration.GeneralEndingRegex;
            TillRegex = EnglishTimePeriodExtractorConfiguration.TillRegex;
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
            if (trimmedText.EndsWith(DateTimeDefinitions.Morning))
            {
                timeOfDay = DateTimeDefinitions.Morning;
            }
            else if (trimmedText.EndsWith(DateTimeDefinitions.Afternoon))
            {
                timeOfDay = DateTimeDefinitions.Afternoon;
            }
            else if (trimmedText.EndsWith(DateTimeDefinitions.Evening))
            {
                timeOfDay = DateTimeDefinitions.Evening;
            }
            else if (trimmedText.Equals(DateTimeDefinitions.Daytime))
            {
                timeOfDay = DateTimeDefinitions.Daytime;
            }
            else if (trimmedText.EndsWith(DateTimeDefinitions.Night))
            {
                timeOfDay = DateTimeDefinitions.Night;
            }
            else if (DateTimeDefinitions.BusinessHourSplitStrings.All(o => trimmedText.Contains(o)))
            {
                timeOfDay = DateTimeDefinitions.BusinessHour;
            }
            else
            {
                timex = null;
                return false;
            }

            timex = DateTimeDefinitions.TimeOfDayTimex[timeOfDay];
            var parseResult = TimexUtility.ParseTimeOfDay(timex);
            beginHour = parseResult.BeginHour;
            endHour = parseResult.EndHour;
            endMin = parseResult.EndMin;

            return true;
        }
    }
}
