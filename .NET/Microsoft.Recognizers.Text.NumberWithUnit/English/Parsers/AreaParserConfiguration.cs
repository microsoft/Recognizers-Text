﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class AreaParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public AreaParserConfiguration()
               : this(new CultureInfo(Culture.English))
        {
        }

        public AreaParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(AreaExtractorConfiguration.AreaSuffixList);
        }
    }
}
