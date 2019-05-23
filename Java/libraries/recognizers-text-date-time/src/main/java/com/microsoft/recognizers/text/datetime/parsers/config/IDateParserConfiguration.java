package com.microsoft.recognizers.text.datetime.parsers.config;

import com.google.common.collect.ImmutableList;
import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;

import java.util.List;
import java.util.regex.Pattern;

public interface IDateParserConfiguration extends IOptionsConfiguration {
    String getDateTokenPrefix();

    IExtractor getIntegerExtractor();

    IExtractor getOrdinalExtractor();

    IExtractor getCardinalExtractor();

    IParser getNumberParser();

    IDateTimeExtractor getDurationExtractor();

    IDateExtractor getDateExtractor();

    IDateTimeParser getDurationParser();

    Iterable<Pattern> getDateRegexes();

    Pattern getOnRegex();

    Pattern getSpecialDayRegex();

    Pattern getSpecialDayWithNumRegex();

    Pattern getNextRegex();

    Pattern getThisRegex();

    Pattern getLastRegex();

    Pattern getUnitRegex();

    Pattern getWeekDayRegex();

    Pattern getMonthRegex();

    Pattern getWeekDayOfMonthRegex();

    Pattern getForTheRegex();

    Pattern getWeekDayAndDayOfMonthRegex();

    Pattern getRelativeMonthRegex();

    Pattern getStrictRelativeRegex();

    Pattern getYearSuffix();

    Pattern getRelativeWeekDayRegex();

    Pattern getRelativeDayRegex();

    Pattern getNextPrefixRegex();

    Pattern getPastPrefixRegex();

    ImmutableMap<String, String> getUnitMap();

    ImmutableMap<String, Integer> getDayOfMonth();

    ImmutableMap<String, Integer> getDayOfWeek();

    ImmutableMap<String, Integer> getMonthOfYear();

    ImmutableMap<String, Integer> getCardinalMap();

    List<String> getSameDayTerms();

    List<String> getPlusOneDayTerms();

    List<String> getMinusOneDayTerms();

    List<String> getPlusTwoDayTerms();

    List<String> getMinusTwoDayTerms();

    IDateTimeUtilityConfiguration getUtilityConfiguration();

    Integer getSwiftMonthOrYear(String text);

    Boolean isCardinalLast(String text);

    String normalize(String text);
}
