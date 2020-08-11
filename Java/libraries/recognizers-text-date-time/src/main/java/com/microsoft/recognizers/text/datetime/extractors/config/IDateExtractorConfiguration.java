package com.microsoft.recognizers.text.datetime.extractors.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;

import java.util.regex.Pattern;

public interface IDateExtractorConfiguration extends IOptionsConfiguration {
    Iterable<Pattern> getDateRegexList();

    Iterable<Pattern> getImplicitDateList();

    Pattern getOfMonth();

    Pattern getMonthEnd();

    Pattern getWeekDayEnd();

    Pattern getDateUnitRegex();

    Pattern getForTheRegex();

    Pattern getWeekDayAndDayOfMonthRegex();

    Pattern getRelativeMonthRegex();

    Pattern getStrictRelativeRegex();

    Pattern getWeekDayRegex();

    Pattern getPrefixArticleRegex();

    Pattern getYearSuffix();

    Pattern getMoreThanRegex();

    Pattern getLessThanRegex();

    Pattern getInConnectorRegex();

    Pattern getRangeUnitRegex();

    Pattern getRangeConnectorSymbolRegex();

    IExtractor getIntegerExtractor();

    IExtractor getOrdinalExtractor();

    IParser getNumberParser();

    IDateTimeExtractor getDurationExtractor();

    IDateTimeUtilityConfiguration getUtilityConfiguration();

    ImmutableMap<String, Integer> getDayOfWeek();

    ImmutableMap<String, Integer> getMonthOfYear();
}
