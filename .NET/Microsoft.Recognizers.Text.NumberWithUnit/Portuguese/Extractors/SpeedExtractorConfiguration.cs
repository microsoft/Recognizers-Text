using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class SpeedExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> SpeedSuffixList = NumbersWithUnitDefinitions.SpeedSuffixList.ToImmutableSortedDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousSpeedUnitList.ToImmutableList();

        public SpeedExtractorConfiguration()
               : base(new CultureInfo(Culture.Portuguese))
        {
        }

        public SpeedExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => SpeedSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_SPEED;
    }
}
