package com.microsoft.recognizers.text.datetime.parsers.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.utilities.MatchedTimexResult;

import java.util.regex.Pattern;

public interface ISetParserConfiguration extends IOptionsConfiguration {
    IDateTimeExtractor getDurationExtractor();

    IDateTimeParser getDurationParser();

    IDateTimeExtractor getTimeExtractor();

    IDateTimeParser getTimeParser();

    IDateTimeExtractor getDateExtractor();

    IDateTimeParser getDateParser();

    IDateTimeExtractor getDateTimeExtractor();

    IDateTimeParser getDateTimeParser();

    IDateTimeExtractor getDatePeriodExtractor();

    IDateTimeParser getDatePeriodParser();

    IDateTimeExtractor getTimePeriodExtractor();

    IDateTimeParser getTimePeriodParser();

    IDateTimeExtractor getDateTimePeriodExtractor();

    IDateTimeParser getDateTimePeriodParser();

    ImmutableMap<String, String> getUnitMap();

    Pattern getEachPrefixRegex();

    Pattern getPeriodicRegex();

    Pattern getEachUnitRegex();

    Pattern getEachDayRegex();

    Pattern getSetWeekDayRegex();

    Pattern getSetEachRegex();

    MatchedTimexResult getMatchedDailyTimex(String text);

    MatchedTimexResult getMatchedUnitTimex(String text);
}

