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

public class OrdinalExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_ORDINAL;
    }

    @Override
    protected NumberOptions getOptions() {
        return NumberOptions.None;
    }

    @Override
    protected Optional<Pattern> getNegativeNumberTermsRegex() {
        return Optional.empty();
    }

    private static final ConcurrentHashMap<String, OrdinalExtractor> instances = new ConcurrentHashMap<>();

    public static OrdinalExtractor getInstance() {
        return getInstance("");
    }

    private static OrdinalExtractor getInstance(String placeholder) {
        if (!instances.containsKey(placeholder)) {
            OrdinalExtractor instance = new OrdinalExtractor();
            instances.put(placeholder, instance);
        }

        return instances.get(placeholder);
    }

    private OrdinalExtractor() {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(Pattern.compile(EnglishNumeric.OrdinalSuffixRegex, Pattern.CASE_INSENSITIVE), "OrdinalNum");
        builder.put(Pattern.compile(EnglishNumeric.OrdinalNumericRegex, Pattern.CASE_INSENSITIVE), "OrdinalNum");
        builder.put(Pattern.compile(EnglishNumeric.OrdinalEnglishRegex, Pattern.CASE_INSENSITIVE), "OrdEng");
        builder.put(Pattern.compile(EnglishNumeric.OrdinalRoundNumberRegex, Pattern.CASE_INSENSITIVE), "OrdEng");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
