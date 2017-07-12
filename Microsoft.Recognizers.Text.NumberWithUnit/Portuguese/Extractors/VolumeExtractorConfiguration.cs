using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class VolumeExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public VolumeExtractorConfiguration() : this(new CultureInfo(Culture.Portuguese)) { }

        public VolumeExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => VolumeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_VOLUME;

        public static readonly ImmutableDictionary<string, string> VolumeSuffixList = new Dictionary<string, string>
        {
            {"Quilômetro cúbico", "quilômetro cúbico|quilómetro cúbico|quilometro cubico|quilômetros cúbicos|quilómetros cúbicos|quilometros cubicos|km3|km^3|km³"},
            {"Hectômetro cúbico", "hectômetro cúbico|hectómetro cúbico|hectometro cubico|hectômetros cúbicos|hectómetros cúbicos|hectometros cubicos|hm3|hm^3|hm³"},
            {"Decâmetro cúbico", "decâmetro cúbico|decámetro cúbico|decametro cubico|decâmetros cúbicos|decámetros cúbicos|decametros cubicosdam3|dam^3|dam³"},
            {"Metro cúbico", "metro cúbico|metro cubico|metros cúbicos|metros cubicos|m3|m^3|m³"},
            {"Decímetro cúbico", "decímetro cúbico|decimetro cubico|decímetros cúbicos|decimetros cubicos|dm3|dm^3|dm³"},
            {"Centímetro cúbico", "centímetro cúbico|centimetro cubico|centímetros cúbicos|centrimetros cubicos|cc|cm3|cm^3|cm³"},
            {"Milímetro cúbico", "milímetro cúbico|milimetro cubico|milímetros cúbicos|milimetros cubicos|mm3|mm^3|mm³"},
            {"Polegada cúbica", "polegada cúbica|polegada cubica|polegadas cúbicas|polegadas cubicas"},
            {"Pé cúbico", "pé cúbico|pe cubico|pés cúbicos|pes cubicos|pé3|pe3|pé^3|pe^3|pé³|pe³|ft3|ft^3|ft³"},
            {"Jarda cúbica", "jarda cúbica|jarda cubica|jardas cúbicas|jardas cubicas|yd3|yd^3|yd³"},
            {"Hectolitro", "hectolitro|hectolitros|hl"},
            {"Litro", "litro|litros|lts|l"},
            {"Mililitro", "mililitro|mililitros|ml"},
            {"Galão", "galão|galões|galao|galoes"},
            {"Pint", "pinta|pintas|pinto|pintos|quartilho|quartilhos|pint|pints"},
            {"Barril", "barril|barris|bbl"},
            {"Onça líquida", "onça líquida|onca liquida|onças líquidas|oncas liquidas"},
        }.ToImmutableDictionary();
    }
}
