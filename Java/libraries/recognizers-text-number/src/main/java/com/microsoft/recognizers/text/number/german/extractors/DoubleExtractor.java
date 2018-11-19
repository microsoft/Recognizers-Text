package com.microsoft.recognizers.text.number.german.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.GermanNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class DoubleExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_DOUBLE;
    }

    public DoubleExtractor() {
        this(GermanNumeric.PlaceHolderDefault);
    }

    public DoubleExtractor(String placeholder) {

        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.DoubleDecimalPointRegex(placeholder), Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.DoubleWithoutIntegralRegex(placeholder), Pattern.UNICODE_CHARACTER_CLASS),"DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.DoubleWithMultiplierRegex), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.DoubleWithRoundNumber, Pattern.UNICODE_CHARACTER_CLASS),"DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.DoubleAllFloatRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoubleGer");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.DoubleExponentialNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.DoubleCaretExponentialNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumDotComma, placeholder), "DoubleNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumNoBreakSpaceComma, placeholder), "DoubleNum");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
