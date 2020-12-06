using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDurationExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDurationExtractorConfiguration
    {
        public static readonly Regex DurationUnitRegex =
            RegexCache.Get(DateTimeDefinitions.DurationUnitRegex, RegexFlags);

        public static readonly Regex SuffixAndRegex =
            RegexCache.Get(DateTimeDefinitions.SuffixAndRegex, RegexFlags);

        public static readonly Regex DurationFollowedUnit =
            RegexCache.Get(DateTimeDefinitions.DurationFollowedUnit, RegexFlags);

        public static readonly Regex NumberCombinedWithDurationUnit =
            RegexCache.Get(DateTimeDefinitions.NumberCombinedWithDurationUnit, RegexFlags);

        public static readonly Regex AnUnitRegex =
            RegexCache.Get(DateTimeDefinitions.AnUnitRegex, RegexFlags);

        public static readonly Regex DuringRegex =
            RegexCache.Get(DateTimeDefinitions.DuringRegex, RegexFlags);

        public static readonly Regex AllRegex =
            RegexCache.Get(DateTimeDefinitions.AllRegex, RegexFlags);

        public static readonly Regex HalfRegex =
            RegexCache.Get(DateTimeDefinitions.HalfRegex, RegexFlags);

        public static readonly Regex ConjunctionRegex =
            RegexCache.Get(DateTimeDefinitions.ConjunctionRegex, RegexFlags);

        public static readonly Regex InexactNumberRegex =
            RegexCache.Get(DateTimeDefinitions.InexactNumberRegex, RegexFlags);

        public static readonly Regex InexactNumberUnitRegex =
            RegexCache.Get(DateTimeDefinitions.InexactNumberUnitRegex, RegexFlags);

        public static readonly Regex RelativeDurationUnitRegex =
            RegexCache.Get(DateTimeDefinitions.RelativeDurationUnitRegex, RegexFlags);

        public static readonly Regex DurationConnectorRegex =
            RegexCache.Get(DateTimeDefinitions.DurationConnectorRegex, RegexFlags);

        public static readonly Regex SpecialNumberUnitRegex =
            RegexCache.Get(DateTimeDefinitions.SpecialNumberUnitRegex, RegexFlags);

        public static readonly Regex MoreThanRegex =
            RegexCache.Get(DateTimeDefinitions.MoreThanRegex, RegexFlags);

        public static readonly Regex LessThanRegex =
            RegexCache.Get(DateTimeDefinitions.LessThanRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public GermanDurationExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            CardinalExtractor = Number.German.NumberExtractor.GetInstance();
            UnitMap = DateTimeDefinitions.UnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.UnitValueMap.ToImmutableDictionary();
        }

        public IExtractor CardinalExtractor { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        bool IDurationExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

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

        Regex IDurationExtractorConfiguration.SpecialNumberUnitRegex => SpecialNumberUnitRegex;

        Regex IDurationExtractorConfiguration.MoreThanRegex => MoreThanRegex;

        Regex IDurationExtractorConfiguration.LessThanRegex => LessThanRegex;
    }
}