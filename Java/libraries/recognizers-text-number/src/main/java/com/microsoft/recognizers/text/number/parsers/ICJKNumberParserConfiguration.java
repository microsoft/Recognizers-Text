package com.microsoft.recognizers.text.number.parsers;

import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public interface ICJKNumberParserConfiguration extends INumberParserConfiguration {
    //region language dictionaries

    Map<Character, Double> getZeroToNineMap();

    Map<Character, Long> getRoundNumberMapChar();

    Map<Character, Character> getFullToHalfMap();

    Map<String, String> getUnitMap();

    Map<Character, Character> getTratoSimMap();

    //endregion

    //region language lists

    List<Character> getRoundDirectList();

    List<Character> getTenChars();

    //endregion

    //region language settings

    Pattern getFracSplitRegex();

    Pattern getDigitNumRegex();

    Pattern getSpeGetNumberRegex();

    Pattern getPercentageRegex();

    Pattern getPercentageNumRegex();

    Pattern getPointRegex();

    Pattern getDoubleAndRoundRegex();

    Pattern getPairRegex();

    Pattern getDozenRegex();

    Pattern getRoundNumberIntegerRegex();

    char getZeroChar();

    char getPairChar();

    //endregion
}
