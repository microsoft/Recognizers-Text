package com.microsoft.recognizers.text.numberwithunit.portuguese.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.PortugueseNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class WeightExtractorConfiguration extends PortugueseNumberWithUnitExtractorConfiguration {

    public WeightExtractorConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
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
        return PortugueseNumericWithUnit.AmbiguousDimensionUnitList;
    }

    public static Map<String, String> WeightSuffixList = PortugueseNumericWithUnit.WeightSuffixList;
}
