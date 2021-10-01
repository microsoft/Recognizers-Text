﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class SpeedParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public SpeedParserConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public SpeedParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
        }
    }
}
