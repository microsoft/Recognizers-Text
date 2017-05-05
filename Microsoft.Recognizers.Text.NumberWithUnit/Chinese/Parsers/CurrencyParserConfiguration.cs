using Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Parsers
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
