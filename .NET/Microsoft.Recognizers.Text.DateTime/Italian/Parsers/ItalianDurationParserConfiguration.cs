using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianDurationParserConfiguration : BaseDateTimeOptionsConfiguration, IDurationParserConfiguration
    {
        public static readonly Regex InexactNumberUnitRegex2 =
            new Regex(DateTimeDefinitions.InexactNumberUnitRegex2, RegexFlags);

        public static readonly Regex PrefixArticleRegex =
            new Regex(DateTimeDefinitions.PrefixArticleRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ItalianDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
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
            InexactNumberUnitRegex = InexactNumberUnitRegex2;
            DurationUnitRegex = ItalianDurationExtractorConfiguration.DurationUnitRegex;
            SpecialNumberUnitRegex = ItalianDurationExtractorConfiguration.SpecialNumberUnitRegex;

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
