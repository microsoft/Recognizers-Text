using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Korean
{
    public class CurrencyParserConfiguration : KoreanNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration()
            : this(new CultureInfo(Culture.Korean))
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
