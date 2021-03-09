using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanDurationParserConfiguration : BaseDateTimeOptionsConfiguration, IDurationParserConfiguration
    {
        public KoreanDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;

            DurationExtractor = new BaseDurationExtractor(new KoreanDurationExtractorConfiguration(this), false);

            NumberCombinedWithUnit = KoreanDurationExtractorConfiguration.NumberCombinedWithDurationUnit;

            AnUnitRegex = KoreanDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = KoreanDurationExtractorConfiguration.DuringRegex;
            AllDateUnitRegex = KoreanDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = KoreanDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = KoreanDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = KoreanDurationExtractorConfiguration.DurationFollowedUnit;
            ConjunctionRegex = KoreanDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = KoreanDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = KoreanDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = KoreanDurationExtractorConfiguration.DurationUnitRegex;
            SpecialNumberUnitRegex = KoreanDurationExtractorConfiguration.SpecialNumberUnitRegex;

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
