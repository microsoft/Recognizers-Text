﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.NumberWithUnit.Korean;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{

    public class KoreanDurationExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKDurationExtractorConfiguration
    {

        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.DurationYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DurationUnitRegex = new Regex(DateTimeDefinitions.DurationUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AnUnitRegex = new Regex(DateTimeDefinitions.AnUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DurationConnectorRegex = new Regex(DateTimeDefinitions.DurationConnectorRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AllRegex = new Regex(DateTimeDefinitions.DurationAllRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex HalfRegex = new Regex(DateTimeDefinitions.DurationHalfRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RelativeDurationUnitRegex = new Regex(DateTimeDefinitions.DurationRelativeDurationUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DuringRegex = new Regex(DateTimeDefinitions.DurationDuringRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SomeRegex = new Regex(DateTimeDefinitions.DurationSomeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MoreOrLessRegex = new Regex(DateTimeDefinitions.DurationMoreOrLessRegex, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private readonly bool merge;

        public KoreanDurationExtractorConfiguration(IDateTimeOptionsConfiguration config, bool merge = true)
            : base(config)
        {
            this.merge = merge;

            InternalExtractor = new NumberWithUnitExtractor(new DurationExtractorConfiguration());

            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToDictionary(k => k.Key, k => k.Value);
            UnitValueMap = DateTimeDefinitions.DurationUnitValueMap;
        }

        public IExtractor InternalExtractor { get; }

        public Dictionary<string, string> UnitMap { get; }

        public Dictionary<string, long> UnitValueMap { get; }

        public Dictionary<Regex, Regex> AmbiguityDurationFiltersDict => null;

        Regex ICJKDurationExtractorConfiguration.DurationUnitRegex => DurationUnitRegex;

        Regex ICJKDurationExtractorConfiguration.DurationConnectorRegex => DurationConnectorRegex;

        Regex ICJKDurationExtractorConfiguration.YearRegex => YearRegex;

        Regex ICJKDurationExtractorConfiguration.AllRegex => AllRegex;

        Regex ICJKDurationExtractorConfiguration.HalfRegex => HalfRegex;

        Regex ICJKDurationExtractorConfiguration.RelativeDurationUnitRegex => RelativeDurationUnitRegex;

        Regex ICJKDurationExtractorConfiguration.DuringRegex => DuringRegex;

        Regex ICJKDurationExtractorConfiguration.SomeRegex => SomeRegex;

        Regex ICJKDurationExtractorConfiguration.MoreOrLessRegex => MoreOrLessRegex;

        internal class DurationExtractorConfiguration : KoreanNumberWithUnitExtractorConfiguration
        {
            public static readonly ImmutableDictionary<string, string> DurationSuffixList = DateTimeDefinitions.DurationSuffixList.ToImmutableDictionary();

            public DurationExtractorConfiguration()
                : base(new CultureInfo(Text.Culture.Korean))
            {
            }

            public override ImmutableDictionary<string, string> SuffixList => DurationSuffixList;

            public override ImmutableDictionary<string, string> PrefixList => null;

            public override string ExtractType => Constants.SYS_DATETIME_DURATION;

            public override ImmutableList<string> AmbiguousUnitList => DateTimeDefinitions.DurationAmbiguousUnits.ToImmutableList();
        }
    }
}