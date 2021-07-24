﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class LengthParserConfiguration : HindiNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration()
               : this(new CultureInfo(Culture.Hindi))
        {
        }

        public LengthParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LengthSuffixList);
        }
    }
}
