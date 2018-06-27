package com.microsoft.recognizers.text.numberwithunit.portuguese.extractors;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.resources.PortugueseNumericWithUnit;

import java.util.List;
import java.util.Map;

public class CurrencyExtractorConfiguration  extends PortugueseNumberWithUnitExtractorConfiguration {

    public CurrencyExtractorConfiguration() {
        this(new CultureInfo(Culture.Portuguese));
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
        return PortugueseNumericWithUnit.AmbiguousCurrencyUnitList;
    }

    @Override
    public Map<String, String> getSuffixList() {
        return CurrencySuffixList;
    }

    @Override
    public Map<String, String> getPrefixList() {
        return CurrencyPrefixList;
    }

    public static Map<String, String> CurrencySuffixList = PortugueseNumericWithUnit.CurrencySuffixList;
    public static Map<String, String> CurrencyPrefixList = PortugueseNumericWithUnit.CurrencyPrefixList;
}
