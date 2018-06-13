package com.microsoft.recognizers.text.number.german.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.GermanNumeric;

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

        builder.put(Pattern.compile(GermanNumeric.DoubleDecimalPointRegex(placeholder), Pattern.CASE_INSENSITIVE), "DoubleNum");
        builder.put(Pattern.compile(GermanNumeric.DoubleWithoutIntegralRegex(placeholder), Pattern.CASE_INSENSITIVE),"DoubleNum");
        builder.put(Pattern.compile(GermanNumeric.DoubleWithMultiplierRegex), "DoubleNum");
        builder.put(Pattern.compile(GermanNumeric.DoubleWithRoundNumber, Pattern.CASE_INSENSITIVE),"DoubleNum");
        builder.put(Pattern.compile(GermanNumeric.DoubleAllFloatRegex, Pattern.CASE_INSENSITIVE), "DoubleGer");
        builder.put(Pattern.compile(GermanNumeric.DoubleExponentialNotationRegex, Pattern.CASE_INSENSITIVE), "DoublePow");
        builder.put(Pattern.compile(GermanNumeric.DoubleCaretExponentialNotationRegex, Pattern.CASE_INSENSITIVE), "DoublePow");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumDotComma, placeholder), "DoubleNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumNoBreakSpaceComma, placeholder), "DoubleNum");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
