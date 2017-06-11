using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class VolumeExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public VolumeExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public VolumeExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => VolumeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_VOLUME;

        public static readonly ImmutableDictionary<string, string> VolumeSuffixList = new Dictionary<string, string>
        {
            {"m3", "m3|cubic meter|cubic meters|cubic metre|cubic|metres"},
            {"l", "l|litre|liter|liters|litres"},
            {"ml", "ml|mls|millilitre|milliliter|millilitres|milliliters"},
            {
                "other",
                "cubic centimeter|cubic centimetre|cubic meter|cubic metre|fl oz|fluid ounce|ounce|oz|cup|hecotoliter|hecotolitre|dekaliter|dekalitre|deciliter|decilitre|cubic yard|cubic milliliter|cubic millilitre|cubic inch|cubic foot|cubic mile|teaspoon|tablespoon|fluid ounce|fluid dram|gill|pint|quart|gallon|minim|barrel|cord|peck|bushel|hogshead"
            }
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "l",
            "oz",
            "cup",
            "peck",
            "cord",
            "gill"
        }.ToImmutableList();
    }
}
