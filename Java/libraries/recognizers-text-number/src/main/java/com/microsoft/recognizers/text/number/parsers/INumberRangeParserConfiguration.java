package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;

import java.util.regex.Pattern;

public interface INumberRangeParserConfiguration {
    CultureInfo getCultureInfo();
    IExtractor getNumberExtractor();
    IExtractor getOrdinalExtractor();
    IParser getNumberParser();
    Pattern getMoreOrEqual();
    Pattern getLessOrEqual();
    Pattern getMoreOrEqualSuffix();
    Pattern getLessOrEqualSuffix();
    Pattern getMoreOrEqualSeparate();
    Pattern getLessOrEqualSeparate();
}
