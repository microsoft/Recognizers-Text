using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class AgeExtractorConfiguration : TurkishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> AgeSuffixList = NumbersWithUnitDefinitions.AgeSuffixList.ToImmutableSortedDictionary();

        public AgeExtractorConfiguration()
            : this(new CultureInfo(Culture.Turkish))
        {
        }

        public AgeExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => AgeSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AGE;
    }
}