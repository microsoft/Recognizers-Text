using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class VolumeExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public VolumeExtractorConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public VolumeExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => VolumeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_VOLUME;

        public static readonly ImmutableDictionary<string, string> VolumeSuffixList = new Dictionary<string, string>
        {
            {"Kilómetro cúbico", "kilómetro cúbico|kilómetros cúbico|km3|km^3|km³"},
            {"Hectómetro cúbico", "hectómetro cúbico|hectómetros cúbico|hm3|hm^3|hm³"},
            {"Decámetro cúbico", "decámetro cúbico|decámetros cúbico|dam3|dam^3|dam³"},
            {"Metro cúbico", "metro cúbico|metros cúbico|m3|m^3|m³"},
            {"Decímetro cúbico", "decímetro cúbico|decímetros cúbico|dm3|dm^3|dm³"},
            {"Centímetro cúbico", "centímetro cúbico|centímetros cúbico|cc|cm3|cm^3|cm³"},
            {"Milímetro cúbico", "milímetro cúbico|milímetros cúbico|mm3|mm^3|mm³"},
            {"Pulgada cúbica", "pulgada cúbics|pulgadas cúbicas"},
            {"Pie cúbico", "pie cúbico|pies cúbicos|pie3|pie^3|pie³|ft3|ft^3|ft³"},
            {"Yarda cúbica", "yarda cúbica|yardas cúbicas|yd3|yd^3|yd³"},
            {"Hectolitro", "hectolitro|hectolitros|hl"},
            {"Litro", "litro|litros|lts|l"},
            {"Mililitro", "mililitro|mililitros|ml"},
            {"Galón", "galón|galones"},
            {"Pinta", "pinta|pintas"},
            {"Barril", "barril|barriles"},
            {"Onza líquida", "onza líquida|onzas líquidas"},
        }.ToImmutableDictionary();
    }
}
