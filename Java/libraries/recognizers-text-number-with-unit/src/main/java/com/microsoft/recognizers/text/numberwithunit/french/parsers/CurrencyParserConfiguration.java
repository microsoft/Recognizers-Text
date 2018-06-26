package com.microsoft.recognizers.text.numberwithunit.french.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.french.extractors.CurrencyExtractorConfiguration;

public class CurrencyParserConfiguration extends FrenchNumberWithUnitParserConfiguration {

    public CurrencyParserConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public CurrencyParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(CurrencyExtractorConfiguration.CurrencySuffixList);
        this.bindDictionary(CurrencyExtractorConfiguration.CurrencyPrefixList);
    }
}
