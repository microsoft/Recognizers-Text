using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDurationExtractorConfiguration : IDurationExtractorConfiguration
    {
        public static readonly Regex UnitRegex =
            new Regex(
                @"(?<unit>años|año|meses|mes|semanas|semana|d[ií]as|d[ií]a|horas|hora|h|hr|hrs|hs|minutos|minuto|mins|min|segundos|segundo|segs|seg)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: improve Spanish the SuffixAndRegex
        public static readonly Regex SuffixAndRegex = new Regex(@"(?<suffix>\s*(y)\s+((un|uno|una)\s+)?(?<suffix_num>media|cuarto))",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex($@"^\s*{UnitRegex}",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit =
            new Regex($@"\b(?<num>\d+(\,\d*)?){UnitRegex}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add half in AnUnitRegex
        public static readonly Regex AnUnitRegex = new Regex($@"\b(un(a)?)\s+{UnitRegex}",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AllRegex = new Regex(@"\b(?<all>tod[oa]?\s+(el|la)\s+(?<unit>año|mes|semana|d[ií]a))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HalfRegex = new Regex(@"\b(?<half>medi[oa]\s+(?<unit>ano|mes|semana|d[íi]a|hora))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: change to Spanish according to corresponding Regex
        public static readonly Regex ConjunctionRegex = new Regex(@"^[\.]",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: change to Spanish according to corresponding Regex
        public static readonly Regex InExactNumberRegex = new Regex(@"\b(a few|few|some|several)\b",
           RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InExactNumberUnitRegex = new Regex($@"\b(a few|few|some|several)\s+{UnitRegex}",
           RegexOptions.IgnoreCase | RegexOptions.Singleline);

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
    }
}
