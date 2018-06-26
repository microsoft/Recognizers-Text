package com.microsoft.recognizers.text.numberwithunit.german.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.german.extractors.SpeedExtractorConfiguration;

public class SpeedParserConfiguration extends GermanNumberWithUnitParserConfiguration {

    public SpeedParserConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public SpeedParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
    }
}
