using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class LengthExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public LengthExtractorConfiguration() : base(new CultureInfo(Culture.Spanish)) { }

        public LengthExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => LenghtSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_LENGTH;

        public static readonly ImmutableDictionary<string, string> LenghtSuffixList = new Dictionary<string, string>
        {
            {"Kilómetro", "km|kilometro|kilómetro|kilometros|kilómetros"},
            {"Hectómetro", "hm|hectometro|hectómetro|hectometros|hectómetros"},
            {"Decámetro", "decametro|decámetro|decametros|decámetros|dam"},
            {"Metro", "m|m.|metro|metros"},
            {"Decímetro", "dm|decimetro|decímetro|decimetros|decímetros"},
            {"Centímetro", "cm|centimetro|centímetro|centimetros|centimetros"},
            {"Milímetro", "mm|milimetro|milímetro|milimetros|milímetros"},
            {"Micrómetro", "µm|um|micrometro|micrómetro|micrometros|micrómetros|micrón|micrónes"},
            {"Nanómetro", "nm|nanometro|nanómetro|nanometros|nanómetros"},
            {"Picómetro", "pm|picometro|picómetro|picometros|picometros"},
            {"Milla", "mi|milla|millas"},
            {"Yarda", "yd|yarda|yardas"},
            {"Pulgada", "pulgada|pulgadas|\""},
            {"Pie", "pie|pies|ft"},
            {"Año luz", "año luz|años luz|al"},
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "mi",
            "área",
            "áreas"
        }.ToImmutableList();
    }
}
