using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class VolumeExtractorConfiguration : TurkishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> VolumeSuffixList = NumbersWithUnitDefinitions.VolumeSuffixList.ToImmutableSortedDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousVolumeUnitList.ToImmutableList();

        public VolumeExtractorConfiguration()
               : this(new CultureInfo(Culture.Turkish))
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