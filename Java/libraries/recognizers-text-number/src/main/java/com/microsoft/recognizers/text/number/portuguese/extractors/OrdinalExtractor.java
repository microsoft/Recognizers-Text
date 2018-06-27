package com.microsoft.recognizers.text.number.portuguese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.PortugueseNumeric;

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

        builder.put(Pattern.compile(PortugueseNumeric.OrdinalSuffixRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "OrdinalNum");
        builder.put(Pattern.compile(PortugueseNumeric. OrdinalEnglishRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "OrdinalPor");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
