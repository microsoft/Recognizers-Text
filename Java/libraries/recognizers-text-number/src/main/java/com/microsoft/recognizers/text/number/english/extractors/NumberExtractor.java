package com.microsoft.recognizers.text.number.english.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.BaseNumbers;
import com.microsoft.recognizers.text.number.resources.EnglishNumeric;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ConcurrentHashMap;
import java.util.regex.Pattern;
import org.javatuples.Pair;


public class NumberExtractor extends BaseNumberExtractor {

    private final Map<Pattern, String> regexes;
    private final Map<Pattern, Pattern> ambiguityFiltersDict;
    private final NumberOptions options;
    private final Pattern negativeNumberTermsRegex;

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

    @Override
    protected NumberOptions getOptions() {
        return this.options;
    }

    @Override
    protected Optional<Pattern> getNegativeNumberTermsRegex() {
        return Optional.of(this.negativeNumberTermsRegex);
    }

    private static final ConcurrentHashMap<Pair<NumberMode, NumberOptions>, NumberExtractor> instances = new ConcurrentHashMap<>();

    public static NumberExtractor getInstance() {
        return getInstance(NumberMode.Default, NumberOptions.None);
    }

    public static NumberExtractor getInstance(NumberMode mode) {
        return getInstance(mode, NumberOptions.None);
    }

    public static NumberExtractor getInstance(NumberMode mode, NumberOptions options) {
        Pair<NumberMode, NumberOptions> key = Pair.with(mode, options);
        if (!instances.containsKey(key)) {
            NumberExtractor instance = new NumberExtractor(mode, options);
            instances.put(key, instance);
        }

        return instances.get(key);
    }


    private NumberExtractor(NumberMode mode, NumberOptions options) {
        this.options = options;
        this.negativeNumberTermsRegex = Pattern.compile(EnglishNumeric.NegativeNumberTermsRegex + '$', Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);

        HashMap<Pattern, String> builder = new HashMap<>();

        // Add Cardinal
        CardinalExtractor cardinalExtractor = null;
        switch (mode) {
            case PureNumber:
                cardinalExtractor = CardinalExtractor.getInstance(EnglishNumeric.PlaceHolderPureNumber);
                break;
            case Currency:
                builder.put(Pattern.compile(BaseNumbers.CurrencyRegex, Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS), "IntegerNum");
                break;
            case Default:
                break;
            default:
                break;
        }

        if (cardinalExtractor == null) {
            cardinalExtractor = CardinalExtractor.getInstance();
        }

        builder.putAll(cardinalExtractor.getRegexes());

        // Add Fraction
        FractionExtractor fractionExtractor = FractionExtractor.getInstance(mode, this.options);
        builder.putAll(fractionExtractor.getRegexes());

        this.regexes = Collections.unmodifiableMap(builder);

        HashMap<Pattern, Pattern> ambiguityFiltersDict = new HashMap<>();
        if (mode != NumberMode.Unit) {
            for (Map.Entry<String, String> pair : EnglishNumeric.AmbiguityFiltersDict.entrySet()) {
                Pattern key = RegExpUtility.getSafeRegExp(pair.getKey());
                Pattern val = RegExpUtility.getSafeRegExp(pair.getValue());
                ambiguityFiltersDict.put(key, val);
            }
        }

        this.ambiguityFiltersDict = ambiguityFiltersDict;
    }
}
