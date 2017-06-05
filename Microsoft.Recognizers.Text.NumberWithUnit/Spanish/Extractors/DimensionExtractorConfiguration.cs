using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class DimensionExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public DimensionExtractorConfiguration() : base(new CultureInfo(Culture.Spanish)) { }

        public DimensionExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => DimensionSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;

        public static readonly ImmutableDictionary<string, string> DimensionSuffixList = new Dictionary<string, string>
        {
            // Length
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
            // Speed
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
            // Area
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
            //Volume
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
            // Weight
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
            // Information
            {"bit", "bit|bits"},
            {"kilobit", "kilobit|kilobits|kb|kbit"},
            {"megabit", "megabit|megabits|Mb|Mbit"},
            {"gigabit", "gigabit|gigabits|Gb|Gbit"},
            {"terabit", "terabit|terabits|Tb|Tbit"},
            {"petabit", "petabit|petabits|Pb|Pbit"},
            {"kibibit", "kibibit|kibibits|kib|kibit"},
            {"mebibit", "mebibit|mebibits|Mib|Mibit"},
            {"gibibit", "gibibit|gibibits|Gib|Gibit"},
            {"tebibit", "tebibit|tebibits|Tib|Tibit"},
            {"pebibit", "pebibit|pebibits|Pib|Pibit"},
            {"byte", "byte|bytes"},
            {"kilobyte", "kilobyte|kilobytes|kB|kByte"},
            {"megabyte", "megabyte|megabytes|MB|MByte"},
            {"gigabyte", "gigabyte|gigabytes|GB|GByte"},
            {"terabyte", "terabyte|terabytes|TB|TByte"},
            {"petabyte", "petabyte|petabytes|PB|PByte"},
            {"kibibyte", "kibibyte|kibibytes|kiB|kiByte"},
            {"mebibyte", "mebibyte|mebibytes|MiB|MiByte"},
            {"gibibyte", "gibibyte|gibibytes|GiB|GiByte"},
            {"tebibyte", "tebibyte|tebibytes|TiB|TiByte"},
            {"pebibyte", "pebibyte|pebibytes|PiB|PiByte"},
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "al",
            "mi",
            "área",
            "áreas",
            "pie",
            "pies"
        }.ToImmutableList();
    }
}
