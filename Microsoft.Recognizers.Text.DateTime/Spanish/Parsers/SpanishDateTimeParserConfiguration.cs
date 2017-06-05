using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeParserConfiguration : IDateTimeParserConfiguration
    {
        public string TokenBeforeDate { get; }

        public string TokenBeforeTime { get; }

        public IExtractor DateExtractor { get; }

        public IExtractor TimeExtractor { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public Regex NowRegex { get; }

        public Regex AMTimeRegex { get; }

        public Regex PMTimeRegex { get; }

        public Regex SimpleTimeOfTodayAfterRegex { get; }

        public Regex SimpleTimeOfTodayBeforeRegex { get; }

        public Regex SpecificNightRegex { get; }

        public Regex TheEndOfRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public SpanishDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            TokenBeforeDate = "el ";
            TokenBeforeTime = "la ";
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            NowRegex = SpanishDateTimeExtractorConfiguration.NowRegex;
            AMTimeRegex = new Regex(@"(?<am>(esta|(por|de|a|en)\s+la)\s+(mañana|madrugada))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PMTimeRegex = new Regex(@"(?<pm>(esta|(por|de|a|en)\s+la)\s+(tarde|noche))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            SimpleTimeOfTodayAfterRegex = SpanishDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
            SimpleTimeOfTodayBeforeRegex = SpanishDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;
            SpecificNightRegex = SpanishDateTimeExtractorConfiguration.SpecificNightRegex;
            TheEndOfRegex = SpanishDateTimeExtractorConfiguration.TheEndOfRegex;
            Numbers = config.Numbers;
        }

        public int GetHour(string text, int hour)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            int result = hour;

            //TODO: Replace with a regex
            if ((trimedText.EndsWith("mañana") || trimedText.EndsWith("madrugada")) && hour >= 12)
            {
                result -= 12;
            }

            //TODO: Replace with a regex
            else if (!(trimedText.EndsWith("mañana") || trimedText.EndsWith("madrugada")) && hour < 12)
            {
                result += 12;
            }
            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            if (trimedText.EndsWith("ahora") || trimedText.EndsWith("mismo") || trimedText.EndsWith("momento"))
            {
                timex = "PRESENT_REF";
            }
            else if (trimedText.EndsWith("posible") || trimedText.EndsWith("pueda") ||
                     trimedText.EndsWith("puedas") || trimedText.EndsWith("podamos") || trimedText.EndsWith("puedan"))
            {
                timex = "FUTURE_REF";
            }
            else if (trimedText.EndsWith("mente"))
            {
                timex = "PAST_REF";
            }
            else
            {
                timex = null;
                return false;
            }
            return true;
        }

        public int GetSwiftDay(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;

            //TODO: Replace with a regex
            if (trimedText.StartsWith("ultimo") || trimedText.StartsWith("último") ||
                trimedText.StartsWith("ultima") || trimedText.StartsWith("última"))
            {
                swift = -1;
            }

            //TODO: Replace with a regex
            else if (trimedText.StartsWith("proximo") || trimedText.StartsWith("próximo") ||
                     trimedText.StartsWith("proxima") || trimedText.StartsWith("próxima") ||
                     trimedText.StartsWith("siguiente"))
            {
                swift = 1;
            }
            return swift;
        }

        public bool HaveAmbiguousToken(string text, string matchedText)
        {
            return text.Contains("esta mañana") && matchedText.Contains("mañana");
        }
    }
}
