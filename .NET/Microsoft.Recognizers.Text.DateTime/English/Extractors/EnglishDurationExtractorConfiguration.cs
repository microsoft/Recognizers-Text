using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Number;
using System.Collections.Immutable;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDurationExtractorConfiguration : BaseOptionsConfiguration, IDurationExtractorConfiguration
    {
        public static readonly Regex DurationUnitRegex =
            new Regex(DateTimeDefinitions.DurationUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SuffixAndRegex = 
            new Regex(DateTimeDefinitions.SuffixAndRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DurationFollowedUnit = 
            new Regex(DateTimeDefinitions.DurationFollowedUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithDurationUnit =
            new Regex(DateTimeDefinitions.NumberCombinedWithDurationUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AnUnitRegex = 
            new Regex(DateTimeDefinitions.AnUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DuringRegex = 
            new Regex(DateTimeDefinitions.DuringRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AllRegex = 
            new Regex(DateTimeDefinitions.AllRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HalfRegex = 
            new Regex(DateTimeDefinitions.HalfRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ConjunctionRegex = 
            new Regex(DateTimeDefinitions.ConjunctionRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InexactNumberRegex = 
            new Regex(DateTimeDefinitions.InexactNumberRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InexactNumberUnitRegex = 
            new Regex(DateTimeDefinitions.InexactNumberUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeDurationUnitRegex =
            new Regex(DateTimeDefinitions.RelativeDurationUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DurationConnectorRegex =
            new Regex(DateTimeDefinitions.DurationConnectorRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MoreThanRegex =
            new Regex(DateTimeDefinitions.MoreThanRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);

        public static readonly Regex LessThanRegex =
            new Regex(DateTimeDefinitions.LessThanRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);

        public EnglishDurationExtractorConfiguration(DateTimeOptions options = DateTimeOptions.None) : base(options)
        {
            CardinalExtractor = Number.English.CardinalExtractor.GetInstance();
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