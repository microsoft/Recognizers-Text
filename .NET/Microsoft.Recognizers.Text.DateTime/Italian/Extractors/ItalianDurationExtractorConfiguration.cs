using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianDurationExtractorConfiguration : BaseOptionsConfiguration, IDurationExtractorConfiguration
    {
        public static readonly Regex DurationUnitRegex =
            new Regex(DateTimeDefinitions.DurationUnitRegex, RegexFlags);

        public static readonly Regex SuffixAndRegex =
            new Regex(DateTimeDefinitions.SuffixAndRegex, RegexFlags);

        public static readonly Regex DurationFollowedUnit =
            new Regex(DateTimeDefinitions.DurationFollowedUnit, RegexFlags);

        public static readonly Regex NumberCombinedWithDurationUnit =
            new Regex(DateTimeDefinitions.NumberCombinedWithDurationUnit, RegexFlags);

        public static readonly Regex AnUnitRegex =
            new Regex(DateTimeDefinitions.AnUnitRegex, RegexFlags);

        public static readonly Regex DuringRegex =
            new Regex(DateTimeDefinitions.DuringRegex, RegexFlags);

        public static readonly Regex AllRegex =
            new Regex(DateTimeDefinitions.AllRegex, RegexFlags);

        public static readonly Regex HalfRegex =
            new Regex(DateTimeDefinitions.HalfRegex, RegexFlags);

        public static readonly Regex ConjunctionRegex =
            new Regex(DateTimeDefinitions.ConjunctionRegex, RegexFlags);

        // @TODO localize comment?
        // quelques = "a few, some," etc
        public static readonly Regex InexactNumberRegex =
            new Regex(DateTimeDefinitions.InexactNumberRegex, RegexFlags);

        public static readonly Regex InexactNumberUnitRegex =
            new Regex(DateTimeDefinitions.InexactNumberUnitRegex, RegexFlags);

        public static readonly Regex RelativeDurationUnitRegex =
            new Regex(DateTimeDefinitions.RelativeDurationUnitRegex, RegexFlags);

        public static readonly Regex DurationConnectorRegex =
            new Regex(DateTimeDefinitions.DurationConnectorRegex, RegexFlags);

        public static readonly Regex MoreThanRegex =
            new Regex(DateTimeDefinitions.MoreThanRegex, RegexFlags);

        public static readonly Regex LessThanRegex =
            new Regex(DateTimeDefinitions.LessThanRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ItalianDurationExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            CardinalExtractor = Number.Italian.CardinalExtractor.GetInstance();
            UnitMap = DateTimeDefinitions.UnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.UnitValueMap.ToImmutableDictionary();
        }

        public IExtractor CardinalExtractor { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        Regex IDurationExtractorConfiguration.FollowedUnit => DurationFollowedUnit;

        Regex IDurationExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithDurationUnit;

        Regex IDurationExtractorConfiguration.AnUnitRegex => AnUnitRegex;

        Regex IDurationExtractorConfiguration.DuringRegex => DuringRegex;

        Regex IDurationExtractorConfiguration.AllRegex => AllRegex;

        Regex IDurationExtractorConfiguration.HalfRegex => HalfRegex;

        Regex IDurationExtractorConfiguration.SuffixAndRegex => SuffixAndRegex;

        Regex IDurationExtractorConfiguration.ConjunctionRegex => ConjunctionRegex;

        Regex IDurationExtractorConfiguration.InexactNumberRegex => InexactNumberRegex;

        Regex IDurationExtractorConfiguration.InexactNumberUnitRegex => InexactNumberUnitRegex;

        Regex IDurationExtractorConfiguration.RelativeDurationUnitRegex => RelativeDurationUnitRegex;

        Regex IDurationExtractorConfiguration.DurationUnitRegex => DurationUnitRegex;

        Regex IDurationExtractorConfiguration.DurationConnectorRegex => DurationConnectorRegex;

        Regex IDurationExtractorConfiguration.MoreThanRegex => MoreThanRegex;

        Regex IDurationExtractorConfiguration.LessThanRegex => LessThanRegex;
    }
}
