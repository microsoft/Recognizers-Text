﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseTimePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, ITimePeriodParserConfiguration
    {
        public PortugueseTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TimeExtractor = config.TimeExtractor;
            IntegerExtractor = config.IntegerExtractor;
            TimeParser = config.TimeParser;
            TimeZoneParser = config.TimeZoneParser;

            PureNumberFromToRegex = PortugueseTimePeriodExtractorConfiguration.PureNumFromTo;
            PureNumberBetweenAndRegex = PortugueseTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeFromToRegex = PortugueseTimePeriodExtractorConfiguration.SpecificTimeFromTo;
            SpecificTimeBetweenAndRegex = PortugueseTimePeriodExtractorConfiguration.SpecificTimeBetweenAnd;
            TimeOfDayRegex = PortugueseTimePeriodExtractorConfiguration.TimeOfDayRegex;
            GeneralEndingRegex = PortugueseTimePeriodExtractorConfiguration.GeneralEndingRegex;
            TillRegex = PortugueseTimePeriodExtractorConfiguration.TillRegex;

            Numbers = config.Numbers;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeParser TimeParser { get; }

        public IExtractor IntegerExtractor { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public Regex SpecificTimeFromToRegex { get; }

        public Regex SpecificTimeBetweenAndRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex GeneralEndingRegex { get; }

        public Regex TillRegex { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public bool GetMatchedTimeRange(string text, out string timex, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim();

            beginHour = 0;
            endHour = 0;
            endMin = 0;

            var timeOfDay = string.Empty;

            if (DateTimeDefinitions.EarlyMorningTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.EarlyMorning;
            }
            else if (DateTimeDefinitions.MorningTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.Morning;
            }
            else if (DateTimeDefinitions.AfternoonTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.Afternoon;
            }
            else if (DateTimeDefinitions.EveningTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.Evening;
            }
            else if (DateTimeDefinitions.NightTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.Night;
            }
            else
            {
                timex = null;
                return false;
            }

            var parseResult = TimexUtility.ResolveTimeOfDay(timeOfDay);
            timex = parseResult.Timex;
            beginHour = parseResult.BeginHour;
            endHour = parseResult.EndHour;
            endMin = parseResult.EndMin;

            return true;
        }
    }
}
