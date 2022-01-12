// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class AreaExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AreaSuffixList = NumbersWithUnitDefinitions.AreaSuffixList.ToImmutableDictionary();

        public static readonly ImmutableList<string> AmbiguousValues = new List<string> { }.ToImmutableList();

        public AreaExtractorConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public AreaExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => AreaSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_AREA;
    }
}
