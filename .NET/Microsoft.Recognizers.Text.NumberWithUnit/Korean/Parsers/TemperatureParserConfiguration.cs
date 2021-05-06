using System.Globalization;

using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Korean
{
    public class TemperatureParserConfiguration : KoreanNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration()
            : this(new CultureInfo(Culture.Korean))
        {
        }

        public TemperatureParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(NumbersWithUnitDefinitions.TemperaturePrefixList);
            this.BindDictionary(NumbersWithUnitDefinitions.TemperatureSuffixList);
        }
    }
}