// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDurationExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDurationExtractorConfiguration
    {
        public static readonly Regex DurationUnitRegex =
            new Regex(DateTimeDefinitions.DurationUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SuffixAndRegex =
            new Regex(DateTimeDefinitions.SuffixAndRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DurationFollowedUnit =
            new Regex(DateTimeDefinitions.DurationFollowedUnit, RegexFlags, RegexTimeOut);

        public static readonly Regex NumberCombinedWithDurationUnit =
            new Regex(DateTimeDefinitions.NumberCombinedWithDurationUnit, RegexFlags, RegexTimeOut);

        public static readonly Regex AnUnitRegex =
            new Regex(DateTimeDefinitions.AnUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DuringRegex =
            new Regex(DateTimeDefinitions.DuringRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AllRegex =
            new Regex(DateTimeDefinitions.AllRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex HalfRegex =
            new Regex(DateTimeDefinitions.HalfRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ConjunctionRegex =
            new Regex(DateTimeDefinitions.ConjunctionRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex InexactNumberRegex =
            new Regex(DateTimeDefinitions.InexactNumberRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex InexactNumberUnitRegex =
            new Regex(DateTimeDefinitions.InexactNumberUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RelativeDurationUnitRegex =
            new Regex(DateTimeDefinitions.RelativeDurationUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DurationConnectorRegex =
            new Regex(DateTimeDefinitions.DurationConnectorRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ModPrefixRegex =
            new Regex(DateTimeDefinitions.ModPrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ModSuffixRegex =
            new Regex(DateTimeDefinitions.ModSuffixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SpecialNumberUnitRegex =
            new Regex(DateTimeDefinitions.SpecialNumberUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MoreThanRegex =
            new Regex(DateTimeDefinitions.MoreThanRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex LessThanRegex =
            new Regex(DateTimeDefinitions.LessThanRegex, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public GermanDurationExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = Number.German.NumberExtractor.GetInstance(numConfig);

            UnitMap = DateTimeDefinitions.UnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.UnitValueMap.ToImmutableDictionary();
        }

        public IExtractor CardinalExtractor { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        bool IDurationExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        Regex IDurationExtractorConfiguration.FollowedUnit => DurationFollowedUnit;

        Regex IDurationExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithDurationUnit;

        Regex IDurationExtractorConfiguration.AnUnitRegex => AnUnitRegex;

        Regex IDurationExtractorConfiguration.DuringRegex => DuringRegex;

        Regex IDurationExtractorConfiguration.AllRegex => AllRegex;

        Regex IDurationExtractorConfiguration.HalfRegex => HalfRegex;

        Regex IDurationExtractorConfiguration.SuffixAndRegex => SuffixAndRegex;

        Regex IDurationExtractorConfiguration.ConjunctionRegex => ConjunctionRegex;

        Regex IDurationExtractorConfiguration.InexactNumberRegex => InexactNumberRegex;

        Regex IDurationExtractorConfiguration.InexactNumberUnitRegex => InexactNumberUnitRegex;

        Regex IDurationExtractorConfiguration.RelativeDurationUnitRegex => RelativeDurationUnitRegex;

        Regex IDurationExtractorConfiguration.DurationUnitRegex => DurationUnitRegex;

        Regex IDurationExtractorConfiguration.DurationConnectorRegex => DurationConnectorRegex;

        Regex IDurationExtractorConfiguration.SpecialNumberUnitRegex => SpecialNumberUnitRegex;

        Regex IDurationExtractorConfiguration.MoreThanRegex => MoreThanRegex;

        Regex IDurationExtractorConfiguration.LessThanRegex => LessThanRegex;

        Regex IDurationExtractorConfiguration.ModPrefixRegex => ModPrefixRegex;

        Regex IDurationExtractorConfiguration.ModSuffixRegex => ModSuffixRegex;

        public Dictionary<Regex, Regex> AmbiguityFiltersDict => null;
    }
}