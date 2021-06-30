using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Arabic;

namespace Microsoft.Recognizers.Text.DateTime.Arabic
{
    public class ArabicDurationParserConfiguration : BaseDateTimeOptionsConfiguration, IDurationParserConfiguration
    {

        public static readonly Regex PrefixArticleRegex =
            new Regex(DateTimeDefinitions.PrefixArticleRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ArabicDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {

            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;

            DurationExtractor = new BaseDurationExtractor(new ArabicDurationExtractorConfiguration(this), false);

            NumberCombinedWithUnit = ArabicDurationExtractorConfiguration.NumberCombinedWithDurationUnit;

            AnUnitRegex = ArabicDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = ArabicDurationExtractorConfiguration.DuringRegex;
            AllDateUnitRegex = ArabicDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = ArabicDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = ArabicDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = ArabicDurationExtractorConfiguration.DurationFollowedUnit;
            ConjunctionRegex = ArabicDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = ArabicDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = ArabicDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = ArabicDurationExtractorConfiguration.DurationUnitRegex;
            SpecialNumberUnitRegex = ArabicDurationExtractorConfiguration.SpecialNumberUnitRegex;

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
