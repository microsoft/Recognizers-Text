// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Japanese
{
    public class DimensionExtractorConfiguration : JapaneseNumberWithUnitExtractorConfiguration
    {
        public DimensionExtractorConfiguration()
            : this(new CultureInfo(Culture.Japanese))
        {
        }

        public DimensionExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => NumbersWithUnitDefinitions.DimensionSuffixList.ToImmutableDictionary();

        public override ImmutableDictionary<string, string> PrefixList => NumbersWithUnitDefinitions.DimensionPrefixList.ToImmutableDictionary();

        public override ImmutableList<string> AmbiguousUnitList => NumbersWithUnitDefinitions.DimensionAmbiguousValues.ToImmutableList();

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;
    }
}