﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class AreaParserConfiguration : FrenchNumberWithUnitParserConfiguration
    {
        public AreaParserConfiguration()
            : this(new CultureInfo(Culture.French))
        {
        }

        public AreaParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(AreaExtractorConfiguration.AreaSuffixList);
        }
    }
}
