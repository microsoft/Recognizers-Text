package com.microsoft.recognizers.text.number.english.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.EnglishNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ConcurrentHashMap;
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

    @Override
    protected NumberOptions getOptions() {
        return NumberOptions.None;
    }

    @Override
    protected Optional<Pattern> getNegativeNumberTermsRegex() {
        return Optional.empty();
    }

    private static final ConcurrentHashMap<String, DoubleExtractor> instances = new ConcurrentHashMap<>();

    public static DoubleExtractor getInstance() {
        return getInstance(EnglishNumeric.PlaceHolderDefault);
    }

    public static DoubleExtractor getInstance(String placeholder) {

        if (!instances.containsKey(placeholder)) {
            DoubleExtractor instance = new DoubleExtractor(placeholder);
            instances.put(placeholder, instance);
        }

        return instances.get(placeholder);
    }

    private DoubleExtractor() {
        this(EnglishNumeric.PlaceHolderDefault);
    }

    private DoubleExtractor(String placeholder) {

        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(RegExpUtility.getSafeLookbehindRegExp(EnglishNumeric.DoubleDecimalPointRegex(placeholder), Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(Pattern.compile(EnglishNumeric.DoubleWithoutIntegralRegex(placeholder), Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(EnglishNumeric.DoubleWithMultiplierRegex), "DoubleNum");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(EnglishNumeric.DoubleWithRoundNumber, Pattern.UNICODE_CHARACTER_CLASS), "DoubleNum");
        builder.put(Pattern.compile(EnglishNumeric.DoubleAllFloatRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoubleEng");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(EnglishNumeric.DoubleExponentialNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        builder.put(RegExpUtility.getSafeLookbehindRegExp(EnglishNumeric.DoubleCaretExponentialNotationRegex, Pattern.UNICODE_CHARACTER_CLASS), "DoublePow");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumCommaDot, placeholder), "DoubleNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumBlankDot, placeholder), "DoubleNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.DoubleNumNoBreakSpaceDot, placeholder), "DoubleNum");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
