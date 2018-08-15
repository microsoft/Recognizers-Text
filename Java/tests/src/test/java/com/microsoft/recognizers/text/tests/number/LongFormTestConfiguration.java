package com.microsoft.recognizers.text.tests.number;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.parsers.INumberParserConfiguration;

import java.util.Collections;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public class LongFormTestConfiguration implements INumberParserConfiguration {

    private final char nonDecimalSep;
    private final char decimalSep;

    public LongFormTestConfiguration(char decimalSep, char nonDecimalSep) {
        this.decimalSep = decimalSep;
        this.nonDecimalSep = nonDecimalSep;
    }

    @Override
    public Map<String, Long> getCardinalNumberMap() {
        return Collections.emptyMap();
    }

    @Override
    public Map<String, Long> getOrdinalNumberMap() {
        return Collections.emptyMap();
    }

    @Override
    public Map<String, Long> getRoundNumberMap() {
        return Collections.emptyMap();
    }

    @Override
    public NumberOptions getOptions() {
        return null;
    }

    @Override
    public CultureInfo getCultureInfo() {
        return new CultureInfo(Culture.English);
    }

    @Override
    public Pattern getDigitalNumberRegex() {
        return Pattern.compile("((?<=\\b)(hundred|thousand|million|billion|trillion|dozen(s)?)(?=\\b))|((?<=(\\d|\\b))(k|t|m|g|b)(?=\\b))", Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
    }

    @Override
    public Pattern getFractionPrepositionRegex() {
        return null;
    }

    @Override
    public String getFractionMarkerToken() {
        return null;
    }

    @Override
    public Pattern getHalfADozenRegex() {
        return null;
    }

    @Override
    public String getHalfADozenText() {
        return null;
    }

    @Override
    public String getLangMarker() {
        return "SelfDefined";
    }

    @Override
    public char getNonDecimalSeparatorChar() {
        return this.nonDecimalSep;
    }

    @Override
    public char getDecimalSeparatorChar() {
        return this.decimalSep;
    }

    @Override
    public String getWordSeparatorToken() {
        return null;
    }

    @Override
    public List<String> getWrittenDecimalSeparatorTexts() {
        return null;
    }

    @Override
    public List<String> getWrittenGroupSeparatorTexts() {
        return null;
    }

    @Override
    public List<String> getWrittenIntegerSeparatorTexts() {
        return null;
    }

    @Override
    public List<String> getWrittenFractionSeparatorTexts() {
        return null;
    }

    @Override
    public Pattern getNegativeNumberSignRegex() {
        return Pattern.compile("[^\\s\\S]");
    }

    @Override
    public List<String> normalizeTokenSet(List<String> tokens, ParseResult context) {
        return null;
    }

    @Override
    public long resolveCompositeNumber(String numberStr) {
        return 0;
    }
}