package com.microsoft.recognizers.text.number.chinese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;

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

        //第一百五十四
        builder.put(Pattern.compile(ChineseNumeric.OrdinalRegex, Pattern.UNICODE_CHARACTER_CLASS), "OrdinalChs");

        //第２５６５,  第1234
        builder.put(Pattern.compile(ChineseNumeric.OrdinalNumbersRegex, Pattern.UNICODE_CHARACTER_CLASS), "OrdinalChs");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
