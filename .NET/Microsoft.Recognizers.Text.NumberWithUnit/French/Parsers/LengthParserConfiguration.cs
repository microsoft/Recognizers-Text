﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class LengthParserConfiguration : FrenchNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration()
            : this(new CultureInfo(Culture.French))
        {
        }

        public LengthParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LengthSuffixList);
        }
    }
}
