﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class VolumeParserConfiguration : PortugueseNumberWithUnitParserConfiguration
    {
        public VolumeParserConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public VolumeParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
        }
    }
}