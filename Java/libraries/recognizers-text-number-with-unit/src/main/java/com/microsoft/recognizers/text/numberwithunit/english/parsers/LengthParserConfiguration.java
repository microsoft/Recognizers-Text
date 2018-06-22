package com.microsoft.recognizers.text.numberwithunit.english.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.english.extractors.LengthExtractorConfiguration;

public class LengthParserConfiguration extends EnglishNumberWithUnitParserConfiguration {

    public LengthParserConfiguration() {
        this(new CultureInfo(Culture.English));
    }

    public LengthParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(LengthExtractorConfiguration.LenghtSuffixList);
    }
}
