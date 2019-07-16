using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class SpeedExtractorConfiguration : TurkishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> SpeedSuffixList = NumbersWithUnitDefinitions.SpeedSuffixList.ToImmutableSortedDictionary();

        public static readonly ImmutableSortedDictionary<string, string> SpeedPrefixList = NumbersWithUnitDefinitions.SpeedPrefixList.ToImmutableSortedDictionary();

        public SpeedExtractorConfiguration()
               : base(new CultureInfo(Culture.Turkish))
        {
        }

        public SpeedExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => SpeedSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => SpeedPrefixList;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_SPEED;
    }
}