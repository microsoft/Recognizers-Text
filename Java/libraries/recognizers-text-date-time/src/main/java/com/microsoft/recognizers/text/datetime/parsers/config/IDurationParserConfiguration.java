package com.microsoft.recognizers.text.datetime.parsers.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;

import java.util.regex.Pattern;

public interface IDurationParserConfiguration extends IOptionsConfiguration {
    IExtractor getCardinalExtractor();

    IExtractor getDurationExtractor();

    IParser getNumberParser();

    Pattern getNumberCombinedWithUnit();

    Pattern getAnUnitRegex();

    Pattern getDuringRegex();

    Pattern getAllDateUnitRegex();

    Pattern getHalfDateUnitRegex();

    Pattern getSuffixAndRegex();

    Pattern getFollowedUnit();

    Pattern getConjunctionRegex();

    Pattern getInexactNumberRegex();

    Pattern getInexactNumberUnitRegex();

    Pattern getDurationUnitRegex();

    ImmutableMap<String, String> getUnitMap();

    ImmutableMap<String, Long> getUnitValueMap();

    ImmutableMap<String, Double> getDoubleNumbers();
}
