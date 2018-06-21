package com.microsoft.recognizers.text.numberwithunit.english.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.numberwithunit.english.extractors.CurrencyExtractorConfiguration;
import com.microsoft.recognizers.text.numberwithunit.resources.EnglishNumericWithUnit;

import java.util.Map;

public class CurrencyParserConfiguration extends EnglishNumberWithUnitParserConfiguration {

    @Override
    public Map<String, String> getCurrencyNameToIsoCodeMap() {
        return EnglishNumericWithUnit.CurrencyNameToIsoCodeMap;
    }

    @Override
    public Map<String, String> getCurrencyFractionCodeList() {
        return EnglishNumericWithUnit.FractionalUnitNameToCodeMap;
    }

    public CurrencyParserConfiguration() {
        this(new CultureInfo(Culture.English));
    }

    public CurrencyParserConfiguration(CultureInfo cultureInfo) {
        super(cultureInfo);

        this.bindDictionary(CurrencyExtractorConfiguration.CurrencySuffixList);
        this.bindDictionary(CurrencyExtractorConfiguration.CurrencyPrefixList);
    }
}
