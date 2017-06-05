using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class TemperatureParserConfiguration : ChineseNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration() : this(new CultureInfo(Culture.Chinese)) { }

        public TemperatureParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(TemperatureExtractorConfiguration.TemperaturePrefixList);
            this.BindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
        }
    }
}
