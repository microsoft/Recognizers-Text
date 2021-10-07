﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class TemperatureParserConfiguration : DutchNumberWithUnitParserConfiguration
    {
        public TemperatureParserConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public TemperatureParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
        }
    }
}
