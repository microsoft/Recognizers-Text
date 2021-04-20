using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class WeightExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> WeightSuffixList =
            NumbersWithUnitDefinitions.WeightSuffixList.ToImmutableDictionary();

        public static readonly ImmutableList<string> AmbiguousUnits =
            NumbersWithUnitDefinitions.AmbiguousWeightUnitList.ToImmutableList();

        public WeightExtractorConfiguration()
               : this(new CultureInfo(Culture.English))
        {
        }

        public WeightExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => WeightSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousUnits;

        public override string ExtractType => Constants.SYS_UNIT_WEIGHT;
    }
}