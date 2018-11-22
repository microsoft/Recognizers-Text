package com.microsoft.recognizers.text.number.spanish.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.SpanishNumeric;
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
        this(SpanishNumeric.PlaceHolderDefault);
    }

    public DoubleExtractor(String placeholder) {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.DoubleDecimalPointRegex(placeholder), Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.DoubleWithoutIntegralRegex(placeholder), Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.DoubleWithMultiplierRegex), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.DoubleWithRoundNumber, Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.DoubleAllFloatRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoubleSpa");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.DoubleExponentialNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(SpanishNumeric.DoubleCaretExponentialNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumDotComma, placeholder), "DoubleNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumNoBreakSpaceComma, placeholder), "DoubleNum");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
