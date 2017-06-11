using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class WeightExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public WeightExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public WeightExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => WeightSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;
        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_WEIGHT;

        public static readonly ImmutableDictionary<string, string> WeightSuffixList = new Dictionary<string, string>
        {
            {"kg", "kg|kilogram|kilograms|kilo|kilos"},
            {"mg", "mg|milligram|milligrams"},
            {"g", "g|gram|grams"},
            {"ton", "ton|tonne|tonnes"},
            {"pound", "pound|pounds|lb"},
            {"ounce", "ounce|oz|ounces"},
            {"other", "pennyweight|grain|british long ton|US short hundredweight|stone|dram"}
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "g",
            "oz",
            "stone",
            "dram",
        }.ToImmutableList();
    }
}
