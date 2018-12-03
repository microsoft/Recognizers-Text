package com.microsoft.recognizers.text.datetime.extractors.config;

import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;

import java.util.regex.Pattern;

public interface ITimeExtractorConfiguration extends IOptionsConfiguration {
    IDateTimeExtractor getTimeZoneExtractor();

    Iterable<Pattern> getTimeRegexList();

    Pattern getAtRegex();

    Pattern getIshRegex();

    Pattern getTimeBeforeAfterRegex();
}
