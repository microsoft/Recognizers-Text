package com.microsoft.recognizers.text.datetime.spanish.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.MatchedTimeRangeResult;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.TimeOfDayResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.TimexUtility;

import java.util.regex.Pattern;

public class SpanishTimePeriodParserConfiguration extends BaseOptionsConfiguration implements ITimePeriodParserConfiguration {

    private final IDateTimeExtractor timeExtractor;
    private final IDateTimeParser timeParser;
    private final IExtractor integerExtractor;
    private final IDateTimeParser timeZoneParser;

    private final Pattern pureNumberFromToRegex;
    private final Pattern pureNumberBetweenAndRegex;
    private final Pattern specificTimeFromToRegex;
    private final Pattern specificTimeBetweenAndRegex;
    private final Pattern timeOfDayRegex;
    private final Pattern generalEndingRegex;
    private final Pattern tillRegex;

    private final ImmutableMap<String, Integer> numbers;
    private final IDateTimeUtilityConfiguration utilityConfiguration;

    public SpanishTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        timeExtractor = config.getTimeExtractor();
        integerExtractor = config.getIntegerExtractor();
        timeParser = config.getTimeParser();
        timeZoneParser = config.getTimeZoneParser();
        pureNumberFromToRegex = SpanishTimePeriodExtractorConfiguration.PureNumFromTo;
        pureNumberBetweenAndRegex = SpanishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
        specificTimeFromToRegex = SpanishTimePeriodExtractorConfiguration.SpecificTimeFromTo;
        specificTimeBetweenAndRegex = SpanishTimePeriodExtractorConfiguration.SpecificTimeBetweenAnd;
        timeOfDayRegex = SpanishTimePeriodExtractorConfiguration.TimeOfDayRegex;
        generalEndingRegex = SpanishTimePeriodExtractorConfiguration.GeneralEndingRegex;
        tillRegex = SpanishTimePeriodExtractorConfiguration.TillRegex;
        numbers = config.getNumbers();
        utilityConfiguration = config.getUtilityConfiguration();
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
    public IDateTimeParser getTimeZoneParser() {
        return timeZoneParser;
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

        beginHour = 0;
        endHour = 0;
        endMin = 0;

        String timeOfDay = "";

        if (SpanishDateTime.EarlyMorningTermList.stream().anyMatch(trimmedText::endsWith)) {
            timeOfDay = Constants.EarlyMorning;
        } else if (SpanishDateTime.MorningTermList.stream().anyMatch(trimmedText::endsWith)) {
            timeOfDay = Constants.Morning;
        } else if (SpanishDateTime.AfternoonTermList.stream().anyMatch(trimmedText::endsWith)) {
            timeOfDay = Constants.Afternoon;
        } else if (SpanishDateTime.EveningTermList.stream().anyMatch(trimmedText::endsWith)) {
            timeOfDay = Constants.Evening;
        } else if (SpanishDateTime.NightTermList.stream().anyMatch(trimmedText::endsWith)) {
            timeOfDay = Constants.Night;
        } else {
            timex = null;
            return new MatchedTimeRangeResult(false, timex, beginHour, endHour, endMin);
        }

        TimeOfDayResolutionResult result = TimexUtility.parseTimeOfDay(timeOfDay);

        return new MatchedTimeRangeResult(true, result.getTimex(), result.getBeginHour(), result.getEndHour(), result.getEndMin());
    }
}
