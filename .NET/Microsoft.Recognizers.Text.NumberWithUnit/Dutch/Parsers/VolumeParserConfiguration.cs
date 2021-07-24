﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class VolumeParserConfiguration : DutchNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public VolumeParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}
