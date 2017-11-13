using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class LengthExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public LengthExtractorConfiguration() : base(new CultureInfo(Culture.Portuguese)) { }

        public LengthExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => LenghtSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_LENGTH;

        public static readonly ImmutableDictionary<string, string> LenghtSuffixList = NumbersWithUnitDefinitions.LenghtSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousLengthUnitList.ToImmutableList();

    }
}
