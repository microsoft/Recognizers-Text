using Microsoft.Recognizers.Text.Number.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors
{
    public class LengthExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public LengthExtractorConfiguration() : base(new CultureInfo(Culture.English)) { }

        public LengthExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => LenghtSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_LENGTH;

        public static readonly ImmutableDictionary<string, string> LenghtSuffixList = new Dictionary<string, string>
        {
            {"m", "m|meter|metre|meters|metres"},
            {"km", "km|kilometer|kilometer|kilometers|kilometres"},
            {"dm", "dm|decimeter|decimeters|decimetre|decimetres"},
            {"cm", "cm|cntimeter|cntimeters|cntimetre|cntimetres"},
            {"mm", "mm|micrometer|micrometre|micrometers|micrometres"},
            {"mile", "mile|miles"},
            {"yard", "yard|yards"},
            {"inch", "inch|inches|in|\""},
            {"foot", "foot|feet|ft"},
            {"light year", "light year|light-year|light years|light-years"},
            {"pt", "pt|pts"},
            {"pm", "pm|picometer|picometre"},
            {"nm", "nm|nanometer|nanometre"}
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
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
