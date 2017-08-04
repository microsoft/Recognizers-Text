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
        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_WEIGHT;

        public static readonly ImmutableDictionary<string, string> WeightSuffixList = new Dictionary<string, string>
        {
            {"Kilogram", "kg|kilogram|kilograms|kilo|kilos"},
            {"Gram", "g|gram|grams"},
            {"Milligram", "mg|milligram|milligrams"},
            {"Barrel", "barrels|barrel"},
            {"Gallon", "-gallon|gallons|gallon"},
            {"Metric ton", "metric tons|metric ton"},
            {"Ton", "-ton|ton|tons|tonne|tonnes"},
            {"Pound", "pound|pounds|lb"},
            {"Ounce", "-ounce|ounce|oz|ounces"},
            {"Weight unit", "pennyweight|grain|british long ton|US short hundredweight|stone|dram"},
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = new List<string>
        {
            "g",
            "oz",
            "stone",
            "dram",
        }.ToImmutableList();
    }
}
