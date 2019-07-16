using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class AreaExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> AreaSuffixList = NumbersWithUnitDefinitions.AreaSuffixList.ToImmutableSortedDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AreaAmbiguousValues.ToImmutableList();

        public AreaExtractorConfiguration()
               : this(new CultureInfo(Culture.Spanish))
        {
        }

        public AreaExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => AreaSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_AREA;
    }
}
