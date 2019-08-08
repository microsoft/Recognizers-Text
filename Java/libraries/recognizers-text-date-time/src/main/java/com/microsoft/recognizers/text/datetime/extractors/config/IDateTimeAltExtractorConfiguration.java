package com.microsoft.recognizers.text.datetime.extractors.config;

import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;

import java.util.regex.Pattern;

public interface IDateTimeAltExtractorConfiguration {
    IDateExtractor getDateExtractor();

    IDateTimeExtractor getDatePeriodExtractor();

    Iterable<Pattern> getRelativePrefixList();

    Iterable<Pattern> getAmPmRegexList();

    Pattern getOrRegex();

    Pattern getThisPrefixRegex();

    Pattern getDayRegex();

    Pattern getRangePrefixRegex();
}
