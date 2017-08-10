using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class CurrencyExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public CurrencyExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public CurrencyExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => CurrencySuffixList;

        public override ImmutableDictionary<string, string> PrefixList => CurrencyPrefixList;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_CURRENCY;

        public static readonly ImmutableDictionary<string, string> CurrencySuffixList = NumericWithUnit.CurrencySuffixList.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> CurrencyPrefixList = NumericWithUnit.CurrencyPrefixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumericWithUnit.AmbiguousCurrencyUnitList.ToImmutableList();
    }
}