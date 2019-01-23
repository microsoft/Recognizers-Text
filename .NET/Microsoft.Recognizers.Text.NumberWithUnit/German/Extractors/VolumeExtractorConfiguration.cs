using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class VolumeExtractorConfiguration : GermanNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> VolumeSuffixList = NumbersWithUnitDefinitions.VolumeSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousVolumeUnitList.ToImmutableList();

        public VolumeExtractorConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public VolumeExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => VolumeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_VOLUME;
    }
}