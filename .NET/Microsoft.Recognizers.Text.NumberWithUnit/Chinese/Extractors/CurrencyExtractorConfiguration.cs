using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class CurrencyExtractorConfiguration : ChineseNumberWithUnitExtractorConfiguration
    {
        public CurrencyExtractorConfiguration()
            : this(new CultureInfo(Culture.Chinese))
        {
        }

        public CurrencyExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => NumbersWithUnitDefinitions.CurrencySuffixList.ToImmutableSortedDictionary();

        public override ImmutableSortedDictionary<string, string> PrefixList => NumbersWithUnitDefinitions.CurrencyPrefixList.ToImmutableSortedDictionary();

        public override ImmutableList<string> AmbiguousUnitList => NumbersWithUnitDefinitions.CurrencyAmbiguousValues.ToImmutableList();

        public override string ExtractType => Constants.SYS_UNIT_CURRENCY;
    }
}