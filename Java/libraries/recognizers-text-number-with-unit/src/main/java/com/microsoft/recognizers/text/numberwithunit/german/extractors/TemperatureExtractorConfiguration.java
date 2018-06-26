package com.microsoft.recognizers.text.numberwithunit.german.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.GermanNumericWithUnit;

import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class TemperatureExtractorConfiguration extends GermanNumberWithUnitExtractorConfiguration {

    public TemperatureExtractorConfiguration() {
        this(new CultureInfo(Culture.German));
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
        return GermanNumericWithUnit.AmbiguousTemperatureUnitList;
    }

    public static Map<String, String> TemperatureSuffixList = new HashMap(GermanNumericWithUnit.TemperatureSuffixList);
}
