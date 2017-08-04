using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class DimensionExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public DimensionExtractorConfiguration() : base(new CultureInfo(Culture.English)) { }

        public DimensionExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => DimensionSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;

        public static readonly ImmutableDictionary<string, string> DimensionSuffixList = new Dictionary<string, string>
        {
            // Information
            {"Bit", "-bit|bit|bits"},
            {"Kilobit", "kilobit|kilobits|kb|kbit"},
            {"Megabit", "megabit|megabits|Mb|Mbit"},
            {"Gigabit", "gigabit|gigabits|Gb|Gbit"},
            {"Terabit", "terabit|terabits|Tb|Tbit"},
            {"Petabit", "petabit|petabits|Pb|Pbit"},
            {"Byte", "-byte|byte|bytes"},
            {"Kilobyte", "-kilobyte|-kilobytes|kilobyte|kB|kilobytes|kilo byte|kilo bytes|kByte"},
            {"Megabyte", "-megabyte|-megabytes|megabyte|mB|megabytes|mega byte|mega bytes|MByte"},
            {"Gigabyte", "-gigabyte|-gigabytes|gigabyte|gB|gigabytes|giga byte|giga bytes|GByte"},
            {"Terabyte", "-terabyte|-terabytes|terabyte|tB|terabytes|tera byte|tera bytes|TByte"},
            {"Petabyte", "-petabyte|-petabytes|petabyte|pB|petabytes|peta byte|peta bytes|PByte"},
        }
        .Concat(AreaExtractorConfiguration.AreaSuffixList)
        .Concat(LengthExtractorConfiguration.LenghtSuffixList)
        .Concat(SpeedExtractorConfiguration.SpeedSuffixList)
        .Concat(VolumeExtractorConfiguration.VolumeSuffixList)
        .Concat(WeightExtractorConfiguration.WeightSuffixList)
        .ToImmutableDictionary(x => x.Key, x => x.Value);

        private static readonly ImmutableList<string> AmbiguousValues = new List<string>
        {
            "barrel",
            "barrels",
            "grain",
            "pound",
            "stone",
            "yards",
            "yard",
            "cord",
            "dram",
            "feet",
            "foot",
            "gill",
            "knot",
            "peck",
            "cup",
            "fps",
            "pts",
            "in",
            "\""
        }.ToImmutableList();
    }
}
