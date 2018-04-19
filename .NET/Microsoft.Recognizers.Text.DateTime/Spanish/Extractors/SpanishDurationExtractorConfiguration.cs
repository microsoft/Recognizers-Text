using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Number;
using System.Collections.Immutable;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDurationExtractorConfiguration : BaseOptionsConfiguration, IDurationExtractorConfiguration
    {
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.UnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: improve Spanish the SuffixAndRegex
        public static readonly Regex SuffixAndRegex = new Regex(DateTimeDefinitions.SuffixAndRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.FollowedUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.DurationNumberCombinedWithUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add half in AnUnitRegex
        public static readonly Regex AnUnitRegex = new Regex(DateTimeDefinitions.AnUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex AllRegex = new Regex(DateTimeDefinitions.AllRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex HalfRegex = new Regex(DateTimeDefinitions.HalfRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: change to Spanish according to corresponding Regex
        public static readonly Regex ConjunctionRegex = new Regex(@"^[\.]", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InexactNumberRegex = new Regex(DateTimeDefinitions.InexactNumberRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex InexactNumberUnitRegex = new Regex(DateTimeDefinitions.InexactNumberUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: change to Spanish according to corresponding Regex
        public static readonly Regex RelativeDurationUnitRegex = new Regex(@"^[\.]", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DurationUnitRegex = new Regex(DateTimeDefinitions.DurationUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DurationConnectorRegex = new Regex(DateTimeDefinitions.DurationConnectorRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishDurationExtractorConfiguration() : base(DateTimeOptions.None)
        {
            CardinalExtractor = Number.Spanish.CardinalExtractor.GetInstance();
            UnitMap = DateTimeDefinitions.UnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.UnitValueMap.ToImmutableDictionary();
        }

        public IExtractor CardinalExtractor { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        Regex IDurationExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex IDurationExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex IDurationExtractorConfiguration.AnUnitRegex => AnUnitRegex;

        Regex IDurationExtractorConfiguration.AllRegex => AllRegex;

        Regex IDurationExtractorConfiguration.HalfRegex => HalfRegex;

        Regex IDurationExtractorConfiguration.SuffixAndRegex => SuffixAndRegex;

        Regex IDurationExtractorConfiguration.ConjunctionRegex => ConjunctionRegex;

        Regex IDurationExtractorConfiguration.InexactNumberRegex => InexactNumberRegex;

        Regex IDurationExtractorConfiguration.InexactNumberUnitRegex => InexactNumberUnitRegex;

        Regex IDurationExtractorConfiguration.RelativeDurationUnitRegex => RelativeDurationUnitRegex;

        Regex IDurationExtractorConfiguration.DurationUnitRegex => DurationUnitRegex;

        Regex IDurationExtractorConfiguration.DurationConnectorRegex => DurationConnectorRegex;
    }
}
