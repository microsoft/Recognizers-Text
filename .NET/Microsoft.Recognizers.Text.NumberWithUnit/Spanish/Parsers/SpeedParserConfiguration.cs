﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class SpeedParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public SpeedParserConfiguration()
               : this(new CultureInfo(Culture.Spanish))
        {
        }

        public SpeedParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
        }
    }
}
