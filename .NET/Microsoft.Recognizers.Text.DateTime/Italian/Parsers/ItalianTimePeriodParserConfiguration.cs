using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianTimePeriodParserConfiguration : BaseOptionsConfiguration, ITimePeriodParserConfiguration
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

        public ItalianTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            TimeExtractor = config.TimeExtractor;
            IntegerExtractor = config.IntegerExtractor;
            TimeParser = config.TimeParser;
            PureNumberFromToRegex = ItalianTimePeriodExtractorConfiguration.PureNumFromTo; 
            PureNumberBetweenAndRegex = ItalianTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeFromToRegex = ItalianTimePeriodExtractorConfiguration.SpecificTimeFromTo;
            SpecificTimeBetweenAndRegex = ItalianTimePeriodExtractorConfiguration.SpecificTimeBetweenAnd;
            TimeOfDayRegex = ItalianTimePeriodExtractorConfiguration.TimeOfDayRegex;
            GeneralEndingRegex = ItalianTimePeriodExtractorConfiguration.GeneralEndingRegex;
            TillRegex = ItalianTimePeriodExtractorConfiguration.TillRegex;
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

            if (trimmedText.EndsWith("matinee") || trimmedText.EndsWith("matin") || trimmedText.EndsWith("matinée"))
            {
                timex = "TMO";
                beginHour = 8;
                endHour = 12;
            }
            else if (trimmedText.EndsWith("apres-midi")||trimmedText.EndsWith("apres midi") 
                || trimmedText.EndsWith("après midi") || trimmedText.EndsWith("après-midi"))
            {
                timex = "TAF";
                beginHour = 12;
                endHour = 16;
            } 
            else if (trimmedText.EndsWith("soir") || trimmedText.EndsWith("soiree") || trimmedText.EndsWith("soirée"))
            {
                timex = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (trimmedText.Equals("jour") || trimmedText.EndsWith("journee") || trimmedText.EndsWith("journée"))
            {
                timex = "TDT";
                beginHour = 8;
                endHour = 18;
            }
            else if (trimmedText.EndsWith("nuit"))
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
