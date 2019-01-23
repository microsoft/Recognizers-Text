using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class VolumeParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration()
               : this(new CultureInfo(Culture.English))
        {
        }

        public VolumeParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}
