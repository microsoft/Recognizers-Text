// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateTimeParserConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeParserConfiguration
    {
        public static readonly Regex AmTimeRegex =
            new Regex(DateTimeDefinitions.AMTimeRegex, RegexFlags);

        public static readonly Regex PmTimeRegex =
            new Regex(DateTimeDefinitions.PMTimeRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex AsapTimeRegex =
            new Regex(DateTimeDefinitions.AsapTimeRegex, RegexFlags);

        public FrenchDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            NowRegex = FrenchDateTimeExtractorConfiguration.NowRegex;

            SimpleTimeOfTodayAfterRegex = FrenchDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
            SimpleTimeOfTodayBeforeRegex = FrenchDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;
            SpecificTimeOfDayRegex = FrenchDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            SpecificEndOfRegex = FrenchDateTimeExtractorConfiguration.SpecificEndOfRegex;
            UnspecificEndOfRegex = FrenchDateTimeExtractorConfiguration.UnspecificEndOfRegex;
            UnitRegex = FrenchTimeExtractorConfiguration.TimeUnitRegex;
            DateNumberConnectorRegex = FrenchDateTimeExtractorConfiguration.DateNumberConnectorRegex;
            YearRegex = FrenchDateTimeExtractorConfiguration.YearRegex;

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

        public Regex YearRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        public IDateTimeParser HolidayTimeParser { get; }

        // Note: French typically uses 24:00 time, consider removing 12:00 am/pm
        public int GetHour(string text, int hour)
        {
            int result = hour;

            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file

            if (trimmedText.EndsWith("matin", StringComparison.Ordinal) && hour >= Constants.HalfDayHourCount)
            {
                result -= Constants.HalfDayHourCount;
            }
            else if (!trimmedText.EndsWith("matin", StringComparison.Ordinal) && hour < Constants.HalfDayHourCount)
            {
                result += Constants.HalfDayHourCount;
            }

            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file

            if (trimmedText.EndsWith("maintenant", StringComparison.Ordinal))
            {
                timex = "PRESENT_REF";
            }
            else if (trimmedText.Equals("récemment", StringComparison.Ordinal) ||
                     trimmedText.Equals("précédemment", StringComparison.Ordinal) ||
                     trimmedText.Equals("auparavant", StringComparison.Ordinal))
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
            var swift = 0;

            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file

            if (trimmedText.StartsWith("prochain", StringComparison.Ordinal) ||
                trimmedText.EndsWith("prochain", StringComparison.Ordinal) ||
                trimmedText.StartsWith("prochaine", StringComparison.Ordinal) ||
                trimmedText.EndsWith("prochaine", StringComparison.Ordinal))
            {
                swift = 1;
            }
            else if (trimmedText.StartsWith("dernier", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("dernière", StringComparison.Ordinal) ||
                     trimmedText.EndsWith("dernier", StringComparison.Ordinal) ||
                     trimmedText.EndsWith("dernière", StringComparison.Ordinal))
            {
                swift = -1;
            }

            return swift;
        }

        public bool ContainsAmbiguousToken(string text, string matchedText) => false;
    }
}
