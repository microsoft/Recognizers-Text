package com.microsoft.recognizers.text.datetime.parsers.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;

import java.util.regex.Pattern;

public interface ITimeParserConfiguration extends IOptionsConfiguration {
    String getTimeTokenPrefix();

    Pattern getAtRegex();

    Iterable<Pattern> getTimeRegexes();

    ImmutableMap<String, Integer> getNumbers();

    IDateTimeUtilityConfiguration getUtilityConfiguration();

    IDateTimeParser getTimeZoneParser();

    PrefixAdjustResult adjustByPrefix(String prefix, int hour, int min, boolean hasMin);

    SuffixAdjustResult adjustBySuffix(String suffix, int hour, int min, boolean hasMin, boolean hasAm, boolean hasPm);
}
