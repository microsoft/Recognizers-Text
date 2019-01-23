using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDurationParserConfiguration : BaseOptionsConfiguration, IDurationParserConfiguration
    {
        public PortugueseDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration(this), false);
            NumberCombinedWithUnit = PortugueseDurationExtractorConfiguration.NumberCombinedWithUnit;
            AnUnitRegex = PortugueseDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = PortugueseDurationExtractorConfiguration.DuringRegex;
            AllDateUnitRegex = PortugueseDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = PortugueseDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = PortugueseDurationExtractorConfiguration.SuffixAndRegex;
            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
            DoubleNumbers = config.DoubleNumbers;
            FollowedUnit = PortugueseDurationExtractorConfiguration.FollowedUnit;
            ConjunctionRegex = PortugueseDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = PortugueseDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = PortugueseDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = PortugueseDurationExtractorConfiguration.DurationUnitRegex;
        }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IExtractor DurationExtractor { get; }

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
