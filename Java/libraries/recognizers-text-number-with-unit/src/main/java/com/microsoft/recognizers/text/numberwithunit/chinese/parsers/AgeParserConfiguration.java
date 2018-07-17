package com.microsoft.recognizers.text.numberwithunit.chinese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.chinese.extractors.AgeExtractorConfiguration;

public class AgeParserConfiguration extends ChineseNumberWithUnitParserConfiguration {

    public AgeParserConfiguration() {
        this(new CultureInfo(Culture.Chinese));
    }

    public AgeParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(AgeExtractorConfiguration.AgeSuffixList);
    }
}

