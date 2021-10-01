﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseSetExtractorConfiguration : BaseDateTimeOptionsConfiguration, ISetExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        public static readonly Regex PeriodicRegex =
            new Regex(DateTimeDefinitions.PeriodicRegex, RegexFlags);

        public static readonly Regex EachUnitRegex =
            new Regex(DateTimeDefinitions.EachUnitRegex, RegexFlags);

        public static readonly Regex EachPrefixRegex =
            new Regex(DateTimeDefinitions.EachPrefixRegex, RegexFlags);

        public static readonly Regex EachDayRegex =
            new Regex(DateTimeDefinitions.EachDayRegex, RegexFlags);

        public static readonly Regex BeforeEachDayRegex =
            new Regex(DateTimeDefinitions.BeforeEachDayRegex, RegexFlags);

        public static readonly Regex SetWeekDayRegex =
            new Regex(DateTimeDefinitions.SetWeekDayRegex, RegexFlags);

        public static readonly Regex SetEachRegex =
            new Regex(DateTimeDefinitions.SetEachRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public PortugueseSetExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration(this));
            DateExtractor = new BaseDateExtractor(new PortugueseDateExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new PortugueseDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new PortugueseDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new PortugueseTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new PortugueseDateTimePeriodExtractorConfiguration(this));
        }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        bool ISetExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        Regex ISetExtractorConfiguration.LastRegex => PortugueseDateExtractorConfiguration.LastDateRegex;

        Regex ISetExtractorConfiguration.EachPrefixRegex => EachPrefixRegex;

        Regex ISetExtractorConfiguration.PeriodicRegex => PeriodicRegex;

        Regex ISetExtractorConfiguration.EachUnitRegex => EachUnitRegex;

        Regex ISetExtractorConfiguration.EachDayRegex => EachDayRegex;

        Regex ISetExtractorConfiguration.BeforeEachDayRegex => BeforeEachDayRegex;

        Regex ISetExtractorConfiguration.SetWeekDayRegex => SetWeekDayRegex;

        Regex ISetExtractorConfiguration.SetEachRegex => SetEachRegex;

        public Tuple<string, int> WeekDayGroupMatchTuple(Match match) => SetHandler.WeekDayGroupMatchTuple(match);
    }
}
