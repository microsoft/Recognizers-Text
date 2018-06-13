package com.microsoft.recognizers.text.number.german.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.GermanNumeric;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
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

    public OrdinalExtractor() {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(Pattern.compile(GermanNumeric.OrdinalSuffixRegex, Pattern.CASE_INSENSITIVE), "OrdinalNum");
        builder.put(Pattern.compile(GermanNumeric.OrdinalNumericRegex, Pattern.CASE_INSENSITIVE), "OrdinalNum");
        builder.put(Pattern.compile(GermanNumeric.OrdinalGermanRegex, Pattern.CASE_INSENSITIVE), "OrdGer");
        builder.put(Pattern.compile(GermanNumeric.OrdinalRoundNumberRegex, Pattern.CASE_INSENSITIVE), "OrdGer");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
