package com.microsoft.recognizers.text.number.spanish.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.resources.SpanishNumeric;
import com.microsoft.recognizers.text.utilities.FormatUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public class SpanishNumberParserConfiguration extends BaseNumberParserConfiguration {

    public SpanishNumberParserConfiguration() {
        this(NumberOptions.None);
    }

    public SpanishNumberParserConfiguration(NumberOptions options) {
        this(new CultureInfo(Culture.Spanish), options);
    }

    public SpanishNumberParserConfiguration(CultureInfo cultureInfo, NumberOptions options) {
        super(
                SpanishNumeric.LangMarker,
                cultureInfo,
                options,
                SpanishNumeric.NonDecimalSeparatorChar,
                SpanishNumeric.DecimalSeparatorChar,
                SpanishNumeric.FractionMarkerToken,
                SpanishNumeric.HalfADozenText,
                SpanishNumeric.WordSeparatorToken,
                SpanishNumeric.WrittenDecimalSeparatorTexts,
                SpanishNumeric.WrittenGroupSeparatorTexts,
                SpanishNumeric.WrittenIntegerSeparatorTexts,
                SpanishNumeric.WrittenFractionSeparatorTexts,
                SpanishNumeric.CardinalNumberMap,
                buildOrdinalNumberMap(),
                SpanishNumeric.RoundNumberMap,
                Pattern.compile(SpanishNumeric.HalfADozenRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(SpanishNumeric.DigitalNumberRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(SpanishNumeric.NegativeNumberSignRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(SpanishNumeric.FractionPrepositionRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS));
    }

    @Override
    public List<String> normalizeTokenSet(List<String> tokens, ParseResult context) {
        List<String> result = new ArrayList<>();

        for (String token : tokens) {
            String tempWord = FormatUtility.trimEnd(token, "s");
            if (this.getOrdinalNumberMap().containsKey(tempWord)) {
                result.add(tempWord);
                continue;
            }

            if (tempWord.endsWith("avo") || tempWord.endsWith("ava")) {
                String origTempWord = tempWord;
                int newLength = origTempWord.length();
                tempWord = origTempWord.substring(0, newLength - 3);
                if (this.getCardinalNumberMap().containsKey(tempWord)) {
                    result.add(tempWord);
                    continue;
                } else {
                    tempWord = origTempWord.substring(0, newLength - 2);
                    if (this.getCardinalNumberMap().containsKey(tempWord)) {
                        result.add(tempWord);
                        continue;
                    }
                }
            }

            result.add(token);
        }

        return result;
    }

    @Override
    public long resolveCompositeNumber(String numberStr) {
        if (this.getOrdinalNumberMap().containsKey(numberStr)) {
            return this.getOrdinalNumberMap().get(numberStr);
        }

        if (this.getCardinalNumberMap().containsKey(numberStr)) {
            return this.getCardinalNumberMap().get(numberStr);
        }

        long value = 0;
        long finalValue = 0;
        StringBuilder strBuilder = new StringBuilder();
        int lastGoodChar = 0;
        for (int i = 0; i < numberStr.length(); i++) {
            strBuilder.append(numberStr.charAt(i));
            if (this.getCardinalNumberMap().containsKey(strBuilder.toString()) && this.getCardinalNumberMap().get(strBuilder.toString()) > value) {
                lastGoodChar = i;
                value = this.getCardinalNumberMap().get(strBuilder.toString());
            }
            if ((i + 1) == numberStr.length()) {
                finalValue += value;
                strBuilder = new StringBuilder();
                i = lastGoodChar++;
                value = 0;
            }
        }
        return finalValue;
    }

    private static Map<String, Long> buildOrdinalNumberMap() {
        ImmutableMap.Builder ordinalNumberMapBuilder = new ImmutableMap.Builder()
                .putAll(SpanishNumeric.SimpleOrdinalNumberMap);
        SpanishNumeric.SufixOrdinalDictionary.forEach((sufixKey, sufixValue) ->
                SpanishNumeric.PrefixCardinalDictionary.forEach((prefixKey, prefixValue) ->
                        ordinalNumberMapBuilder.put(prefixKey + sufixKey, prefixValue * sufixValue)));

        return ordinalNumberMapBuilder.build();
    }

}
