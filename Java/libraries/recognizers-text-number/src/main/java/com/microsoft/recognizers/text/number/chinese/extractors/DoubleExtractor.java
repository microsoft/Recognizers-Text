package com.microsoft.recognizers.text.number.chinese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;
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
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(Pattern.compile(ChineseNumeric.DoubleSpecialsChars, Pattern.CASE_INSENSITIVE  | Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        // (-)2.5, can avoid cases like ip address xx.xx.xx.xx
        builder.put(Pattern.compile(ChineseNumeric.DoubleSpecialsCharsWithNegatives, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        //(-).2
        builder.put(Pattern.compile(ChineseNumeric.SimpleDoubleSpecialsChars, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        // 1.0 K
        builder.put(Pattern.compile(ChineseNumeric.DoubleWithMultiplierRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        //１５.２万
        builder.put(Pattern.compile(ChineseNumeric.DoubleWithThousandsRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoubleChs");
        //四十五点三三
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.DoubleAllFloatRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoubleChs");
        // 2e6, 21.2e0
        builder.put(Pattern.compile(ChineseNumeric.DoubleExponentialNotationRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        //2^5
        builder.put(Pattern.compile(ChineseNumeric.DoubleScientificNotationRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
