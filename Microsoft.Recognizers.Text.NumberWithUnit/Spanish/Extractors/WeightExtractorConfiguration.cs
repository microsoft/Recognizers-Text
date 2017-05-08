using Microsoft.Recognizers.Text.Number.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Extractors
{
    public class WeightExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public WeightExtractorConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public WeightExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => WeightSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_WEIGHT;

        public static readonly ImmutableDictionary<string, string> WeightSuffixList = new Dictionary<string, string>
        {
            {"Tonelada métrica", "tonelada métrica|toneladas métricas"},
            {"Tonelada", "ton|tonelada|toneladas"},
            {"Kilogramo", "kg|kilogramo|kilogramos"},
            {"Hectogramo", "hg|hectogramo|hectogramos"},
            {"Decagramo", "dag|decagramo|decagramos"},
            {"Gramo", "g|gr|gramo|gramos"},
            {"Decigramo", "dg|decigramo|decigramos"},
            {"Centigramo", "cg|centigramo|centigramos"},
            {"Miligramo", "mg|miligramo|miligramos"},
            {"Microgramo", "µg|ug|microgramo|microgramos"},
            {"Nanogramo", "ng|nanogramo|nanogramos"},
            {"Picogramo", "pg|picogramo|picogramos"},
            {"Libra", "lb|libra|libras"},
            {"Onza", "oz|onza|onzas"},
            {"Grano", "grano|granos"},
            {"Quilate", "ct|kt|quilate|quilates"},
        }.ToImmutableDictionary();
    }
}
