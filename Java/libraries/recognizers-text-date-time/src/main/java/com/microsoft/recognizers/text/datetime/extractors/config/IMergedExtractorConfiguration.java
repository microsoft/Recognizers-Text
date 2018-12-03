package com.microsoft.recognizers.text.datetime.extractors.config;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeListExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeZoneExtractor;
import com.microsoft.recognizers.text.matcher.StringMatcher;

import java.util.regex.Pattern;

public interface IMergedExtractorConfiguration extends IOptionsConfiguration {
    IDateTimeExtractor getDateExtractor();

    IDateTimeExtractor getTimeExtractor();

    IDateTimeExtractor getDateTimeExtractor();

    IDateTimeExtractor getDatePeriodExtractor();

    IDateTimeExtractor getTimePeriodExtractor();

    IDateTimeExtractor getDateTimePeriodExtractor();

    IDateTimeExtractor getDurationExtractor();

    IDateTimeExtractor getSetExtractor();

    IDateTimeExtractor getHolidayExtractor();

    IDateTimeZoneExtractor getTimeZoneExtractor();

    IDateTimeListExtractor getDateTimeAltExtractor();

    IExtractor getIntegerExtractor();

    Iterable<Pattern> getFilterWordRegexList();

    Pattern getAfterRegex();

    Pattern getBeforeRegex();

    Pattern getSinceRegex();

    Pattern getFromToRegex();

    Pattern getSingleAmbiguousMonthRegex();

    Pattern getPrepositionSuffixRegex();

    Pattern getNumberEndingPattern();

    Pattern getDateAfterRegex();

    Pattern getUnspecificDatePeriodRegex();

    StringMatcher getSuperfluousWordMatcher();
}
