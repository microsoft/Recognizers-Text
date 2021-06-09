using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Korean
{
    public class DimensionExtractorConfiguration : KoreanNumberWithUnitExtractorConfiguration
    {
        public DimensionExtractorConfiguration()
            : this(new CultureInfo(Culture.Korean))
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