package com.microsoft.recognizers.text.datetime.english.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.MatchedTimeRangeResult;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;

import java.util.regex.Pattern;

public class EnglishTimePeriodParserConfiguration extends BaseOptionsConfiguration implements ITimePeriodParserConfiguration {

    private final IDateTimeExtractor timeExtractor;
    private final IDateTimeParser timeParser;
    private final IExtractor integerExtractor;

    private final Pattern specificTimeFromToRegex;
    private final Pattern specificTimeBetweenAndRegex;
    private final Pattern pureNumberFromToRegex;
    private final Pattern pureNumberBetweenAndRegex;
    private final Pattern timeOfDayRegex;
    private final Pattern generalEndingRegex;
    private final Pattern tillRegex;

    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final ImmutableMap<String, Integer> numbers;

    public EnglishTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        timeExtractor = config.getTimeExtractor();
        integerExtractor = config.getIntegerExtractor();
        timeParser = config.getTimeParser();
        numbers = config.getNumbers();
        utilityConfiguration = config.getUtilityConfiguration();

        pureNumberFromToRegex = EnglishTimePeriodExtractorConfiguration.PureNumFromTo;
        pureNumberBetweenAndRegex = EnglishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
        specificTimeFromToRegex = EnglishTimePeriodExtractorConfiguration.SpecificTimeFromTo;
        specificTimeBetweenAndRegex = EnglishTimePeriodExtractorConfiguration.SpecificTimeBetweenAnd;
        timeOfDayRegex = EnglishTimePeriodExtractorConfiguration.TimeOfDayRegex;

        generalEndingRegex = EnglishTimePeriodExtractorConfiguration.GeneralEndingRegex;
        tillRegex = EnglishTimePeriodExtractorConfiguration.TillRegex;
    }

    @Override
    public IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    @Override
    public IDateTimeParser getTimeParser() {
        return timeParser;
    }

    @Override
    public IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    @Override
    public Pattern getPureNumberFromToRegex() {
        return pureNumberFromToRegex;
    }

    @Override
    public Pattern getPureNumberBetweenAndRegex() {
        return pureNumberBetweenAndRegex;
    }

    @Override
    public Pattern getSpecificTimeFromToRegex() {
        return specificTimeFromToRegex;
    }

    @Override
    public Pattern getSpecificTimeBetweenAndRegex() {
        return specificTimeBetweenAndRegex;
    }

    @Override
    public Pattern getTimeOfDayRegex() {
        return timeOfDayRegex;
    }

    @Override
    public Pattern getGeneralEndingRegex() {
        return generalEndingRegex;
    }

    @Override
    public Pattern getTillRegex() {
        return tillRegex;
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
    public MatchedTimeRangeResult getMatchedTimexRange(String text, String timex, int beginHour, int endHour, int endMin) {

        String trimmedText = text.trim().toLowerCase();
        if (trimmedText.endsWith("s")) {
            trimmedText = trimmedText.substring(0, trimmedText.length() - 1);
        }

        beginHour = 0;
        endHour = 0;
        endMin = 0;
        if (trimmedText.endsWith("morning")) {
            timex = "TMO";
            beginHour = 8;
            endHour = Constants.HalfDayHourCount;
        } else if (trimmedText.endsWith("afternoon")) {
            timex = "TAF";
            beginHour = Constants.HalfDayHourCount;
            endHour = 16;
        } else if (trimmedText.endsWith("evening")) {
            timex = "TEV";
            beginHour = 16;
            endHour = 20;
        } else if (trimmedText.equals("daytime")) {
            timex = "TDT";
            beginHour = 8;
            endHour = 18;
        } else if (trimmedText.endsWith("night")) {
            timex = "TNI";
            beginHour = 20;
            endHour = 23;
            endMin = 59;
        } else {
            timex = null;
        }

        return new MatchedTimeRangeResult(true, timex, beginHour, endHour, endMin);
    }
}
