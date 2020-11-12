using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class CurrencyExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> CurrencySuffixList =
            NumbersWithUnitDefinitions.CurrencySuffixList.ToImmutableDictionary();

        // Merge CurrencyNameToIsoCodeMap with CurrencyPrefixList (excluding fake and unofficial Iso codes starting with underscore)
        public static readonly Dictionary<string, string> CurrencyPrefixDict =
            NumbersWithUnitDefinitions.CurrencyPrefixList
            .Concat(NumbersWithUnitDefinitions.CurrencyNameToIsoCodeMap.Where(x => !x.Value.StartsWith("_"))
                .ToDictionary(x => x.Key, x => x.Value.ToLower())).GroupBy(x => x.Key)
            .ToDictionary(x => x.Key, y => y.Count() > 1 ? string.Join("|", new string[] { y.First().Value, y.Last().Value }) : y.First().Value);

        public static readonly ImmutableDictionary<string, string> CurrencyPrefixList = CurrencyPrefixDict.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> FractionalUnitNameToCodeMap =
            NumbersWithUnitDefinitions.FractionalUnitNameToCodeMap.ToImmutableDictionary();

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

        public override ImmutableDictionary<string, string> SuffixList => CurrencySuffixList;

        public override ImmutableDictionary<string, string> PrefixList => CurrencyPrefixList;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_CURRENCY;
    }
}