// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.spanish.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.SpanishNumericWithUnit;

import java.util.Collections;
import java.util.List;
import java.util.Map;

public class LengthExtractorConfiguration extends SpanishNumberWithUnitExtractorConfiguration {

    public LengthExtractorConfiguration() {
        this(new CultureInfo(Culture.Spanish));
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
        return LengthSuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return Collections.emptyMap();
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return SpanishNumericWithUnit.AmbiguousLengthUnitList;
    }

    public static Map<String, String> LengthSuffixList = SpanishNumericWithUnit.LengthSuffixList;
}
