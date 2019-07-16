using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class VolumeExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> VolumeSuffixList = NumbersWithUnitDefinitions.VolumeSuffixList.ToImmutableSortedDictionary();

        public VolumeExtractorConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public VolumeExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => VolumeSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_VOLUME;
    }
}
