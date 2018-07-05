using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateTimeParserConfiguration : BaseOptionsConfiguration, IDateTimeParserConfiguration
    {
        public string TokenBeforeDate { get; }

        public string TokenBeforeTime { get; }

        public IDateTimeExtractor DateExtractor { get; }

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

        public Regex TheEndOfRegex { get; }

        public Regex UnitRegex { get; }

        public Regex DateNumberConnectorRegex { get; }

        public Regex PrepositionRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public FrenchDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            NowRegex = FrenchDateTimeExtractorConfiguration.NowRegex;
            AMTimeRegex = new Regex(DateTimeDefinitions.AMTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PMTimeRegex = new Regex(DateTimeDefinitions.PMTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            SimpleTimeOfTodayAfterRegex = FrenchDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
            SimpleTimeOfTodayBeforeRegex = FrenchDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;
            SpecificTimeOfDayRegex = FrenchDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            TheEndOfRegex = FrenchDateTimeExtractorConfiguration.TheEndOfRegex;
            UnitRegex = FrenchTimeExtractorConfiguration.TimeUnitRegex;
            DateNumberConnectorRegex = FrenchDateTimeExtractorConfiguration.DateNumberConnectorRegex;
            Numbers = config.Numbers;
            CardinalExtractor = config.CardinalExtractor;
            IntegerExtractor = config.IntegerExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        // Note: French typically uses 24:00 time, consider removing 12:00 am/pm 

        public int GetHour(string text, int hour)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            int result = hour;
            if (trimedText.EndsWith("matin") && hour >= Constants.HalfDayHourCount)
            {
                result -= Constants.HalfDayHourCount;
            }
            else if (!trimedText.EndsWith("matin") && hour < Constants.HalfDayHourCount)
            {
                result += Constants.HalfDayHourCount;
            }
            return result;
        }
        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            if (trimedText.EndsWith("maintenant"))
            {
                timex = "PRESENT_REF";
            }
            else if (trimedText.Equals("récemment") || trimedText.Equals("précédemment")||trimedText.Equals("auparavant"))
            {
                timex = "PAST_REF";
            }
            else if (trimedText.Equals("dès que possible") || trimedText.Equals("dqp"))
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
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;
            if (trimedText.StartsWith("prochain") || trimedText.EndsWith("prochain") ||
                trimedText.StartsWith("prochaine") || trimedText.EndsWith("prochaine"))
            {
                swift = 1;
            }
            else if (trimedText.StartsWith("dernier") || trimedText.StartsWith("dernière") ||
                      trimedText.EndsWith("dernier") || trimedText.EndsWith("dernière"))
            {
                swift = -1;
            }
            return swift;
        }

        public bool ContainsAmbiguousToken(string text, string matchedText) => false;
    }
}
