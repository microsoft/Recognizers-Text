package com.microsoft.recognizers.text.numberwithunit.portuguese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.portuguese.extractors.CurrencyExtractorConfiguration;

public class CurrencyParserConfiguration extends PortugueseNumberWithUnitParserConfiguration {

    public CurrencyParserConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
    }

    public CurrencyParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(CurrencyExtractorConfiguration.CurrencySuffixList);
        this.bindDictionary(CurrencyExtractorConfiguration.CurrencyPrefixList);
    }
}
