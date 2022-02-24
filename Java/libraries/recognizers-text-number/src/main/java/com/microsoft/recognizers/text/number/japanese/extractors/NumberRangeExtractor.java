// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number.japanese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.NumberRangeConstants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberRangeExtractor;
import com.microsoft.recognizers.text.number.japanese.JapaneseNumberExtractorMode;
import com.microsoft.recognizers.text.number.japanese.parsers.JapaneseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseCJKNumberParser;
import com.microsoft.recognizers.text.number.resources.JapaneseNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class NumberRangeExtractor extends BaseNumberRangeExtractor {

    private final NumberOptions options;
    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    public NumberOptions getOptions() {
        return options;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUMRANGE;
    }

    public NumberRangeExtractor() {
        this(JapaneseNumberExtractorMode.Default, NumberOptions.None);
    }

    public NumberRangeExtractor(NumberOptions options) {
        this(JapaneseNumberExtractorMode.Default, options);
    }

    public NumberRangeExtractor(JapaneseNumberExtractorMode mode) {
        this(mode, NumberOptions.None);
    }

    public NumberRangeExtractor(JapaneseNumberExtractorMode mode, NumberOptions options) {

        super(new NumberExtractor(mode, options), new OrdinalExtractor(), new BaseCJKNumberParser(new JapaneseNumberParserConfiguration()));

        this.options = options;

        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.TwoNumberRangeRegex1, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS),
            NumberRangeConstants.TWONUMBETWEEN);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.TwoNumberRangeRegex2, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.TWONUM);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.TwoNumberRangeRegex3, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.TWONUM);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.TwoNumberRangeRegex4, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.TWONUMTILL);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.OneNumberRangeMoreRegex1, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.MORE);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.OneNumberRangeMoreRegex3, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.MORE);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.OneNumberRangeMoreRegex4, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.MORE);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.OneNumberRangeMoreRegex5, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.MORE);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.TwoNumberRangeMoreSuffix, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.MORE);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.OneNumberRangeLessRegex1, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.LESS);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.OneNumberRangeLessRegex3, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.LESS);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.OneNumberRangeLessRegex4, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.LESS);

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.OneNumberRangeEqualRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), NumberRangeConstants.EQUAL);

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
