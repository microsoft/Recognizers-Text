package com.microsoft.recognizers.text.numberwithunit.french.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.french.extractors.LengthExtractorConfiguration;

public class LengthParserConfiguration extends FrenchNumberWithUnitParserConfiguration {

    public LengthParserConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public LengthParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(LengthExtractorConfiguration.LenghtSuffixList);
    }
}
