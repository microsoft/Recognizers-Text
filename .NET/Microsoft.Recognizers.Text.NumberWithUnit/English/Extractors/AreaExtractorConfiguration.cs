﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class AreaExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AreaSuffixList =
            NumbersWithUnitDefinitions.AreaSuffixList.ToImmutableDictionary();

        public static readonly ImmutableList<string> AmbiguousUnits =
            NumbersWithUnitDefinitions.AmbiguousAreaUnitList.ToImmutableList();

        public AreaExtractorConfiguration()
               : this(new CultureInfo(Culture.English))
        {
        }

        public AreaExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => AreaSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousUnits;

        public override string ExtractType => Constants.SYS_UNIT_AREA;
    }
}