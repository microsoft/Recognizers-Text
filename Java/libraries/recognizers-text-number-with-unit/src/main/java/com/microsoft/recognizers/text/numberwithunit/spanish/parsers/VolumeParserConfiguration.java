package com.microsoft.recognizers.text.numberwithunit.spanish.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.spanish.extractors.VolumeExtractorConfiguration;

public class VolumeParserConfiguration extends SpanishNumberWithUnitParserConfiguration {

    public VolumeParserConfiguration() {
        this(new CultureInfo(Culture.Spanish));
    }

    public VolumeParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(VolumeExtractorConfiguration.VolumeSuffixList);
    }
}
