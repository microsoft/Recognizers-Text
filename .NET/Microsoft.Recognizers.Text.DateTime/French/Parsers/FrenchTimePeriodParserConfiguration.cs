using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;
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

        public FrenchTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
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
            var trimedText = text.Trim().ToLowerInvariant();
            if (trimedText.EndsWith("s"))
            {
                trimedText = trimedText.Substring(0, trimedText.Length - 1);
            }

            beginHour = 0;
            endHour = 0;
            endMin = 0;

            if (trimedText.EndsWith("matinee") || trimedText.EndsWith("matin") || trimedText.EndsWith("matinée"))
            {
                timex = "TMO";
                beginHour = 8;
                endHour = 12;
            }
            else if (trimedText.EndsWith("apres-midi")||trimedText.EndsWith("apres midi") 
                || trimedText.EndsWith("après midi") || trimedText.EndsWith("après-midi"))
            {
                timex = "TAF";
                beginHour = 12;
                endHour = 16;
            } 
            else if (trimedText.EndsWith("soir") || trimedText.EndsWith("soiree") || trimedText.EndsWith("soirée"))
            {
                timex = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (trimedText.Equals("jour") || trimedText.EndsWith("journee") || trimedText.EndsWith("journée"))
            {
                timex = "TDT";
                beginHour = 8;
                endHour = 18;
            }
            else if (trimedText.EndsWith("nuit"))
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
