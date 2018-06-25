package com.microsoft.recognizers.text.numberwithunit.parsers;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.numberwithunit.resources.BaseCurrency;
import com.microsoft.recognizers.text.numberwithunit.utilities.DictionaryUtils;

import java.util.HashMap;
import java.util.Map;

public abstract class BaseNumberWithUnitParserConfiguration implements INumberWithUnitParserConfiguration {
    private final Map<String, String> unitMap;
    private final Map<String, Long> currencyFractionNumMap;
    private final Map<String, String> currencyFractionMapping;
    private final CultureInfo cultureInfo;
    private final Map<String, String> currencyNameToIsoCodeMap;
    private final Map<String, String> currencyFractionCodeList;

    @Override
    public Map<String, String> getUnitMap() {
        return this.unitMap;
    }

    @Override
    public Map<String, Long> getCurrencyFractionNumMap() {
        return this.currencyFractionNumMap;
    }

    @Override
    public Map<String, String> getCurrencyFractionMapping() {
        return this.currencyFractionMapping;
    }

    @Override
    public Map<String, String> getCurrencyNameToIsoCodeMap() {
        return this.currencyNameToIsoCodeMap;
    }

    @Override
    public Map<String, String> getCurrencyFractionCodeList() {
        return this.currencyFractionCodeList;
    }

    @Override
    public CultureInfo getCultureInfo() {
        return this.cultureInfo;
    }

    @Override
    public abstract IParser getInternalNumberParser();

    @Override
    public abstract IExtractor getInternalNumberExtractor();

    @Override
    public abstract String getConnectorToken();

    protected BaseNumberWithUnitParserConfiguration(CultureInfo ci) {
        this.cultureInfo = ci;
        this.unitMap = new HashMap<>();
        this.currencyFractionNumMap = BaseCurrency.CurrencyFractionalRatios;
        this.currencyFractionMapping = BaseCurrency.CurrencyFractionMapping;
        this.currencyNameToIsoCodeMap = new HashMap<>();
        this.currencyFractionCodeList = new HashMap<>();
    }

    @Override
    public void bindDictionary(Map<String, String> dictionary) {
        DictionaryUtils.bindDictionary(dictionary, getUnitMap());
    }
}
