// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number.portuguese.extractors;

import static com.microsoft.recognizers.text.number.NumberMode.Default;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.BaseNumbers;
import com.microsoft.recognizers.text.number.resources.PortugueseNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.regex.Pattern;
import org.javatuples.Pair;

public class NumberExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;
    private final Map<Pattern, Pattern> ambiguityFiltersDict;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected Map<Pattern, Pattern> getAmbiguityFiltersDict() {
        return this.ambiguityFiltersDict;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM;
    }

    private static final ConcurrentHashMap<Pair<NumberMode, NumberOptions>, NumberExtractor> instances = new ConcurrentHashMap<>();

    public static NumberExtractor getInstance(NumberOptions options) {
        return getInstance(NumberMode.Default, options);
    }

    public static NumberExtractor getInstance(NumberMode mode) {
        return getInstance(mode, NumberOptions.None);
    }

    public static NumberExtractor getInstance() {
        return getInstance(NumberMode.Default, NumberOptions.None);
    }

    public static NumberExtractor getInstance(NumberMode mode, NumberOptions options) {
        Pair<NumberMode, NumberOptions> key = Pair.with(mode, options);
        if (!instances.containsKey(key)) {
            NumberExtractor instance = new NumberExtractor(mode, options);
            instances.put(key, instance);
        }

        return instances.get(key);
    }

    private NumberExtractor(NumberMode mode, NumberOptions options) {
        HashMap<Pattern, String> builder = new HashMap<>();

        // Add Cardinal
        CardinalExtractor cardExtract = null;
        switch (mode) {
            case PureNumber:
                cardExtract = CardinalExtractor.getInstance(PortugueseNumeric.PlaceHolderPureNumber);
                break;
            case Currency:
                builder.put(Pattern.compile(BaseNumbers.CurrencyRegex), "IntegerNum");
                break;
            case Default:
                break;
            default:
                break;
        }

        if (cardExtract == null) {
            cardExtract = CardinalExtractor.getInstance();
        }

        builder.putAll(cardExtract.getRegexes());

        // Add Fraction
        FractionExtractor fracExtract = new FractionExtractor(mode);
        builder.putAll(fracExtract.getRegexes());

        this.regexes = Collections.unmodifiableMap(builder);

        HashMap<Pattern, Pattern> ambiguityFiltersDict = new HashMap<>();
        if (mode != NumberMode.Unit) {
            for (Map.Entry<String, String> pair : PortugueseNumeric.AmbiguityFiltersDict.entrySet()) {
                Pattern key = RegExpUtility.getSafeRegExp(pair.getKey());
                Pattern val = RegExpUtility.getSafeRegExp(pair.getValue());
                ambiguityFiltersDict.put(key, val);
            }
        }

        this.ambiguityFiltersDict = ambiguityFiltersDict;
    }
}
