using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Swedish
{
    public class DimensionExtractorConfiguration : SwedishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> DimensionSuffixList =
            NumbersWithUnitDefinitions.InformationSuffixList
            .Concat(AreaExtractorConfiguration.AreaSuffixList)
            .Concat(LengthExtractorConfiguration.LengthSuffixList)
            .Concat(SpeedExtractorConfiguration.SpeedSuffixList)
            .Concat(VolumeExtractorConfiguration.VolumeSuffixList)
            .Concat(WeightExtractorConfiguration.WeightSuffixList)
            .ToImmutableDictionary(x => x.Key, x => x.Value);

        private static readonly ImmutableList<string> AmbiguousValues =
            NumbersWithUnitDefinitions.AmbiguousDimensionUnitList.ToImmutableList();

        public DimensionExtractorConfiguration()
            : base(new CultureInfo(Culture.Swedish))
        {
        }

        public DimensionExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => DimensionSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;
    }
}
