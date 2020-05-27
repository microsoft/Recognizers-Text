package com.microsoft.recognizers.text.number.chinese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.chinese.ChineseNumberExtractorMode;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class NumberExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;
    private final Map<Pattern, Pattern> ambiguityFiltersDict;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected Map<Pattern, Pattern> getAmbiguityFiltersDict() {
        return this.ambiguityFiltersDict;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM;
    }

    public NumberExtractor() {
        this(ChineseNumberExtractorMode.Default);
    }

    public NumberExtractor(ChineseNumberExtractorMode mode) {

        HashMap<Pattern, String> builder = new HashMap<>();

        // Add Cardinal
        CardinalExtractor cardExtractChs = new CardinalExtractor(mode);
        builder.putAll(cardExtractChs.getRegexes());

        // Add Fraction
        FractionExtractor fracExtractChs = new FractionExtractor();
        builder.putAll(fracExtractChs.getRegexes());

        this.regexes = Collections.unmodifiableMap(builder);

        HashMap<Pattern, Pattern> ambiguityFiltersDict = new HashMap<>();

        for (Map.Entry<String, String> pair : ChineseNumeric.AmbiguityFiltersDict.entrySet()) {
            Pattern key = RegExpUtility.getSafeRegExp(pair.getKey());
            Pattern val = RegExpUtility.getSafeRegExp(pair.getValue());
            ambiguityFiltersDict.put(key, val);
        }

        this.ambiguityFiltersDict = ambiguityFiltersDict;
    }
}
