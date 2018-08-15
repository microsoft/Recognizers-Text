package com.microsoft.recognizers.text.numberwithunit.german.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.german.extractors.VolumeExtractorConfiguration;

public class VolumeParserConfiguration extends GermanNumberWithUnitParserConfiguration {

    public VolumeParserConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public VolumeParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
    }
}
