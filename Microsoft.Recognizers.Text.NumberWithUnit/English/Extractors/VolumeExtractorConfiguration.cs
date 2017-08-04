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

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_VOLUME;

        public static readonly ImmutableDictionary<string, string> VolumeSuffixList = new Dictionary<string, string>
        {
            {"Cubic meter", "m3|cubic meter|cubic meters|cubic metre|cubic metres"},
            {"Cubic centimeter", "cubic centimeter|cubic centimetre|cubic centimeters|cubic centimetres"},
            {"Cubic millimiter", "cubic millimiter|cubic millimitre|cubic millimiters|cubic millimitres"},
            {"Hectoliter", "hectoliter|hectolitre|hectoliters|hectolitres"},
            {"Decaliter", "decaliter|decalitre|dekaliter|dekalitre|decaliters|decalitres|dekaliters|dekalitres"},
            {"Liter", "l|litre|liter|liters|litres"},
            {"Deciliter", "dl|deciliter|decilitre|deciliters|decilitres"},
            {"Centiliter", "cl|centiliter|centilitre|centiliters|centilitres"},
            {"Milliliter", "ml|mls|millilitre|milliliter|millilitres|milliliters"},
            {"Cubic yard", "cubic yard|cubic yards"},
            {"Cubic inch", "cubic inch|cubic inches"},
            {"Cubic foot", "cubic foot|cubic feet"},
            {"Cubic mile", "cubic mile|cubic miles"},
            {"Fluid ounce", "fl oz|fluid ounce|fluid ounces"},
            {"Teaspoon", "teaspoon|teaspoons"},
            {"Tablespoon", "tablespoon|tablespoons"},
            {"Pint", "pint|pints" },
            {"Volume unit", "fluid dram|gill|quart|minim|barrel|cord|peck|bushel|hogshead" }
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = new List<string>
        {
            "l",
            "ounce",
            "oz",
            "cup",
            "peck",
            "cord",
            "gill"
        }.ToImmutableList();
    }
}
