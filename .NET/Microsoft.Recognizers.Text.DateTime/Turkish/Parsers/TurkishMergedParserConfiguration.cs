// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Turkish;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Turkish
{
    public sealed class TurkishMergedParserConfiguration : TurkishCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex SinceRegex =
            new Regex(DateTimeDefinitions.SinceRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public TurkishMergedParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            AroundRegex = TurkishMergedExtractorConfiguration.AroundRegex;
            EqualRegex = TurkishMergedExtractorConfiguration.EqualRegex;
            SuffixAfter = TurkishMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = TurkishDatePeriodExtractorConfiguration.YearRegex;

            SuperfluousWordMatcher = TurkishMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new TurkishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new TurkishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new TurkishDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new TurkishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new TurkishHolidayParserConfiguration(this));
            TimeZoneParser = new BaseTimeZoneParser(new TurkishTimeZoneParserConfiguration(this));
        }

        Regex IMergedParserConfiguration.BeforeRegex => BeforeRegex;

        Regex IMergedParserConfiguration.AfterRegex => AfterRegex;

        Regex IMergedParserConfiguration.SinceRegex => SinceRegex;

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