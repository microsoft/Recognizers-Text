using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Swedish
{
    public class VolumeParserConfiguration : SwedishNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration()
            : this(new CultureInfo(Culture.Swedish))
        {
        }

        public VolumeParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}
