﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public sealed class SpanishMergedParserConfiguration : SpanishCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public SpanishMergedParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            BeforeRegex = SpanishMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = SpanishMergedExtractorConfiguration.AfterRegex;
            SinceRegex = (config.Options & DateTimeOptions.ExperimentalMode) != 0 ? SpanishMergedExtractorConfiguration.SinceRegexExp :
                SpanishMergedExtractorConfiguration.SinceRegex;
            AroundRegex = SpanishMergedExtractorConfiguration.AroundRegex;
            EqualRegex = SpanishMergedExtractorConfiguration.EqualRegex;
            SuffixAfter = SpanishMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = SpanishDatePeriodExtractorConfiguration.YearRegex;

            SuperfluousWordMatcher = SpanishMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new DateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new SpanishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new SpanishHolidayParserConfiguration(this));
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
