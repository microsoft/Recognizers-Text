package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.number.NumberOptions;

import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public abstract class BaseNumberParserConfiguration implements INumberParserConfiguration {

    private final String langMarker;
    private final CultureInfo cultureInfo;
    private final NumberOptions options;

    private final char nonDecimalSeparatorChar;
    private final char decimalSeparatorChar;
    private final String fractionMarkerToken;
    private final String halfADozenText;
    private final String wordSeparatorToken;

    private final List<String> writtenDecimalSeparatorTexts;
    private final List<String> writtenGroupSeparatorTexts;
    private final List<String> writtenIntegerSeparatorTexts;
    private final List<String> writtenFractionSeparatorTexts;

    private final Map<String,Long> cardinalNumberMap;
    private final Map<String,Long> ordinalNumberMap;
    private final Map<String, Long> roundNumberMap;
    private final Pattern halfADozenRegex;
    private final Pattern digitalNumberRegex;
    private final Pattern negativeNumberSignRegex;
    private final Pattern fractionPrepositionRegex;

    protected BaseNumberParserConfiguration(String langMarker, CultureInfo cultureInfo, NumberOptions options, char nonDecimalSeparatorChar, char decimalSeparatorChar, String fractionMarkerToken, String halfADozenText, String wordSeparatorToken, List<String> writtenDecimalSeparatorTexts, List<String> writtenGroupSeparatorTexts, List<String> writtenIntegerSeparatorTexts, List<String> writtenFractionSeparatorTexts, Map<String, Long> cardinalNumberMap, Map<String, Long> ordinalNumberMap, Map<String, Long> roundNumberMap, Pattern halfADozenRegex, Pattern digitalNumberRegex, Pattern negativeNumberSignRegex, Pattern fractionPrepositionRegex) {
        this.langMarker = langMarker;
        this.cultureInfo = cultureInfo;
        this.options = options;
        this.nonDecimalSeparatorChar = nonDecimalSeparatorChar;
        this.decimalSeparatorChar = decimalSeparatorChar;
        this.fractionMarkerToken = fractionMarkerToken;
        this.halfADozenText = halfADozenText;
        this.wordSeparatorToken = wordSeparatorToken;
        this.writtenDecimalSeparatorTexts = writtenDecimalSeparatorTexts;
        this.writtenGroupSeparatorTexts = writtenGroupSeparatorTexts;
        this.writtenIntegerSeparatorTexts = writtenIntegerSeparatorTexts;
        this.writtenFractionSeparatorTexts = writtenFractionSeparatorTexts;
        this.cardinalNumberMap = cardinalNumberMap;
        this.ordinalNumberMap = ordinalNumberMap;
        this.roundNumberMap = roundNumberMap;
        this.halfADozenRegex = halfADozenRegex;
        this.digitalNumberRegex = digitalNumberRegex;
        this.negativeNumberSignRegex = negativeNumberSignRegex;
        this.fractionPrepositionRegex = fractionPrepositionRegex;
    }

    @Override
    public Map<String, Long> getCardinalNumberMap() { return this.cardinalNumberMap; }

    @Override
    public Map<String, Long> getOrdinalNumberMap() { return this.ordinalNumberMap; }

    @Override
    public Map<String, Long> getRoundNumberMap() { return this.roundNumberMap; }

    @Override
    public NumberOptions getOptions() { return this.options; }

    @Override
    public CultureInfo getCultureInfo() { return this.cultureInfo; }

    @Override
    public Pattern getDigitalNumberRegex() { return this.digitalNumberRegex; }

    @Override
    public Pattern getFractionPrepositionRegex() { return this.fractionPrepositionRegex; }

    @Override
    public String getFractionMarkerToken() { return this.fractionMarkerToken; }

    @Override
    public Pattern getHalfADozenRegex() { return this.halfADozenRegex; }

    @Override
    public String getHalfADozenText() { return this.halfADozenText; }

    @Override
    public String getLangMarker() { return this.langMarker; }

    @Override
    public char getNonDecimalSeparatorChar() { return this.nonDecimalSeparatorChar; }

    @Override
    public char getDecimalSeparatorChar() { return this.decimalSeparatorChar; }

    @Override
    public String getWordSeparatorToken() { return this.wordSeparatorToken; }

    @Override
    public List<String> getWrittenDecimalSeparatorTexts() { return this.writtenDecimalSeparatorTexts; }

    @Override
    public List<String> getWrittenGroupSeparatorTexts() { return this.writtenGroupSeparatorTexts; }

    @Override
    public List<String> getWrittenIntegerSeparatorTexts() { return this.writtenIntegerSeparatorTexts; }

    @Override
    public List<String> getWrittenFractionSeparatorTexts() { return this.writtenFractionSeparatorTexts; }

    @Override
    public Pattern getNegativeNumberSignRegex() { return this.negativeNumberSignRegex; }
}
