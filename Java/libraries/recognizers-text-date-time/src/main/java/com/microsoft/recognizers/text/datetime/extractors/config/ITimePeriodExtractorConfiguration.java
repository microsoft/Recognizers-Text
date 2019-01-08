package com.microsoft.recognizers.text.datetime.extractors.config;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;

import java.util.regex.Pattern;

public interface ITimePeriodExtractorConfiguration extends IOptionsConfiguration {
    String getTokenBeforeDate();

    IExtractor getIntegerExtractor();

    Iterable<Pattern> getSimpleCasesRegex();

    Pattern getTillRegex();

    Pattern getTimeOfDayRegex();

    Pattern getGeneralEndingRegex();

    IDateTimeExtractor getSingleTimeExtractor();

    ResultIndex getFromTokenIndex(String text);

    boolean hasConnectorToken(String text);

    ResultIndex getBetweenTokenIndex(String text);

    IDateTimeExtractor getTimeZoneExtractor();
}
