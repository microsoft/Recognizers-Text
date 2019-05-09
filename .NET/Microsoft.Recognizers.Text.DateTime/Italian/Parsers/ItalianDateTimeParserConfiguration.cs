using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianDateTimeParserConfiguration : BaseOptionsConfiguration, IDateTimeParserConfiguration
    {
        public ItalianDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config.Options)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            NowRegex = ItalianDateTimeExtractorConfiguration.NowRegex;
            AMTimeRegex = new Regex(DateTimeDefinitions.AMTimeRegex, RegexOptions.Singleline);
            PMTimeRegex = new Regex(DateTimeDefinitions.PMTimeRegex, RegexOptions.Singleline);
            SimpleTimeOfTodayAfterRegex = ItalianDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
            SimpleTimeOfTodayBeforeRegex = ItalianDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;
            SpecificTimeOfDayRegex = ItalianDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            SpecificEndOfRegex = ItalianDateTimeExtractorConfiguration.SpecificEndOfRegex;
            UnspecificEndOfRegex = ItalianDateTimeExtractorConfiguration.UnspecificEndOfRegex;
            UnitRegex = ItalianTimeExtractorConfiguration.TimeUnitRegex;
            DateNumberConnectorRegex = ItalianDateTimeExtractorConfiguration.DateNumberConnectorRegex;
            YearRegex = ItalianDateTimeExtractorConfiguration.YearRegex;
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

        public Regex YearRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        // Note: Italian typically uses 24:00 time, consider removing 12:00 am/pm
        public int GetHour(string text, int hour)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            int result = hour;
            if (trimmedText.EndsWith("matin") && hour >= 12)
            {
                result -= 12;
            }
            else if (!trimmedText.EndsWith("matin") && hour < 12)
            {
                result += 12;
            }

            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            if (trimmedText.EndsWith("maintenant"))
            {
                timex = "PRESENT_REF";
            }
            else if (trimmedText.Equals("récemment") || trimmedText.Equals("précédemment") || trimmedText.Equals("auparavant"))
            {
                timex = "PAST_REF";
            }
            else if (trimmedText.Equals("dès que possible") || trimmedText.Equals("dqp"))
            {
                timex = "FUTURE_REF";
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
            if (trimmedText.StartsWith("prochain") || trimmedText.EndsWith("prochain") ||
                trimmedText.StartsWith("prochaine") || trimmedText.EndsWith("prochaine"))
            {
                swift = 1;
            }
            else if (trimmedText.StartsWith("dernier") || trimmedText.StartsWith("dernière") ||
                      trimmedText.EndsWith("dernier") || trimmedText.EndsWith("dernière"))
            {
                swift = -1;
            }

            return swift;
        }

        public bool ContainsAmbiguousToken(string text, string matchedText) => false;
    }
}
