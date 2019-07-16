using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class AreaExtractorConfiguration : DutchNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> AreaSuffixList = NumbersWithUnitDefinitions.AreaSuffixList.ToImmutableSortedDictionary();

        public AreaExtractorConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public AreaExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => AreaSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AREA;
    }
}