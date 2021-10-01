// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.german.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.german.extractors.AgeExtractorConfiguration;

public class AgeParserConfiguration extends GermanNumberWithUnitParserConfiguration {

    public AgeParserConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public AgeParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(AgeExtractorConfiguration.AgeSuffixList);
    }
}
