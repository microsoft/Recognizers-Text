package com.microsoft.recognizers.text.number.chinese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class PercentageExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }


    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_PERCENTAGE;
    }

    public PercentageExtractor() {
        HashMap<Pattern, String> builder = new HashMap<>();

        //二十个百分点, 四点五个百分点
        builder.put(RegExpUtility.getSafeRegExp(ChineseNumeric.PercentagePointRegex, Pattern.UNICODE_CHARACTER_CLASS), "Per" + ChineseNumeric.LangMarker);

        //百分之五十  百分之一点五
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.SimplePercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "Per" + ChineseNumeric.LangMarker);

        //百分之５６.２　百分之１２
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.NumbersPercentagePointRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //百分之3,000  百分之１，１２３
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.NumbersPercentageWithSeparatorRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //百分之3.2 k
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.NumbersPercentageWithMultiplierRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //12.56个百分点  ０.４个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.FractionPercentagePointRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //15,123个百分点  １１１，１１１个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.FractionPercentageWithSeparatorRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //12.1k个百分点  １５.1k个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.FractionPercentageWithMultiplierRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //百分之22  百分之１２０
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.SimpleNumbersPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //百分之15k
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.SimpleNumbersPercentageWithMultiplierRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //百分之1,111  百分之９，９９９
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.SimpleNumbersPercentagePointRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //12个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.IntegerPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //12k个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.IntegerPercentageWithMultiplierRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //2,123个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.NumbersFractionPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //32.5%
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.SimpleIntegerPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //2折 ２.５折
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.NumbersFoldsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //三折 六点五折 七五折
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.FoldsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //5成 6成半 6成4
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.SimpleFoldsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //七成半 七成五
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.SpecialsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //2成 ２.５成
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.NumbersSpecialsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //三成 六点五成
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.SimpleSpecialsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //打对折 半成
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.SpecialsFoldsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
