using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class DimensionExtractorConfiguration : FrenchNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> DimensionSuffixList =
            NumbersWithUnitDefinitions.InformationSuffixList
            .Concat(AreaExtractorConfiguration.AreaSuffixList)
            .Concat(LengthExtractorConfiguration.LengthSuffixList)
            .Concat(SpeedExtractorConfiguration.SpeedSuffixList)
            .Concat(VolumeExtractorConfiguration.VolumeSuffixList)
            .Concat(WeightExtractorConfiguration.WeightSuffixList)
            .ToImmutableDictionary(x => x.Key, x => x.Value);

        public static readonly ImmutableDictionary<string, string> DimensionTypeList =
            NumbersWithUnitDefinitions.InformationSuffixList.ToDictionary(x => x.Key, x => Constants.INFORMATION)
            .Concat(AreaExtractorConfiguration.AreaSuffixList.ToDictionary(x => x.Key, x => Constants.AREA))
            .Concat(LengthExtractorConfiguration.LengthSuffixList.ToDictionary(x => x.Key, x => Constants.LENGTH))
            .Concat(SpeedExtractorConfiguration.SpeedSuffixList.ToDictionary(x => x.Key, x => Constants.SPEED))
            .Concat(VolumeExtractorConfiguration.VolumeSuffixList.ToDictionary(x => x.Key, x => Constants.VOLUME))
            .Concat(WeightExtractorConfiguration.WeightSuffixList.ToDictionary(x => x.Key, x => Constants.WEIGHT))
            .ToImmutableDictionary(x => x.Key, x => x.Value);

        private static readonly ImmutableList<string> AmbiguousValues =
            NumbersWithUnitDefinitions.AmbiguousDimensionUnitList.ToImmutableList();

        public DimensionExtractorConfiguration()
            : base(new CultureInfo(Culture.French))
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
