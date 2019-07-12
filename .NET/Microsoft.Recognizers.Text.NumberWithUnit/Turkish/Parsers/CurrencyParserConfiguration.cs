using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class CurrencyParserConfiguration : TurkishNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration()
               : this(new CultureInfo(Culture.Turkish))
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
