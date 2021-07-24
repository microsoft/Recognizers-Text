// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number.french.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.FrenchNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class OrdinalExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_ORDINAL;
    }

    public OrdinalExtractor() {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.OrdinalSuffixRegex, Pattern.UNICODE_CHARACTER_CLASS), "OrdinalNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(FrenchNumeric.OrdinalFrenchRegex, Pattern.UNICODE_CHARACTER_CLASS), "Ord" + FrenchNumeric.LangMarker);

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
