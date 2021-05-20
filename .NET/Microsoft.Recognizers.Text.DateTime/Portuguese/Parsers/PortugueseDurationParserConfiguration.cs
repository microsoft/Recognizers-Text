using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDurationParserConfiguration : BaseDateTimeOptionsConfiguration, IDurationParserConfiguration
    {
        public static readonly Regex PrefixArticleRegex =
            new Regex(DateTimeDefinitions.PrefixArticleRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

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
            SpecialNumberUnitRegex = PortugueseDurationExtractorConfiguration.SpecialNumberUnitRegex;
        }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

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
