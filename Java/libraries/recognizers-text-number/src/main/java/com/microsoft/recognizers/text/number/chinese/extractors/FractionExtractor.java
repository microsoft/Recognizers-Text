package com.microsoft.recognizers.text.number.chinese.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.ChineseNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class FractionExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_FRACTION;
    }

    public FractionExtractor() {
        HashMap<Pattern, String> builder = new HashMap<>();

        // -4 5/2,       ４ ６／３
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.FractionNotationSpecialsCharsRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        // 8/3
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.FractionNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        //四分之六十五
        builder.put(RegExpUtility.getSafeLookbehindRegExp(ChineseNumeric.AllFractionNumber, Pattern.UNICODE_CHARACTER_CLASS), "Frac" + ChineseNumeric.LangMarker);

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
