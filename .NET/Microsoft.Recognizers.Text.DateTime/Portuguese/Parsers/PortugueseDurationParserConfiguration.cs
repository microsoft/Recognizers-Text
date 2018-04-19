using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDurationParserConfiguration : BaseOptionsConfiguration, IDurationParserConfiguration
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

        public PortugueseDurationParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration(), false);
            NumberCombinedWithUnit = PortugueseDurationExtractorConfiguration.NumberCombinedWithUnit;
            AnUnitRegex = PortugueseDurationExtractorConfiguration.AnUnitRegex;
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
    }
}
