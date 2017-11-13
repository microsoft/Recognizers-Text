using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDurationParserConfiguration : IDurationParserConfiguration
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

        public SpanishDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            NumberCombinedWithUnit = SpanishDurationExtractorConfiguration.NumberCombinedWithUnit;
            AnUnitRegex = SpanishDurationExtractorConfiguration.AnUnitRegex;
            AllDateUnitRegex = SpanishDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = SpanishDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = SpanishDurationExtractorConfiguration.SuffixAndRegex;
            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
            DoubleNumbers = config.DoubleNumbers;
            FollowedUnit = SpanishDurationExtractorConfiguration.FollowedUnit;
            ConjunctionRegex = SpanishDurationExtractorConfiguration.ConjunctionRegex;
            InExactNumberRegex = SpanishDurationExtractorConfiguration.InExactNumberRegex;
            InExactNumberUnitRegex = SpanishDurationExtractorConfiguration.InExactNumberUnitRegex;
        }
    }
}
