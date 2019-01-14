using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class CurrencyParserConfiguration : PortugueseNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
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
