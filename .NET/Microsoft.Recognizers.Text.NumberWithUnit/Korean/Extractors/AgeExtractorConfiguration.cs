using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Korean
{
    public class AgeExtractorConfiguration : KoreanNumberWithUnitExtractorConfiguration
    {
        public AgeExtractorConfiguration()
            : this(new CultureInfo(Culture.Korean))
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
