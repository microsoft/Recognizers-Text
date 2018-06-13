package com.microsoft.recognizers.text.number.spanish.extractors;

import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BaseNumberExtractor;
import com.microsoft.recognizers.text.number.resources.SpanishNumeric;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

import static com.microsoft.recognizers.text.number.NumberMode.Default;

public class NumberExtractor extends BaseNumberExtractor {

    private final Map<Pattern,String> regexes;

    @Override
    protected Map<Pattern, String> getRegexes() {
        return this.regexes;
    }

    @Override
    protected String getExtractType() {
        return Constants.SYS_NUM;
    }

    public NumberExtractor() {
        this(Default);
    }

    public NumberExtractor(NumberMode mode) {
        HashMap<Pattern, String> builder = new HashMap<>();

        //Add Cardinal
        CardinalExtractor cardExtract = null;
        switch (mode)
        {
            case PureNumber:
                cardExtract = CardinalExtractor.getInstance(SpanishNumeric.PlaceHolderPureNumber);
                break;
            case Currency:
                builder.put(Pattern.compile(SpanishNumeric.CurrencyRegex, Pattern.CASE_INSENSITIVE), "IntegerNum");
                break;
            case Default:
                break;
        }

        if (cardExtract == null)
        {
            cardExtract = CardinalExtractor.getInstance();
        }

        builder.putAll(cardExtract.getRegexes());

        //Add Fraction
        FractionExtractor fracExtract = new FractionExtractor();
        builder.putAll(fracExtract.getRegexes());

        this.regexes = Collections.unmodifiableMap(builder);
    }
}
