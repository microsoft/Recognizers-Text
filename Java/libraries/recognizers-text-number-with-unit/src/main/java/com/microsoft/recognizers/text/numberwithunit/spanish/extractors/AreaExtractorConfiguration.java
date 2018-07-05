package com.microsoft.recognizers.text.numberwithunit.spanish.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.SpanishNumericWithUnit;

import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class AreaExtractorConfiguration extends SpanishNumberWithUnitExtractorConfiguration {

    public AreaExtractorConfiguration() {
        this(new CultureInfo(Culture.Spanish));
    }

    public AreaExtractorConfiguration(CultureInfo ci) {
        super(ci);
    }

    @Override
    public String getExtractType() {
        return Constants.SYS_UNIT_AREA;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return AreaSuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return Collections.emptyMap();
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return Collections.emptyList();
    }

    public static Map<String, String> AreaSuffixList = SpanishNumericWithUnit.AreaSuffixList;
}
