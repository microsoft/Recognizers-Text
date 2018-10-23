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
            if (trimmedText.EndsWith(Constants.EN_MORNING))
            {
                timex = DateTimeDefinitions.TimeOfDayTimex[Constants.EN_MORNING];
                beginHour = DateTimeDefinitions.TimeOfDayBeginHour[Constants.EN_MORNING];
                endHour = DateTimeDefinitions.TimeOfDayEndHour[Constants.EN_MORNING];
            }
            else if (trimmedText.EndsWith(Constants.EN_AFTERNOON))
            {
                timex = DateTimeDefinitions.TimeOfDayTimex[Constants.EN_AFTERNOON];
                beginHour = DateTimeDefinitions.TimeOfDayBeginHour[Constants.EN_AFTERNOON];
                endHour = DateTimeDefinitions.TimeOfDayEndHour[Constants.EN_AFTERNOON];
            }
            else if (trimmedText.EndsWith(Constants.EN_EVENING))
            {
                timex = DateTimeDefinitions.TimeOfDayTimex[Constants.EN_EVENING];
                beginHour = DateTimeDefinitions.TimeOfDayBeginHour[Constants.EN_EVENING];
                endHour = DateTimeDefinitions.TimeOfDayEndHour[Constants.EN_EVENING];
            }
            else if (trimmedText.Equals(Constants.EN_DAYTIME))
            {
                timex = DateTimeDefinitions.TimeOfDayTimex[Constants.EN_DAYTIME];
                beginHour = DateTimeDefinitions.TimeOfDayBeginHour[Constants.EN_DAYTIME];
                endHour = DateTimeDefinitions.TimeOfDayEndHour[Constants.EN_DAYTIME];
            }
            else if (trimmedText.EndsWith(Constants.EN_NIGHT))
            {
                timex = DateTimeDefinitions.TimeOfDayTimex[Constants.EN_NIGHT];
                beginHour = DateTimeDefinitions.TimeOfDayBeginHour[Constants.EN_NIGHT];
                endHour = DateTimeDefinitions.TimeOfDayEndHour[Constants.EN_NIGHT];
                endMin = DateTimeDefinitions.TimeOfDayEndMin[Constants.EN_NIGHT];
            }
            else if (DateTimeDefinitions.BusinessHourSplitStrings.All(o => trimmedText.Contains(o)))
            {
                timex = DateTimeDefinitions.TimeOfDayTimex[Constants.EN_BUSINESS_HOUR];
                beginHour = DateTimeDefinitions.TimeOfDayBeginHour[Constants.EN_BUSINESS_HOUR];
                endHour = DateTimeDefinitions.TimeOfDayEndHour[Constants.EN_BUSINESS_HOUR];
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
