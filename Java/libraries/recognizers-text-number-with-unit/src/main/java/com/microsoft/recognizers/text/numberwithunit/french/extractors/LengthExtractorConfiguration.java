package com.microsoft.recognizers.text.numberwithunit.french.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.FrenchNumericWithUnit;

import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class LengthExtractorConfiguration extends FrenchNumberWithUnitExtractorConfiguration {

    public LengthExtractorConfiguration() {
        this(new CultureInfo(Culture.French));
    }

    public LengthExtractorConfiguration(CultureInfo ci) {
        super(ci);
    }

    @Override
    public String getExtractType() {
        return Constants.SYS_UNIT_LENGTH;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return LenghtSuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return Collections.emptyMap();
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return FrenchNumericWithUnit.AmbiguousLengthUnitList;
    }

    public static Map<String, String> LenghtSuffixList = FrenchNumericWithUnit.LengthSuffixList;
}
