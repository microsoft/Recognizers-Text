﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class SpeedParserConfiguration : HindiNumberWithUnitParserConfiguration
    {
        public SpeedParserConfiguration()
               : this(new CultureInfo(Culture.Hindi))
        {
        }

        public SpeedParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
        }
    }
}
