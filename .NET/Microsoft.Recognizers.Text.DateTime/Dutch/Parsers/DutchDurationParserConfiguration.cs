using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchDurationParserConfiguration : BaseDateTimeOptionsConfiguration, IDurationParserConfiguration
    {
        public DutchDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = new BaseDurationExtractor(new DutchDurationExtractorConfiguration(this), false);
            NumberCombinedWithUnit = DutchDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            AnUnitRegex = DutchDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = DutchDurationExtractorConfiguration.DuringRegex;
            AllDateUnitRegex = DutchDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = DutchDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = DutchDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = DutchDurationExtractorConfiguration.DurationFollowedUnit;

            ConjunctionRegex = DutchDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = DutchDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = DutchDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = DutchDurationExtractorConfiguration.DurationUnitRegex;
            SpecialNumberUnitRegex = DutchDurationExtractorConfiguration.SpecialNumberUnitRegex;

            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
            DoubleNumbers = config.DoubleNumbers;
        }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

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

        public Regex SpecialNumberUnitRegex { get; }

        bool IDurationParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        public IImmutableDictionary<string, double> DoubleNumbers { get; }
    }
}
