package com.microsoft.recognizers.text.numberwithunit.french.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.french.extractors.AreaExtractorConfiguration;

public class AreaParserConfiguration extends FrenchNumberWithUnitParserConfiguration {

    public AreaParserConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public AreaParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(AreaExtractorConfiguration.AreaSuffixList);
    }
}
