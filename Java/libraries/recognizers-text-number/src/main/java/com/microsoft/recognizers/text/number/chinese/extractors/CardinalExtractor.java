package com.microsoft.recognizers.text.number.chinese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.chinese.ChineseNumberExtractorMode;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class CardinalExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_CARDINAL;
    }

    public CardinalExtractor() {
        this(ChineseNumberExtractorMode.Default);
    }

    public CardinalExtractor(ChineseNumberExtractorMode mode) {
        HashMap<Pattern, String> builder = new HashMap<>();

        IntegerExtractor intExtractChs = new IntegerExtractor(mode);
        builder.putAll(intExtractChs.getRegexes());

        DoubleExtractor douExtractorChs = new DoubleExtractor();
        builder.putAll(douExtractorChs.getRegexes());

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
