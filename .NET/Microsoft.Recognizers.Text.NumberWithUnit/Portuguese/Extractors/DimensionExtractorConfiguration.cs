﻿using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class DimensionExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> DimensionSuffixList = NumbersWithUnitDefinitions.InformationSuffixList
            .Concat(AreaExtractorConfiguration.AreaSuffixList)
            .Concat(LengthExtractorConfiguration.LengthSuffixList)
            .Concat(SpeedExtractorConfiguration.SpeedSuffixList)
            .Concat(VolumeExtractorConfiguration.VolumeSuffixList)
            .Concat(WeightExtractorConfiguration.WeightSuffixList)
            .ToImmutableSortedDictionary(x => x.Key, x => x.Value);

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousDimensionUnitList.ToImmutableList();

        public DimensionExtractorConfiguration()
                : base(new CultureInfo(Culture.Portuguese))
        {
        }

        public DimensionExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => DimensionSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;
    }
}
