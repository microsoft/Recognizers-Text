using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.Hindi;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class CurrencyParserConfiguration : HindiNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration()
               : this(new CultureInfo(Culture.Hindi))
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
