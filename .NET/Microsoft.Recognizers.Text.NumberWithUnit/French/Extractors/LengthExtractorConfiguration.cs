using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class LengthExtractorConfiguration : FrenchNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> LengthSuffixList = NumbersWithUnitDefinitions.LengthSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousLengthUnitList.ToImmutableList();

        public LengthExtractorConfiguration()
            : base(new CultureInfo(Culture.French))
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