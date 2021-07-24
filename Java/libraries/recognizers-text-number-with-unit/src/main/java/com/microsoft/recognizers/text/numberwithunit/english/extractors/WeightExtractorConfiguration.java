// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.english.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.EnglishNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class WeightExtractorConfiguration extends EnglishNumberWithUnitExtractorConfiguration {

    public WeightExtractorConfiguration() {
        this(new CultureInfo(Culture.English));
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
        return EnglishNumericWithUnit.AmbiguousWeightUnitList;
    }

    public static Map<String, String> WeightSuffixList = EnglishNumericWithUnit.WeightSuffixList;
}
