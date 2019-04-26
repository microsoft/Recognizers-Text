package com.microsoft.recognizers.text.datetime.parsers.config;

import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.matcher.StringMatcher;

import java.util.regex.Pattern;

public interface IMergedParserConfiguration extends ICommonDateTimeParserConfiguration {
    Pattern getBeforeRegex();

    Pattern getAfterRegex();

    Pattern getSinceRegex();

    Pattern getAroundRegex();

    Pattern getSuffixAfterRegex();

    Pattern getYearRegex();

    IDateTimeParser getGetParser();

    IDateTimeParser getHolidayParser();

    IDateTimeParser getTimeZoneParser();

    StringMatcher getSuperfluousWordMatcher();
}
