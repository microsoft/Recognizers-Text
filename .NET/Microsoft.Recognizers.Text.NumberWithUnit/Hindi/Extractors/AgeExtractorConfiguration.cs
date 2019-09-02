using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Hindi;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class AgeExtractorConfiguration : HindiNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AgeSuffixList =
            NumbersWithUnitDefinitions.AgeSuffixList.ToImmutableDictionary();

        public AgeExtractorConfiguration()
               : this(new CultureInfo(Culture.Hindi))
        {
        }

        public AgeExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => AgeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AGE;
    }
}