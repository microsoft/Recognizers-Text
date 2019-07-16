using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class WeightExtractorConfiguration : TurkishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> WeightSuffixList = NumbersWithUnitDefinitions.WeightSuffixList.ToImmutableSortedDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousWeightUnitList.ToImmutableList();

        public WeightExtractorConfiguration()
               : this(new CultureInfo(Culture.Turkish))
        {
        }

        public WeightExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => WeightSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_WEIGHT;
    }
}