using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class VolumeExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public VolumeExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public VolumeExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => VolumeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_VOLUME;

        public static readonly ImmutableDictionary<string, string> VolumeSuffixList = NumericWithUnit.VolumeSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumericWithUnit.AmbiguousVolumeUnitList.ToImmutableList();
    }
}