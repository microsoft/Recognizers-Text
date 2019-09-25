using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class AgeExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AgeSuffixList = NumbersWithUnitDefinitions.AgeSuffixList.ToImmutableDictionary();

        public static readonly ImmutableList<string> AmbiguousAgeUnitList = NumbersWithUnitDefinitions.AmbiguousAgeUnitList.ToImmutableList();

        public AgeExtractorConfiguration()
               : this(new CultureInfo(Culture.Spanish))
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