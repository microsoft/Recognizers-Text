package com.microsoft.recognizers.text.numberwithunit.parsers;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;

import java.util.Map;

public interface INumberWithUnitParserConfiguration {

    Map<String, String> getUnitMap();
    Map<String, Long> getCurrencyFractionNumMap();
    Map<String, String> getCurrencyFractionMapping();

    Map<String, String> getCurrencyNameToIsoCodeMap();
    Map<String, String> getCurrencyFractionCodeList();

    CultureInfo getCultureInfo();
    IParser getInternalNumberParser();
    IExtractor getInternalNumberExtractor();
    String getConnectorToken();

    void bindDictionary(Map<String, String> dictionary);
}
