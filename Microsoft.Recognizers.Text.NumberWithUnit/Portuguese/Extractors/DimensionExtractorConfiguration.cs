using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class DimensionExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public DimensionExtractorConfiguration() : base(new CultureInfo(Culture.Portuguese)) { }

        public DimensionExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => DimensionSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;

        public static readonly ImmutableDictionary<string, string> DimensionSuffixList = new Dictionary<string, string>
        {
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
        }
        .Concat(AreaExtractorConfiguration.AreaSuffixList)
        .Concat(LengthExtractorConfiguration.LenghtSuffixList)
        .Concat(SpeedExtractorConfiguration.SpeedSuffixList)
        .Concat(TemperatureExtractorConfiguration.TemperatureSuffixList)
        .Concat(VolumeExtractorConfiguration.VolumeSuffixList)
        .Concat(WeightExtractorConfiguration.WeightSuffixList)
        .ToImmutableDictionary(x => x.Key, x => x.Value);

        private static readonly ImmutableList<string> AmbiguousValues = new List<string>
        {
            "ton",
            "tonelada",
            "área",
            "area",
            "áreas",
            "areas",
            "milha",
            "milhas"
        }.ToImmutableList();
    }
}
