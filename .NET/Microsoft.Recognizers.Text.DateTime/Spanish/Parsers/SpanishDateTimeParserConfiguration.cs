using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeParserConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

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

            AMTimeRegex = new Regex(DateTimeDefinitions.AmTimeRegex, RegexFlags);
            PMTimeRegex = new Regex(DateTimeDefinitions.PmTimeRegex, RegexFlags);
            NightTimeRegex = new Regex(DateTimeDefinitions.NightTimeRegex, RegexFlags);
            LastNightTimeRegex = new Regex(DateTimeDefinitions.LastNightTimeRegex, RegexFlags);
            NowTimeRegex = new Regex(DateTimeDefinitions.NowTimeRegex, RegexFlags);
            RecentlyTimeRegex = new Regex(DateTimeDefinitions.RecentlyTimeRegex, RegexFlags);
            AsapTimeRegex = new Regex(DateTimeDefinitions.AsapTimeRegex, RegexFlags);

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

        public Regex NightTimeRegex { get; }

        public Regex LastNightTimeRegex { get; }

        public Regex NowTimeRegex { get; }

        public Regex RecentlyTimeRegex { get; }

        public Regex AsapTimeRegex { get; }

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
            int result = hour;

            var trimmedText = text.Trim();

            if (AMTimeRegex.MatchEnd(trimmedText, trim: true).Success && hour >= Constants.HalfDayHourCount)
            {
                result -= Constants.HalfDayHourCount;
            }
            else if (!AMTimeRegex.MatchEnd(trimmedText, trim: true).Success && hour < Constants.HalfDayHourCount &&
                !(NightTimeRegex.MatchEnd(trimmedText, trim: true).Success && hour < Constants.QuarterDayHourCount))
            {
                result += Constants.HalfDayHourCount;
            }

            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            if (NowTimeRegex.MatchEnd(trimmedText, trim: true).Success)
            {
                timex = "PRESENT_REF";
            }
            else if (RecentlyTimeRegex.MatchEnd(trimmedText, trim: true).Success)
            {
                timex = "PAST_REF";
            }
            else if (AsapTimeRegex.MatchEnd(trimmedText, trim: true).Success)
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
            var trimmedText = text.Trim();
            var swift = 0;

            if (SpanishDatePeriodParserConfiguration.PreviousPrefixRegex.IsMatch(trimmedText) ||
                LastNightTimeRegex.IsMatch(trimmedText))
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
            // @TODO move hardcoded values to resources file
            return text.Contains("esta mañana") && matchedText.Contains("mañana");
        }
    }
}
