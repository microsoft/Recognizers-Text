using System.Collections.Immutable;
using System.Text.RegularExpressions;

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

        public EnglishTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
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
            var trimedText = text.Trim().ToLowerInvariant();
            if (trimedText.EndsWith("s"))
            {
                trimedText = trimedText.Substring(0, trimedText.Length - 1);
            }

            beginHour = 0;
            endHour = 0;
            endMin = 0;
            if (trimedText.EndsWith("morning"))
            {
                timex = "TMO";
                beginHour = 8;
                endHour = Constants.HalfDayHourCount;
            }
            else if (trimedText.EndsWith("afternoon"))
            {
                timex = "TAF";
                beginHour = Constants.HalfDayHourCount;
                endHour = 16;
            }
            else if (trimedText.EndsWith("evening"))
            {
                timex = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (trimedText.Equals("daytime"))
            {
                timex = "TDT";
                beginHour = 8;
                endHour = 18;
            }
            else if (trimedText.EndsWith("night"))
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
