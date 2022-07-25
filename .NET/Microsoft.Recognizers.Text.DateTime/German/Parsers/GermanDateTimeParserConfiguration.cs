// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDateTimeParserConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public GermanDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            NowRegex = GermanDateTimeExtractorConfiguration.NowRegex;

            AMTimeRegex = new Regex(DateTimeDefinitions.AMTimeRegex, RegexFlags);
            PMTimeRegex = new Regex(DateTimeDefinitions.PMTimeRegex, RegexFlags);

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

        public IDateTimeExtractor HolidayExtractor { get; }

        public IDateTimeParser HolidayTimeParser { get; }

        public int GetHour(string text, int hour)
        {
            var trimmedText = text.Trim();
            int result = hour;

            // @TODO Move all hardcoded strings to resource file

            if ((trimmedText.EndsWith("morgen", StringComparison.Ordinal) ||
                 trimmedText.EndsWith("morgens", StringComparison.Ordinal)) &&
                hour >= Constants.HalfDayHourCount)
            {
                result -= Constants.HalfDayHourCount;
            }
            else if (!(trimmedText.EndsWith("morgen", StringComparison.Ordinal) ||
                       trimmedText.EndsWith("morgens", StringComparison.Ordinal)) &&
                     hour < Constants.HalfDayHourCount)
            {
                result += Constants.HalfDayHourCount;
            }

            return result;
        }

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            // @TODO Move all hardcoded strings to resource file

            if (trimmedText.EndsWith("jetzt", StringComparison.Ordinal) ||
                trimmedText.Equals("momentan", StringComparison.Ordinal) ||
                trimmedText.Equals("gerade", StringComparison.Ordinal) ||
                trimmedText.Equals("aktuell", StringComparison.Ordinal) ||
                trimmedText.Equals("aktuelle", StringComparison.Ordinal) ||
                trimmedText.Equals("im moment", StringComparison.Ordinal) ||
                trimmedText.Equals("in diesem moment", StringComparison.Ordinal) ||
                trimmedText.Equals("derzeit", StringComparison.Ordinal))
            {
                timex = "PRESENT_REF";
            }
            else if (trimmedText.Equals("neulich", StringComparison.Ordinal) ||
                     trimmedText.Equals("vorher", StringComparison.Ordinal) ||
                     trimmedText.Equals("vorhin", StringComparison.Ordinal))
            {
                timex = "PAST_REF";
            }
            else if (trimmedText.Equals("so früh wie möglich", StringComparison.Ordinal) ||
                     trimmedText.Equals("asap", StringComparison.Ordinal))
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
            if (trimmedText.StartsWith("nächsten", StringComparison.Ordinal) ||
                trimmedText.StartsWith("nächste", StringComparison.Ordinal) ||
                trimmedText.StartsWith("nächstes", StringComparison.Ordinal) ||
                trimmedText.StartsWith("nächster", StringComparison.Ordinal))
            {
                swift = 1;
            }
            else if (trimmedText.StartsWith("letzten", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("letzte", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("letztes", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("letzter", StringComparison.Ordinal))
            {
                swift = -1;
            }

            return swift;
        }

        public bool ContainsAmbiguousToken(string text, string matchedText) => false;
    }
}
