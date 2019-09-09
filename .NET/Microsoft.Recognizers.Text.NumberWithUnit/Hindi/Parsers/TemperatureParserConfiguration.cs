using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class TemperatureParserConfiguration : HindiNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration()
               : this(new CultureInfo(Culture.Hindi))
        {
        }

        public TemperatureParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
        }
    }
}
