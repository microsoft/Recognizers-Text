using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class CurrencyExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> CurrencySuffixList = NumbersWithUnitDefinitions.CurrencySuffixList.ToImmutableSortedDictionary();

        public static readonly ImmutableSortedDictionary<string, string> CurrencyPrefixList = NumbersWithUnitDefinitions.CurrencyPrefixList.ToImmutableSortedDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousCurrencyUnitList.ToImmutableList();

        public CurrencyExtractorConfiguration()
               : this(new CultureInfo(Culture.Spanish))
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
