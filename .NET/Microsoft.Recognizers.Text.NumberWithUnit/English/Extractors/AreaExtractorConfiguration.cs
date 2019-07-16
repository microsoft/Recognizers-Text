using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class AreaExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> AreaSuffixList =
            NumbersWithUnitDefinitions.AreaSuffixList.ToImmutableSortedDictionary();

        public AreaExtractorConfiguration()
               : this(new CultureInfo(Culture.English))
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