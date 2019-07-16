using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class SpeedExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> SpeedSuffixList =
            NumbersWithUnitDefinitions.SpeedSuffixList.ToImmutableSortedDictionary();

        public SpeedExtractorConfiguration()
               : base(new CultureInfo(Culture.English))
        {
        }

        public SpeedExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => SpeedSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_SPEED;
    }
}