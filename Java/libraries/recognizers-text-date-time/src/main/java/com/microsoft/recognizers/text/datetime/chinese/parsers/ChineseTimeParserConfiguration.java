package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.utilities.ChineseDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseTimeZoneParser;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.PrefixAdjustResult;
import com.microsoft.recognizers.text.datetime.parsers.config.SuffixAdjustResult;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;

import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class ChineseTimeParserConfiguration extends BaseOptionsConfiguration implements ITimeParserConfiguration {

    private final ImmutableMap<String, Integer> numbers;
    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final IDateTimeParser timeZoneParser;
    private final Iterable<Pattern> timeRegexes;

    public ChineseTimeParserConfiguration() {
        this(DateTimeOptions.None);
    }

    public ChineseTimeParserConfiguration(DateTimeOptions options) {
        super(options);

        Map<String, Integer> numberMap = new HashMap<>();
        for (Map.Entry<Character, Integer> entry : ChineseDateTime.TimeNumberDictionary.entrySet()) {
            numberMap.put(String.valueOf(entry.getKey()), entry.getValue());
        }
        numbers = ImmutableMap.copyOf(numberMap);

        utilityConfiguration = new ChineseDatetimeUtilityConfiguration();
        timeZoneParser = new BaseTimeZoneParser();
        timeRegexes = ChineseTimeExtractorConfiguration.TimeRegexList;
    }

    @Override
    public String getTimeTokenPrefix() {
        return "";
    }

    @Override
    public Pattern getAtRegex() {
        return null;
    }

    @Override
    public Iterable<Pattern> getTimeRegexes() {
        return timeRegexes;
    }

    @Override
    public ImmutableMap<String, Integer> getNumbers() {
        return numbers;
    }

    @Override
    public IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }

    @Override
    public IDateTimeParser getTimeZoneParser() {
        return timeZoneParser;
    }

    @Override
    public PrefixAdjustResult adjustByPrefix(String prefix, int hour, int min, boolean hasMin) {
        return new PrefixAdjustResult(hour, min, hasMin);
    }

    @Override
    public SuffixAdjustResult adjustBySuffix(String suffix, int hour, int min, boolean hasMin, boolean hasAm, boolean hasPm) {
        return new SuffixAdjustResult(hour, min, hasMin, hasAm, hasPm);
    }
}
