// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.portuguese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.portuguese.extractors.LengthExtractorConfiguration;

public class LengthParserConfiguration extends PortugueseNumberWithUnitParserConfiguration {

    public LengthParserConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
    }

    public LengthParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(LengthExtractorConfiguration.LengthSuffixList);
    }
}
