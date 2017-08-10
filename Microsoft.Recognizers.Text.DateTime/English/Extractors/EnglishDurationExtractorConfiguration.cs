using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDurationExtractorConfiguration : IDurationExtractorConfiguration
    {
        public static readonly Regex DurationUnitRegex =
            new Regex(
                DateTimeDefinition.DurationUnitRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SuffixAndRegex = new Regex(DateTimeDefinition.SuffixAndRegex,
           RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DurationFollowedUnit = new Regex(DateTimeDefinition.DurationFollowedUnit,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithDurationUnit =
            new Regex(DateTimeDefinition.NumberCombinedWithDurationUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AnUnitRegex = new Regex(DateTimeDefinition.AnUnitRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AllRegex = new Regex(DateTimeDefinition.AllRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HalfRegex = new Regex(DateTimeDefinition.HalfRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public EnglishDurationExtractorConfiguration()
        {
            CardinalExtractor = new CardinalExtractor();
        }

        public IExtractor CardinalExtractor { get; }

        Regex IDurationExtractorConfiguration.FollowedUnit => DurationFollowedUnit;

        Regex IDurationExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithDurationUnit;

        Regex IDurationExtractorConfiguration.AnUnitRegex => AnUnitRegex;

        Regex IDurationExtractorConfiguration.AllRegex => AllRegex;

        Regex IDurationExtractorConfiguration.HalfRegex => HalfRegex;
        
        Regex IDurationExtractorConfiguration.SuffixAndRegex => SuffixAndRegex;
    }
}