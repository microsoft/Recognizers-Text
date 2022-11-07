﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseMergedExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.ParserConfigurationBefore, RegexFlags, RegexTimeOut);
        public static readonly Regex UnspecificDatePeriodRegex = new Regex(DateTimeDefinitions.UnspecificDatePeriodRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.ParserConfigurationAfter, RegexFlags, RegexTimeOut);
        public static readonly Regex UntilRegex = new Regex(DateTimeDefinitions.ParserConfigurationUntil, RegexFlags, RegexTimeOut);
        public static readonly Regex SincePrefixRegex = new Regex(DateTimeDefinitions.ParserConfigurationSincePrefix, RegexFlags, RegexTimeOut);
        public static readonly Regex SinceSuffixRegex = new Regex(DateTimeDefinitions.ParserConfigurationSinceSuffix, RegexFlags, RegexTimeOut);
        public static readonly Regex AroundPrefixRegex = new Regex(DateTimeDefinitions.ParserConfigurationAroundPrefix, RegexFlags, RegexTimeOut);
        public static readonly Regex AroundSuffixRegex = new Regex(DateTimeDefinitions.ParserConfigurationAroundSuffix, RegexFlags, RegexTimeOut);
        public static readonly Regex EqualRegex = new Regex(BaseDateTime.EqualRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex PotentialAmbiguousRangeRegex = new Regex(DateTimeDefinitions.FromToRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex AmbiguousRangeModifierPrefix = new Regex(DateTimeDefinitions.AmbiguousRangeModifierPrefix, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ChineseMergedExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityFiltersDict);

            DateExtractor = new BaseCJKDateExtractor(new ChineseDateExtractorConfiguration(this));
            TimeExtractor = new BaseCJKTimeExtractor(new ChineseTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseCJKDateTimeExtractor(new ChineseDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseCJKDatePeriodExtractor(new ChineseDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new ChineseTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseCJKDateTimePeriodExtractor(new ChineseDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new ChineseDurationExtractorConfiguration(this));
            SetExtractor = new BaseCJKSetExtractor(new ChineseSetExtractorConfiguration(this));
            HolidayExtractor = new BaseCJKHolidayExtractor(new ChineseHolidayExtractorConfiguration(this));
        }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor SetExtractor { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        Regex ICJKMergedExtractorConfiguration.AfterRegex => AfterRegex;

        Regex ICJKMergedExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex ICJKMergedExtractorConfiguration.UnspecificDatePeriodRegex => UnspecificDatePeriodRegex;

        Regex ICJKMergedExtractorConfiguration.SincePrefixRegex => SincePrefixRegex;

        Regex ICJKMergedExtractorConfiguration.SinceSuffixRegex => SinceSuffixRegex;

        Regex ICJKMergedExtractorConfiguration.AroundPrefixRegex => AroundPrefixRegex;

        Regex ICJKMergedExtractorConfiguration.AroundSuffixRegex => AroundSuffixRegex;

        Regex ICJKMergedExtractorConfiguration.UntilRegex => UntilRegex;

        Regex ICJKMergedExtractorConfiguration.EqualRegex => EqualRegex;

        Regex ICJKMergedExtractorConfiguration.PotentialAmbiguousRangeRegex => PotentialAmbiguousRangeRegex;

        Regex ICJKMergedExtractorConfiguration.AmbiguousRangeModifierPrefix => AmbiguousRangeModifierPrefix;

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; }
    }
}