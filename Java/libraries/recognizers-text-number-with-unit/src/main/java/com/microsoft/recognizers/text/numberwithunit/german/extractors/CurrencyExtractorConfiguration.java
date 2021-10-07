// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.german.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.GermanNumericWithUnit;

import java.util.List;
import java.util.Map;

public class CurrencyExtractorConfiguration extends GermanNumberWithUnitExtractorConfiguration {

    public CurrencyExtractorConfiguration() {
        this(new CultureInfo(Culture.German));
    }

    public CurrencyExtractorConfiguration(CultureInfo ci) {
        super(ci);
    }

    @Override
    public String getExtractType() {
        return Constants.SYS_UNIT_CURRENCY;
    }

    @Override
    public List<String> getAmbiguousUnitList() {
        return GermanNumericWithUnit.AmbiguousCurrencyUnitList;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return CurrencySuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return CurrencyPrefixList;
    }

    public static Map<String, String> CurrencySuffixList = GermanNumericWithUnit.CurrencySuffixList;
    public static Map<String, String> CurrencyPrefixList = GermanNumericWithUnit.CurrencyPrefixList;
}
