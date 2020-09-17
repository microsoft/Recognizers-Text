using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Swedish
{
    public class LengthExtractorConfiguration : SwedishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> LengthSuffixList =
            NumbersWithUnitDefinitions.LengthSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues =
            NumbersWithUnitDefinitions.AmbiguousLengthUnitList.ToImmutableList();

        public LengthExtractorConfiguration()
            : base(new CultureInfo(Culture.Swedish))
        {
        }

        public LengthExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => LengthSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_LENGTH;
    }
}