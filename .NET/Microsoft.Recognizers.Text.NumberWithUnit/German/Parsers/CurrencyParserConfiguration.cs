using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class CurrencyParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public CurrencyParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(CurrencyExtractorConfiguration.CurrencySuffixList);
            this.BindDictionary(CurrencyExtractorConfiguration.CurrencyPrefixList);
        }
    }
}
