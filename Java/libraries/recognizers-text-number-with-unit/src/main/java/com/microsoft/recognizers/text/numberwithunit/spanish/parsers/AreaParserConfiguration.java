package com.microsoft.recognizers.text.numberwithunit.spanish.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.spanish.extractors.AreaExtractorConfiguration;

public class AreaParserConfiguration extends SpanishNumberWithUnitParserConfiguration {

    public AreaParserConfiguration() {
        this(new CultureInfo(Culture.Spanish));
    }

    public AreaParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(AreaExtractorConfiguration.AreaSuffixList);
    }
}
