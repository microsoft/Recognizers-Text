﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class DimensionParserConfiguration : PortugueseNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
        }
    }
}
