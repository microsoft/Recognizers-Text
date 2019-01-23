using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class VolumeParserConfiguration : FrenchNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration()
            : this(new CultureInfo(Culture.French))
        {
        }

        public VolumeParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}
