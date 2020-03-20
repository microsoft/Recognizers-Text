using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.DateTime.Turkish
{
    public class TurkishDurationParserConfiguration : BaseDateTimeOptionsConfiguration, IDurationParserConfiguration
    {
        public TurkishDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;

            DurationExtractor = new BaseDurationExtractor(new TurkishDurationExtractorConfiguration(this), false);

            NumberCombinedWithUnit = TurkishDurationExtractorConfiguration.NumberCombinedWithDurationUnit;

            AnUnitRegex = TurkishDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = TurkishDurationExtractorConfiguration.DuringRegex;
            AllDateUnitRegex = TurkishDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = TurkishDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = TurkishDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = TurkishDurationExtractorConfiguration.DurationFollowedUnit;
            ConjunctionRegex = TurkishDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = TurkishDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = TurkishDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = TurkishDurationExtractorConfiguration.DurationUnitRegex;
            SpecialNumberUnitRegex = TurkishDurationExtractorConfiguration.SpecialNumberUnitRegex;

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
