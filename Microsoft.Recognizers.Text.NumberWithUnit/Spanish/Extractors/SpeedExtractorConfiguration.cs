using Microsoft.Recognizers.Text.Number.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Extractors
{
    public class SpeedExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public SpeedExtractorConfiguration() : base(new CultureInfo(Culture.Spanish)) { }

        public SpeedExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => SpeedSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_SPEED;

        public static readonly ImmutableDictionary<string, string> SpeedSuffixList = new Dictionary<string, string>
        {
            {"Metro por segundo", "metro/segundo|m/s|metro por segundo|metros por segundo|metros por segundos"},
            {"Kilómetro por hora", "km/h|kilómetro por hora|kilometro por hora|kilómetros por hora|kilometros por hora|kilómetro/hora|kilometro/hora|kilómetros/hora|kilometros/hora"},
            {"Kilómetro por minuto", "km/min|kilómetro por minuto|kilometro por minuto|kilómetros por minuto|kilometros por minuto|kilómetro/minuto|kilometro/minuto|kilómetros/minuto|kilometros/minuto"},
            {"Kilómetro por segundo", "km/seg|kilómetro por segundo|kilometro por segundo|kilómetros por segundo|kilometros por segundo|kilómetro/segundo|kilometro/segundo|kilómetros/segundo|kilometros/segundo"},
            {"Milla por hora", "mph|milla por hora|mi/h|milla/hora|millas/hora|millas por hora"},
            {"Nudo", "kt|nudo|nudos|kn"},
            {"Pie por segundo", "ft/s|pie/s|ft/seg|pie/seg|pie por segundo|pies por segundo"},
            {"Pie por minuto", "ft/min|pie/min|pie por minuto|pies por minuto"},
            {"Yarda por minuto", "yardas por minuto|yardas/minuto|yardas/min"},
            {"Yarda por segundo", "yardas por segundo|yardas/segundo|yardas/seg"},
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "nudo",
            "nudos"
        }.ToImmutableList();
    }
}
