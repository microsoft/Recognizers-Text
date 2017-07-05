using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class SpeedExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public SpeedExtractorConfiguration() : base(new CultureInfo(Culture.Portuguese)) { }

        public SpeedExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => SpeedSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_SPEED;

        public static readonly ImmutableDictionary<string, string> SpeedSuffixList = new Dictionary<string, string>
        {
            {"Metro por segundo", "metro/segundo|m/s|metro por segundo|metros por segundo|metros por segundos"},
            {"Quilômetro por hora", "km/h|quilômetro por hora|quilómetro por hora|quilometro por hora|quilômetros por hora|quilómetros por hora|quilometros por hora|quilômetro/hora|quilómetro/hora|quilometro/hora|quilômetros/hora|quilómetros/hora|quilometros/hora"},
            {"Quilômetro por minuto", "km/min|quilômetro por minuto|quilómetro por minuto|quilometro por minuto|quilômetros por minuto|quilómetros por minuto|quilometros por minuto|quilômetro/minuto|quilómetro/minuto|quilometro/minuto|quilômetros/minuto|quilómetros/minuto|quilometros/minuto"},
            {"Quilômetro por segundo", "km/seg|quilômetro por segundo|quilómetro por segundo|quilometro por segundo|quilômetros por segundo|quilómetros por segundo|quilometros por segundo|quilômetro/segundo|quilómetro/segundo|quilometro/segundo|quilômetros/segundo|quilómetros/segundo|quilometros/segundo"},
            {"Milha por hora", "mph|milha por hora|mi/h|milha/hora|milhas/hora|milhas por hora"},
            {"Nó", "kt|nó|nós|kn"},
            {"Pé por segundo", "ft/s|pé/s|pe/s|ft/seg|pé/seg|pe/seg|pé por segundo|pe por segundo|pés por segundo|pes por segundo"},
            {"Pé por minuto", "ft/min|pé/mind|pe/min|pé por minuto|pe por minuto|pés por minuto|pes por minuto"},
            {"Jarda por minuto", "jardas por minuto|jardas/minuto|jardas/min"},
            {"Jarda por segundo", "jardas por segundo|jardas/segundo|jardas/seg"},
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = new List<string>
        {
            "nó", "no",
            "nós", "nos"
        }.ToImmutableList();
    }
}
