package com.microsoft.recognizers.text.number.portuguese.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.resources.PortugueseNumeric;
import com.microsoft.recognizers.text.utilities.FormatUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public class PortugueseNumberParserConfiguration extends BaseNumberParserConfiguration {

    public PortugueseNumberParserConfiguration() {
        this(NumberOptions.None);
    }

    public PortugueseNumberParserConfiguration(NumberOptions options) {
        this(new CultureInfo(Culture.Portuguese), options);
    }

    public PortugueseNumberParserConfiguration(CultureInfo cultureInfo, NumberOptions options) {
        super(
                PortugueseNumeric.LangMarker,
                cultureInfo,
                options,
                PortugueseNumeric.NonDecimalSeparatorChar,
                PortugueseNumeric.DecimalSeparatorChar,
                PortugueseNumeric.FractionMarkerToken,
                PortugueseNumeric.HalfADozenText,
                PortugueseNumeric.WordSeparatorToken,
                PortugueseNumeric.WrittenDecimalSeparatorTexts,
                PortugueseNumeric.WrittenGroupSeparatorTexts,
                PortugueseNumeric.WrittenIntegerSeparatorTexts,
                PortugueseNumeric.WrittenFractionSeparatorTexts,
                PortugueseNumeric.CardinalNumberMap,
                buildOrdinalNumberMap(),
                PortugueseNumeric.RoundNumberMap,
                Pattern.compile(PortugueseNumeric.HalfADozenRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(PortugueseNumeric.DigitalNumberRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(PortugueseNumeric.NegativeNumberSignRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(PortugueseNumeric.FractionPrepositionRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS));
    }


    @Override
    public List<String> normalizeTokenSet(List<String> tokens, ParseResult context) {
        Map<String, Long> cardinalNumberMap = this.getCardinalNumberMap();
        Map<String, Long> ordinalNumberMap = this.getOrdinalNumberMap();
        
        List<String> result = new ArrayList<>();

        for (String token : tokens) {
            String tempWord = FormatUtility.trimEnd(token, String.valueOf(PortugueseNumeric.PluralSuffix));
            if (ordinalNumberMap.containsKey(tempWord)) {
                result.add(tempWord);
                continue;
            }

            // ends with 'avo' or 'ava'
            String finalTempWord = tempWord;
            if (PortugueseNumeric.WrittenFractionSuffix.stream().anyMatch(suffix -> finalTempWord.endsWith(suffix))) {
                String origTempWord = tempWord;
                int newLength = origTempWord.length();
                tempWord = origTempWord.substring(0, newLength - 3);

                if (tempWord.isEmpty()) {
                    // Ignore avos in fractions.
                    continue;
                } else if (cardinalNumberMap.containsKey(tempWord)) {
                    result.add(tempWord);
                    continue;
                } else {
                    tempWord = origTempWord.substring(0, newLength - 2);
                    if (cardinalNumberMap.containsKey(tempWord)) {
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
        Map<String, Long> cardinalNumberMap = this.getCardinalNumberMap();
        Map<String, Long> ordinalNumberMap = this.getOrdinalNumberMap();
        if (ordinalNumberMap.containsKey(numberStr)) {
            return ordinalNumberMap.get(numberStr);
        }

        if (cardinalNumberMap.containsKey(numberStr)) {
            return cardinalNumberMap.get(numberStr);
        }

        long value = 0;
        long finalValue = 0;
        StringBuilder strBuilder = new StringBuilder();
        int lastGoodChar = 0;
        for (int i = 0; i < numberStr.length(); i++) {
            strBuilder.append(numberStr.charAt(i));
            String tmp = strBuilder.toString();
            if (cardinalNumberMap.containsKey(tmp) && cardinalNumberMap.get(tmp) > value) {
                lastGoodChar = i;
                value = cardinalNumberMap.get(tmp);
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
        ImmutableMap.Builder builder = new ImmutableMap.Builder()
                .putAll(PortugueseNumeric.OrdinalNumberMap);

        PortugueseNumeric.SuffixOrdinalMap.forEach((sufixKey, sufixValue) ->
                PortugueseNumeric.PrefixCardinalMap.forEach((prefixKey, prefixValue) ->
                        builder.put(prefixKey + sufixKey, prefixValue * sufixValue)));

        return builder.build();
    }
}
