package com.microsoft.recognizers.text.datetime.extractors.config;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;

import java.util.regex.Pattern;

public interface IDateTimeExtractorConfiguration extends IOptionsConfiguration {
    Pattern getNowRegex();

    Pattern getSuffixRegex();

    Pattern getTimeOfTodayAfterRegex();

    Pattern getSimpleTimeOfTodayAfterRegex();

    Pattern getTimeOfTodayBeforeRegex();

    Pattern getSimpleTimeOfTodayBeforeRegex();

    Pattern getTimeOfDayRegex();

    Pattern getSpecificEndOfRegex();

    Pattern getUnspecificEndOfRegex();

    Pattern getUnitRegex();

    Pattern getNumberAsTimeRegex();

    Pattern getDateNumberConnectorRegex();

    Pattern getSuffixAfterRegex();

    IDateTimeExtractor getDurationExtractor();

    IDateTimeExtractor getDatePointExtractor();

    IDateTimeExtractor getTimePointExtractor();

    IExtractor getIntegerExtractor();

    boolean isConnector(String text);

    IDateTimeUtilityConfiguration getUtilityConfiguration();
}
