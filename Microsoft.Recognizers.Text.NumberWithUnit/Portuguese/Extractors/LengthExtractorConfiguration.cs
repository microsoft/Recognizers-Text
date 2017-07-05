using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class LengthExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public LengthExtractorConfiguration() : base(new CultureInfo(Culture.Portuguese)) { }

        public LengthExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => LenghtSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_LENGTH;

        public static readonly ImmutableDictionary<string, string> LenghtSuffixList = new Dictionary<string, string>
        {
            {"Quilômetro", "km|quilometro|quilômetro|quilómetro|quilometros|quilômetros|quilómetros"},
            {"Hectômetro", "hm|hectometro|hectômetro|hectómetro|hectometros|hectômetros|hectómetros"},
            {"Decâmetro", "decametro|decâmetro|decámetro|decametros|decâmetro|decámetros|dam"},
            {"Metro", "m|m.|metro|metros"},
            {"Decímetro", "dm|decimetro|decímetro|decimetros|decímetros"},
            {"Centímetro", "cm|centimetro|centímetro|centimetros|centimetros"},
            {"Milímetro", "mm|milimetro|milímetro|milimetros|milímetros"},
            {"Micrômetro", "µm|um|micrometro|micrômetro|micrómetro|micrometros|micrômetros|micrómetros|micron|mícron|microns|mícrons|micra"},
            {"Nanômetro", "nm|nanometro|nanômetro|nanómetro|nanometros|nanômetros|nanómetros|milimicron|milimícron|milimicrons|milimícrons"},
            {"Picômetro", "pm|picometro|picômetro|picómetro|picometros|picômetros|picómetros"},
            {"Milha", "mi|milha|milhas"},
            {"Jarda", "yd|jarda|jardas"},
            {"Polegada", "polegada|polegadas|\""},
            {"Pé", "pé|pe|pés|pes|ft"},
            {"Ano luz", "ano luz|anos luz|al"},
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = new List<string>
        {
            "mi", "milha", "milhas"
        }.ToImmutableList();

    }
}
