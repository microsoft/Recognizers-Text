using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class TemperatureParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration()
               : this(new CultureInfo(Culture.English))
        {
        }

        public TemperatureParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
        }
    }
}
