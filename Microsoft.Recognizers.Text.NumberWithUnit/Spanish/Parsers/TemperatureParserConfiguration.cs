using Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Parsers
{
    public class TemperatureParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public TemperatureParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
        }
    }
}
