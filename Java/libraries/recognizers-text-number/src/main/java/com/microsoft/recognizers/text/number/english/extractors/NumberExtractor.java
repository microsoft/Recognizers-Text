package com.microsoft.recognizers.text.number.english.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.EnglishNumeric;
import org.javatuples.Pair;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ConcurrentHashMap;
import java.util.regex.Pattern;

public class NumberExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;
    private final NumberOptions options;
    private final Pattern negativeNumberTermsRegex;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM;
    }

    @Override
    protected NumberOptions getOptions() {
        return this.options;
    }

    @Override
    protected Optional<Pattern> getNegativeNumberTermsRegex() {
        return Optional.of(this.negativeNumberTermsRegex);
    }

    private static final ConcurrentHashMap<Pair<NumberMode, NumberOptions>, NumberExtractor> instances = new ConcurrentHashMap<>();

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
        this.options = options;
        this.negativeNumberTermsRegex = Pattern.compile(EnglishNumeric.NegativeNumberTermsRegex + '$', Pattern.CASE_INSENSITIVE);

        HashMap<Pattern, String> builder = new HashMap<>();

        // Add Cardinal
        CardinalExtractor cardinalExtractor = null;
        switch (mode) {
            case PureNumber:
                cardinalExtractor = CardinalExtractor.getInstance(EnglishNumeric.PlaceHolderPureNumber);
                break;
            case Currency:
                builder.put(Pattern.compile(EnglishNumeric.CurrencyRegex, Pattern.CASE_INSENSITIVE), "IntegerNum");
                break;
            case Default:
                break;
        }

        if (cardinalExtractor == null) {
            cardinalExtractor = CardinalExtractor.getInstance();
        }

        builder.putAll(cardinalExtractor.getRegexes());

        // Add Fraction
        FractionExtractor fractionExtractor = FractionExtractor.getInstance(this.options);
        builder.putAll(fractionExtractor.getRegexes());

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
