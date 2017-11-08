using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDurationExtractorConfiguration : IDurationExtractorConfiguration
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

        public static readonly Regex AllRegex = 
            new Regex(DateTimeDefinitions.AllRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HalfRegex = 
            new Regex(DateTimeDefinitions.HalfRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ConjunctionRegex = 
            new Regex(DateTimeDefinitions.ConjunctionRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InExactNumberRegex = 
            new Regex(DateTimeDefinitions.InExactNumberRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InExactNumberUnitRegex = 
            new Regex(DateTimeDefinitions.InExactNumberUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeDurationUnitRegex =
            new Regex(DateTimeDefinitions.RelativeDurationUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public EnglishDurationExtractorConfiguration()
        {
            CardinalExtractor = Number.English.CardinalExtractor.GetInstance();
        }

        public IExtractor CardinalExtractor { get; }

        Regex IDurationExtractorConfiguration.FollowedUnit => DurationFollowedUnit;

        Regex IDurationExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithDurationUnit;

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