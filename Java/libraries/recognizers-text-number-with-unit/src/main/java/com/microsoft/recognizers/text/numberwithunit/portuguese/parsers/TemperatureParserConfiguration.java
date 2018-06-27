package com.microsoft.recognizers.text.numberwithunit.portuguese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.portuguese.extractors.TemperatureExtractorConfiguration;

public class TemperatureParserConfiguration extends PortugueseNumberWithUnitParserConfiguration {

    public TemperatureParserConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
    }

    public TemperatureParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
    }
}
