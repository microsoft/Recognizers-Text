// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.english.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.english.extractors.WeightExtractorConfiguration;

public class WeightParserConfiguration extends EnglishNumberWithUnitParserConfiguration {

    public WeightParserConfiguration() {
        this(new CultureInfo(Culture.English));
    }

    public WeightParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(WeightExtractorConfiguration.WeightSuffixList);
    }
}
