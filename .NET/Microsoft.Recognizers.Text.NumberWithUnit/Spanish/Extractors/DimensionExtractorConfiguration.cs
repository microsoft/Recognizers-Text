using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class DimensionExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> DimensionSuffixList = NumbersWithUnitDefinitions.DimensionSuffixList.ToImmutableSortedDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousDimensionUnitList.ToImmutableList();

        public DimensionExtractorConfiguration()
               : base(new CultureInfo(Culture.Spanish))
        {
        }

        public DimensionExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => DimensionSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;
    }
}
