using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDurationExtractorConfiguration : IDurationExtractorConfiguration
    {
        public static readonly Regex DurationUnitRegex =
            new Regex(
                @"(?<unit>ann[eé]es|ann[eé]e|ans|mois|semaines|semaine|heure|heures|h|hr|hrs|minutes|minute|mins|min|secondes|seconde|secs|sec)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: improve French the SuffixAndRegex
        public static readonly Regex SuffixAndRegex = new Regex(@"(?<suffix>\s*(et)\s+((une)\s+)?(?<suffix_num>demi|quart))",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DurationFollowedUnit = new Regex($@"^\s*{SuffixAndRegex}?(\s+|-)?{DurationUnitRegex}",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithDurationUnit = new Regex($@"\b(?<num>\d+(\.\d*)?)(-)?{DurationUnitRegex}",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AnUnitRegex = new Regex($@"\b(((?<half>demi(-)?\s+)?(une|un))|(un|une))\s+{DurationUnitRegex}",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AllRegex = new Regex(@"\b(?<all>tout?\s+(le|la|l')\s+(?<unit>ann[eé]e|mois|semaine|journ[eé]e))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HalfRegex = new Regex(@"\b(?<half>demi(-)?\s+(?<unit>ann[eé]e|mois|semaine|journ[eé]e|heure))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ConjunctionRegex = new Regex(@"\b((et(\s+pendant)?)|avec)\b", 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InExactNumberRegex = new Regex(@"\b(quelques|peu|some|plusieurs)\b", //quelques = "a few, some," etc 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InExactNumberUnitRegex = new Regex($@"({InExactNumberRegex})\s+({DurationUnitRegex})",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public FrenchDurationExtractorConfiguration()
        {
            CardinalExtractor = Number.French.CardinalExtractor.GetInstance();
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
    }
}
