using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Resources.English;

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

        public static readonly ImmutableDictionary<string, string> WeightSuffixList = NumericWithUnit.WeightSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumericWithUnit.AmbiguousWeightUnitList.ToImmutableList();
    }
}