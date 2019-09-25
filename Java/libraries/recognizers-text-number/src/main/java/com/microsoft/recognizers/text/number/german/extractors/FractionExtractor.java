package com.microsoft.recognizers.text.number.german.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.GermanNumeric;
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

        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.FractionNotationWithSpacesRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.FractionNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.FractionNounRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracGer");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.FractionNounWithArticleRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracGer");
        if (mode != NumberMode.Unit) {
            builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.FractionPrepositionRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracGer");
        }

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
