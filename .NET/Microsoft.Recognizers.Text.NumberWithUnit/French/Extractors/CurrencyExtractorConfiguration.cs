using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class CurrencyExtractorConfiguration : FrenchNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> CurrencySuffixList =
            NumbersWithUnitDefinitions.CurrencySuffixList.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> CurrencyPrefixList =
            NumbersWithUnitDefinitions.CurrencyPrefixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues =
            NumbersWithUnitDefinitions.AmbiguousCurrencyUnitList.ToImmutableList();

        public CurrencyExtractorConfiguration()
            : this(new CultureInfo(Culture.French))
        {
        }

        public CurrencyExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => CurrencySuffixList;

        public override ImmutableDictionary<string, string> PrefixList => CurrencyPrefixList;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_CURRENCY;
    }
}