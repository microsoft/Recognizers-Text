package com.microsoft.recognizers.text.number.french.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.FrenchNumeric;
import org.javatuples.Pair;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.regex.Pattern;

import static com.microsoft.recognizers.text.number.NumberMode.Default;

public class NumberExtractor extends BaseNumberExtractor {
    private final Map<Pattern, String> regexes;
    private final NumberOptions options;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM;
    }

    @Override
    protected NumberOptions getOptions() {
        return this.options;
    }

    private static final ConcurrentHashMap<Pair<NumberMode, NumberOptions>, NumberExtractor> instances = new ConcurrentHashMap<>();

    public static NumberExtractor getInstance(NumberOptions options) {
        return getInstance(NumberMode.Default, options);
    }

    public static NumberExtractor getInstance(NumberMode mode) {
        return getInstance(mode, NumberOptions.None);
    }

    public static NumberExtractor getInstance() {
        return getInstance(NumberMode.Default, NumberOptions.None);
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
        HashMap<Pattern, String> builder = new HashMap<>();

        CardinalExtractor cardExtract = null;
        switch(mode)
        {
            case PureNumber:
                cardExtract = CardinalExtractor.getInstance(FrenchNumeric.PlaceHolderPureNumber);
                break;
            case Currency:
                builder.put(Pattern.compile(FrenchNumeric.CurrencyRegex), "IntegerNum");
                break;
            case Default:
                break;
        }

        if (cardExtract == null)
        {
            cardExtract = CardinalExtractor.getInstance();
        }

        builder.putAll(cardExtract.getRegexes());

        FractionExtractor fracExtract = new FractionExtractor();
        builder.putAll(fracExtract.getRegexes());

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
