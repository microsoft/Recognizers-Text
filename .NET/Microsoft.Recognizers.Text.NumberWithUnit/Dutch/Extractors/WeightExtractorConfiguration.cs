using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class WeightExtractorConfiguration : DutchNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> WeightSuffixList =
            NumbersWithUnitDefinitions.WeightSuffixList.ToImmutableSortedDictionary();

        private static readonly ImmutableList<string> AmbiguousValues =
            NumbersWithUnitDefinitions.AmbiguousWeightUnitList.ToImmutableList();

        public WeightExtractorConfiguration()
            : this(new CultureInfo(Culture.Dutch))
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