// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class WeightParserConfiguration : PortugueseNumberWithUnitParserConfiguration
    {
        public WeightParserConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public WeightParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(WeightExtractorConfiguration.WeightSuffixList);
        }
    }
}
