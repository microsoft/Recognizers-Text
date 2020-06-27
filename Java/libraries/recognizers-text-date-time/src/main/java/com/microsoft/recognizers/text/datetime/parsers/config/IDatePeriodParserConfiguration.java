package com.microsoft.recognizers.text.datetime.parsers.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;

import java.util.regex.Pattern;

public interface IDatePeriodParserConfiguration extends IOptionsConfiguration {
    String getTokenBeforeDate();

    IDateExtractor getDateExtractor();

    IExtractor getCardinalExtractor();

    IExtractor getOrdinalExtractor();

    IExtractor getIntegerExtractor();

    IParser getNumberParser();

    IDateTimeExtractor getDurationExtractor();

    IDateTimeParser getDurationParser();

    IDateTimeParser getDateParser();

    Pattern getMonthFrontBetweenRegex();

    Pattern getBetweenRegex();

    Pattern getMonthFrontSimpleCasesRegex();

    Pattern getSimpleCasesRegex();

    Pattern getOneWordPeriodRegex();

    Pattern getMonthWithYear();

    Pattern getMonthNumWithYear();

    Pattern getYearRegex();

    Pattern getPastRegex();

    Pattern getFutureRegex();

    Pattern getFutureSuffixRegex();

    Pattern getNumberCombinedWithUnit();

    Pattern getWeekOfMonthRegex();

    Pattern getWeekOfYearRegex();

    Pattern getQuarterRegex();

    Pattern getQuarterRegexYearFront();

    Pattern getAllHalfYearRegex();

    Pattern getSeasonRegex();

    Pattern getWhichWeekRegex();

    Pattern getWeekOfRegex();

    Pattern getMonthOfRegex();

    Pattern getInConnectorRegex();

    Pattern getWithinNextPrefixRegex();

    Pattern getNextPrefixRegex();

    Pattern getPastPrefixRegex();

    Pattern getThisPrefixRegex();

    Pattern getRestOfDateRegex();

    Pattern getLaterEarlyPeriodRegex();

    Pattern getWeekWithWeekDayRangeRegex();

    Pattern getYearPlusNumberRegex();

    Pattern getDecadeWithCenturyRegex();

    Pattern getYearPeriodRegex();

    Pattern getComplexDatePeriodRegex();

    Pattern getRelativeDecadeRegex();

    Pattern getReferenceDatePeriodRegex();

    Pattern getAgoRegex();

    Pattern getLaterRegex();

    Pattern getLessThanRegex();

    Pattern getMoreThanRegex();

    Pattern getCenturySuffixRegex();

    Pattern getRelativeRegex();

    Pattern getUnspecificEndOfRangeRegex();

    Pattern getNowRegex();

    ImmutableMap<String, String> getUnitMap();

    ImmutableMap<String, Integer> getCardinalMap();

    ImmutableMap<String, Integer> getDayOfMonth();

    ImmutableMap<String, Integer> getMonthOfYear();

    ImmutableMap<String, String> getSeasonMap();

    ImmutableMap<String, String> getSpecialYearPrefixesMap();

    ImmutableMap<String, Integer> getWrittenDecades();

    ImmutableMap<String, Integer> getNumbers();

    ImmutableMap<String, Integer> getSpecialDecadeCases();

    boolean isFuture(String text);

    boolean isYearToDate(String text);

    boolean isMonthToDate(String text);

    boolean isWeekOnly(String text);

    boolean isWeekend(String text);

    boolean isMonthOnly(String text);

    boolean isYearOnly(String text);

    int getSwiftYear(String text);

    int getSwiftDayOrMonth(String text);

    boolean isLastCardinal(String text);
}
