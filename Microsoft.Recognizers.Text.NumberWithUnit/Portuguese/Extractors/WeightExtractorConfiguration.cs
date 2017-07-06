using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class WeightExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public WeightExtractorConfiguration() : this(new CultureInfo(Culture.Portuguese)) { }

        public WeightExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => WeightSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_WEIGHT;

        public static readonly ImmutableDictionary<string, string> WeightSuffixList = new Dictionary<string, string>
        {
            {"Tonelada métrica", "tonelada métrica|tonelada metrica|toneladas métricas|toneladas metricas"},
            {"Tonelada", "ton|tonelada|toneladas"},
            {"Quilograma", "kg|quilograma|quilogramas|quilo|quilos|kilo|kilos"},
            {"Hectograma", "hg|hectograma|hectogramas"},
            {"Decagrama", "dag|decagrama|decagramas"},
            {"Grama", "g|gr|grama|gramas"},
            {"Decigrama", "dg|decigrama|decigramas"},
            {"Centigrama", "cg|centigrama|centigramas"},
            {"Miligrama", "mg|miligrama|miligramas"},
            {"Micrograma", "µg|ug|micrograma|microgramas"},
            {"Nanograma", "ng|nanograma|nanogramas"},
            {"Picograma", "pg|picograma|picogramas"},
            {"Libra", "lb|libra|libras"},
            {"Onça", "oz|onça|onca|onças|oncas"},
            {"Grão", "grão|grao|grãos|graos|gr"},
            {"Quilate", "ct|kt|quilate|quilates"},
        }.ToImmutableDictionary();
    }
}
