package com.microsoft.recognizers.text.numberwithunit.french.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.FrenchNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class WeightExtractorConfiguration extends FrenchNumberWithUnitExtractorConfiguration {

    public WeightExtractorConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public WeightExtractorConfiguration(CultureInfo ci) {
        super(ci);
    }

    @Override
    public String getExtractType() {
        return Constants.SYS_UNIT_WEIGHT;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return WeightSuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return Collections.emptyMap();
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return FrenchNumericWithUnit.AmbiguousDimensionUnitList;
    }

    public static Map<String, String> WeightSuffixList = FrenchNumericWithUnit.WeightSuffixList;
}
