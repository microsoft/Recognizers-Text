using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class TemperatureParserConfiguration : PortugueseNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public TemperatureParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
        }
    }
}
