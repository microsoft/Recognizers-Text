package com.microsoft.recognizers.text.datetime.parsers.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;

import java.util.regex.Pattern;

public interface ICommonDateTimeParserConfiguration extends IOptionsConfiguration {
    IExtractor getCardinalExtractor();

    IExtractor getIntegerExtractor();

    IExtractor getOrdinalExtractor();

    IParser getNumberParser();

    IDateExtractor getDateExtractor();

    IDateTimeExtractor getTimeExtractor();

    IDateTimeExtractor getDateTimeExtractor();

    IDateTimeExtractor getDurationExtractor();

    IDateTimeExtractor getDatePeriodExtractor();

    IDateTimeExtractor getTimePeriodExtractor();

    IDateTimeExtractor getDateTimePeriodExtractor();

    IDateTimeParser getDateParser();

    IDateTimeParser getTimeParser();

    IDateTimeParser getDateTimeParser();

    IDateTimeParser getDurationParser();

    IDateTimeParser getDatePeriodParser();

    IDateTimeParser getTimePeriodParser();

    IDateTimeParser getDateTimePeriodParser();

    IDateTimeParser getDateTimeAltParser();

    IDateTimeParser getTimeZoneParser();

    ImmutableMap<String, Integer> getMonthOfYear();

    ImmutableMap<String, Integer> getNumbers();

    ImmutableMap<String, Long> getUnitValueMap();

    ImmutableMap<String, String> getSeasonMap();

    ImmutableMap<String, String> getSpecialYearPrefixesMap();

    ImmutableMap<String, String> getUnitMap();

    ImmutableMap<String, Integer> getCardinalMap();

    ImmutableMap<String, Integer> getDayOfMonth();

    ImmutableMap<String, Integer> getDayOfWeek();

    ImmutableMap<String, Double> getDoubleNumbers();

    ImmutableMap<String, Integer> getWrittenDecades();

    ImmutableMap<String, Integer> getSpecialDecadeCases();

    IDateTimeUtilityConfiguration getUtilityConfiguration();
}
