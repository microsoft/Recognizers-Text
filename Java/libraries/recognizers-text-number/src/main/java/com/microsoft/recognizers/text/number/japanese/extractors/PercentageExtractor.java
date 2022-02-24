// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number.japanese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.JapaneseNumeric;
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

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.PercentagePointRegex, Pattern.UNICODE_CHARACTER_CLASS), "Per" + JapaneseNumeric.LangMarker);

        //百分之五十  百分之一点五
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.SimplePercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "Per" + JapaneseNumeric.LangMarker);

        //百分之５６.２　百分之１２
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.NumbersPercentagePointRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //百分之3,000  百分之１，１２３
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.NumbersPercentageWithSeparatorRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //百分之3.2 k
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.NumbersPercentageWithMultiplierRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //12.56个百分点  ０.４个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.FractionPercentagePointRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //15,123个百分点  １１１，１１１个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.FractionPercentageWithSeparatorRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //12.1k个百分点  １５.1k个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.FractionPercentageWithMultiplierRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //百分之22  百分之１２０
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.SimpleNumbersPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //百分之15k
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.SimpleNumbersPercentageWithMultiplierRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //百分之1,111  百分之９，９９９
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.SimpleNumbersPercentagePointRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //12个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.IntegerPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //12k个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.IntegerPercentageWithMultiplierRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //2,123个百分点
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.NumbersFractionPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //32.5%
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.SimpleIntegerPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerNum");

        //2折 ２.５折
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.NumbersFoldsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //三折 六点五折 七五折
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.FoldsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //5成 6成半 6成4
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.SimpleFoldsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //七成半 七成五
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.SpecialsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //2成 ２.５成
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.NumbersSpecialsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //三成 六点五成
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.SimpleSpecialsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        //打对折 半成
        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.SpecialsFoldsPercentageRegex, Pattern.UNICODE_CHARACTER_CLASS), "PerSpe");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
