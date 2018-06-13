package com.microsoft.recognizers.text.number.french.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.FrenchNumeric;

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
        builder.put(Pattern.compile(FrenchNumeric.FractionNotationWithSpacesRegex, Pattern.CASE_INSENSITIVE), "FracNum");
        builder.put(Pattern.compile(FrenchNumeric.FractionNotationRegex, Pattern.CASE_INSENSITIVE), "FracNum");
        builder.put(Pattern.compile(FrenchNumeric.FractionNounRegex, Pattern.CASE_INSENSITIVE), "FracFr");
        builder.put(Pattern.compile(FrenchNumeric.FractionNounWithArticleRegex, Pattern.CASE_INSENSITIVE), "FracFr");
        builder.put(Pattern.compile(FrenchNumeric.FractionPrepositionRegex, Pattern.CASE_INSENSITIVE), "FracFr");
        this.regexes = Collections.unmodifiableMap(builder);
    }
}
