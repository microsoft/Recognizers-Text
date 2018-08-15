package com.microsoft.recognizers.text.number.french.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.FrenchNumeric;

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

        builder.put(Pattern.compile(FrenchNumeric.DoubleDecimalPointRegex(placeholder), Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(Pattern.compile(FrenchNumeric.DoubleWithoutIntegralRegex(placeholder), Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(Pattern.compile(FrenchNumeric.DoubleWithMultiplierRegex), "DoubleNum");
        builder.put(Pattern.compile(FrenchNumeric.DoubleWithRoundNumber, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(Pattern.compile(FrenchNumeric.DoubleAllFloatRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "DoubleFr");
        builder.put(Pattern.compile(FrenchNumeric.DoubleExponentialNotationRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        builder.put(Pattern.compile(FrenchNumeric.DoubleCaretExponentialNotationRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumDotComma, placeholder), "DoubleNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumNoBreakSpaceComma, placeholder), "DoubleNum");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
