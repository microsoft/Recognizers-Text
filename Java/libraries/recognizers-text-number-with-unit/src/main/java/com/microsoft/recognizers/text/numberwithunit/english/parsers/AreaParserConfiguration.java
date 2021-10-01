// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.english.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.english.extractors.AreaExtractorConfiguration;

public class AreaParserConfiguration extends EnglishNumberWithUnitParserConfiguration {

    public AreaParserConfiguration() {
        this(new CultureInfo(Culture.English));
    }

    public AreaParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(AreaExtractorConfiguration.AreaSuffixList);
    }
}
