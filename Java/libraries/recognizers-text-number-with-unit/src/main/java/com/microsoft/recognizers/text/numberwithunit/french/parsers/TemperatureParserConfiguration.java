package com.microsoft.recognizers.text.numberwithunit.french.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.french.extractors.TemperatureExtractorConfiguration;

public class TemperatureParserConfiguration extends FrenchNumberWithUnitParserConfiguration {

    public TemperatureParserConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public TemperatureParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(TemperatureExtractorConfiguration.TemperatureSuffixList);
    }

    @Override
    public String getConnectorToken() {
        return null;
    }
}
