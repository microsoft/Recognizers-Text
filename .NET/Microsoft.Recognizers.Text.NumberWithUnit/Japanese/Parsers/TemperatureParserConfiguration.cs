// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Japanese
{
    public class TemperatureParserConfiguration : JapaneseNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration()
            : this(new CultureInfo(Culture.Japanese))
        {
        }

        public TemperatureParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(NumbersWithUnitDefinitions.TemperaturePrefixList);
            this.BindDictionary(NumbersWithUnitDefinitions.TemperatureSuffixList);
            this.CheckFirstSuffix = NumbersWithUnitDefinitions.CheckFirstSuffix;
        }
    }
}