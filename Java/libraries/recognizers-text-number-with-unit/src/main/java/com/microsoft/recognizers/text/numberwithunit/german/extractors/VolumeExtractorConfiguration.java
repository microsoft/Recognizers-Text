package com.microsoft.recognizers.text.numberwithunit.german.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.GermanNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class VolumeExtractorConfiguration extends GermanNumberWithUnitExtractorConfiguration {

    public VolumeExtractorConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public VolumeExtractorConfiguration(CultureInfo ci) {
        super(ci);
    }

    @Override
    public String getExtractType() {
        return Constants.SYS_UNIT_VOLUME;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return VolumeSuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return Collections.emptyMap();
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return GermanNumericWithUnit.AmbiguousVolumeUnitList;
    }

    public static Map<String, String> VolumeSuffixList = GermanNumericWithUnit.VolumeSuffixList;
}
