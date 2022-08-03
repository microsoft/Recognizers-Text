// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishMergedExtractorConfiguration : BaseDateTimeOptionsConfiguration, IMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex AroundRegex =
            new Regex(DateTimeDefinitions.AroundRegex, RegexFlags);

        public static readonly Regex EqualRegex =
            new Regex(BaseDateTime.EqualRegex, RegexFlags);

        // TODO: change the following three regexes to Spanish if there is same requirement of split from A to B as two time points
        public static readonly Regex FromToRegex =
            new Regex(DateTimeDefinitions.FromToRegex, RegexFlags);

        public static readonly Regex SingleAmbiguousMonthRegex =
            new Regex(DateTimeDefinitions.UnspecificDatePeriodRegex, RegexFlags);

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

        public static readonly Regex[] TermFilterRegexes = System.Array.Empty<Regex>();

        public static readonly StringMatcher SuperfluousWordMatcher = new StringMatcher();

        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.YearRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public SpanishMergedExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(this));
            SetExtractor = new BaseSetExtractor(new SpanishSetExtractorConfiguration(this));
            DateTimeAltExtractor = new BaseDateTimeAltExtractor(new SpanishDateTimeAltExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new SpanishHolidayExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new SpanishTimeZoneExtractorConfiguration(this));

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

            IntegerExtractor = Number.Spanish.IntegerExtractor.GetInstance(numConfig);

            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityFiltersDict);
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

        Regex IMergedExtractorConfiguration.AmbiguousRangeModifierPrefix => null;

        Regex IMergedExtractorConfiguration.PotentialAmbiguousRangeRegex => null;

        Regex IMergedExtractorConfiguration.NumberEndingPattern => NumberEndingPattern;

        Regex IMergedExtractorConfiguration.SuffixAfterRegex => SuffixAfterRegex;

        Regex IMergedExtractorConfiguration.UnspecificDatePeriodRegex => UnspecificDatePeriodRegex;

        Regex IMergedExtractorConfiguration.UnspecificTimePeriodRegex => null;

        Regex IMergedExtractorConfiguration.YearRegex => YearRegex;

        public Regex FailFastRegex { get; } = null;

        IEnumerable<Regex> IMergedExtractorConfiguration.TermFilterRegexes => TermFilterRegexes;

        StringMatcher IMergedExtractorConfiguration.SuperfluousWordMatcher => SuperfluousWordMatcher;

        bool IMergedExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        public Regex TasksModeMentionFilters { get; }

    }
}
