// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number.japanese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.japanese.JapaneseNumberExtractorMode;
import com.microsoft.recognizers.text.number.resources.JapaneseNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class NumberExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;
    private final NumberOptions options;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM;
    }

    public NumberExtractor() {
        this(JapaneseNumberExtractorMode.Default, NumberOptions.None);
    }

    public NumberExtractor(JapaneseNumberExtractorMode mode) {
        this(mode, NumberOptions.None);
    }

    public NumberExtractor(JapaneseNumberExtractorMode mode, NumberOptions options) {
        this.options = options;
        HashMap<Pattern, String> builder = new HashMap<>();

        // Add Cardinal
        CardinalExtractor cardExtractChs = new CardinalExtractor(mode);
        builder.putAll(cardExtractChs.getRegexes());

        // Add Fraction
        FractionExtractor fracExtractChs = new FractionExtractor();
        builder.putAll(fracExtractChs.getRegexes());

        this.regexes = Collections.unmodifiableMap(builder);

    }
}
