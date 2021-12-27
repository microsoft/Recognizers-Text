// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.DateTime.Swedish
{
    public class SwedishDurationParserConfiguration : BaseDateTimeOptionsConfiguration, IDurationParserConfiguration
    {

        public static readonly Regex PrefixArticleRegex =
            new Regex(DateTimeDefinitions.PrefixArticleRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public SwedishDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;

            DurationExtractor = new BaseDurationExtractor(new SwedishDurationExtractorConfiguration(this), false);

            NumberCombinedWithUnit = SwedishDurationExtractorConfiguration.NumberCombinedWithDurationUnit;

            AnUnitRegex = SwedishDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = SwedishDurationExtractorConfiguration.DuringRegex;
            AllDateUnitRegex = SwedishDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = SwedishDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = SwedishDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = SwedishDurationExtractorConfiguration.DurationFollowedUnit;
            ConjunctionRegex = SwedishDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = SwedishDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = SwedishDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = SwedishDurationExtractorConfiguration.DurationUnitRegex;
            SpecialNumberUnitRegex = SwedishDurationExtractorConfiguration.SpecialNumberUnitRegex;

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
