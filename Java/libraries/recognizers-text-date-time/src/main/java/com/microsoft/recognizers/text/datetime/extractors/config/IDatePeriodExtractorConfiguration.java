package com.microsoft.recognizers.text.datetime.extractors.config;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;

import java.util.regex.Pattern;

public interface IDatePeriodExtractorConfiguration {
    Iterable<Pattern> getSimpleCasesRegexes();
    Pattern getYearRegex();
    Pattern getTillRegex();
    Pattern getDateUnitRegex();
    Pattern getTimeUnitRegex();
    Pattern getFollowedDateUnit();
    Pattern getNumberCombinedWithDateUnit();
    Pattern getPastRegex();
    Pattern getFutureRegex();
    Pattern getFutureSuffixRegex();
    Pattern getWeekOfRegex();
    Pattern getMonthOfRegex();
    Pattern getRangeUnitRegex();
    Pattern getInConnectorRegex();
    Pattern getWithinNextPrefixRegex();
    Pattern getYearPeriodRegex();
    Pattern getRelativeDecadeRegex();
    Pattern getComplexDatePeriodRegex();
    Pattern getReferenceDatePeriodRegex();
    Pattern getAgoRegex();
    Pattern getLaterRegex();
    Pattern getLessThanRegex();
    Pattern getMoreThanRegex();
    Pattern getCenturySuffixRegex();
    IDateTimeExtractor getDatePointExtractor();
    IExtractor getCardinalExtractor();
    IExtractor getOrdinalExtractor();
    IDateTimeExtractor getDurationExtractor();
    IParser getNumberParser();
    ResultIndex GetFromTokenIndex(String text);
    boolean HasConnectorToken(String text);
    ResultIndex GetBetweenTokenIndex(String text);
    String[] getDurationDateRestrictions();
}