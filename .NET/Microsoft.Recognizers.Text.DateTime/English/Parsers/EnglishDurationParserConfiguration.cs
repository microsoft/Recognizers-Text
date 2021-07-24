﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDurationParserConfiguration : BaseDateTimeOptionsConfiguration, IDurationParserConfiguration
    {

        public static readonly Regex PrefixArticleRegex =
            new Regex(DateTimeDefinitions.PrefixArticleRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public EnglishDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;

            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(this), false);

            NumberCombinedWithUnit = EnglishDurationExtractorConfiguration.NumberCombinedWithDurationUnit;

            AnUnitRegex = EnglishDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = EnglishDurationExtractorConfiguration.DuringRegex;
            AllDateUnitRegex = EnglishDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = EnglishDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = EnglishDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = EnglishDurationExtractorConfiguration.DurationFollowedUnit;
            ConjunctionRegex = EnglishDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = EnglishDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = EnglishDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = EnglishDurationExtractorConfiguration.DurationUnitRegex;
            SpecialNumberUnitRegex = EnglishDurationExtractorConfiguration.SpecialNumberUnitRegex;

            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
            DoubleNumbers = config.DoubleNumbers;
        }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex AnUnitRegex { get; }

        Regex IDurationParserConfiguration.PrefixArticleRegex => PrefixArticleRegex;

        public Regex DuringRegex { get; }

        public Regex AllDateUnitRegex { get; }

        public Regex HalfDateUnitRegex { get; }

        public Regex SuffixAndRegex { get; }

        public Regex FollowedUnit { get; }

        public Regex ConjunctionRegex { get; }

        public Regex InexactNumberRegex { get; }

        public Regex InexactNumberUnitRegex { get; }

        public Regex DurationUnitRegex { get; }

        public Regex SpecialNumberUnitRegex { get; }

        bool IDurationParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        public IImmutableDictionary<string, double> DoubleNumbers { get; }
    }
}
