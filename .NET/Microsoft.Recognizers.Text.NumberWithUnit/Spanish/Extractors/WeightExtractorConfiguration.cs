using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class WeightExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> WeightSuffixList = NumbersWithUnitDefinitions.WeightSuffixList.ToImmutableSortedDictionary();

        public WeightExtractorConfiguration()
               : this(new CultureInfo(Culture.Spanish))
        {
        }

        public WeightExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => WeightSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_WEIGHT;
    }
}
