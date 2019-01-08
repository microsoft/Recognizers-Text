package com.microsoft.recognizers.text.number.spanish.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.SpanishNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
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

    public OrdinalExtractor() {

        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.OrdinalSuffixRegex, Pattern.UNICODE_CHARACTER_CLASS), "OrdinalNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.OrdinalNounRegex, Pattern.UNICODE_CHARACTER_CLASS), "OrdinalSpa");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
