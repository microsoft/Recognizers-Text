package com.microsoft.recognizers.text.number.french.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.FrenchNumeric;
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
        this(FrenchNumeric.PlaceHolderDefault);
    }

    public DoubleExtractor(String placeholder) {

        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.DoubleDecimalPointRegex(placeholder), Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.DoubleWithoutIntegralRegex(placeholder), Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.DoubleWithMultiplierRegex), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.DoubleWithRoundNumber, Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.DoubleAllFloatRegex, Pattern.UNICODE_CHARACTER_CLASS), "Double" + FrenchNumeric.LangMarker);
        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.DoubleExponentialNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.DoubleCaretExponentialNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumDotComma, placeholder), "DoubleNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumNoBreakSpaceComma, placeholder), "DoubleNum");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
