using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class VolumeParserConfiguration : TurkishNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration()
               : this(new CultureInfo(Culture.Turkish))
        {
        }

        public VolumeParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}
