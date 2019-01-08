using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class WeightExtractorConfiguration : GermanNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> WeightSuffixList = NumbersWithUnitDefinitions.WeightSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousWeightUnitList.ToImmutableList();

        public WeightExtractorConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public WeightExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => WeightSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_WEIGHT;
    }
}