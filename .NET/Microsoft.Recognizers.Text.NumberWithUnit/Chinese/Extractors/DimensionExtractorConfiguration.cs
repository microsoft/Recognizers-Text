using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class DimensionExtractorConfiguration : ChineseNumberWithUnitExtractorConfiguration
    {
        public DimensionExtractorConfiguration()
            : this(new CultureInfo(Culture.Chinese))
        {
        }

        public DimensionExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => NumbersWithUnitDefinitions.DimensionSuffixList.ToImmutableDictionary();

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => NumbersWithUnitDefinitions.DimensionAmbiguousValues.ToImmutableList();

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;
    }
}