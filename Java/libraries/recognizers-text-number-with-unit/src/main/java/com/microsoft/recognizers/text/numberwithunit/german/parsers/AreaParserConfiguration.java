// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.german.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.german.extractors.AreaExtractorConfiguration;

public class AreaParserConfiguration extends GermanNumberWithUnitParserConfiguration {

    public AreaParserConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public AreaParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(AreaExtractorConfiguration.AreaSuffixList);
    }
}
