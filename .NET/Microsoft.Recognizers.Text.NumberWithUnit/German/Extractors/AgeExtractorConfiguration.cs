using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class AgeExtractorConfiguration : GermanNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AgeSuffixList = NumbersWithUnitDefinitions.AgeSuffixList.ToImmutableDictionary();

        public static readonly ImmutableList<string> AmbiguousAgeUnitList = NumbersWithUnitDefinitions.AmbiguousAgeUnitList.ToImmutableList();

        public AgeExtractorConfiguration()
            : this(new CultureInfo(Culture.German))
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