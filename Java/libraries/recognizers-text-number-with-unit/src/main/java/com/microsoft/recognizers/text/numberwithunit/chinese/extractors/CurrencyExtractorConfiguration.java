package com.microsoft.recognizers.text.numberwithunit.chinese.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.ChineseNumericWithUnit;

import java.util.List;
import java.util.Map;

public class CurrencyExtractorConfiguration extends ChineseNumberWithUnitExtractorConfiguration {

    public CurrencyExtractorConfiguration() {
        this(new CultureInfo(Culture.Chinese));
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
        return ChineseNumericWithUnit.CurrencyAmbiguousValues;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return CurrencySuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return CurrencyPrefixList;
    }

    public static Map<String, String> CurrencySuffixList = ChineseNumericWithUnit.CurrencySuffixList;
    public static Map<String, String> CurrencyPrefixList = ChineseNumericWithUnit.CurrencyPrefixList;
}
