// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class CurrencyExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> CurrencySuffixList = NumbersWithUnitDefinitions.CurrencySuffixList.ToImmutableDictionary();

        // CurrencyNameToIsoCodeMap dictionary (excluding fake and unofficial Iso codes starting with underscore)
        public static readonly Dictionary<string, string> IsoCodeDict =
            NumbersWithUnitDefinitions.CurrencyNameToIsoCodeMap.Where(x => !x.Value.StartsWith("_", StringComparison.Ordinal))
                .ToDictionary(x => x.Key, x => x.Value.ToLower(CultureInfo.InvariantCulture));

        // CurrencyNameToIsoCodeMap followed by '$' symbol (e.g. 'AUD$')
        public static readonly Dictionary<string, string> IsoCodeWithSymbolDict =
            NumbersWithUnitDefinitions.CurrencyNameToIsoCodeMap.Where(x => !x.Value.StartsWith("_", StringComparison.Ordinal))
                .ToDictionary(x => x.Key, x => x.Value.ToLower(CultureInfo.InvariantCulture) + "$");

        // Merge IsoCodeDict and IsoCodeWithSymbolDict
        public static readonly Dictionary<string, string> IsoCodeCombinedDict = IsoCodeDict.Concat(IsoCodeWithSymbolDict)
            .GroupBy(x => x.Key).ToDictionary(x => x.Key, y => y.Count() > 1 ? string.Join("|", new string[] { y.First().Value, y.Last().Value }) : y.First().Value);

        // Merge IsoCodeCombinedDict with CurrencyPrefixList (excluding fake and unofficial Iso codes starting with underscore)
        public static readonly Dictionary<string, string> CurrencyPrefixDict = NumbersWithUnitDefinitions.CurrencyPrefixList.Concat(IsoCodeCombinedDict)
            .GroupBy(x => x.Key).ToDictionary(x => x.Key, y => y.Count() > 1 ? string.Join("|", new string[] { y.First().Value, y.Last().Value }) : y.First().Value);

        public static readonly ImmutableDictionary<string, string> CurrencyPrefixList = CurrencyPrefixDict.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousCurrencyUnitList.ToImmutableList();

        public CurrencyExtractorConfiguration()
               : this(new CultureInfo(Culture.Spanish))
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
