// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.portuguese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.portuguese.extractors.WeightExtractorConfiguration;

public class WeightParserConfiguration extends PortugueseNumberWithUnitParserConfiguration {

    public WeightParserConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
    }

    public WeightParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(WeightExtractorConfiguration.WeightSuffixList);
    }
}
