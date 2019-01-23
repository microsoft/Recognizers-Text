using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class VolumeParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public VolumeParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}
