// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseSetParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKSetParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex DayTypeRegex =
            new Regex(DateTimeDefinitions.DayTypeRegex, RegexFlags);

        private static readonly Regex WeekTypeRegex =
            new Regex(DateTimeDefinitions.WeekTypeRegex, RegexFlags);

        private static readonly Regex BiWeekTypeRegex =
            new Regex(DateTimeDefinitions.BiWeekTypeRegex, RegexFlags);

        private static readonly Regex MonthTypeRegex =
            new Regex(DateTimeDefinitions.MonthTypeRegex, RegexFlags);

        private static readonly Regex YearTypeRegex =
            new Regex(DateTimeDefinitions.YearTypeRegex, RegexFlags);

        public JapaneseSetParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            DurationExtractor = config.DurationExtractor;
            TimeExtractor = config.TimeExtractor;
            TimePeriodExtractor = config.TimePeriodExtractor;
            DateExtractor = config.DateExtractor;
            DateTimeExtractor = config.DateTimeExtractor;

            DurationParser = config.DurationParser;
            TimeParser = config.TimeParser;
            TimePeriodParser = config.TimePeriodParser;
            DateParser = config.DateParser;
            DateTimeParser = config.DateTimeParser;

            EachPrefixRegex = JapaneseSetExtractorConfiguration.EachPrefixRegex;
            EachUnitRegex = JapaneseSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = JapaneseSetExtractorConfiguration.EachDayRegex;
            EachDateUnitRegex = JapaneseSetExtractorConfiguration.EachDateUnitRegex;
            UnitMap = config.UnitMap;
        }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public Regex EachPrefixRegex { get; }

        public Regex EachUnitRegex { get; }

        public Regex EachDayRegex { get; }

        public Regex EachDateUnitRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public bool GetMatchedUnitTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            if (DayTypeRegex.IsMatch(trimmedText))
            {
                timex = "P1D";
            }
            else if (BiWeekTypeRegex.IsMatch(trimmedText))
            {
                timex = "P2W";
            }
            else if (WeekTypeRegex.IsMatch(trimmedText))
            {
                timex = "P1W";
            }
            else if (MonthTypeRegex.IsMatch(trimmedText))
            {
                timex = "P1M";
            }
            else if (YearTypeRegex.IsMatch(trimmedText))
            {
                timex = "P1Y";
            }
            else
            {
                timex = null;
                return false;
            }

            return true;
        }
    }
}