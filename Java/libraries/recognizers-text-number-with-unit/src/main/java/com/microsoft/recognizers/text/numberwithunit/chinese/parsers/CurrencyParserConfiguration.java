package com.microsoft.recognizers.text.numberwithunit.chinese.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.chinese.extractors.CurrencyExtractorConfiguration;
import com.microsoft.recognizers.text.numberwithunit.resources.ChineseNumericWithUnit;

import java.util.Map;

public class CurrencyParserConfiguration extends ChineseNumberWithUnitParserConfiguration {

    @Override
    public Map<String, String> getCurrencyNameToIsoCodeMap() {
        return ChineseNumericWithUnit.CurrencyNameToIsoCodeMap;
    }

    @Override
    public Map<String, String> getCurrencyFractionCodeList() {
        return ChineseNumericWithUnit.FractionalUnitNameToCodeMap;
    }

    public CurrencyParserConfiguration() {
        this(new CultureInfo(Culture.Chinese));
    }

    public CurrencyParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(CurrencyExtractorConfiguration.CurrencySuffixList);
        this.bindDictionary(CurrencyExtractorConfiguration.CurrencyPrefixList);
    }
}
