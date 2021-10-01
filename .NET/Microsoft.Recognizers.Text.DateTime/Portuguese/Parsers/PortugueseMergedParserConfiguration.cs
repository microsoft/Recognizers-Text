﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public sealed class PortugueseMergedParserConfiguration : PortugueseCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public PortugueseMergedParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            BeforeRegex = PortugueseMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = PortugueseMergedExtractorConfiguration.AfterRegex;
            SinceRegex = PortugueseMergedExtractorConfiguration.SinceRegex;
            AroundRegex = PortugueseMergedExtractorConfiguration.AroundRegex;
            EqualRegex = PortugueseMergedExtractorConfiguration.EqualRegex;
            SuffixAfter = PortugueseMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = PortugueseDatePeriodExtractorConfiguration.YearRegex;

            SuperfluousWordMatcher = PortugueseMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new PortugueseDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new PortugueseTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new DateTimePeriodParser(new PortugueseDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new PortugueseSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new PortugueseHolidayParserConfiguration(this));
            TimeZoneParser = new DummyTimeZoneParser();
        }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public Regex AroundRegex { get; }

        public Regex EqualRegex { get; }

        public Regex SuffixAfter { get; }

        public Regex YearRegex { get; }

        public IDateTimeParser SetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public StringMatcher SuperfluousWordMatcher { get; }

        bool IMergedParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;
    }
}
