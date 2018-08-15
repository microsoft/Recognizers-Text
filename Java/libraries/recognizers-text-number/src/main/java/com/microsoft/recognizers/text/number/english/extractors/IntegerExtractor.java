package com.microsoft.recognizers.text.number.english.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.LongFormatType;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.EnglishNumeric;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ConcurrentHashMap;
import java.util.regex.Pattern;

public class IntegerExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_INTEGER;
    }

    @Override
    protected NumberOptions getOptions() {
        return NumberOptions.None;
    }

    @Override
    protected Optional<Pattern> getNegativeNumberTermsRegex() {
        return Optional.empty();
    }

    private static final ConcurrentHashMap<String, IntegerExtractor> instances = new ConcurrentHashMap<>();

    public static IntegerExtractor getInstance() {
        return getInstance(EnglishNumeric.PlaceHolderDefault);
    }

    public static IntegerExtractor getInstance(String placeholder) {
        if (!instances.containsKey(placeholder)) {
            IntegerExtractor instance = new IntegerExtractor(placeholder);
            instances.put(placeholder, instance);
        }

        return instances.get(placeholder);
    }

    private IntegerExtractor()
    {
        this(EnglishNumeric.PlaceHolderDefault);
    }

    private IntegerExtractor(String placeholder) {
        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(Pattern.compile(EnglishNumeric.NumbersWithPlaceHolder(placeholder), Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        builder.put(Pattern.compile(EnglishNumeric.NumbersWithSuffix), "IntegerNum");
        builder.put(Pattern.compile(EnglishNumeric.RoundNumberIntegerRegexWithLocks, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        builder.put(Pattern.compile(EnglishNumeric.NumbersWithDozenSuffix, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
        builder.put(Pattern.compile(EnglishNumeric.AllIntRegexWithLocks, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "IntegerEng");
        builder.put(Pattern.compile(EnglishNumeric.AllIntRegexWithDozenSuffixLocks, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "IntegerEng");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumComma, placeholder), "IntegerNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumBlank, placeholder), "IntegerNum");
        builder.put(generateLongFormatNumberRegexes(LongFormatType.IntegerNumNoBreakSpace, placeholder), "IntegerNum");

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
