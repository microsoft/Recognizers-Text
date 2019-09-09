using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Hindi;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class AreaExtractorConfiguration : HindiNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AreaSuffixList =
            NumbersWithUnitDefinitions.AreaSuffixList.ToImmutableDictionary();

        public AreaExtractorConfiguration()
               : this(new CultureInfo(Culture.Hindi))
        {
        }

        public AreaExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => AreaSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AREA;
    }
}