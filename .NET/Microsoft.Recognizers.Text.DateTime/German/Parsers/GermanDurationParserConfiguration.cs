using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDurationParserConfiguration : BaseOptionsConfiguration, IDurationParserConfiguration
    {
        public GermanDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(this), false);
            NumberCombinedWithUnit = GermanDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            AnUnitRegex = GermanDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = GermanDurationExtractorConfiguration.DuringRegex;
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

        public IExtractor CardinalExtractor { get; }

        public IExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex AnUnitRegex { get; }

        public Regex DuringRegex { get; }

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
    }
}
