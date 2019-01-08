using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDurationExtractorConfiguration : BaseOptionsConfiguration, IDurationExtractorConfiguration
    {
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.UnitRegex, RegexOptions.Singleline);

        // TODO: improve Portuguese the SuffixAndRegex
        public static readonly Regex SuffixAndRegex = new Regex(DateTimeDefinitions.SuffixAndRegex, RegexOptions.Singleline);
        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.FollowedUnit, RegexOptions.Singleline);
        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.DurationNumberCombinedWithUnit, RegexOptions.Singleline);

        // TODO: add half in AnUnitRegex
        public static readonly Regex AnUnitRegex = new Regex(DateTimeDefinitions.AnUnitRegex, RegexOptions.Singleline);
        public static readonly Regex AllRegex = new Regex(DateTimeDefinitions.AllRegex, RegexOptions.Singleline);
        public static readonly Regex DuringRegex = new Regex(DateTimeDefinitions.DuringRegex, RegexOptions.Singleline);
        public static readonly Regex HalfRegex = new Regex(DateTimeDefinitions.HalfRegex, RegexOptions.Singleline);

        public static readonly Regex ConjunctionRegex = new Regex(DateTimeDefinitions.ConjunctionRegex, RegexOptions.Singleline);

        public static readonly Regex InexactNumberRegex = new Regex(DateTimeDefinitions.InexactNumberRegex, RegexOptions.Singleline);
        public static readonly Regex InexactNumberUnitRegex = new Regex(DateTimeDefinitions.InexactNumberUnitRegex, RegexOptions.Singleline);

        public static readonly Regex RelativeDurationUnitRegex = new Regex(DateTimeDefinitions.RelativeDurationUnitRegex, RegexOptions.Singleline);

        public static readonly Regex DurationUnitRegex = new Regex(DateTimeDefinitions.DurationUnitRegex, RegexOptions.Singleline);

        public static readonly Regex DurationConnectorRegex = new Regex(DateTimeDefinitions.DurationConnectorRegex, RegexOptions.Singleline);

        public static readonly Regex MoreThanRegex =
            new Regex(DateTimeDefinitions.MoreThanRegex, RegexOptions.Singleline);

        public static readonly Regex LessThanRegex =
            new Regex(DateTimeDefinitions.LessThanRegex, RegexOptions.Singleline);

        public PortugueseDurationExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            CardinalExtractor = Number.Portuguese.CardinalExtractor.GetInstance();
            UnitMap = DateTimeDefinitions.UnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.UnitValueMap.ToImmutableDictionary();
        }

        public IExtractor CardinalExtractor { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        Regex IDurationExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex IDurationExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

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
