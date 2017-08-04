using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class AreaExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public AreaExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public AreaExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => AreaSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AREA;

        public static readonly ImmutableDictionary<string, string> AreaSuffixList = new Dictionary<string, string>
        {
            {"Square kilometer", "sq km|sq kilometer|sq kilometre|sq kilometers|sq kilometres|square kilometer|square kilometre|square kilometers|square kilometres|km2|km^2|km²"},
            {"Square hectometer", "sq hm|sq hectometer|sq hectometre|sq hectometers|sq hectometres|square hectometer|square hectometre|square hectometers|square hectometres|hm2|hm^2|hm²|hectare|hectares"},
            {"Square decameter", "sq dam|sq decameter|sq decametre|sq decameters|sq decametres|square decameter|square decametre|square decameters|square decametres|sq dekameter|sq dekametre|sq dekameters|sq dekametres|square dekameter|square dekametre|square dekameters|square dekametres|dam2|dam^2|dam²"},
            {"Square meter", "sq m|sq meter|sq metre|sq meters|sq metres|sq metre|square meter|square meters|square metre|square metres|m2|m^2|m²"},
            {"Square decimeter", "sq dm|sq decimeter|sq decimetre|sq decimeters|sq decimetres|square decimeter|square decimetre|square decimeters|square decimetres|dm2|dm^2|dm²"},
            {"Square centimeter", "sq cm|sq centimeter|sq centimetre|sq centimeters|sq centimetres|square centimeter|square centimetre|square centimeters|square centimetres|cm2|cm^2|cm²"},
            {"Square millimeter", "sq mm|sq millimeter|sq millimetre|sq millimeters|sq millimetres|square millimeter|square millimetre|square millimeters|square millimetres|mm2|mm^2|mm²"},
            {"Square inch", "sq in|sq inch|square inch|square inches|in2|in^2|in²"},
            {"Square foot", "sqft|sq ft|sq foot|sq feet|square foot|square feet|feet2|feet^2|feet²|ft2|ft^2|ft²"},
            {"Square mile", "sq mi|sq mile|sqmiles|square mile|square miles|mi2|mi^2|mi²"},
            {"Square yard", "sq yd|sq yard|sq yards|square yard|square yards|yd2|yd^2|yd²"},
            {"Acre", "-acre|acre|acres"},
        }.ToImmutableDictionary();

    }
}
