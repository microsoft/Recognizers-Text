// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDateTimeParserConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public PortugueseDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            HolidayExtractor = config.HolidayExtractor;
            HolidayTimeParser = config.HolidayTimeParser;

            NowRegex = PortugueseDateTimeExtractorConfiguration.NowRegex;
            AMTimeRegex = new Regex(DateTimeDefinitions.AmTimeRegex, RegexFlags);
            PMTimeRegex = new Regex(DateTimeDefinitions.PmTimeRegex, RegexFlags);
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

        public IDateTimeExtractor HolidayExtractor { get; }

        public IDateTimeParser HolidayTimeParser { get; }

        public int GetHour(string text, int hour)
        {
            var trimmedText = text.Trim().Normalized(DateTimeDefinitions.SpecialCharactersEquivalent);
            int result = hour;

            // @TODO move hardcoded values to resources file
            if ((trimmedText.EndsWith("manha", StringComparison.Ordinal) ||
                 trimmedText.EndsWith("madrugada", StringComparison.Ordinal)) &&
                hour >= Constants.HalfDayHourCount)
            {
                result -= Constants.HalfDayHourCount;
            }
            else if (!(trimmedText.EndsWith("manha", StringComparison.Ordinal) ||
                       trimmedText.EndsWith("madrugada", StringComparison.Ordinal)) &&
                     hour < Constants.HalfDayHourCount)
            {
                result += Constants.HalfDayHourCount;
            }

            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim().Normalized(DateTimeDefinitions.SpecialCharactersEquivalent);

            // @TODO move hardcoded values to resources file
            if (trimmedText.EndsWith("agora", StringComparison.Ordinal) ||
                trimmedText.EndsWith("mesmo", StringComparison.Ordinal) ||
                trimmedText.EndsWith("momento", StringComparison.Ordinal))
            {
                timex = "PRESENT_REF";
            }
            else if (trimmedText.EndsWith("possivel", StringComparison.Ordinal) ||
                     trimmedText.EndsWith("possa", StringComparison.Ordinal) ||
                     trimmedText.EndsWith("possas", StringComparison.Ordinal) ||
                     trimmedText.EndsWith("possamos", StringComparison.Ordinal) ||
                     trimmedText.EndsWith("possam", StringComparison.Ordinal))
            {
                timex = "FUTURE_REF";
            }
            else if (trimmedText.EndsWith("mente", StringComparison.Ordinal))
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
            var trimmedText = text.Trim();
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
