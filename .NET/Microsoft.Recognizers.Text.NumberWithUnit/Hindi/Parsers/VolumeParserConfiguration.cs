using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class VolumeParserConfiguration : HindiNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration()
               : this(new CultureInfo(Culture.Hindi))
        {
        }

        public VolumeParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}
