using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class TemperatureParserConfiguration : TurkishNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration()
               : this(new CultureInfo(Culture.Turkish))
        {
        }

        public TemperatureParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
        }
    }
}
