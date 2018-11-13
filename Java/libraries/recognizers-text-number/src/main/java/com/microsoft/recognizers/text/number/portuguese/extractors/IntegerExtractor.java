package com.microsoft.recognizers.text.number.portuguese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.PortugueseNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

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
        this(PortugueseNumeric.PlaceHolderDefault);
    }

    public IntegerExtractor(String placeholder) {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.NumbersWithPlaceHolder(placeholder), Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.NumbersWithSuffix), "IntegerNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumDot, placeholder), "IntegerNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumBlank, placeholder), "IntegerNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumNoBreakSpace, placeholder), "IntegerNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.RoundNumberIntegerRegexWithLocks, Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.NumbersWithDozen2Suffix, Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.NumbersWithDozenSuffix, Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.AllIntRegexWithLocks, Pattern.UNICODE_CHARACTER_CLASS), "IntegerPor");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.AllIntRegexWithDozenSuffixLocks, Pattern.UNICODE_CHARACTER_CLASS), "IntegerPor");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
