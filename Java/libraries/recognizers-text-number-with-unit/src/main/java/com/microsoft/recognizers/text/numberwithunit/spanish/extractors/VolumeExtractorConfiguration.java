package com.microsoft.recognizers.text.numberwithunit.spanish.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.SpanishNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class VolumeExtractorConfiguration extends SpanishNumberWithUnitExtractorConfiguration {

    public VolumeExtractorConfiguration() {
        this(new CultureInfo(Culture.Spanish));
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
        return Collections.emptyList();
    }

    public static Map<String, String> VolumeSuffixList = SpanishNumericWithUnit.VolumeSuffixList;
}
