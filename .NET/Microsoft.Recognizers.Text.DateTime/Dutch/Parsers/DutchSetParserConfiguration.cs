// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchSetParserConfiguration : BaseDateTimeOptionsConfiguration, ISetParserConfiguration
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

        private static readonly Regex QuarterTypeRegex =
            new Regex(DateTimeDefinitions.QuarterTypeRegex, RegexFlags);

        private static readonly Regex YearTypeRegex =
            new Regex(DateTimeDefinitions.YearTypeRegex, RegexFlags);

        private static readonly Regex SemiYearTypeRegex =
            new Regex(DateTimeDefinitions.SemiYearTypeRegex, RegexFlags);

        private static readonly Regex WeekendTypeRegex =
            new Regex(DateTimeDefinitions.WeekendTypeRegex, RegexFlags);

        public DutchSetParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            DurationExtractor = config.DurationExtractor;
            TimeExtractor = config.TimeExtractor;
            DateExtractor = config.DateExtractor;
            DateTimeExtractor = config.DateTimeExtractor;
            DatePeriodExtractor = config.DatePeriodExtractor;
            TimePeriodExtractor = config.TimePeriodExtractor;
            DateTimePeriodExtractor = config.DateTimePeriodExtractor;

            DurationParser = config.DurationParser;
            TimeParser = config.TimeParser;
            DateParser = config.DateParser;
            DateTimeParser = config.DateTimeParser;
            DatePeriodParser = config.DatePeriodParser;
            TimePeriodParser = config.TimePeriodParser;
            DateTimePeriodParser = config.DateTimePeriodParser;
            UnitMap = config.UnitMap;

            EachPrefixRegex = DutchSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = DutchSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = DutchSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = DutchSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = DutchSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = DutchSetExtractorConfiguration.SetEachRegex;
        }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeParser DatePeriodParser { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        public IDateTimeParser DateTimePeriodParser { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public Regex EachPrefixRegex { get; }

        public Regex PeriodicRegex { get; }

        public Regex EachUnitRegex { get; }

        public Regex EachDayRegex { get; }

        public Regex SetWeekDayRegex { get; }

        public Regex SetEachRegex { get; }

        public bool GetMatchedDailyTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            if (DayTypeRegex.IsMatch(trimmedText))
            {
                timex = "P1D";
            }
            else if (WeekTypeRegex.IsMatch(trimmedText))
            {
                timex = "P1W";
            }
            else if (BiWeekTypeRegex.IsMatch(trimmedText))
            {
                timex = "P2W";
            }
            else if (MonthTypeRegex.IsMatch(trimmedText))
            {
                timex = "P1M";
            }
            else if (YearTypeRegex.IsMatch(trimmedText))
            {
                timex = "P1Y";
            }
            else if (SemiYearTypeRegex.IsMatch(trimmedText))
            {
                timex = "P0.5Y";
            }
            else if (QuarterTypeRegex.IsMatch(trimmedText))
            {
                timex = "P3M";
            }
            else if (WeekendTypeRegex.IsMatch(trimmedText))
            {
                timex = "XXXX-WXX-WE";
            }
            else
            {
                timex = null;
                return false;
            }

            return true;
        }

        public bool GetMatchedUnitTimex(string text, out string timex)
        {
            return GetMatchedDailyTimex(text, out timex);
        }

        public string WeekDayGroupMatchString(Match match) => SetHandler.WeekDayGroupMatchString(match);
    }
}