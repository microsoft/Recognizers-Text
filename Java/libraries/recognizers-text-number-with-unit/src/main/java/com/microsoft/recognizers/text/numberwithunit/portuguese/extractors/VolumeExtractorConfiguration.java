package com.microsoft.recognizers.text.numberwithunit.portuguese.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.PortugueseNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class VolumeExtractorConfiguration extends PortugueseNumberWithUnitExtractorConfiguration {

    public VolumeExtractorConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
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

    public static Map<String, String> VolumeSuffixList = PortugueseNumericWithUnit.VolumeSuffixList;
}
