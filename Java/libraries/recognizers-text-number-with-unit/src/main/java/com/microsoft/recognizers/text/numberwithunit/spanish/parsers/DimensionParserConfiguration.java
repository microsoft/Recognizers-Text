package com.microsoft.recognizers.text.numberwithunit.spanish.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.spanish.extractors.DimensionExtractorConfiguration;

public class DimensionParserConfiguration extends SpanishNumberWithUnitParserConfiguration {

    public DimensionParserConfiguration() {
        this(new CultureInfo(Culture.Spanish));
    }

    public DimensionParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
    }
}
