// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class DimensionParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public static readonly IDictionary<string, string> LengthUnitToSubUnitMap = DimensionExtractorConfiguration.LengthUnitToSubUnitMap;

        public static readonly IDictionary<string, long> LengthSubUnitFractionalRatios = DimensionExtractorConfiguration.LengthSubUnitFractionalRatios;

        public DimensionParserConfiguration()
               : this(new CultureInfo(Culture.English))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
        }
    }
}
