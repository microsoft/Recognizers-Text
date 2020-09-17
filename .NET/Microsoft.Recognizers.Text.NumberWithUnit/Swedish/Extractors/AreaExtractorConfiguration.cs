using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Swedish
{
    public class AreaExtractorConfiguration : SwedishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AreaSuffixList = NumbersWithUnitDefinitions.AreaSuffixList.ToImmutableDictionary();

        public AreaExtractorConfiguration()
            : this(new CultureInfo(Culture.Swedish))
        {
        }

        public AreaExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => AreaSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AREA;
    }
}