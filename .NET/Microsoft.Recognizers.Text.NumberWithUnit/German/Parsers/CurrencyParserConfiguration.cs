using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class CurrencyParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public CurrencyParserConfiguration()
            : this(new CultureInfo(Culture.German))
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
