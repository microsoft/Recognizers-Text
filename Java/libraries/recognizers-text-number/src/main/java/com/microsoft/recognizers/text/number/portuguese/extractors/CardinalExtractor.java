package com.microsoft.recognizers.text.number.portuguese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.PortugueseNumeric;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.regex.Pattern;

public class CardinalExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_CARDINAL;
    }

    private static final ConcurrentHashMap<String, CardinalExtractor> instances = new ConcurrentHashMap<>();

    public static CardinalExtractor getInstance() {
        return getInstance(PortugueseNumeric.PlaceHolderDefault);
    }

    public static CardinalExtractor getInstance(String placeholder) {
        if (!instances.containsKey(placeholder)) {
            CardinalExtractor instance = new CardinalExtractor(placeholder);
            instances.put(placeholder, instance);
        }

        return instances.get(placeholder);
    }

    private CardinalExtractor(String placeholder) {
        HashMap<Pattern, String> builder = new HashMap<>();

        // Add Integer Regexes
        IntegerExtractor intExtract = new IntegerExtractor(placeholder);
        builder.putAll(intExtract.getRegexes());

        // Add Double Regexes
        DoubleExtractor douExtract = new DoubleExtractor(placeholder);
        builder.putAll(douExtract.getRegexes());

        this.regexes = Collections.unmodifiableMap(builder);
    }

}
