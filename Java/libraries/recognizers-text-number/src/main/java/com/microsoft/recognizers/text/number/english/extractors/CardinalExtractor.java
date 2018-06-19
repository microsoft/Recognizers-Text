package com.microsoft.recognizers.text.number.english.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.EnglishNumeric;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ConcurrentHashMap;
import java.util.regex.Pattern;

public class CardinalExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_CARDINAL;
    }

    @Override
    protected NumberOptions getOptions() {
        return NumberOptions.None;
    }

    @Override
    protected Optional<Pattern> getNegativeNumberTermsRegex() {
        return Optional.empty();
    }

    private static final ConcurrentHashMap<String, CardinalExtractor> instances = new ConcurrentHashMap<>();

    public static CardinalExtractor getInstance() {
        return getInstance(EnglishNumeric.PlaceHolderDefault);
    }

    public static CardinalExtractor getInstance(String placeholder) {
        if (!instances.containsKey(placeholder)) {
            CardinalExtractor instance = new CardinalExtractor(placeholder);
            instances.put(placeholder, instance);
        }

        return instances.get(placeholder);
    }

    private CardinalExtractor() {
        this(EnglishNumeric.PlaceHolderDefault);
    }

    private CardinalExtractor(String placeholder) {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.putAll(IntegerExtractor.getInstance(placeholder).getRegexes());
        builder.putAll(DoubleExtractor.getInstance(placeholder).getRegexes());

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
