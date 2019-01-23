using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianDurationParserConfiguration : BaseOptionsConfiguration, IDurationParserConfiguration
    {
        public ItalianDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config.Options)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = new BaseDurationExtractor(new ItalianDurationExtractorConfiguration(this), false);
            NumberCombinedWithUnit = ItalianDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            AnUnitRegex = ItalianDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = ItalianDurationExtractorConfiguration.DuringRegex;
            AllDateUnitRegex = ItalianDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = ItalianDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = ItalianDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = ItalianDurationExtractorConfiguration.DurationFollowedUnit;
            ConjunctionRegex = ItalianDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = ItalianDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = ItalianDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = ItalianDurationExtractorConfiguration.DurationUnitRegex;
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
