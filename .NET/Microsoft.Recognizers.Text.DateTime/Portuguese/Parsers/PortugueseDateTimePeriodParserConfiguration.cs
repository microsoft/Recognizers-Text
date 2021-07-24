﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDateTimePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, IDateTimePeriodParserConfiguration
    {
        public PortugueseDateTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;

            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateTimeExtractor = config.DateTimeExtractor;
            TimePeriodExtractor = config.TimePeriodExtractor;
            CardinalExtractor = config.CardinalExtractor;
            DurationExtractor = config.DurationExtractor;
            NumberParser = config.NumberParser;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            DateTimeParser = config.DateTimeParser;
            TimePeriodParser = config.TimePeriodParser;
            DurationParser = config.DurationParser;
            TimeZoneParser = config.TimeZoneParser;

            PureNumberFromToRegex = PortugueseTimePeriodExtractorConfiguration.PureNumFromTo;
            HyphenDateRegex = PortugueseDateTimePeriodExtractorConfiguration.HyphenDateRegex;
            PureNumberBetweenAndRegex = PortugueseTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeOfDayRegex = PortugueseDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            TimeOfDayRegex = PortugueseDateTimeExtractorConfiguration.TimeOfDayRegex;
            PreviousPrefixRegex = PortugueseDatePeriodExtractorConfiguration.PastRegex;
            FutureRegex = PortugueseDatePeriodExtractorConfiguration.FutureRegex;
            FutureSuffixRegex = PortugueseDatePeriodExtractorConfiguration.FutureSuffixRegex;
            NumberCombinedWithUnitRegex = PortugueseDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit;
            UnitRegex = PortugueseTimePeriodExtractorConfiguration.UnitRegex;
            PeriodTimeOfDayWithDateRegex = PortugueseDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex;
            RelativeTimeUnitRegex = PortugueseDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex;
            RestOfDateTimeRegex = PortugueseDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex;
            AmDescRegex = PortugueseDateTimePeriodExtractorConfiguration.AmDescRegex;
            PmDescRegex = PortugueseDateTimePeriodExtractorConfiguration.PmDescRegex;
            WithinNextPrefixRegex = PortugueseDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex;
            PrefixDayRegex = PortugueseDateTimePeriodExtractorConfiguration.PrefixDayRegex;
            BeforeRegex = PortugueseDateTimePeriodExtractorConfiguration.BeforeRegex;
            AfterRegex = PortugueseDateTimePeriodExtractorConfiguration.AfterRegex;
            UnitMap = config.UnitMap;
            Numbers = config.Numbers;
        }

        public string TokenBeforeDate { get; }

        public string TokenBeforeTime { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex HyphenDateRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public Regex SpecificTimeOfDayRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex PreviousPrefixRegex { get; }

        public Regex FutureRegex { get; }

        public Regex FutureSuffixRegex { get; }

        public Regex NumberCombinedWithUnitRegex { get; }

        public Regex UnitRegex { get; }

        public Regex PeriodTimeOfDayWithDateRegex { get; }

        public Regex RelativeTimeUnitRegex { get; }

        public Regex RestOfDateTimeRegex { get; }

        public Regex AmDescRegex { get; }

        public Regex PmDescRegex { get; }

        public Regex WithinNextPrefixRegex { get; }

        public Regex PrefixDayRegex { get; }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        bool IDateTimePeriodParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public bool GetMatchedTimeRange(string text, out string todSymbol, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim().Normalized(DateTimeDefinitions.SpecialCharactersEquivalent);

            beginHour = 0;
            endHour = 0;
            endMin = 0;

            // @TODO move hardcoded values to resources file
            if (trimmedText.EndsWith("madrugada", StringComparison.Ordinal))
            {
                todSymbol = Constants.EarlyMorning;
            }
            else if (trimmedText.EndsWith("manha", StringComparison.Ordinal))
            {
                todSymbol = Constants.Morning;
            }
            else if (trimmedText.Contains("passado o meio dia") || trimmedText.Contains("depois do meio dia"))
            {
                todSymbol = Constants.Afternoon;
            }
            else if (trimmedText.EndsWith("tarde", StringComparison.Ordinal))
            {
                todSymbol = Constants.Evening;
            }
            else if (trimmedText.EndsWith("noite", StringComparison.Ordinal))
            {
                todSymbol = Constants.Night;
            }
            else
            {
                todSymbol = null;
                return false;
            }

            var parseResult = TimexUtility.ResolveTimeOfDay(todSymbol);
            todSymbol = parseResult.Timex;
            beginHour = parseResult.BeginHour;
            endHour = parseResult.EndHour;
            endMin = parseResult.EndMin;

            return true;
        }

        public int GetSwiftPrefix(string text)
        {
            var trimmedText = text.Trim();
            var swift = 0;

            // @TODO move hardcoded values to resources file
            if (PortugueseDatePeriodParserConfiguration.PreviousPrefixRegex.IsMatch(trimmedText) ||
                trimmedText.Equals("anoche", StringComparison.Ordinal))
            {
                swift = -1;
            }
            else if (PortugueseDatePeriodParserConfiguration.NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }

            return swift;
        }
    }
}
