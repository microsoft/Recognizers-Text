// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Swedish;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Swedish
{
    public sealed class SwedishMergedParserConfiguration : SwedishCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public SwedishMergedParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            BeforeRegex = SwedishMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = SwedishMergedExtractorConfiguration.AfterRegex;
            SinceRegex = (config.Options & DateTimeOptions.ExperimentalMode) != 0 ? SwedishMergedExtractorConfiguration.SinceRegexExp :
                SwedishMergedExtractorConfiguration.SinceRegex;
            AroundRegex = SwedishMergedExtractorConfiguration.AroundRegex;
            EqualRegex = SwedishMergedExtractorConfiguration.EqualRegex;
            SuffixAfter = SwedishMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = SwedishDatePeriodExtractorConfiguration.YearRegex;

            SuperfluousWordMatcher = SwedishMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new SwedishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new SwedishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new SwedishDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new SwedishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new SwedishHolidayParserConfiguration(this));
            TimeZoneParser = new BaseTimeZoneParser();
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