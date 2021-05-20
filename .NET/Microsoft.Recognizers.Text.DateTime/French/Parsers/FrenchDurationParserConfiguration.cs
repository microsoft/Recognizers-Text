using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDurationParserConfiguration : BaseDateTimeOptionsConfiguration, IDurationParserConfiguration
    {
        public static readonly Regex PrefixArticleRegex =
            new Regex(DateTimeDefinitions.PrefixArticleRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public FrenchDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(this), false);
            NumberCombinedWithUnit = FrenchDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            AnUnitRegex = FrenchDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = FrenchDurationExtractorConfiguration.DuringRegex;
            AllDateUnitRegex = FrenchDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = FrenchDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = FrenchDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = FrenchDurationExtractorConfiguration.DurationFollowedUnit;

            ConjunctionRegex = FrenchDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = FrenchDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = FrenchDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = FrenchDurationExtractorConfiguration.DurationUnitRegex;
            SpecialNumberUnitRegex = FrenchDurationExtractorConfiguration.SpecialNumberUnitRegex;

            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
            DoubleNumbers = config.DoubleNumbers;
        }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex AnUnitRegex { get; }

        Regex IDurationParserConfiguration.PrefixArticleRegex => PrefixArticleRegex;

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
