﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Swedish
{
    public class WeightParserConfiguration : SwedishNumberWithUnitParserConfiguration
    {
        public WeightParserConfiguration()
            : this(new CultureInfo(Culture.Swedish))
        {
        }

        public WeightParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(WeightExtractorConfiguration.WeightSuffixList);
        }
    }
}
