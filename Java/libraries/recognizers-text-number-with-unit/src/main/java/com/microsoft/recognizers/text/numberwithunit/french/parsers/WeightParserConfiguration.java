// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.french.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.french.extractors.WeightExtractorConfiguration;

public class WeightParserConfiguration extends FrenchNumberWithUnitParserConfiguration {

    public WeightParserConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public WeightParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(WeightExtractorConfiguration.WeightSuffixList);
    }
}
