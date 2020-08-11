package com.microsoft.recognizers.text.number.english.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.EnglishNumeric;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ConcurrentHashMap;
import java.util.regex.Pattern;
import org.javatuples.Pair;

public class FractionExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;
    private final NumberOptions options;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM_FRACTION;
    }

    @Override
    protected NumberOptions getOptions() {
        return this.options;
    }

    @Override
    protected Optional<Pattern> getNegativeNumberTermsRegex() {
        return Optional.empty();
    }

    private static final ConcurrentHashMap<Pair<NumberMode, NumberOptions>, FractionExtractor> instances = new ConcurrentHashMap<>();

    public static FractionExtractor getInstance(NumberMode mode, NumberOptions options) {
        Pair<NumberMode, NumberOptions> key = Pair.with(mode, options);
        if (!instances.containsKey(key)) {
            FractionExtractor instance = new FractionExtractor(mode, options);
            instances.put(key, instance);
        }

        return instances.get(key);
    }

    private FractionExtractor(NumberMode mode, NumberOptions options) {
        this.options = options;

        HashMap<Pattern, String> builder = new HashMap<>();

        builder.put(Pattern.compile(EnglishNumeric.FractionNotationWithSpacesRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(Pattern.compile(EnglishNumeric.FractionNotationRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracNum");
        builder.put(Pattern.compile(EnglishNumeric.FractionNounRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracEng");
        builder.put(Pattern.compile(EnglishNumeric.FractionNounWithArticleRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracEng");

        if (mode != NumberMode.Unit) {
            if ((options.ordinal() & NumberOptions.PercentageMode.ordinal()) != 0) {
                builder.put(Pattern.compile(EnglishNumeric.FractionPrepositionWithinPercentModeRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracEng");
            } else {
                builder.put(Pattern.compile(EnglishNumeric.FractionPrepositionRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "FracEng");
            }
        }

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
