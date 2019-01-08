using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class VolumeParserConfiguration : DutchNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public VolumeParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}
