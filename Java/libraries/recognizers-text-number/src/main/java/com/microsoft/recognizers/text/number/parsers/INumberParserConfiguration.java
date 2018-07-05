package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberOptions;

import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public interface INumberParserConfiguration {

    //region language dictionaries

    Map<String, Long> getCardinalNumberMap();

    Map<String, Long> getOrdinalNumberMap();

    Map<String, Long> getRoundNumberMap();

    //endregion

    //region language settings

    NumberOptions getOptions();

    CultureInfo getCultureInfo();

    Pattern getDigitalNumberRegex();

    Pattern getFractionPrepositionRegex();

    String getFractionMarkerToken();

    Pattern getHalfADozenRegex();

    String getHalfADozenText();

    String getLangMarker();

    char getNonDecimalSeparatorChar();

    char getDecimalSeparatorChar();

    String getWordSeparatorToken();

    List<String> getWrittenDecimalSeparatorTexts();

    List<String> getWrittenGroupSeparatorTexts();

    List<String> getWrittenIntegerSeparatorTexts();

    List<String> getWrittenFractionSeparatorTexts();

    Pattern getNegativeNumberSignRegex();

    //endregion

    /**
     * Used when requiring to normalize a token to a valid expression supported by the ImmutableDictionaries (language dictionaries)
     *
     * @param tokens  list of tokens to normalize
     * @param context context of the call
     * @return list of normalized tokens
     */
    List<String> normalizeTokenSet(List<String> tokens, ParseResult context);

    /**
     * Used when requiring to convert a String to a valid number supported by the language
     *
     * @param numberStr composite number
     * @return value of the String
     */
    long resolveCompositeNumber(String numberStr);
}
