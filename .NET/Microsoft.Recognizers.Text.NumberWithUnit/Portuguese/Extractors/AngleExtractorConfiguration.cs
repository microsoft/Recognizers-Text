// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class AngleExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AngleSuffixList =
            NumbersWithUnitDefinitions.AngleSuffixList.ToImmutableDictionary();

        public static readonly ImmutableList<string> AmbiguousUnits =
            NumbersWithUnitDefinitions.AmbiguousAngleUnitList.ToImmutableList();

        public AngleExtractorConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public AngleExtractorConfiguration(CultureInfo ci)
               : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => AngleSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousUnits;

        public override string ExtractType => Constants.SYS_UNIT_ANGLE;
    }
}