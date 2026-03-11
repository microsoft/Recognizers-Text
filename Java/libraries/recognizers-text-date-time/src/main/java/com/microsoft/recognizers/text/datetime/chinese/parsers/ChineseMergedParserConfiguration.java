package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseSetParser;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.IMergedParserConfiguration;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class ChineseMergedParserConfiguration extends ChineseCommonDateTimeParserConfiguration implements IMergedParserConfiguration {

    private final Pattern beforeRegex;
    private final Pattern afterRegex;
    private final Pattern sinceRegex;
    private final Pattern aroundRegex;
    private final Pattern suffixAfterRegex;
    private final Pattern yearRegex;
    private final IDateTimeParser getParser;
    private final IDateTimeParser holidayParser;
    private final StringMatcher superfluousWordMatcher;

    public ChineseMergedParserConfiguration(DateTimeOptions options) {
        super(options);

        beforeRegex = ChineseMergedExtractorConfiguration.BeforeRegex;
        afterRegex = ChineseMergedExtractorConfiguration.AfterRegex;
        sinceRegex = ChineseMergedExtractorConfiguration.SinceRegex;
        aroundRegex = RegExpUtility.getSafeRegExp("^$");
        suffixAfterRegex = RegExpUtility.getSafeRegExp("^$");
        yearRegex = ChineseDatePeriodExtractorConfiguration.YearRegex;
        superfluousWordMatcher = ChineseMergedExtractorConfiguration.SuperfluousWordMatcher;

        getParser = new BaseSetParser(new ChineseSetParserConfiguration(this));
        holidayParser = new BaseHolidayParser(new ChineseHolidayParserConfiguration());
    }

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
