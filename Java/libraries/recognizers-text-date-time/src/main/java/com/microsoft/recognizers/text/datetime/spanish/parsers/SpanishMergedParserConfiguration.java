package com.microsoft.recognizers.text.datetime.spanish.parsers;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseSetParser;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.IMergedParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishMergedExtractorConfiguration;
import com.microsoft.recognizers.text.matcher.StringMatcher;

import java.util.regex.Pattern;

public class SpanishMergedParserConfiguration extends SpanishCommonDateTimeParserConfiguration implements IMergedParserConfiguration {

    public SpanishMergedParserConfiguration(DateTimeOptions options) {
        super(options);

        beforeRegex = SpanishMergedExtractorConfiguration.BeforeRegex;
        afterRegex = SpanishMergedExtractorConfiguration.AfterRegex;
        sinceRegex = SpanishMergedExtractorConfiguration.SinceRegex;
        aroundRegex = SpanishMergedExtractorConfiguration.AroundRegex;
        suffixAfterRegex = SpanishMergedExtractorConfiguration.SuffixAfterRegex;
        yearRegex = SpanishDatePeriodExtractorConfiguration.YearRegex;
        superfluousWordMatcher = SpanishMergedExtractorConfiguration.SuperfluousWordMatcher;

        getParser = new BaseSetParser(new SpanishSetParserConfiguration(this));
        holidayParser = new BaseHolidayParser(new SpanishHolidayParserConfiguration());
    }

    private final Pattern beforeRegex;
    private final Pattern afterRegex;
    private final Pattern sinceRegex;
    private final Pattern aroundRegex;
    private final Pattern suffixAfterRegex;
    private final Pattern yearRegex;
    private final IDateTimeParser getParser;
    private final IDateTimeParser holidayParser;
    private final StringMatcher superfluousWordMatcher;

    public Pattern getBeforeRegex() {
        return beforeRegex;
    }

    public Pattern getAfterRegex() {
        return afterRegex;
    }

    public Pattern getSinceRegex() {
        return sinceRegex;
    }

    public Pattern getAroundRegex() {
        return aroundRegex;
    }

    public Pattern getSuffixAfterRegex() {
        return suffixAfterRegex;
    }

    public Pattern getYearRegex() {
        return yearRegex;
    }

    public IDateTimeParser getGetParser() {
        return getParser;
    }

    public IDateTimeParser getHolidayParser() {
        return holidayParser;
    }

    public StringMatcher getSuperfluousWordMatcher() {
        return superfluousWordMatcher;
    }
}
