package com.microsoft.recognizers.text.number.chinese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberRangeConstants;
import com.microsoft.recognizers.text.number.chinese.ChineseNumberExtractorMode;
import com.microsoft.recognizers.text.number.chinese.parsers.ChineseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.extractors.BaseNumberRangeExtractor;
import com.microsoft.recognizers.text.number.parsers.BaseCJKNumberParser;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class NumberRangeExtractor extends BaseNumberRangeExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUMRANGE;
    }

    public NumberRangeExtractor() {
        this(ChineseNumberExtractorMode.Default);
    }

    public NumberRangeExtractor(ChineseNumberExtractorMode mode) {

        super(new NumberExtractor(), new OrdinalExtractor(), new BaseCJKNumberParser(new ChineseNumberParserConfiguration()));

        HashMap<Pattern, String> builder = new HashMap<>();

        // 在...和...之间
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.TwoNumberRangeRegex1, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.TWONUMBETWEEN);

        // 大于...小于...
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.TwoNumberRangeRegex2, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.TWONUM);

        // 小于...大于...
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.TwoNumberRangeRegex3, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.TWONUM);

        // ...到/至..., 20~30
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.TwoNumberRangeRegex4, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.TWONUMTILL);

        // 大于/多于/高于...
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.OneNumberRangeMoreRegex1, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.MORE);

        // 比...大/高/多
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.OneNumberRangeMoreRegex2, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.MORE);

        // ...多/以上/之上
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.OneNumberRangeMoreRegex3, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.MORE);

        // 小于/少于/低于...
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.OneNumberRangeLessRegex1, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.LESS);

        // 比...小/低/少
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.OneNumberRangeLessRegex2, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.LESS);

        // .../以下/之下
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.OneNumberRangeLessRegex3, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.LESS);

        // 等于...
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.OneNumberRangeEqualRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.EQUAL);


        this.regexes = Collections.unmodifiableMap(builder);
    }
}
