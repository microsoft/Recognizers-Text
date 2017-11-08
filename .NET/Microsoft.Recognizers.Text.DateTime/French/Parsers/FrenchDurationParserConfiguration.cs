using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDurationParserConfiguration : IDurationParserConfiguration
    {
        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex AnUnitRegex { get; }

        public Regex AllDateUnitRegex { get; }

        public Regex HalfDateUnitRegex { get; }

        public Regex SuffixAndRegex { get; }

        public Regex FollowedUnit { get; }

        public Regex ConjunctionRegex { get; }

        public Regex InExactNumberRegex { get; }

        public Regex InExactNumberUnitRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        public IImmutableDictionary<string, double> DoubleNumbers { get; }

        public FrenchDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            NumberCombinedWithUnit = FrenchDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            AnUnitRegex = FrenchDurationExtractorConfiguration.AnUnitRegex;
            AllDateUnitRegex = FrenchDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = FrenchDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = FrenchDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = FrenchDurationExtractorConfiguration.DurationFollowedUnit;
            ConjunctionRegex = FrenchDurationExtractorConfiguration.ConjunctionRegex;
            InExactNumberRegex = FrenchDurationExtractorConfiguration.InExactNumberRegex;
            InExactNumberUnitRegex = FrenchDurationExtractorConfiguration.InExactNumberUnitRegex;
            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
            DoubleNumbers = config.DoubleNumbers;
        }
    }
}
