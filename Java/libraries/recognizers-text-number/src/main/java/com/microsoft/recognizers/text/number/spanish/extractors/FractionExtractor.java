package com.microsoft.recognizers.text.number.spanish.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.SpanishNumeric;
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

        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.FractionNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.FractionNotationWithSpacesRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.FractionNounRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracSpa");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.FractionNounWithArticleRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracSpa");
        if (mode != NumberMode.Unit) {
            builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.FractionPrepositionRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracSpa");
        }

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
