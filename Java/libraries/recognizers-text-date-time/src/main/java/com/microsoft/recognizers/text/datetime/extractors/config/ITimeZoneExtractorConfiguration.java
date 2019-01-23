package com.microsoft.recognizers.text.datetime.extractors.config;

import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.matcher.StringMatcher;

import java.util.List;
import java.util.regex.Pattern;

public interface ITimeZoneExtractorConfiguration extends IOptionsConfiguration {
    Pattern getDirectUtcRegex();

    Pattern getLocationTimeSuffixRegex();

    StringMatcher getLocationMatcher();

    StringMatcher getTimeZoneMatcher();

    List<String> getAmbiguousTimezoneList();
}
