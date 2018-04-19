using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDurationParserConfiguration : BaseOptionsConfiguration, IDurationParserConfiguration
    {
        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IExtractor DurationExtractor { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex AnUnitRegex { get; }

        public Regex AllDateUnitRegex { get; }

        public Regex HalfDateUnitRegex { get; }

        public Regex SuffixAndRegex { get; }

        public Regex FollowedUnit { get; }

        public Regex ConjunctionRegex { get; }

        public Regex InexactNumberRegex { get; }

        public Regex InexactNumberUnitRegex { get; }

        public Regex DurationUnitRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        public IImmutableDictionary<string, double> DoubleNumbers { get; }

        public SpanishDurationParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(), false);
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
            InexactNumberRegex = SpanishDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = SpanishDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = SpanishDurationExtractorConfiguration.DurationUnitRegex;
        }
    }
}
