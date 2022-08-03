// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Swedish;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Swedish
{
    public class SwedishMergedExtractorConfiguration : BaseDateTimeOptionsConfiguration, IMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex AroundRegex =
            new Regex(DateTimeDefinitions.AroundRegex, RegexFlags);

        public static readonly Regex EqualRegex =
            new Regex(BaseDateTime.EqualRegex, RegexFlags);

        public static readonly Regex FromToRegex =
            new Regex(DateTimeDefinitions.FromToRegex, RegexFlags);

        public static readonly Regex SingleAmbiguousMonthRegex =
            new Regex(DateTimeDefinitions.SingleAmbiguousMonthRegex, RegexFlags);

        public static readonly Regex PrepositionSuffixRegex =
            new Regex(DateTimeDefinitions.PrepositionSuffixRegex, RegexFlags);

        public static readonly Regex AmbiguousRangeModifierPrefix =
            new Regex(DateTimeDefinitions.AmbiguousRangeModifierPrefix, RegexFlags);

        public static readonly Regex NumberEndingPattern =
            new Regex(DateTimeDefinitions.NumberEndingPattern, RegexFlags);

        public static readonly Regex SuffixAfterRegex =
            new Regex(DateTimeDefinitions.SuffixAfterRegex, RegexFlags);

        public static readonly Regex UnspecificDatePeriodRegex =
            new Regex(DateTimeDefinitions.UnspecificDatePeriodRegex, RegexFlags);

        public static readonly Regex UnspecificTimePeriodRegex =
            new Regex(DateTimeDefinitions.UnspecificTimePeriodRegex, RegexFlags);

        public static readonly Regex FailFastRegex =
            new Regex(DateTimeDefinitions.FailFastRegex, RegexFlags | RegexOptions.Compiled);

        public static readonly Regex YearRegex =
           new Regex(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex[] TermFilterRegexes =
        {
            // one on one
            new Regex(DateTimeDefinitions.OneOnOneRegex, RegexFlags),

            // (the)? (day|week|month|year)
            new Regex(DateTimeDefinitions.SingleAmbiguousTermsRegex, RegexFlags),
        };

        public static readonly StringMatcher SuperfluousWordMatcher = new StringMatcher();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public SwedishMergedExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DateExtractor = new BaseDateExtractor(new SwedishDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new SwedishTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new SwedishDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new SwedishDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new SwedishTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SwedishDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new SwedishDurationExtractorConfiguration(this));
            SetExtractor = new BaseSetExtractor(new SwedishSetExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new SwedishHolidayExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new SwedishTimeZoneExtractorConfiguration(this));
            DateTimeAltExtractor = new BaseDateTimeAltExtractor(new SwedishDateTimeAltExtractorConfiguration(this));

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            if ((config.Options & DateTimeOptions.ExperimentalMode) != 0)
            {
                SinceRegex = SinceRegexExp;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = Number.Swedish.IntegerExtractor.GetInstance(numConfig);

            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityFiltersDict);

            if ((Options & DateTimeOptions.EnablePreview) != 0)
            {
                SuperfluousWordMatcher.Init(DateTimeDefinitions.SuperfluousWordList);
            }
        }

        // Used in Standard mode
        public static Regex SinceRegex { get; set; } = new Regex(DateTimeDefinitions.SinceRegex, RegexFlags);

        // used in Experimental mode
        public static Regex SinceRegexExp { get; } = new Regex(DateTimeDefinitions.SinceRegexExp, RegexFlags);

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor SetExtractor { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        public IDateTimeZoneExtractor TimeZoneExtractor { get; }

        public IDateTimeListExtractor DateTimeAltExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; }

        Regex IMergedExtractorConfiguration.AfterRegex => AfterRegex;

        Regex IMergedExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex IMergedExtractorConfiguration.SinceRegex => SinceRegex;

        Regex IMergedExtractorConfiguration.AroundRegex => AroundRegex;

        Regex IMergedExtractorConfiguration.EqualRegex => EqualRegex;

        Regex IMergedExtractorConfiguration.FromToRegex => FromToRegex;

        Regex IMergedExtractorConfiguration.SingleAmbiguousMonthRegex => SingleAmbiguousMonthRegex;

        Regex IMergedExtractorConfiguration.PrepositionSuffixRegex => PrepositionSuffixRegex;

        Regex IMergedExtractorConfiguration.AmbiguousRangeModifierPrefix => AmbiguousRangeModifierPrefix;

        Regex IMergedExtractorConfiguration.PotentialAmbiguousRangeRegex => FromToRegex;

        Regex IMergedExtractorConfiguration.NumberEndingPattern => NumberEndingPattern;

        Regex IMergedExtractorConfiguration.SuffixAfterRegex => SuffixAfterRegex;

        Regex IMergedExtractorConfiguration.UnspecificDatePeriodRegex => UnspecificDatePeriodRegex;

        Regex IMergedExtractorConfiguration.UnspecificTimePeriodRegex => UnspecificTimePeriodRegex;

        Regex IMergedExtractorConfiguration.YearRegex => YearRegex;

        Regex IMergedExtractorConfiguration.FailFastRegex => FailFastRegex;

        IEnumerable<Regex> IMergedExtractorConfiguration.TermFilterRegexes => TermFilterRegexes;

        StringMatcher IMergedExtractorConfiguration.SuperfluousWordMatcher => SuperfluousWordMatcher;

        bool IMergedExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        public Regex TasksModeMentionFilters { get; }

    }
}
