using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Hindi;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class SpeedExtractorConfiguration : HindiNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> SpeedSuffixList =
            NumbersWithUnitDefinitions.SpeedSuffixList.ToImmutableDictionary();

        public SpeedExtractorConfiguration()
               : base(new CultureInfo(Culture.Hindi))
        {
        }

        public SpeedExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => SpeedSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_SPEED;
    }
}