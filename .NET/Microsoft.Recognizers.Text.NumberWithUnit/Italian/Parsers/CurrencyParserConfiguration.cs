using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Italian
{
    public class CurrencyParserConfiguration : ItalianNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration()
            : this(new CultureInfo(Culture.Italian))
        {
        }

        public CurrencyParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(CurrencyExtractorConfiguration.CurrencySuffixList);
            this.BindDictionary(CurrencyExtractorConfiguration.CurrencyPrefixList);
            this.CurrencyNameToIsoCodeMap = NumbersWithUnitDefinitions.CurrencyNameToIsoCodeMap.ToImmutableDictionary();
            this.CurrencyFractionCodeList = NumbersWithUnitDefinitions.FractionalUnitNameToCodeMap.ToImmutableDictionary();
        }
    }
}
