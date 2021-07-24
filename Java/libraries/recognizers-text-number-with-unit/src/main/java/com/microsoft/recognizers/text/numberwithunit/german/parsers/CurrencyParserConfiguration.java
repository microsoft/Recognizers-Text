// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.german.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.german.extractors.CurrencyExtractorConfiguration;

public class CurrencyParserConfiguration extends GermanNumberWithUnitParserConfiguration {
    public CurrencyParserConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public CurrencyParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(CurrencyExtractorConfiguration.CurrencySuffixList);
        this.bindDictionary(CurrencyExtractorConfiguration.CurrencyPrefixList);
    }
}
