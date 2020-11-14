using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Swedish
{
    public class CurrencyParserConfiguration : SwedishNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration()
            : this(new CultureInfo(Culture.Swedish))
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
