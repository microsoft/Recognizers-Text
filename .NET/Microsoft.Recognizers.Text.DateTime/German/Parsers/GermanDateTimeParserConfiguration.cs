using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDateTimeParserConfiguration : BaseOptionsConfiguration, IDateTimeParserConfiguration
    {
        public GermanDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;

            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;

            NowRegex = GermanDateTimeExtractorConfiguration.NowRegex;

            AMTimeRegex = new Regex(DateTimeDefinitions.AMTimeRegex, RegexOptions.Singleline);
            PMTimeRegex = new Regex(DateTimeDefinitions.PMTimeRegex, RegexOptions.Singleline);

            SimpleTimeOfTodayAfterRegex = GermanDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
            SimpleTimeOfTodayBeforeRegex = GermanDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;
            SpecificTimeOfDayRegex = GermanDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            SpecificEndOfRegex = GermanDateTimeExtractorConfiguration.SpecificEndOfRegex;
            UnspecificEndOfRegex = GermanDateTimeExtractorConfiguration.UnspecificEndOfRegex;
            UnitRegex = GermanTimeExtractorConfiguration.TimeUnitRegex;
            DateNumberConnectorRegex = GermanDateTimeExtractorConfiguration.DateNumberConnectorRegex;
            YearRegex = GermanDateTimeExtractorConfiguration.YearRegex;

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
            var trimmedText = text.Trim().ToLowerInvariant();
            int result = hour;
            if ((trimmedText.EndsWith("morgen") || trimmedText.EndsWith("morgens")) && hour >= Constants.HalfDayHourCount)
            {
                result -= Constants.HalfDayHourCount;
            }
            else if (!(trimmedText.EndsWith("morgen") || trimmedText.EndsWith("morgens")) && hour < Constants.HalfDayHourCount)
            {
                result += Constants.HalfDayHourCount;
            }

            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            if (trimmedText.EndsWith("jetzt") || trimmedText.Equals("momentan") || trimmedText.Equals("gerade") || trimmedText.Equals("aktuell") ||
                trimmedText.Equals("im moment") || trimmedText.Equals("in diesem moment") || trimmedText.Equals("derzeit"))
            {
                timex = "PRESENT_REF";
            }
            else if (trimmedText.Equals("neulich") || trimmedText.Equals("vorher") || trimmedText.Equals("vorhin"))
            {
                timex = "PAST_REF";
            }
            else if (trimmedText.Equals("so früh wie möglich") || trimmedText.Equals("asap"))
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
            if (trimmedText.StartsWith("nächsten") || trimmedText.StartsWith("nächste") || trimmedText.StartsWith("nächstes") || trimmedText.StartsWith("nächster"))
            {
                swift = 1;
            }
            else if (trimmedText.StartsWith("letzten") || trimmedText.StartsWith("letzte") || trimmedText.StartsWith("letztes") || trimmedText.StartsWith("letzter"))
            {
                swift = -1;
            }

            return swift;
        }

        public bool ContainsAmbiguousToken(string text, string matchedText) => false;
    }
}
