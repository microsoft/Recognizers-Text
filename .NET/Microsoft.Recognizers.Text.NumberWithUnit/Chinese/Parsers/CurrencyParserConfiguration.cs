using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class CurrencyParserConfiguration : ChineseNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration()
            : this(new CultureInfo(Culture.Chinese))
        {
        }

        public CurrencyParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(NumbersWithUnitDefinitions.CurrencyPrefixList);
            this.BindDictionary(NumbersWithUnitDefinitions.CurrencySuffixList);
            this.CurrencyNameToIsoCodeMap = NumbersWithUnitDefinitions.CurrencyNameToIsoCodeMap.ToImmutableDictionary();
            this.CurrencyFractionCodeList = NumbersWithUnitDefinitions.FractionalUnitNameToCodeMap.ToImmutableDictionary();
        }
    }
}
