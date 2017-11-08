using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDurationExtractorConfiguration : IDurationExtractorConfiguration
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

        public static readonly Regex InExactNumberRegex = new Regex(DateTimeDefinitions.InExactNumberRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex InExactNumberUnitRegex = new Regex(DateTimeDefinitions.InExactNumberUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: change to Spanish according to corresponding Regex
        public static readonly Regex RelativeDurationUnitRegex = new Regex(@"^[\.]", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishDurationExtractorConfiguration()
        {
            CardinalExtractor = Number.Spanish.CardinalExtractor.GetInstance();
        }

        public IExtractor CardinalExtractor { get; }

        Regex IDurationExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex IDurationExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex IDurationExtractorConfiguration.AnUnitRegex => AnUnitRegex;

        Regex IDurationExtractorConfiguration.AllRegex => AllRegex;

        Regex IDurationExtractorConfiguration.HalfRegex => HalfRegex;

        Regex IDurationExtractorConfiguration.SuffixAndRegex => SuffixAndRegex;

        Regex IDurationExtractorConfiguration.ConjunctionRegex => ConjunctionRegex;

        Regex IDurationExtractorConfiguration.InExactNumberRegex => InExactNumberRegex;

        Regex IDurationExtractorConfiguration.InExactNumberUnitRegex => InExactNumberUnitRegex;

        Regex IDurationExtractorConfiguration.RelativeDurationUnitRegex => RelativeDurationUnitRegex;
    }
}
