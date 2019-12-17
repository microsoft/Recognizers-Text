package com.microsoft.recognizers.text.number.chinese.extractors;

import static com.microsoft.recognizers.text.number.chinese.ChineseNumberExtractorMode.Default;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.chinese.ChineseNumberExtractorMode;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;
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
        this(Default);
    }

    public IntegerExtractor(ChineseNumberExtractorMode mode) {
        HashMap<Pattern, String> builder = new HashMap<>();

        // 123456,  －１２３４５６
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.NumbersSpecialsChars, Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        //15k,  16 G
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.NumbersSpecialsCharsWithSuffix, Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        //1,234,  ２，３３２，１１１
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.DottedNumbersSpecialsChar, Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        //半百  半打
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.NumbersWithHalfDozen, Pattern.UNICODE_CHARACTER_CLASS), "Integer" + ChineseNumeric.LangMarker);
        //一打  五十打
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.NumbersWithDozen, Pattern.UNICODE_CHARACTER_CLASS), "Integer" + ChineseNumeric.LangMarker);

        switch (mode) {
            case Default:
                // 一百五十五, 负一亿三百二十二.
                // Uses an allow list to avoid extracting "四" from "四川"
                builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersWithAllowListRegex, Pattern.UNICODE_CHARACTER_CLASS), "Integer" + ChineseNumeric.LangMarker);
                break;

            case ExtractAll:
                // 一百五十五, 负一亿三百二十二, "四" from "四川".
                // Uses no allow lists and extracts all potential integers (useful in Units, for example).
                builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersAggressiveRegex, Pattern.UNICODE_CHARACTER_CLASS), "Integer" + ChineseNumeric.LangMarker);
                break;
            
            default:
                break;
        }

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
