using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDurationParserConfiguration : BaseOptionsConfiguration, IDurationParserConfiguration
    {
        public IExtractor CardinalExtractor { get; }

        public IExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

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

        public GermanDurationParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(), false);
            NumberCombinedWithUnit = GermanDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            AnUnitRegex = GermanDurationExtractorConfiguration.AnUnitRegex;
            AllDateUnitRegex = GermanDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = GermanDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = GermanDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = GermanDurationExtractorConfiguration.DurationFollowedUnit;
            ConjunctionRegex = GermanDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = GermanDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = GermanDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = GermanDurationExtractorConfiguration.DurationUnitRegex;
            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
            DoubleNumbers = config.DoubleNumbers;
        }
    }
}
