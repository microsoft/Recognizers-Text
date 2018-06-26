package com.microsoft.recognizers.text.number.german.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.GermanNumeric;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class FractionExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_FRACTION;
    }

    public FractionExtractor() {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(Pattern.compile(GermanNumeric.FractionNotationWithSpacesRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(Pattern.compile(GermanNumeric.FractionNotationRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(Pattern.compile(GermanNumeric.FractionNounRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracGer");
        builder.put(Pattern.compile(GermanNumeric.FractionNounWithArticleRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracGer");
        builder.put(Pattern.compile(GermanNumeric.FractionPrepositionRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracGer");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
