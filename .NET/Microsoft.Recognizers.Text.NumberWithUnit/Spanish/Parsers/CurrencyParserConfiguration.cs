using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class CurrencyParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration()
               : this(new CultureInfo(Culture.Spanish))
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
