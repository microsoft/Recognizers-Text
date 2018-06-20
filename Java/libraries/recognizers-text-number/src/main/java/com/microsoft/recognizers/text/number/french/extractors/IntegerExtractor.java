package com.microsoft.recognizers.text.number.french.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.FrenchNumeric;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class IntegerExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_INTEGER;
    }

    public IntegerExtractor() {
        this(FrenchNumeric.PlaceHolderDefault);
    }

    public IntegerExtractor(String placeholder) {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(Pattern.compile(FrenchNumeric.NumbersWithPlaceHolder(placeholder), Pattern.CASE_INSENSITIVE), "IntegerNum");
        builder.put(Pattern.compile(FrenchNumeric.NumbersWithSuffix), "IntegerNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumDot, placeholder), "IntegerNum");
        builder.put(Pattern.compile(FrenchNumeric.RoundNumberIntegerRegexWithLocks, Pattern.CASE_INSENSITIVE), "IntegerNum");
        builder.put(Pattern.compile(FrenchNumeric.NumbersWithDozenSuffix, Pattern.CASE_INSENSITIVE), "IntegerNum");
        builder.put(Pattern.compile(FrenchNumeric.AllIntRegexWithLocks, Pattern.CASE_INSENSITIVE), "IntegerFr");
        builder.put(Pattern.compile(FrenchNumeric.AllIntRegexWithDozenSuffixLocks, Pattern.CASE_INSENSITIVE), "IntegerFr");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumBlank, placeholder), "IntegerNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumNoBreakSpace, placeholder), "IntegerNum");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
