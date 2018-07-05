package com.microsoft.recognizers.text.numberwithunit.spanish.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.spanish.extractors.LengthExtractorConfiguration;

public class LengthParserConfiguration extends SpanishNumberWithUnitParserConfiguration {

    public LengthParserConfiguration() {
        this(new CultureInfo(Culture.Spanish));
    }

    public LengthParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(LengthExtractorConfiguration.LenghtSuffixList);
    }
}
