package com.microsoft.recognizers.text.numberwithunit.spanish.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.spanish.extractors.CurrencyExtractorConfiguration;

public class CurrencyParserConfiguration extends SpanishNumberWithUnitParserConfiguration {

    public CurrencyParserConfiguration() {
        this(new CultureInfo(Culture.Spanish));
    }

    public CurrencyParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(CurrencyExtractorConfiguration.CurrencySuffixList);
        this.bindDictionary(CurrencyExtractorConfiguration.CurrencyPrefixList);
    }
}
