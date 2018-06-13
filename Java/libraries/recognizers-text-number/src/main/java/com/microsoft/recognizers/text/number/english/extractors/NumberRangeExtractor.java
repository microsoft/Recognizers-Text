package com.microsoft.recognizers.text.number.english.extractors;

import com.microsoft.recognizers.text.number.NumberRangeConstants;
import com.microsoft.recognizers.text.number.extractors.BaseNumberRangeExtractor;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.number.english.parsers.EnglishNumberParserConfiguration;
import com.microsoft.recognizers.text.number.resources.EnglishNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class NumberRangeExtractor extends BaseNumberRangeExtractor {

    private final Map<Pattern, String> regexes;

    public NumberRangeExtractor() {
        super(NumberExtractor.getInstance(), OrdinalExtractor.getInstance(), new BaseNumberParser(new EnglishNumberParserConfiguration()));

        HashMap<Pattern, String> builder = new HashMap<>();

        // between...and...
        builder.put(Pattern.compile(EnglishNumeric.TwoNumberRangeRegex1, Pattern.CASE_INSENSITIVE), NumberRangeConstants.TWONUMBETWEEN);
        // more than ... less than ...
        builder.put(Pattern.compile(RegExpUtility.sanitize(EnglishNumeric.TwoNumberRangeRegex2), Pattern.CASE_INSENSITIVE), NumberRangeConstants.TWONUM);
        // less than ... more than ...
        builder.put(Pattern.compile(RegExpUtility.sanitize(EnglishNumeric.TwoNumberRangeRegex3), Pattern.CASE_INSENSITIVE), NumberRangeConstants.TWONUM);
        // from ... to/~/- ...
        builder.put(Pattern.compile(EnglishNumeric.TwoNumberRangeRegex4, Pattern.CASE_INSENSITIVE), NumberRangeConstants.TWONUMTILL);
        // more/greater/higher than ...
        builder.put(Pattern.compile(EnglishNumeric.OneNumberRangeMoreRegex1, Pattern.CASE_INSENSITIVE), NumberRangeConstants.MORE);
        // 30 and/or greater/higher
        builder.put(Pattern.compile(EnglishNumeric.OneNumberRangeMoreRegex2, Pattern.CASE_INSENSITIVE), NumberRangeConstants.MORE);
        // less/smaller/lower than ...
        builder.put(Pattern.compile(EnglishNumeric.OneNumberRangeLessRegex1, Pattern.CASE_INSENSITIVE), NumberRangeConstants.LESS);
        // 30 and/or less/smaller/lower
        builder.put(Pattern.compile(EnglishNumeric.OneNumberRangeLessRegex2, Pattern.CASE_INSENSITIVE), NumberRangeConstants.LESS);
        // equal to ...
        builder.put(Pattern.compile(EnglishNumeric.OneNumberRangeEqualRegex, Pattern.CASE_INSENSITIVE), NumberRangeConstants.EQUAL);
        // equal to 30 or more than, larger than 30 or equal to ...
        builder.put(Pattern.compile(RegExpUtility.sanitize(EnglishNumeric.OneNumberRangeMoreSeparateRegex), Pattern.CASE_INSENSITIVE), NumberRangeConstants.MORE);
        // equal to 30 or less, smaller than 30 or equal ...
        builder.put(Pattern.compile(RegExpUtility.sanitize(EnglishNumeric.OneNumberRangeLessSeparateRegex), Pattern.CASE_INSENSITIVE), NumberRangeConstants.LESS);

        this.regexes = Collections.unmodifiableMap(builder);
    }

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }
}
