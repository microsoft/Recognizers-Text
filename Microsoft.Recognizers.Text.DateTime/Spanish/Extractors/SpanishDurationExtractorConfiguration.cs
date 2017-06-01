using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Extractors;
using Microsoft.Recognizers.Text.Number.Spanish.Extractors;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Extractors
{
    public class SpanishDurationExtractorConfiguration : IDurationExtractorConfiguration
    {
        public static readonly Regex UnitRegex =
            new Regex(
                @"(?<unit>años|año|meses|mes|semanas|semana|d[ií]as|d[ií]a|horas|hora|h|hr|hrs|hs|minutos|minuto|mins|min|segundos|segundo|segs|seg)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex($@"^\s*{UnitRegex}\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit =
            new Regex($@"\b(?<num>\d+(\,\d*)?){UnitRegex}\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AnUnitRegex = new Regex($@"\b(un(a)?)\s+{UnitRegex}\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AllRegex = new Regex(@"\b(?<all>tod[oa]?\s+(el|la)\s+(?<unit>año|mes|semana|d[ií]a))\b",
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
    }
}
