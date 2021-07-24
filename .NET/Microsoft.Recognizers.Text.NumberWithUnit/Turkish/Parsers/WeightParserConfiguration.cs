﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class WeightParserConfiguration : TurkishNumberWithUnitParserConfiguration
    {
        public WeightParserConfiguration()
               : this(new CultureInfo(Culture.Turkish))
        {
        }

        public WeightParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(WeightExtractorConfiguration.WeightSuffixList);
        }
    }
}
