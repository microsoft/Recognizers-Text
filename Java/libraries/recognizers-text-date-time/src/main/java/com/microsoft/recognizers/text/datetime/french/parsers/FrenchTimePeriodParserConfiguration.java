package com.microsoft.recognizers.text.datetime.french.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.MatchedTimeRangeResult;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.TimeOfDayResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.TimexUtility;
import java.util.regex.Pattern;

public class FrenchTimePeriodParserConfiguration extends BaseOptionsConfiguration implements ITimePeriodParserConfiguration {

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

    public FrenchTimePeriodParserConfiguration(final ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        timeExtractor = config.getTimeExtractor();
        integerExtractor = config.getIntegerExtractor();
        timeParser = config.getTimeParser();
        timeZoneParser = config.getTimeZoneParser();
        pureNumberFromToRegex = FrenchTimePeriodExtractorConfiguration.PureNumFromTo;
        pureNumberBetweenAndRegex = FrenchTimePeriodExtractorConfiguration.PureNumBetweenAnd;
        specificTimeFromToRegex = FrenchTimePeriodExtractorConfiguration.SpecificTimeFromTo;
        specificTimeBetweenAndRegex = FrenchTimePeriodExtractorConfiguration.SpecificTimeBetweenAnd;
        timeOfDayRegex = FrenchTimePeriodExtractorConfiguration.TimeOfDayRegex;
        generalEndingRegex = FrenchTimePeriodExtractorConfiguration.GeneralEndingRegex;
        tillRegex = FrenchTimePeriodExtractorConfiguration.TillRegex;
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
    public MatchedTimeRangeResult getMatchedTimexRange(final String text,
                                                       final String timex,
                                                       int beginHour,
                                                       int endHour,
                                                       int endMin) {
        String mutatedText = text.trim();
        if (mutatedText.endsWith("s")) {
            mutatedText = mutatedText.substring(0, mutatedText.length() - 1);
        }

        final String trimmedText = mutatedText;

        beginHour = 0;
        endHour = 0;
        endMin = 0;

        String timeOfDay = "";
        if (FrenchDateTime.MorningTermList.stream().anyMatch(o -> trimmedText.endsWith(o))) {
            timeOfDay = Constants.Morning;
        } else if (FrenchDateTime.AfternoonTermList.stream().anyMatch(o -> trimmedText.endsWith(o))) {
            timeOfDay = Constants.Afternoon;
        } else if (FrenchDateTime.EveningTermList.stream().anyMatch(o -> trimmedText.endsWith(o))) {
            timeOfDay = Constants.Evening;
        } else if (FrenchDateTime.DaytimeTermList.stream().anyMatch(o -> trimmedText.equals(o))) {
            timeOfDay = Constants.Daytime;
        } else if (FrenchDateTime.NightTermList.stream().anyMatch(o -> trimmedText.endsWith(o))) {
            timeOfDay = Constants.Night;
        } else {
            return new MatchedTimeRangeResult(false, null, beginHour, endHour, endMin);
        }

        final TimeOfDayResolutionResult result = TimexUtility.parseTimeOfDay(timeOfDay);

        return new MatchedTimeRangeResult(true, result.getTimex(), result.getBeginHour(), result.getEndHour(),
            result.getEndMin());
    }
}
