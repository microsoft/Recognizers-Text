﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class LengthParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public LengthParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LengthSuffixList);
        }
    }
}
