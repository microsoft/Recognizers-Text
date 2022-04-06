// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Japanese
{
    public class DimensionParserConfiguration : JapaneseNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration()
            : this(new CultureInfo(Culture.Japanese))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(NumbersWithUnitDefinitions.DimensionPrefixList);
            this.BindDictionary(NumbersWithUnitDefinitions.DimensionSuffixList);
            this.CheckFirstSuffix = NumbersWithUnitDefinitions.CheckFirstSuffix;
        }
    }
}