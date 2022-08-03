// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanMergedExtractorConfiguration : BaseDateTimeOptionsConfiguration, IMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex SinceRegex =
            new Regex(DateTimeDefinitions.SinceRegex, RegexFlags);

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

        public static readonly Regex YearRegex =
          new Regex(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly StringMatcher SuperfluousWordMatcher = new StringMatcher();

        public static readonly Regex[] TermFilterRegexes =
        {
            // one on one
            new Regex(DateTimeDefinitions.OneOnOneRegex, RegexFlags),
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public GermanMergedExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DateExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new GermanTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new GermanDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new GermanDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(this));
            SetExtractor = new BaseSetExtractor(new GermanSetExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new GermanHolidayExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new GermanTimeZoneExtractorConfiguration(this));

            DateTimeAltExtractor = new BaseDateTimeAltExtractor(new GermanDateTimeAltExtractorConfiguration(this));

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = Number.German.IntegerExtractor.GetInstance(numConfig);

            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityFiltersDict);
        }

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

        public IExtractor IntegerExtractor { get; }

        public IDateTimeListExtractor DateTimeAltExtractor { get; }

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
