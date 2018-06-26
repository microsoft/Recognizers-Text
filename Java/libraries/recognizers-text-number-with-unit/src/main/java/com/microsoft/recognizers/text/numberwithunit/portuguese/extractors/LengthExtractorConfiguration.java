package com.microsoft.recognizers.text.numberwithunit.portuguese.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.PortugueseNumericWithUnit;

import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class LengthExtractorConfiguration extends PortugueseNumberWithUnitExtractorConfiguration {

    public LengthExtractorConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
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
        return PortugueseNumericWithUnit.AmbiguousLengthUnitList;
    }

    public static Map<String, String> LenghtSuffixList = PortugueseNumericWithUnit.LenghtSuffixList;
}
