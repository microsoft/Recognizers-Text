// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number.japanese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.JapaneseNumeric;
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

        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.DoubleSpecialsChars, Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");

        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.DoubleSpecialsCharsWithNegatives, Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");

        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.SimpleDoubleSpecialsChars, Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");

        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.DoubleRoundNumberSpecialsChars, Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");

        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.DoubleWithMultiplierRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");

        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.DoubleWithThousandsRegex, Pattern.UNICODE_CHARACTER_CLASS), "Double" + JapaneseNumeric.LangMarker);

        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.DoubleExponentialNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");

        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.DoubleExponentialNotationKanjiRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");

        builder.put(RegExpUtility.getSafeRegExp(JapaneseNumeric.DoubleAllFloatRegex, Pattern.UNICODE_CHARACTER_CLASS), "Double" + JapaneseNumeric.LangMarker);

        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.DoubleScientificNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");

        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumFullWidthBlankDot), "DoubleNum");

        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumBlankDot), "DoubleNum");

        builder.put(RegExpUtility.getSafeLookbehindRegExp(JapaneseNumeric.DoubleScientificNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
