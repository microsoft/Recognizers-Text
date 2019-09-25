package com.microsoft.recognizers.text.number.portuguese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.PortugueseNumeric;
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

        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.FractionNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.FractionNotationWithSpacesRegex, Pattern.UNICODE_CHARACTER_CLASS) , "FracNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.FractionNounRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracPor");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.FractionNounWithArticleRegex, Pattern.UNICODE_CHARACTER_CLASS) , "FracPor");
        if (mode != NumberMode.Unit) {
            builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.FractionPrepositionRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracPor");
        }

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
