using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDurationParserConfiguration : IDurationParserConfiguration
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

        public EnglishDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            NumberCombinedWithUnit = EnglishDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            AnUnitRegex = EnglishDurationExtractorConfiguration.AnUnitRegex;
            AllDateUnitRegex = EnglishDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = EnglishDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = EnglishDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = EnglishDurationExtractorConfiguration.DurationFollowedUnit;
            ConjunctionRegex = EnglishDurationExtractorConfiguration.ConjunctionRegex;
            InExactNumberRegex = EnglishDurationExtractorConfiguration.InExactNumberRegex;
            InExactNumberUnitRegex = EnglishDurationExtractorConfiguration.InExactNumberUnitRegex;
            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
            DoubleNumbers = config.DoubleNumbers;
        }
    }
}
