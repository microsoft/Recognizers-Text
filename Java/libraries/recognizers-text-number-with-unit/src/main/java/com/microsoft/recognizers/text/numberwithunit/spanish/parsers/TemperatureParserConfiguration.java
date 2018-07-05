package com.microsoft.recognizers.text.numberwithunit.spanish.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.spanish.extractors.TemperatureExtractorConfiguration;

public class TemperatureParserConfiguration extends SpanishNumberWithUnitParserConfiguration {

    public TemperatureParserConfiguration() {
        this(new CultureInfo(Culture.Spanish));
    }

    public TemperatureParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
    }
}
