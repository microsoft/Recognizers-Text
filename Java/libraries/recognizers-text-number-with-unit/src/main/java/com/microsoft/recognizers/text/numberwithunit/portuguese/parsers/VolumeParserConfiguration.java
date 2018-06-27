package com.microsoft.recognizers.text.numberwithunit.portuguese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.portuguese.extractors.VolumeExtractorConfiguration;

public class VolumeParserConfiguration extends PortugueseNumberWithUnitParserConfiguration {

    public VolumeParserConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
    }

    public VolumeParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
    }
}
