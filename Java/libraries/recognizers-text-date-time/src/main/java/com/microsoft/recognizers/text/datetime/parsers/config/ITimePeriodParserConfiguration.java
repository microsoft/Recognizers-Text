package com.microsoft.recognizers.text.datetime.parsers.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;

import java.util.regex.Pattern;

public interface ITimePeriodParserConfiguration extends IOptionsConfiguration {
    IDateTimeExtractor getTimeExtractor();

    IDateTimeParser getTimeParser();

    IExtractor getIntegerExtractor();

    IDateTimeParser getTimeZoneParser();

    Pattern getPureNumberFromToRegex();

    Pattern getPureNumberBetweenAndRegex();

    Pattern getSpecificTimeFromToRegex();

    Pattern getSpecificTimeBetweenAndRegex();

    Pattern getTimeOfDayRegex();

    Pattern getGeneralEndingRegex();

    Pattern getTillRegex();

    ImmutableMap<String, Integer> getNumbers();

    IDateTimeUtilityConfiguration getUtilityConfiguration();

    MatchedTimeRangeResult getMatchedTimexRange(String text, String timex, int beginHour, int endHour, int endMin);
}
