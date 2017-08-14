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

        //TODO: change to Spanish the AndRegex
        public static readonly Regex SuffixAndRegex = new Regex(@"(?<suffix>\s*(and)\s+((an|a)\s+)?(?<suffix_num>half|quarter))",
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

        //TODO: change to Spanish the HalfRegex
        public static readonly Regex HalfRegex = new Regex(@"\b(?<half>half\s+(?<unit>year|month|week|day|hour))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishDurationExtractorConfiguration()
        {
            CardinalExtractor = new CardinalExtractor();
        }

        public IExtractor CardinalExtractor { get; }

        Regex IDurationExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex IDurationExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex IDurationExtractorConfiguration.AnUnitRegex => AnUnitRegex;

        Regex IDurationExtractorConfiguration.AllRegex => AllRegex;

        Regex IDurationExtractorConfiguration.HalfRegex => HalfRegex;

        Regex IDurationExtractorConfiguration.SuffixAndRegex => SuffixAndRegex;
    }
}
