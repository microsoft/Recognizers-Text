using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class AgeExtractorConfiguration : ChineseNumberWithUnitExtractorConfiguration
    {
        public AgeExtractorConfiguration()
            : this(new CultureInfo(Culture.Chinese))
        {
        }

        public AgeExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => NumbersWithUnitDefinitions.AgeSuffixList.ToImmutableDictionary();

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => NumbersWithUnitDefinitions.AgeAmbiguousValues.ToImmutableList();

        public override string ExtractType => Constants.SYS_UNIT_AGE;
    }
}
