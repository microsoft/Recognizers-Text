package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.MatchedTimeRangeResult;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.TimeOfDayResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.TimexUtility;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class ChineseTimePeriodParserConfiguration extends BaseOptionsConfiguration implements ITimePeriodParserConfiguration {

    private final IDateTimeExtractor timeExtractor;
    private final IDateTimeParser timeParser;
    private final IExtractor integerExtractor;
    private final IDateTimeParser timeZoneParser;

    private final Pattern timeOfDayRegex;
    private final Pattern tillRegex;

    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final ImmutableMap<String, Integer> numbers;
    private final ImmutableMap<String, Integer> timeLowBoundDesc;

    public ChineseTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) {
        super(config.getOptions());

        timeExtractor = config.getTimeExtractor();
        integerExtractor = config.getIntegerExtractor();
        timeParser = config.getTimeParser();
        timeZoneParser = config.getTimeZoneParser();
        numbers = config.getNumbers();
        utilityConfiguration = config.getUtilityConfiguration();

        timeOfDayRegex = ChineseTimePeriodExtractorConfiguration.TimeOfDayRegex;
        tillRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimePeriodTimePeriodConnectWords);
        timeLowBoundDesc = ChineseDateTime.TimeLowBoundDesc;
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
        return null;
    }

    @Override
    public Pattern getPureNumberBetweenAndRegex() {
        return null;
    }

    @Override
    public Pattern getSpecificTimeFromToRegex() {
        return null;
    }

    @Override
    public Pattern getSpecificTimeBetweenAndRegex() {
        return null;
    }

    @Override
    public Pattern getTimeOfDayRegex() {
        return timeOfDayRegex;
    }

    @Override
    public Pattern getGeneralEndingRegex() {
        return null;
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

    public ImmutableMap<String, Integer> getTimeLowBoundDesc() {
        return timeLowBoundDesc;
    }

    @Override
    public MatchedTimeRangeResult getMatchedTimexRange(String text, String timex, int beginHour, int endHour, int endMin) {
        String trimmedText = text.trim();

        beginHour = 0;
        endHour = 0;
        endMin = 0;

        String timeOfDay = "";

        if (ChineseDateTime.MorningTermList.stream().anyMatch(trimmedText::endsWith)) {
            timeOfDay = Constants.Morning;
        } else if (ChineseDateTime.MidDayTermList.stream().anyMatch(trimmedText::endsWith)) {
            timeOfDay = Constants.MidDay;
        } else if (ChineseDateTime.AfternoonTermList.stream().anyMatch(trimmedText::endsWith)) {
            timeOfDay = Constants.Afternoon;
        } else if (ChineseDateTime.EveningTermList.stream().anyMatch(trimmedText::endsWith)) {
            timeOfDay = Constants.Evening;
        } else if (ChineseDateTime.DaytimeTermList.stream().anyMatch(trimmedText::equals)) {
            timeOfDay = Constants.Daytime;
        } else if (ChineseDateTime.NightTermList.stream().anyMatch(trimmedText::endsWith)) {
            timeOfDay = Constants.Night;
        } else {
            timex = null;
            return new MatchedTimeRangeResult(false, timex, beginHour, endHour, endMin);
        }

        TimeOfDayResolutionResult result = TimexUtility.parseTimeOfDay(timeOfDay);

        return new MatchedTimeRangeResult(true, result.getTimex(), result.getBeginHour(), result.getEndHour(), result.getEndMin());
    }
}
