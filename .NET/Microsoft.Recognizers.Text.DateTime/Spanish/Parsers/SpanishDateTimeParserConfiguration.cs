using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeParserConfiguration : BaseOptionsConfiguration, IDateTimeParserConfiguration
    {
        public SpanishDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            NowRegex = SpanishDateTimeExtractorConfiguration.NowRegex;
            AMTimeRegex = new Regex(DateTimeDefinitions.AmTimeRegex, RegexOptions.Singleline);
            PMTimeRegex = new Regex(DateTimeDefinitions.PmTimeRegex, RegexOptions.Singleline);
            SimpleTimeOfTodayAfterRegex = SpanishDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
            SimpleTimeOfTodayBeforeRegex = SpanishDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;
            SpecificTimeOfDayRegex = SpanishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            SpecificEndOfRegex = SpanishDateTimeExtractorConfiguration.SpecificEndOfRegex;
            UnspecificEndOfRegex = SpanishDateTimeExtractorConfiguration.UnspecificEndOfRegex;
            UnitRegex = SpanishDateTimeExtractorConfiguration.UnitRegex;
            DateNumberConnectorRegex = SpanishDateTimeExtractorConfiguration.DateNumberConnectorRegex;
            YearRegex = SpanishDateTimeExtractorConfiguration.YearRegex;
            Numbers = config.Numbers;
            CardinalExtractor = config.CardinalExtractor;
            IntegerExtractor = config.IntegerExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public string TokenBeforeDate { get; }

        public string TokenBeforeTime { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IExtractor CardinalExtractor { get; }

        public IExtractor IntegerExtractor { get; }

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

        public Regex SpecificEndOfRegex { get; }

        public Regex UnspecificEndOfRegex { get; }

        public Regex UnitRegex { get; }

        public Regex DateNumberConnectorRegex { get; }

        public Regex PrepositionRegex { get; }

        public Regex ConnectorRegex { get; }

        public Regex YearRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public int GetHour(string text, int hour)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            int result = hour;

            // TODO: Replace with a regex
            if ((trimmedText.EndsWith("mañana") || trimmedText.EndsWith("madrugada")) && hour >= Constants.HalfDayHourCount)
            {
                result -= Constants.HalfDayHourCount;
            }
            else if (!(trimmedText.EndsWith("mañana") || trimmedText.EndsWith("madrugada")) && hour < Constants.HalfDayHourCount)
            {
                result += Constants.HalfDayHourCount;
            }

            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            if (trimmedText.EndsWith("ahora") || trimmedText.EndsWith("mismo") || trimmedText.EndsWith("momento"))
            {
                timex = "PRESENT_REF";
            }
            else if (trimmedText.EndsWith("posible") || trimmedText.EndsWith("pueda") ||
                     trimmedText.EndsWith("puedas") || trimmedText.EndsWith("podamos") || trimmedText.EndsWith("puedan"))
            {
                timex = "FUTURE_REF";
            }
            else if (trimmedText.EndsWith("mente"))
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
            var trimmedText = text.Trim().ToLowerInvariant();
            var swift = 0;

            if (SpanishDatePeriodParserConfiguration.PreviousPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }
            else if (SpanishDatePeriodParserConfiguration.NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }

            return swift;
        }

        public bool ContainsAmbiguousToken(string text, string matchedText)
        {
            return text.Contains("esta mañana") && matchedText.Contains("mañana");
        }
    }
}
