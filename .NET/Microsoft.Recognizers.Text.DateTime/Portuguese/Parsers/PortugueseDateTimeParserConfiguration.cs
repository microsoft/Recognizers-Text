using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDateTimeParserConfiguration : BaseOptionsConfiguration, IDateTimeParserConfiguration
    {
        public PortugueseDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            NowRegex = PortugueseDateTimeExtractorConfiguration.NowRegex;
            AMTimeRegex = new Regex(DateTimeDefinitions.AmTimeRegex, RegexOptions.Singleline);
            PMTimeRegex = new Regex(DateTimeDefinitions.PmTimeRegex, RegexOptions.Singleline);
            SimpleTimeOfTodayAfterRegex = PortugueseDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
            SimpleTimeOfTodayBeforeRegex = PortugueseDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;
            SpecificTimeOfDayRegex = PortugueseDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            SpecificEndOfRegex = PortugueseDateTimeExtractorConfiguration.SpecificEndOfRegex;
            UnspecificEndOfRegex = PortugueseDateTimeExtractorConfiguration.UnspecificEndOfRegex;
            UnitRegex = PortugueseDateTimeExtractorConfiguration.UnitRegex;
            DateNumberConnectorRegex = PortugueseDateTimeExtractorConfiguration.DateNumberConnectorRegex;
            YearRegex = PortugueseDateTimeExtractorConfiguration.YearRegex;
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

        public Regex YearRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public int GetHour(string text, int hour)
        {
            var trimmedText = text.Trim().ToLowerInvariant().Normalized(DateTimeDefinitions.SpecialCharactersEquivalent);
            int result = hour;

            // TODO: Replace with a regex
            if ((trimmedText.EndsWith("manha") || trimmedText.EndsWith("madrugada")) && hour >= Constants.HalfDayHourCount)
            {
                result -= Constants.HalfDayHourCount;
            }
            else if (!(trimmedText.EndsWith("manha") || trimmedText.EndsWith("madrugada")) && hour < Constants.HalfDayHourCount)
            {
                result += Constants.HalfDayHourCount;
            }

            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim().ToLowerInvariant().Normalized(DateTimeDefinitions.SpecialCharactersEquivalent);

            if (trimmedText.EndsWith("agora") || trimmedText.EndsWith("mesmo") || trimmedText.EndsWith("momento"))
            {
                timex = "PRESENT_REF";
            }
            else if (trimmedText.EndsWith("possivel") || trimmedText.EndsWith("possa") ||
                     trimmedText.EndsWith("possas") || trimmedText.EndsWith("possamos") || trimmedText.EndsWith("possam"))
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

            if (PortugueseDatePeriodParserConfiguration.PreviousPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }
            else if (PortugueseDatePeriodParserConfiguration.NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }

            return swift;
        }

        public bool ContainsAmbiguousToken(string text, string matchedText)
        {
            return false;
        }
    }
}
