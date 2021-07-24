﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class AreaParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public AreaParserConfiguration()
               : this(new CultureInfo(Culture.Spanish))
        {
        }

        public AreaParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(AreaExtractorConfiguration.AreaSuffixList);
        }
    }
}
