﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishMergedExtractorConfiguration : BaseDateTimeOptionsConfiguration, IMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AroundRegex =
            new Regex(DateTimeDefinitions.AroundRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex EqualRegex =
            new Regex(BaseDateTime.EqualRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FromToRegex =
            new Regex(DateTimeDefinitions.FromToRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SingleAmbiguousMonthRegex =
            new Regex(DateTimeDefinitions.SingleAmbiguousMonthRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PrepositionSuffixRegex =
            new Regex(DateTimeDefinitions.PrepositionSuffixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AmbiguousRangeModifierPrefix =
            new Regex(DateTimeDefinitions.AmbiguousRangeModifierPrefix, RegexFlags, RegexTimeOut);

        public static readonly Regex NumberEndingPattern =
            new Regex(DateTimeDefinitions.NumberEndingPattern, RegexFlags, RegexTimeOut);

        public static readonly Regex SuffixAfterRegex =
            new Regex(DateTimeDefinitions.SuffixAfterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex UnspecificDatePeriodRegex =
            new Regex(DateTimeDefinitions.UnspecificDatePeriodRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex UnspecificTimePeriodRegex =
            new Regex(DateTimeDefinitions.UnspecificTimePeriodRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FailFastRegex =
            new Regex(DateTimeDefinitions.FailFastRegex, RegexFlags | RegexOptions.Compiled);

        // Used to skip only year references in a text in TasksMode
        public static readonly Regex YearRegex =
            new Regex(DateTimeDefinitions.YearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex[] TermFilterRegexes =
        {
            // one on one
            new Regex(DateTimeDefinitions.OneOnOneRegex, RegexFlags, RegexTimeOut),

            // (the)? (day|week|month|year)
            new Regex(DateTimeDefinitions.SingleAmbiguousTermsRegex, RegexFlags, RegexTimeOut),

        };

        public static readonly StringMatcher SuperfluousWordMatcher = new StringMatcher();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public EnglishMergedExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(this));
            SetExtractor = new BaseSetExtractor(new EnglishSetExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(this));
            DateTimeAltExtractor = new BaseDateTimeAltExtractor(new EnglishDateTimeAltExtractorConfiguration(this));

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            if ((config.Options & DateTimeOptions.ExperimentalMode) != 0)
            {
                SinceRegex = SinceRegexExp;
            }

            /*
             supression under tasksmode, cases like 1ampm, 1p --> pm,
             holiday time reference whose celebration dates are region specific,
             Remove decade regex ex seventies, twenties
             Remove decade regex ex 1990s, 9s,
             Suppress "Q1 2018", "2nd quarter",
            "2016 Q1", "last year the 4th quarter",
             "2015 the H1", "H2 of 2016", "1st half 2018", "2nd half this year",
            summer winter etc
             */
            if ((config.Options & DateTimeOptions.TasksMode) != 0)
            {
                TasksModeMentionFilters = new Regex(DateTimeDefinitions.TasksModeSupressionRegexes, RegexFlags, RegexTimeOut);
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = Number.English.IntegerExtractor.GetInstance(numConfig);

            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityFiltersDict);

            if ((Options & DateTimeOptions.EnablePreview) != 0)
            {
                SuperfluousWordMatcher.Init(DateTimeDefinitions.SuperfluousWordList);
            }
        }

        // Used in Standard mode
        public static Regex SinceRegex { get; set; } = new Regex(DateTimeDefinitions.SinceRegex, RegexFlags, RegexTimeOut);

        // used in Experimental mode
        public static Regex SinceRegexExp { get; } = new Regex(DateTimeDefinitions.SinceRegexExp, RegexFlags, RegexTimeOut);

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
