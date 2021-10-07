﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class LengthParserConfiguration : TurkishNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration()
               : this(new CultureInfo(Culture.Turkish))
        {
        }

        public LengthParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LengthSuffixList);
        }
    }
}
