package com.microsoft.recognizers.text.numberwithunit.french.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.french.extractors.VolumeExtractorConfiguration;

public class VolumeParserConfiguration extends FrenchNumberWithUnitParserConfiguration {

    public VolumeParserConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public VolumeParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
    }
}
