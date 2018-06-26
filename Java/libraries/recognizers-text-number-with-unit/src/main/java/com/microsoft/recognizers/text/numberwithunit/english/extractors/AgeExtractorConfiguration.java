package com.microsoft.recognizers.text.numberwithunit.english.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.EnglishNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class AgeExtractorConfiguration extends EnglishNumberWithUnitExtractorConfiguration {

    public AgeExtractorConfiguration() {
        this(new CultureInfo(Culture.English));
    }

    public AgeExtractorConfiguration(CultureInfo ci) {
        super(ci);
    }

    @Override
    public String getExtractType() {
        return Constants.SYS_UNIT_AGE;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return AgeSuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return Collections.emptyMap();
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return Collections.emptyList();
    }

    public static Map<String, String> AgeSuffixList = EnglishNumericWithUnit.AgeSuffixList;
}
