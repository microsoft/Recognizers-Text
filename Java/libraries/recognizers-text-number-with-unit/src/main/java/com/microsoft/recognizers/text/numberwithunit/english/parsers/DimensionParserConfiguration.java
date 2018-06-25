package com.microsoft.recognizers.text.numberwithunit.english.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.english.extractors.DimensionExtractorConfiguration;

public class DimensionParserConfiguration extends EnglishNumberWithUnitParserConfiguration {

    public DimensionParserConfiguration() {
        this(new CultureInfo(Culture.English));
    }

    public DimensionParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
    }
}
