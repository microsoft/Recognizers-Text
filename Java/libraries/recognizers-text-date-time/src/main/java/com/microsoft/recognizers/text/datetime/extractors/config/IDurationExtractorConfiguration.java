package com.microsoft.recognizers.text.datetime.extractors.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;

import java.util.regex.Pattern;

public interface IDurationExtractorConfiguration extends IOptionsConfiguration {
    Pattern getFollowedUnit();

    Pattern getNumberCombinedWithUnit();

    Pattern getAnUnitRegex();

    Pattern getDuringRegex();

    Pattern getAllRegex();

    Pattern getHalfRegex();

    Pattern getSuffixAndRegex();

    Pattern getConjunctionRegex();

    Pattern getInexactNumberRegex();

    Pattern getInexactNumberUnitRegex();

    Pattern getRelativeDurationUnitRegex();

    Pattern getDurationUnitRegex();

    Pattern getDurationConnectorRegex();

    Pattern getLessThanRegex();

    Pattern getMoreThanRegex();

    IExtractor getCardinalExtractor();

    ImmutableMap<String, String> getUnitMap();

    ImmutableMap<String, Long> getUnitValueMap();
}
