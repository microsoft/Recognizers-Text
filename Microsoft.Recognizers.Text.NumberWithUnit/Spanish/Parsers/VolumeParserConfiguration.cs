using Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Parsers
{
    public class VolumeParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public VolumeParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}
