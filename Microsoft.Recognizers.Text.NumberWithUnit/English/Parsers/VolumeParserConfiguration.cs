using Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Parsers
{
    public class VolumeParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration() : this(new CultureInfo(Culture.English)) { }

        public VolumeParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}
