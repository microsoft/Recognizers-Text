package com.microsoft.recognizers.text.datetime.french.parsers;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseSetParser;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.IMergedParserConfiguration;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import java.util.regex.Pattern;

public class FrenchMergedParserConfiguration extends FrenchCommonDateTimeParserConfiguration implements IMergedParserConfiguration {

    public FrenchMergedParserConfiguration(final DateTimeOptions options) {
        super(options);

        beforeRegex = FrenchMergedExtractorConfiguration.BeforeRegex;
        afterRegex = FrenchMergedExtractorConfiguration.AfterRegex;
        sinceRegex = FrenchMergedExtractorConfiguration.SinceRegex;
        aroundRegex = FrenchMergedExtractorConfiguration.AroundRegex;
        suffixAfterRegex = FrenchMergedExtractorConfiguration.SuffixAfterRegex;
        yearRegex = FrenchDatePeriodExtractorConfiguration.YearRegex;
        superfluousWordMatcher = FrenchMergedExtractorConfiguration.SuperfluousWordMatcher;

        getParser = new BaseSetParser(new FrenchSetParserConfiguration(this));
        holidayParser = new BaseHolidayParser(new FrenchHolidayParserConfiguration());
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
