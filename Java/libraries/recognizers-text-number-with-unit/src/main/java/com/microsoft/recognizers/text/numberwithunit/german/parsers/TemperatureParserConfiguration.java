// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.german.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.german.extractors.TemperatureExtractorConfiguration;

public class TemperatureParserConfiguration extends GermanNumberWithUnitParserConfiguration {

    public TemperatureParserConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public TemperatureParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
    }
}
