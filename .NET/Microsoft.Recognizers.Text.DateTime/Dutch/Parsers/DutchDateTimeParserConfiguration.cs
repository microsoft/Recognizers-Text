using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchDateTimeParserConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeParserConfiguration
    {
        public static readonly Regex AmTimeRegex =
            new Regex(DateTimeDefinitions.AMTimeRegex, RegexFlags);

        public static readonly Regex PmTimeRegex =
            new Regex(DateTimeDefinitions.PMTimeRegex, RegexFlags);

        public static readonly Regex NightTimeRegex =
             new Regex(DateTimeDefinitions.NightTimeRegex, RegexFlags);

        public static readonly Regex MorningTimeRegex =
             new Regex(DateTimeDefinitions.MorningTimeRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex NowTimeRegex =
            new Regex(DateTimeDefinitions.NowTimeRegex, RegexFlags);

        private static readonly Regex RecentlyTimeRegex =
            new Regex(DateTimeDefinitions.RecentlyTimeRegex, RegexFlags);

        private static readonly Regex AsapTimeRegex =
            new Regex(DateTimeDefinitions.AsapTimeRegex, RegexFlags);

        private static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags);

        private static readonly Regex PreviousPrefixRegex =
            new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags);

        public DutchDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;

            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;

            NowRegex = DutchDateTimeExtractorConfiguration.NowRegex;

            SimpleTimeOfTodayAfterRegex = DutchDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
            SimpleTimeOfTodayBeforeRegex = DutchDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;
            SpecificTimeOfDayRegex = DutchDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            SpecificEndOfRegex = DutchDateTimeExtractorConfiguration.SpecificEndOfRegex;
            UnspecificEndOfRegex = DutchDateTimeExtractorConfiguration.UnspecificEndOfRegex;
            UnitRegex = DutchTimeExtractorConfiguration.TimeUnitRegex;
            DateNumberConnectorRegex = DutchDateTimeExtractorConfiguration.DateNumberConnectorRegex;
            YearRegex = DutchDateTimeExtractorConfiguration.YearRegex;

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

        public Regex AMTimeRegex => AmTimeRegex;

        public Regex PMTimeRegex => PmTimeRegex;

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

            if (MorningTimeRegex.MatchEnd(trimmedText, trim: true).Success && hour >= Constants.HalfDayHourCount)
            {
                result -= Constants.HalfDayHourCount;
            }
            else if (!MorningTimeRegex.MatchEnd(trimmedText, trim: true).Success && hour < Constants.HalfDayHourCount &&
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
            else if (RecentlyTimeRegex.IsExactMatch(trimmedText, trim: true))
            {
                timex = "PAST_REF";
            }
            else if (AsapTimeRegex.IsExactMatch(trimmedText, trim: true))
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
            if (NextPrefixRegex.MatchBegin(trimmedText, trim: true).Success)
            {
                swift = 1;
            }
            else if (PreviousPrefixRegex.MatchBegin(trimmedText, trim: true).Success)
            {
                swift = -1;
            }

            return swift;
        }

        public bool ContainsAmbiguousToken(string text, string matchedText) => false;
    }
}
