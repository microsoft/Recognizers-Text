package com.microsoft.recognizers.text.number.portuguese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.PortugueseNumeric;
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

        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric.OrdinalSuffixRegex, Pattern.UNICODE_CHARACTER_CLASS), "OrdinalNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(PortugueseNumeric. OrdinalEnglishRegex, Pattern.UNICODE_CHARACTER_CLASS), "OrdinalPor");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
