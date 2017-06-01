using Microsoft.Recognizers.Text.DateTime.Spanish.Extractors;
using Microsoft.Recognizers.Text.DateTime.Parsers;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Parsers
{
    public class SpanishDurationParserConfiguration : IDurationParserConfiguration
    {
        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex AnUnitRegex { get; }

        public Regex AllDateUnitRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        public SpanishDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            NumberCombinedWithUnit = SpanishDurationExtractorConfiguration.NumberCombinedWithUnit;
            AnUnitRegex = SpanishDurationExtractorConfiguration.AnUnitRegex;
            AllDateUnitRegex = SpanishDurationExtractorConfiguration.AllRegex;
            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
        }
    }
}
