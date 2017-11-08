using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateTimeParserConfiguration : IDateTimeParserConfiguration
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

        public EnglishDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;

            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;

            NowRegex = EnglishDateTimeExtractorConfiguration.NowRegex;

            AMTimeRegex = new Regex(DateTimeDefinitions.AMTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PMTimeRegex = new Regex(DateTimeDefinitions.PMTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            SimpleTimeOfTodayAfterRegex = EnglishDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
            SimpleTimeOfTodayBeforeRegex = EnglishDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;
            SpecificTimeOfDayRegex = EnglishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            TheEndOfRegex = EnglishDateTimeExtractorConfiguration.TheEndOfRegex;
            UnitRegex = EnglishTimeExtractorConfiguration.TimeUnitRegex;

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
            if (trimedText.EndsWith("morning") && hour >= 12)
            {
                result -= 12;
            }
            else if (!trimedText.EndsWith("morning") && hour < 12)
            {
                result += 12;
            }
            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            if (trimedText.EndsWith("now"))
            {
                timex = "PRESENT_REF";
            }
            else if (trimedText.Equals("recently") || trimedText.Equals("previously"))
            {
                timex = "PAST_REF";
            }
            else if (trimedText.Equals("as soon as possible") || trimedText.Equals("asap"))
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
            if (trimedText.StartsWith("next"))
            {
                swift = 1;
            }
            else if (trimedText.StartsWith("last"))
            {
                swift = -1;
            }

            return swift;
        }

        public bool HaveAmbiguousToken(string text, string matchedText) => false;
    }
}
