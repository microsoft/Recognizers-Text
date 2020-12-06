using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchDateTimeParserConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeParserConfiguration
    {
        public static readonly Regex AmTimeRegex =
            RegexCache.Get(DateTimeDefinitions.AMTimeRegex, RegexFlags);

        public static readonly Regex PmTimeRegex =
            RegexCache.Get(DateTimeDefinitions.PMTimeRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

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

            // @TODO move hardcoded values to resources file

            if (trimmedText.EndsWith("ochtend", StringComparison.Ordinal) && hour >= Constants.HalfDayHourCount)
            {
                result -= Constants.HalfDayHourCount;
            }
            else if (!trimmedText.EndsWith("ochtend", StringComparison.Ordinal) && hour < Constants.HalfDayHourCount)
            {
                result += Constants.HalfDayHourCount;
            }

            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file

            if (trimmedText.EndsWith("nu", StringComparison.Ordinal))
            {
                timex = "PRESENT_REF";
            }
            else if (trimmedText.Equals("kort geleden", StringComparison.Ordinal) ||
                     trimmedText.Equals("eerder", StringComparison.Ordinal))
            {
                timex = "PAST_REF";
            }
            else if (trimmedText.Equals("zo snel mogelijk", StringComparison.Ordinal) ||
                     trimmedText.Equals("zsm", StringComparison.Ordinal))
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

            // @TODO move hardcoded values to resources file

            if (trimmedText.StartsWith("volgende", StringComparison.Ordinal))
            {
                swift = 1;
            }
            else if (trimmedText.StartsWith("vorige", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("laatste", StringComparison.Ordinal))
            {
                swift = -1;
            }

            return swift;
        }

        public bool ContainsAmbiguousToken(string text, string matchedText) => false;
    }
}
