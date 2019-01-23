using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class TemperatureParserConfiguration : FrenchNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration()
            : this(new CultureInfo(Culture.French))
        {
        }

        public TemperatureParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
        }

        public override string ConnectorToken => null;
    }
}
