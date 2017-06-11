using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class AreaExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public AreaExtractorConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public AreaExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => AreaSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_AREA;

        public static readonly ImmutableDictionary<string, string> AreaSuffixList = new Dictionary<string, string>
        {
            {"Kilómetro cuadrado", "kilómetro cuadrado|kilómetros cuadrados|km2|km^2|km²"},
            {"Hectómetro cuadrado", "hectómetro cuadrado|hectómetros cuadrados|hm2|hm^2|hm²|hectárea|hectáreas"},
            {"Decámetro cuadrado", "decámetro cuadrado|decámetros cuadrados|dam2|dam^2|dam²|área|áreas"},
            {"Metro cuadrado", "metro cuadrado|metros cuadrados|m2|m^2|m²"},
            {"Decímetro cuadrado", "decímetro cuadrado|decímetros cuadrados|dm2|dm^2|dm²"},
            {"Centímetro cuadrado", "centímetro cuadrado|centímetros cuadrados|cm2|cm^2|cm²"},
            {"Milímetro cuadrado", "milímetro cuadrado|milímetros cuadrados|mm2|mm^2|mm²"},
            {"Pulgada cuadrado", "pulgada cuadrada|pulgadas cuadradas"},
            {"Pie cuadrado", "pie cuadrado|pies cuadrados|pie2|pie^2|pie²|ft2|ft^2|ft²"},
            {"Yarda cuadrado", "yarda cuadrada|yardas cuadradas|yd2|yd^2|yd²"},
            {"Acre", "acre|acres"},
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "área",
            "áreas"
        }.ToImmutableList();
    }
}
