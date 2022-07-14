// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
        // CurrencyNameToIsoCodeMap dictionary (excluding fake and unofficial Iso codes starting with underscore)
        public static readonly Dictionary<string, string> IsoCodeDict =
            NumbersWithUnitDefinitions.CurrencyNameToIsoCodeMap.Where(x => !x.Value.StartsWith("_", StringComparison.Ordinal))
                .ToDictionary(x => x.Key, x => x.Value.ToLower(CultureInfo.InvariantCulture));

        // CurrencyNameToIsoCodeMap followed by '$' symbol (e.g. 'AUD$')
        public static readonly Dictionary<string, string> IsoCodeWithSymbolDict =
            NumbersWithUnitDefinitions.CurrencyNameToIsoCodeMap.Where(x => !x.Value.StartsWith("_", StringComparison.Ordinal))
                .ToDictionary(x => x.Key, x => x.Value.ToLower(CultureInfo.InvariantCulture) + "$");

        // CurrencyNameToIsoCodeMap preceded by 'M' symbol (e.g. 'MUSD')
        public static readonly Dictionary<string, string> IsoCodeWithMutiplierDict =
            NumbersWithUnitDefinitions.CurrencyNameToIsoCodeMap.Where(x => !x.Value.StartsWith("_", StringComparison.Ordinal))
                .ToDictionary(x => x.Key, x => "m" + x.Value.ToLower(CultureInfo.InvariantCulture));

        // Merge IsoCodeDict and IsoCodeWithSymbolDict
        public static readonly Dictionary<string, string> IsoCodeCombinedDictWithSymbol = IsoCodeDict.Concat(IsoCodeWithSymbolDict)
            .GroupBy(x => x.Key).ToDictionary(x => x.Key, y => y.Count() > 1 ? string.Join("|", new string[] { y.First().Value, y.Last().Value }) : y.First().Value);

        // Merge IsoCodeDict and IsoCodeWithMutiplierDict
        public static readonly Dictionary<string, string> IsoCodeCombinedDict = IsoCodeCombinedDictWithSymbol.Concat(IsoCodeWithMutiplierDict)
            .GroupBy(x => x.Key).ToDictionary(x => x.Key, y => y.Count() > 1 ? string.Join("|", new string[] { y.First().Value, y.Last().Value }) : y.First().Value);

        // Merge IsoCodeCombinedDict with CurrencyPrefixList (excluding fake and unofficial Iso codes starting with underscore)
        public static readonly Dictionary<string, string> CurrencyPrefixDict = NumbersWithUnitDefinitions.CurrencyPrefixList.Concat(IsoCodeCombinedDict)
            .GroupBy(x => x.Key).ToDictionary(x => x.Key, y => y.Count() > 1 ? string.Join("|", new string[] { y.First().Value, y.Last().Value }) : y.First().Value);

        // Merge IsoCodeCombinedDict with CurrencySuffixList (excluding fake and unofficial Iso codes starting with underscore)
        public static readonly Dictionary<string, string> CurrencySuffixDict = NumbersWithUnitDefinitions.CurrencySuffixList.Concat(IsoCodeCombinedDict)
            .GroupBy(x => x.Key).ToDictionary(x => x.Key, y => y.Count() > 1 ? string.Join("|", new string[] { y.First().Value, y.Last().Value }) : y.First().Value);

        public static readonly ImmutableDictionary<string, string> CurrencyPrefixList = CurrencyPrefixDict.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> CurrencySuffixList = CurrencySuffixDict.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> FractionalUnitNameToCodeMap =
            NumbersWithUnitDefinitions.FractionalUnitNameToCodeMap.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousUnits =
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

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousUnits;

        public override string ExtractType => Constants.SYS_UNIT_CURRENCY;
    }
}