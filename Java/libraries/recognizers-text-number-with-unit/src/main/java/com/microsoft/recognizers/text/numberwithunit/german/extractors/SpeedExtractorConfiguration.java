// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.german.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.GermanNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class SpeedExtractorConfiguration extends GermanNumberWithUnitExtractorConfiguration {

    public SpeedExtractorConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public SpeedExtractorConfiguration(CultureInfo ci) {
        super(ci);
    }

    @Override
    public String getExtractType() {
        return Constants.SYS_UNIT_SPEED;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return SpeedSuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return Collections.emptyMap();
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return Collections.emptyList();
    }

    public static Map<String, String> SpeedSuffixList = GermanNumericWithUnit.SpeedSuffixList;
}
