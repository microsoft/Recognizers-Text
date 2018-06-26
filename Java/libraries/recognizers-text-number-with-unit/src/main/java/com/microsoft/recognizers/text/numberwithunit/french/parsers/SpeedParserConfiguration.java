package com.microsoft.recognizers.text.numberwithunit.french.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.french.extractors.SpeedExtractorConfiguration;

public class SpeedParserConfiguration extends FrenchNumberWithUnitParserConfiguration {

    public SpeedParserConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public SpeedParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
    }
}
