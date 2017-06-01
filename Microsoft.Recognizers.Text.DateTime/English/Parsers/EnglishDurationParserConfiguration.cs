using Microsoft.Recognizers.Text.DateTime.English.Extractors;
using Microsoft.Recognizers.Text.DateTime.Parsers;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English.Parsers
{
    public class EnglishDurationParserConfiguration : IDurationParserConfiguration
    {
        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex AnUnitRegex { get; }

        public Regex AllDateUnitRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        public EnglishDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            NumberCombinedWithUnit = EnglishDurationExtractorConfiguration.NumberCombinedWithUnit;
            AnUnitRegex = EnglishDurationExtractorConfiguration.AnUnitRegex;
            AllDateUnitRegex = EnglishDurationExtractorConfiguration.AllRegex;
            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
        }
    }
}
