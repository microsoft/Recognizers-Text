﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class LengthExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> LengthSuffixList =
            NumbersWithUnitDefinitions.LengthSuffixList.ToImmutableDictionary();

        public static readonly ImmutableList<string> AmbiguousUnits =
            NumbersWithUnitDefinitions.AmbiguousLengthUnitList.ToImmutableList();

        public LengthExtractorConfiguration()
               : base(new CultureInfo(Culture.English))
        {
        }

        public LengthExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => LengthSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousUnits;

        public override string ExtractType => Constants.SYS_UNIT_LENGTH;
    }
}