package com.microsoft.recognizers.text.number.portuguese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.PortugueseNumeric;

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

        builder.put(Pattern.compile(PortugueseNumeric.FractionNotationRegex, Pattern.CASE_INSENSITIVE), "FracNum");
        builder.put(Pattern.compile(PortugueseNumeric.FractionNotationWithSpacesRegex, Pattern.CASE_INSENSITIVE) , "FracNum");
        builder.put(Pattern.compile(PortugueseNumeric.FractionNounRegex, Pattern.CASE_INSENSITIVE), "FracPor");
        builder.put(Pattern.compile(PortugueseNumeric.FractionNounWithArticleRegex, Pattern.CASE_INSENSITIVE) , "FracPor");
        builder.put(Pattern.compile(PortugueseNumeric.FractionPrepositionRegex, Pattern.CASE_INSENSITIVE), "FracPor");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
