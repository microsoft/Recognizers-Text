package com.microsoft.recognizers.text.number.german.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.resources.GermanNumeric;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public class GermanNumberParserConfiguration extends BaseNumberParserConfiguration {

    public GermanNumberParserConfiguration() {
        this(NumberOptions.None);
    }

    public GermanNumberParserConfiguration(NumberOptions options) {
        this(new CultureInfo(Culture.German), options);
    }

    public GermanNumberParserConfiguration(CultureInfo cultureInfo, NumberOptions options) {
        super(
                GermanNumeric.LangMarker,
                cultureInfo,
                options,
                GermanNumeric.NonDecimalSeparatorChar,
                GermanNumeric.DecimalSeparatorChar,
                GermanNumeric.FractionMarkerToken,
                GermanNumeric.HalfADozenText,
                GermanNumeric.WordSeparatorToken,
                GermanNumeric.WrittenDecimalSeparatorTexts,
                GermanNumeric.WrittenGroupSeparatorTexts,
                GermanNumeric.WrittenIntegerSeparatorTexts,
                GermanNumeric.WrittenFractionSeparatorTexts,
                GermanNumeric.CardinalNumberMap,
                GermanNumeric.OrdinalNumberMap,
                GermanNumeric.RoundNumberMap,
                Pattern.compile(GermanNumeric.HalfADozenRegex, Pattern.CASE_INSENSITIVE),
                Pattern.compile(GermanNumeric.DigitalNumberRegex, Pattern.CASE_INSENSITIVE),
                Pattern.compile(GermanNumeric.NegativeNumberSignRegex, Pattern.CASE_INSENSITIVE),
                Pattern.compile(GermanNumeric.FractionPrepositionRegex, Pattern.CASE_INSENSITIVE));
    }


    @Override
    public List<String> normalizeTokenSet(List<String> tokens, ParseResult context) {
        List<String> fracWords = new ArrayList<>();
        List<String> tokenList = new ArrayList<>(tokens);
        int tokenLen = tokenList.size();

        for (int i = 0; i < tokenLen; i++) {
            if (tokenList.get(i).contains("-")) {
                String[] splitedTokens = tokenList.get(i).split(Pattern.quote("-"));
                if (splitedTokens.length == 2 && getOrdinalNumberMap().containsKey(splitedTokens[1])) {
                    fracWords.add(splitedTokens[0]);
                    fracWords.add(splitedTokens[1]);
                } else {
                    fracWords.add(tokenList.get(i));
                }
            } else if (i < tokenLen - 2 && tokenList.get(i + 1).equals("-")) {
                if (getOrdinalNumberMap().containsKey(tokenList.get(i + 2))) {
                    fracWords.add(tokenList.get(i));
                    fracWords.add(tokenList.get(i + 2));
                } else {
                    fracWords.add(tokenList.get(i) + tokenList.get(i + 1) + tokenList.get(i + 2));
                }

                i += 2;
            } else {
                fracWords.add(tokenList.get(i));
            }
        }

        return fracWords;
    }

    @Override
    public long resolveCompositeNumber(String numberStr) {

        Map<String, Long> ordinalNumberMap = getOrdinalNumberMap();
        Map<String, Long> cardinalNumberMap = getCardinalNumberMap();

        if (numberStr.contains("-")) {
            String[] numbers = numberStr.split(Pattern.quote("-"));
            long ret = 0;
            for (String number : numbers) {
                if (ordinalNumberMap.containsKey(number)) {
                    ret += ordinalNumberMap.get(number);
                } else if (cardinalNumberMap.containsKey(number)) {
                    ret += cardinalNumberMap.get(number);
                }
            }

            return ret;
        }

        if (ordinalNumberMap.containsKey(numberStr)) {
            return ordinalNumberMap.get(numberStr);
        }

        if (cardinalNumberMap.containsKey(numberStr)) {
            return cardinalNumberMap.get(numberStr);
        }

        return 0;
    }
}
