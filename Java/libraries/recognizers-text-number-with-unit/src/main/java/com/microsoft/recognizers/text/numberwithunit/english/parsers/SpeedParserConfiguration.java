// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.english.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.english.extractors.SpeedExtractorConfiguration;

public class SpeedParserConfiguration extends EnglishNumberWithUnitParserConfiguration {

    public SpeedParserConfiguration() {
        this(new CultureInfo(Culture.English));
    }

    public SpeedParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
    }
}
