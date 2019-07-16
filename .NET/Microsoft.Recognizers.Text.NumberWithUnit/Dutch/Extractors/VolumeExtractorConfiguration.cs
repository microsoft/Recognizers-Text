using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class VolumeExtractorConfiguration : DutchNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> VolumeSuffixList =
            NumbersWithUnitDefinitions.VolumeSuffixList.ToImmutableSortedDictionary();

        private static readonly ImmutableList<string> AmbiguousValues =
            NumbersWithUnitDefinitions.AmbiguousVolumeUnitList.ToImmutableList();

        public VolumeExtractorConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public VolumeExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => VolumeSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_VOLUME;
    }
}