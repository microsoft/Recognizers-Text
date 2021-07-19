// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.portuguese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.portuguese.extractors.DimensionExtractorConfiguration;

public class DimensionParserConfiguration extends PortugueseNumberWithUnitParserConfiguration {

    public DimensionParserConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
    }

    public DimensionParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
    }
}
