package com.microsoft.recognizers.text.number.chinese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.chinese.ChineseNumberExtractorMode;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

import static com.microsoft.recognizers.text.number.chinese.ChineseNumberExtractorMode.*;

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
        builder.put(Pattern.compile(ChineseNumeric.NumbersSpecialsChars, Pattern.CASE_INSENSITIVE), "IntegerNum");
        //15k,  16 G
        builder.put(Pattern.compile(ChineseNumeric.NumbersSpecialsCharsWithSuffix), "IntegerNum");
        //1,234,  ２，３３２，１１１
        builder.put(Pattern.compile(ChineseNumeric.DottedNumbersSpecialsChar, Pattern.CASE_INSENSITIVE), "IntegerNum");
        //半百  半打
        builder.put(Pattern.compile(ChineseNumeric.NumbersWithHalfDozen), "IntegerChs");
        //一打  五十打
        builder.put(Pattern.compile(ChineseNumeric.NumbersWithDozen), "IntegerChs");

        switch (mode)
        {
            case Default:
                // 一百五十五, 负一亿三百二十二.
                // Uses an allow list to avoid extracting "四" from "四川"
                builder.put(Pattern.compile(ChineseNumeric.NumbersWithAllowListRegex), "IntegerChs");
                break;

            case ExtractAll:
                // 一百五十五, 负一亿三百二十二, "四" from "四川".
                // Uses no allow lists and extracts all potential integers (useful in Units, for example).
                builder.put(Pattern.compile(ChineseNumeric.NumbersAggressiveRegex), "IntegerChs");
                break;
        }

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
