// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class DimensionExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> DimensionSuffixList =
            NumbersWithUnitDefinitions.InformationSuffixList
            .Concat(AreaExtractorConfiguration.AreaSuffixList)
            .Concat(LengthExtractorConfiguration.LengthSuffixList)
            .Concat(SpeedExtractorConfiguration.SpeedSuffixList)
            .Concat(VolumeExtractorConfiguration.VolumeSuffixList)
            .Concat(WeightExtractorConfiguration.WeightSuffixList)
            .Concat(AngleExtractorConfiguration.AngleSuffixList)
            .ToImmutableDictionary(x => x.Key, x => x.Value);

        public static readonly ImmutableDictionary<string, string> DimensionTypeList =
            NumbersWithUnitDefinitions.InformationSuffixList.ToDictionary(x => x.Key, x => Constants.INFORMATION)
            .Concat(AreaExtractorConfiguration.AreaSuffixList.ToDictionary(x => x.Key, x => Constants.AREA))
            .Concat(LengthExtractorConfiguration.LengthSuffixList.ToDictionary(x => x.Key, x => Constants.LENGTH))
            .Concat(SpeedExtractorConfiguration.SpeedSuffixList.ToDictionary(x => x.Key, x => Constants.SPEED))
            .Concat(VolumeExtractorConfiguration.VolumeSuffixList.ToDictionary(x => x.Key, x => Constants.VOLUME))
            .Concat(WeightExtractorConfiguration.WeightSuffixList.ToDictionary(x => x.Key, x => Constants.WEIGHT))
            .Concat(AngleExtractorConfiguration.AngleSuffixList.ToDictionary(x => x.Key, x => Constants.ANGLE))
            .ToImmutableDictionary(x => x.Key, x => x.Value);

        public static readonly IDictionary<string, string> LengthUnitToSubUnitMap = NumbersWithUnitDefinitions.LengthUnitToSubUnitMap;

        public static readonly IDictionary<string, long> LengthSubUnitFractionalRatios = NumbersWithUnitDefinitions.LengthSubUnitFractionalRatios;

        private static readonly ImmutableList<string> AmbiguousUnits =
            NumbersWithUnitDefinitions.AmbiguousDimensionUnitList
            .Concat(AreaExtractorConfiguration.AmbiguousUnits)
            .Concat(LengthExtractorConfiguration.AmbiguousUnits)
            .Concat(SpeedExtractorConfiguration.AmbiguousUnits)
            .Concat(VolumeExtractorConfiguration.AmbiguousUnits)
            .Concat(WeightExtractorConfiguration.AmbiguousUnits)
            .Concat(AngleExtractorConfiguration.AmbiguousUnits)
            .Distinct()
            .ToImmutableList();

        public DimensionExtractorConfiguration()
            : base(new CultureInfo(Culture.English))
        {
        }

        public DimensionExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => DimensionSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousUnits;

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;
    }
}
