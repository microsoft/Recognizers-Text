using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class CurrencyParserConfiguration : ChineseNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration() : this(new CultureInfo(Culture.Chinese)) { }

        public CurrencyParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(CurrencyExtractorConfiguration.CurrencyPrefixList);
            this.BindDictionary(CurrencyExtractorConfiguration.CurrencySuffixList);
        }
    }
}
