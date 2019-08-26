using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class AgeExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AgeSuffixList = NumbersWithUnitDefinitions.AgeSuffixList.ToImmutableDictionary();

        public static readonly ImmutableList<string> AmbiguousAgeUnitList = NumbersWithUnitDefinitions.AmbiguousAgeUnitList.ToImmutableList();

        public AgeExtractorConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public AgeExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => AgeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousAgeUnitList;

        public override string ExtractType => Constants.SYS_UNIT_AGE;
    }
}