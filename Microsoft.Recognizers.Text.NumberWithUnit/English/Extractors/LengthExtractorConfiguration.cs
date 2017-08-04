using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class LengthExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public LengthExtractorConfiguration() : base(new CultureInfo(Culture.English)) { }

        public LengthExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => LenghtSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_LENGTH;

        public static readonly ImmutableDictionary<string, string> LenghtSuffixList = new Dictionary<string, string>
        {
            {"Kilometer", "km|kilometer|kilometre|kilometers|kilometres|kilo meter|kilo meters|kilo metres|kilo metre"},
            {"Hectometer", "hm|hectometer|hectometre|hectometers|hectometres|hecto meter|hecto meters|hecto metres|hecto metre"},
            {"Decameter", "dam|decameter|decametre|decameters|decametres|deca meter|deca meters|deca metres|deca metre"},
            {"Meter", "m|meter|metre|meters|metres"},
            {"Decimeter", "dm|decimeter|decimeters|decimetre|decimetres|deci meter|deci meters|deci metres|deci metre"},
            {"Centimeter", "cm|centimeter|centimeters|centimetre|centimetres|centi meter|centi meters|centi metres|centi metre"},
            {"Millimeter", "mm|millimeter|millimeters|millimetre|millimetres|milli meter|milli meters|milli metres|milli metre"},
            {"Micrometer", "μm|micrometer|micrometre|micrometers|micrometres|micro meter|micro meters|micro metres|micro metre"},
            {"Nanometer", "nm|nanometer|nanometre|nanometers|nanometres|nano meter|nano meters|nano metres|nano metre"},
            {"Picometer", "pm|picometer|picometre|picometers|picometres|pico meter|pico meters|pico metres|pico metre"},
            {"Mile", "-mile|mile|miles"},
            {"Yard", "yard|yards"},
            {"Inch", "-inch|inch|inches|in|\""},
            {"Foot", "-foot|foot|feet|ft"},
            {"Light year", "light year|light-year|light years|light-years"},
            {"Pt", "pt|pts"},
            
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = new List<string>
        {
            "m",
            "yard",
            "yards",
            "pm",
            "pt",
            "pts",
        }.ToImmutableList();
    }
}
