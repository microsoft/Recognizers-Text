// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.german.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.german.extractors.DimensionExtractorConfiguration;

public class DimensionParserConfiguration extends GermanNumberWithUnitParserConfiguration {

    public DimensionParserConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public DimensionParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
    }
}
