package com.microsoft.recognizers.text.number.portuguese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.PortugueseNumeric;

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
        this(PortugueseNumeric.PlaceHolderDefault);
    }

    public DoubleExtractor(String placeholder) {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(Pattern.compile(PortugueseNumeric.DoubleDecimalPointRegex(placeholder), Pattern.CASE_INSENSITIVE), "DoubleNum");
        builder.put(Pattern.compile(PortugueseNumeric.DoubleWithoutIntegralRegex(placeholder), Pattern.CASE_INSENSITIVE), "DoubleNum");
        builder.put(Pattern.compile(PortugueseNumeric.DoubleWithMultiplierRegex), "DoubleNum");
        builder.put(Pattern.compile(PortugueseNumeric.DoubleWithRoundNumber, Pattern.CASE_INSENSITIVE), "DoubleNum");
        builder.put(Pattern.compile(PortugueseNumeric.DoubleAllFloatRegex, Pattern.CASE_INSENSITIVE), "DoublePor");
        builder.put(Pattern.compile(PortugueseNumeric.DoubleExponentialNotationRegex, Pattern.CASE_INSENSITIVE), "DoublePow");
        builder.put(Pattern.compile(PortugueseNumeric.DoubleCaretExponentialNotationRegex, Pattern.CASE_INSENSITIVE), "DoublePow");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumDotComma, placeholder), "DoubleNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumNoBreakSpaceComma, placeholder), "DoubleNum");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
