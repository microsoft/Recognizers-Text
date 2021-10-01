// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number.french.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.resources.FrenchNumeric;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public class FrenchNumberParserConfiguration extends BaseNumberParserConfiguration {

    public FrenchNumberParserConfiguration() {
        this(NumberOptions.None);
    }

    public FrenchNumberParserConfiguration(NumberOptions options) {
        this(new CultureInfo(Culture.French), options);
    }

    public FrenchNumberParserConfiguration(CultureInfo cultureInfo, NumberOptions options) {
        super(
                FrenchNumeric.LangMarker,
                cultureInfo,
                FrenchNumeric.CompoundNumberLanguage,
                FrenchNumeric.MultiDecimalSeparatorCulture,
                options,
                FrenchNumeric.NonDecimalSeparatorChar,
                FrenchNumeric.DecimalSeparatorChar,
                FrenchNumeric.FractionMarkerToken,
                FrenchNumeric.HalfADozenText,
                FrenchNumeric.WordSeparatorToken,
                FrenchNumeric.WrittenDecimalSeparatorTexts,
                FrenchNumeric.WrittenGroupSeparatorTexts,
                FrenchNumeric.WrittenIntegerSeparatorTexts,
                FrenchNumeric.WrittenFractionSeparatorTexts,
                FrenchNumeric.CardinalNumberMap,
                buildOrdinalNumberMap(),
                FrenchNumeric.RoundNumberMap,

                Pattern.compile(FrenchNumeric.HalfADozenRegex, Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(FrenchNumeric.DigitalNumberRegex, Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(FrenchNumeric.NegativeNumberSignRegex, Pattern.UNICODE_CHARACTER_CLASS),
                Pattern.compile(FrenchNumeric.FractionPrepositionRegex, Pattern.UNICODE_CHARACTER_CLASS));
    }

    @Override
    public List<String> normalizeTokenSet(List<String> tokens, ParseResult context) {
        return new ArrayList<String>(tokens);
    }

    @Override
    public long resolveCompositeNumber(String numberStr) {

        Map<String, Long> ordinalNumberMap = getOrdinalNumberMap();
        Map<String, Long> cardinalNumberMap = getCardinalNumberMap();

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
        ImmutableMap.Builder<String, Long> builder = new ImmutableMap.Builder<String, Long>()
                .putAll(FrenchNumeric.OrdinalNumberMap);

        FrenchNumeric.SuffixOrdinalMap.forEach((suffixKey, suffixValue) ->
                FrenchNumeric.PrefixCardinalMap.forEach((prefixKey, prefixValue) ->
                        builder.put(prefixKey + suffixKey, prefixValue * suffixValue)));

        return builder.build();
    }
}
