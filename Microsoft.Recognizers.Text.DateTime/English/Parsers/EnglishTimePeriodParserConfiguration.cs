using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishTimePeriodParserConfiguration : ITimePeriodParserConfiguration
    {
        public IExtractor TimeExtractor { get; }

        public IDateTimeParser TimeParser { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public EnglishTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            TimeExtractor = config.TimeExtractor;
            TimeParser = config.TimeParser;
            PureNumberFromToRegex = EnglishTimePeriodExtractorConfiguration.PureNumFromTo;
            PureNumberBetweenAndRegex = EnglishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            Numbers = config.Numbers;
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
                endHour = 12;
            }
            else if (trimedText.EndsWith("afternoon"))
            {
                timex = "TAF";
                beginHour = 12;
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
