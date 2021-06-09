using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class AgeExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AgeSuffixList =
            NumbersWithUnitDefinitions.AgeSuffixList.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> AgePrefixList =
            NumbersWithUnitDefinitions.AgePrefixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousUnits =
            NumbersWithUnitDefinitions.AmbiguousAgeUnitList.ToImmutableList();

        public AgeExtractorConfiguration()
               : this(new CultureInfo(Culture.English))
        {
        }

        public AgeExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => AgeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => AgePrefixList;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousUnits;

        public override string ExtractType => Constants.SYS_UNIT_AGE;
    }
}