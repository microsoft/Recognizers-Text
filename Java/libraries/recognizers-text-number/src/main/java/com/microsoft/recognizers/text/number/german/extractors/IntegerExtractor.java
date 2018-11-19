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
        this(GermanNumeric.PlaceHolderDefault);
    }

    public IntegerExtractor(String placeholder) {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.NumbersWithPlaceHolder(placeholder), Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
            "IntegerNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.NumbersWithSuffix), "IntegerNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.RoundNumberIntegerRegexWithLocks, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
            "IntegerNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.NumbersWithDozenSuffix,Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.AllIntRegexWithLocks, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "IntegerGer");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(GermanNumeric.AllIntRegexWithDozenSuffixLocks, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "IntegerGer");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumComma, placeholder), "IntegerNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumBlank, placeholder), "IntegerNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumNoBreakSpace, placeholder), "IntegerNum");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
