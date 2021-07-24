﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Swedish
{
    public class LengthParserConfiguration : SwedishNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration()
            : this(new CultureInfo(Culture.Swedish))
        {
        }

        public LengthParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LengthSuffixList);
        }
    }
}
