using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeParserConfiguration : IDateTimeParserConfiguration
    {
        public string TokenBeforeDate { get; }

        public string TokenBeforeTime { get; }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public Regex NowRegex { get; }

        public Regex AMTimeRegex { get; }

        public Regex PMTimeRegex { get; }

        public Regex SimpleTimeOfTodayAfterRegex { get; }

        public Regex SimpleTimeOfTodayBeforeRegex { get; }

        public Regex SpecificTimeOfDayRegex { get; }

        public Regex TheEndOfRegex { get; }

        public Regex UnitRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public SpanishDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            NowRegex = SpanishDateTimeExtractorConfiguration.NowRegex;
            AMTimeRegex = new Regex(DateTimeDefinitions.AmTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PMTimeRegex = new Regex(DateTimeDefinitions.PmTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            SimpleTimeOfTodayAfterRegex = SpanishDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
            SimpleTimeOfTodayBeforeRegex = SpanishDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;
            SpecificTimeOfDayRegex = SpanishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            TheEndOfRegex = SpanishDateTimeExtractorConfiguration.TheEndOfRegex;
            UnitRegex = SpanishDateTimeExtractorConfiguration.UnitRegex;
            Numbers = config.Numbers;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
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

            if (SpanishDatePeriodParserConfiguration.PastPrefixRegex.IsMatch(trimedText))
            {
                swift = -1;
            }
            else if (SpanishDatePeriodParserConfiguration.NextPrefixRegex.IsMatch(trimedText))
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
