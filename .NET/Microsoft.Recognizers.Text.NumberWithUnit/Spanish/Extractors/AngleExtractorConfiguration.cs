// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class AngleExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableDictionary<string, string> AngleSuffixList =
            NumbersWithUnitDefinitions.AngleSuffixList.ToImmutableDictionary();

        public static readonly ImmutableList<string> AmbiguousUnits =
            NumbersWithUnitDefinitions.AmbiguousAngleUnitList.ToImmutableList();

        public AngleExtractorConfiguration()
               : this(new CultureInfo(Culture.Spanish))
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