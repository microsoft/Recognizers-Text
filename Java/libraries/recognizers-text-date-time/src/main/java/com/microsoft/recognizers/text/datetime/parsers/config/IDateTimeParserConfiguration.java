package com.microsoft.recognizers.text.datetime.parsers.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultTimex;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;

import java.util.regex.Pattern;

public interface IDateTimeParserConfiguration extends IOptionsConfiguration {
    String getTokenBeforeDate();

    String getTokenBeforeTime();

    IDateTimeExtractor getDateExtractor();

    IDateTimeExtractor getTimeExtractor();

    IDateTimeParser getDateParser();

    IDateTimeParser getTimeParser();

    IExtractor getCardinalExtractor();

    IExtractor getIntegerExtractor();

    IParser getNumberParser();

    IDateTimeExtractor getDurationExtractor();

    IDateTimeParser getDurationParser();

    Pattern getNowRegex();

    Pattern getAMTimeRegex();

    Pattern getPMTimeRegex();

    Pattern getSimpleTimeOfTodayAfterRegex();

    Pattern getSimpleTimeOfTodayBeforeRegex();

    Pattern getSpecificTimeOfDayRegex();

    Pattern getSpecificEndOfRegex();

    Pattern getUnspecificEndOfRegex();

    Pattern getUnitRegex();

    Pattern getDateNumberConnectorRegex();

    ImmutableMap<String, String> getUnitMap();

    ImmutableMap<String, Integer> getNumbers();

    IDateTimeUtilityConfiguration getUtilityConfiguration();

    boolean containsAmbiguousToken(String text, String matchedText);

    ResultTimex getMatchedNowTimex(String text);

    int getSwiftDay(String text);

    int getHour(String text, int hour);
}