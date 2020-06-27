package com.microsoft.recognizers.text.number.french.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.FrenchNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

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

    public FractionExtractor(NumberMode mode) {

        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.FractionNotationWithSpacesRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.FractionNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.FractionNounRegex, Pattern.UNICODE_CHARACTER_CLASS), "Frac" + FrenchNumeric.LangMarker);
        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.FractionNounWithArticleRegex, Pattern.UNICODE_CHARACTER_CLASS), "Frac" + FrenchNumeric.LangMarker);
        if (mode != NumberMode.Unit) {
            builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.FractionPrepositionRegex, Pattern.UNICODE_CHARACTER_CLASS), "Frac" + FrenchNumeric.LangMarker);
        }

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
