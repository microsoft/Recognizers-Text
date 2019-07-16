using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class CurrencyExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> CurrencySuffixList =
            NumbersWithUnitDefinitions.CurrencySuffixList.ToImmutableSortedDictionary();

        public static readonly ImmutableSortedDictionary<string, string> CurrencyPrefixList =
            NumbersWithUnitDefinitions.CurrencyPrefixList.ToImmutableSortedDictionary();

        public static readonly ImmutableSortedDictionary<string, string> FractionalUnitNameToCodeMap =
            NumbersWithUnitDefinitions.FractionalUnitNameToCodeMap.ToImmutableSortedDictionary();

        private static readonly ImmutableList<string> AmbiguousValues =
            NumbersWithUnitDefinitions.AmbiguousCurrencyUnitList.ToImmutableList();

        public CurrencyExtractorConfiguration()
            : this(new CultureInfo(Culture.English))
        {
        }

        public CurrencyExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => CurrencySuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => CurrencyPrefixList;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_CURRENCY;
    }
}