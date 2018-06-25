package com.microsoft.recognizers.text.numberwithunit.english.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.EnglishNumericWithUnit;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class CurrencyExtractorConfiguration extends EnglishNumberWithUnitExtractorConfiguration {

    public CurrencyExtractorConfiguration() {
        this(new CultureInfo(Culture.English));
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
        return EnglishNumericWithUnit.AmbiguousCurrencyUnitList;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return CurrencySuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return CurrencyPrefixList;
    }

    public static Map<String, String> CurrencySuffixList = EnglishNumericWithUnit.CurrencySuffixList;
    public static Map<String, String> CurrencyPrefixList = EnglishNumericWithUnit.CurrencyPrefixList;
}
