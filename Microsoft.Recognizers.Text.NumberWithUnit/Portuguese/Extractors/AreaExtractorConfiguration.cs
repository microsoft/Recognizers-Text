using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class AreaExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public AreaExtractorConfiguration() : this(new CultureInfo(Culture.Portuguese)) { }

        public AreaExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => AreaSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_AREA;

        public static readonly ImmutableDictionary<string, string> AreaSuffixList = new Dictionary<string, string>
        {
            {"Quilômetro quadrado", "quilômetro quadrado|quilómetro quadrado|quilometro quadrado|quilômetros quadrados|quilómetros quadrados|quilomeros quadrados|km2|km^2|km²"},
            {"Hectare", "hectômetro quadrado|hectómetro quadrado|hectômetros quadrados|hectómetros cuadrados|hm2|hm^2|hm²|hectare|hectares"},
            {"Decâmetro quadrado", "decâmetro quadrado|decametro quadrado|decâmetros quadrados|decametro quadrado|dam2|dam^2|dam²|area|ares"},
            {"Metro quadrado", "metro quadrado|metros quadrados|m2|m^2|m²"},
            {"Decímetro quadrado", "decímetro quadrado|decimentro quadrado|decímetros quadrados|decimentros quadrados|dm2|dm^2|dm²"},
            {"Centímetro quadrado", "centímetro quadrado|centimetro quadrado|centímetros quadrados|centrimetros quadrados|cm2|cm^2|cm²"},
            {"Milímetro quadrado", "milímetro quadrado|milimetro quadrado|milímetros quadrados|militmetros quadrados|mm2|mm^2|mm²"},
            {"Polegada quadrada", "polegada quadrada|polegadas quadradas|in2|in^2|in²"},
            {"Pé quadrado", "pé quadrado|pe quadrado|pés quadrados|pes quadrados|pé2|pé^2|pé²|sqft|sq ft|ft2|ft^2|ft²"},
            {"Jarda quadrada", "jarda quadrada|jardas quadradas|yd2|yd^2|yd²"},
            {"Milha quadrada", "milha quadrada|milhas quadradas|mi2|mi^2|mi²"},
            {"Acre", "acre|acres"},
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = new List<string>{ }.ToImmutableList();
    }
}
