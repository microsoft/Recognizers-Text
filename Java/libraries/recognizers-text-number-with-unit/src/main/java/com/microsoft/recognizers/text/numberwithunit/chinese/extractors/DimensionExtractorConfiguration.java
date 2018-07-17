package com.microsoft.recognizers.text.numberwithunit.chinese.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.ChineseNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class DimensionExtractorConfiguration extends ChineseNumberWithUnitExtractorConfiguration {

    public DimensionExtractorConfiguration() {
        this(new CultureInfo(Culture.Chinese));
    }

    public DimensionExtractorConfiguration(CultureInfo ci) {
        super(ci);
    }

    @Override
    public String getExtractType() {
        return Constants.SYS_UNIT_DIMENSION;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return DimensionSuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return Collections.emptyMap();
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return ChineseNumericWithUnit.DimensionAmbiguousValues;
    }

    public static Map<String, String> DimensionSuffixList = ChineseNumericWithUnit.DimensionSuffixList;
}
