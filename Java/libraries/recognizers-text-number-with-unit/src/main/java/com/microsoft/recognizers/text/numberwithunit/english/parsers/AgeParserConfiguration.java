package com.microsoft.recognizers.text.numberwithunit.english.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.english.extractors.AgeExtractorConfiguration;

public class AgeParserConfiguration extends EnglishNumberWithUnitParserConfiguration {

    public AgeParserConfiguration() {
        this(new CultureInfo(Culture.English));
    }

    public AgeParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(AgeExtractorConfiguration.AgeSuffixList);
    }
}
