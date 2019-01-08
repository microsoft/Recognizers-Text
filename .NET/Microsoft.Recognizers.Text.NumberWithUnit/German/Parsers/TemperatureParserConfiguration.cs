using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class TemperatureParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public TemperatureParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
        }
    }
}
