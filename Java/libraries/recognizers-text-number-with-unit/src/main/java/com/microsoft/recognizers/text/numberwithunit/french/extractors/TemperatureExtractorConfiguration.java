package com.microsoft.recognizers.text.numberwithunit.french.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.FrenchNumericWithUnit;

import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class TemperatureExtractorConfiguration extends FrenchNumberWithUnitExtractorConfiguration {

    public TemperatureExtractorConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public TemperatureExtractorConfiguration(CultureInfo ci) {
        super(ci);
    }

    @Override
    public String getExtractType() {
        return Constants.SYS_UNIT_TEMPERATURE;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return TemperatureSuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return Collections.emptyMap();
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return Collections.emptyList();
    }

    public static Map<String, String> TemperatureSuffixList = new HashMap(FrenchNumericWithUnit.TemperatureSuffixList);
}