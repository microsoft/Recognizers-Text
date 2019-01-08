using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class AgeExtractorConfiguration : FrenchNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AgeSuffixList = NumbersWithUnitDefinitions.AgeSuffixList.ToImmutableDictionary();

        public AgeExtractorConfiguration()
            : this(new CultureInfo(Culture.French))
        {
        }

        public AgeExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => AgeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AGE;
    }
}