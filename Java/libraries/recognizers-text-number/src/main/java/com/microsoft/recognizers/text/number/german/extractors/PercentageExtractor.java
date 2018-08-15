package com.microsoft.recognizers.text.number.german.extractors;

import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.extractors.BasePercentageExtractor;
import com.microsoft.recognizers.text.number.resources.GermanNumeric;

import java.util.HashSet;
import java.util.Set;
import java.util.regex.Pattern;

public class PercentageExtractor extends BasePercentageExtractor {

    private final NumberOptions options;
    private final Set<Pattern> regexes;

    @Override
    protected NumberOptions getOptions() {
        return this.options;
    }

    @Override
    protected Set<Pattern> getRegexes() {
        return this.regexes;
    }

    public PercentageExtractor() {
        this(NumberOptions.None);
    }

    public PercentageExtractor(NumberOptions options) {
        super(NumberExtractor.getInstance(options));
        this.options = options;

        Set<String> builder = new HashSet<>();
        builder.add(GermanNumeric.NumberWithSuffixPercentage);
        builder.add(GermanNumeric.NumberWithPrefixPercentage);
        this.regexes = buildRegexes(builder);
    }
}
