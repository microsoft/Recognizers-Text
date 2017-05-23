using Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Parsers
{
    public class TemperatureParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration() : this(new CultureInfo(Culture.English)) { }

        public TemperatureParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
        }
    }
}
