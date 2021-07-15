// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.french.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.french.extractors.DimensionExtractorConfiguration;

public class DimensionParserConfiguration extends FrenchNumberWithUnitParserConfiguration {

    public DimensionParserConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public DimensionParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
    }
}
