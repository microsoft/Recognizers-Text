using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Turkish;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Turkish
{
    public class TurkishTimePeriodParserConfiguration : BaseOptionsConfiguration, ITimePeriodParserConfiguration
    {
        public TurkishTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TimeExtractor = config.TimeExtractor;
            IntegerExtractor = config.IntegerExtractor;
            TimeParser = config.TimeParser;
            TimeZoneParser = config.TimeZoneParser;

            PureNumberFromToRegex = TurkishTimePeriodExtractorConfiguration.PureNumFromTo;
            PureNumberBetweenAndRegex = TurkishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeFromToRegex = TurkishTimePeriodExtractorConfiguration.SpecificTimeFromTo;
            SpecificTimeBetweenAndRegex = TurkishTimePeriodExtractorConfiguration.SpecificTimeBetweenAnd;
            TimeOfDayRegex = TurkishTimePeriodExtractorConfiguration.TimeOfDayRegex;
            GeneralEndingRegex = TurkishTimePeriodExtractorConfiguration.GeneralEndingRegex;
            TillRegex = TurkishTimePeriodExtractorConfiguration.TillRegex;

            Numbers = config.Numbers;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeParser TimeParser { get; }

        public IExtractor IntegerExtractor { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public Regex SpecificTimeFromToRegex { get; }

        public Regex SpecificTimeBetweenAndRegex { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex GeneralEndingRegex { get; }

        public Regex TillRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public bool GetMatchedTimexRange(string text, out string timex, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim();
            if (trimmedText.EndsWith("s"))
            {
                trimmedText = trimmedText.Substring(0, trimmedText.Length - 1);
            }

            beginHour = 0;
            endHour = 0;
            endMin = 0;

            var timeOfDay = string.Empty;
            if (DateTimeDefinitions.MorningTermList.Any(o => trimmedText.EndsWith(o)))
            {
                timeOfDay = Constants.Morning;
            }
            else if (DateTimeDefinitions.AfternoonTermList.Any(o => trimmedText.EndsWith(o)))
            {
                timeOfDay = Constants.Afternoon;
            }
            else if (DateTimeDefinitions.EveningTermList.Any(o => trimmedText.EndsWith(o)))
            {
                timeOfDay = Constants.Evening;
            }
            else if (DateTimeDefinitions.DaytimeTermList.Any(o => trimmedText.Equals(o)))
            {
                timeOfDay = Constants.Daytime;
            }
            else if (DateTimeDefinitions.NightTermList.Any(o => trimmedText.EndsWith(o)))
            {
                timeOfDay = Constants.Night;
            }
            else if (DateTimeDefinitions.BusinessHourSplitStrings.All(o => trimmedText.Contains(o)))
            {
                timeOfDay = Constants.BusinessHour;
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
