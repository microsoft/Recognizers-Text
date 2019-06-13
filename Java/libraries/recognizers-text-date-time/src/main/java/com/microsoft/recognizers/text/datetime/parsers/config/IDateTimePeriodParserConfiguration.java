package com.microsoft.recognizers.text.datetime.parsers.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;

import java.util.regex.Pattern;

public interface IDateTimePeriodParserConfiguration extends IOptionsConfiguration {
    String getTokenBeforeDate();

    IDateTimeExtractor getDateExtractor();

    IDateTimeExtractor getTimeExtractor();

    IDateTimeExtractor getDateTimeExtractor();

    IDateTimeExtractor getTimePeriodExtractor();

    IDateTimeExtractor getDurationExtractor();

    IExtractor getCardinalExtractor();

    IParser getNumberParser();

    IDateTimeParser getDateParser();

    IDateTimeParser getTimeParser();

    IDateTimeParser getDateTimeParser();

    IDateTimeParser getTimePeriodParser();

    IDateTimeParser getDurationParser();

    IDateTimeParser getTimeZoneParser();

    Pattern getPureNumberFromToRegex();

    Pattern getPureNumberBetweenAndRegex();

    Pattern getSpecificTimeOfDayRegex();

    Pattern getTimeOfDayRegex();

    Pattern getPastRegex();

    Pattern getFutureRegex();

    Pattern getFutureSuffixRegex();

    Pattern getNumberCombinedWithUnitRegex();

    Pattern getUnitRegex();

    Pattern getPeriodTimeOfDayWithDateRegex();

    Pattern getRelativeTimeUnitRegex();

    Pattern getRestOfDateTimeRegex();

    Pattern getAmDescRegex();

    Pattern getPmDescRegex();

    Pattern getWithinNextPrefixRegex();

    Pattern getPrefixDayRegex();

    Pattern getBeforeRegex();

    Pattern getAfterRegex();

    ImmutableMap<String, String> getUnitMap();

    ImmutableMap<String, Integer> getNumbers();

    MatchedTimeRangeResult getMatchedTimeRange(String text, String timeStr, int beginHour, int endHour, int endMin);

    int getSwiftPrefix(String text);
}
