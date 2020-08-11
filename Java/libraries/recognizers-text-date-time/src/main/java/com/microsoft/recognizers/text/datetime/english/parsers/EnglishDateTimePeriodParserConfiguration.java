package com.microsoft.recognizers.text.datetime.english.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.MatchedTimeRangeResult;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class EnglishDateTimePeriodParserConfiguration extends BaseOptionsConfiguration implements IDateTimePeriodParserConfiguration {

    private final String tokenBeforeDate;

    private final IDateTimeExtractor dateExtractor;
    private final IDateTimeExtractor timeExtractor;
    private final IDateTimeExtractor dateTimeExtractor;
    private final IDateTimeExtractor timePeriodExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IExtractor cardinalExtractor;

    private final IParser numberParser;
    private final IDateTimeParser dateParser;
    private final IDateTimeParser timeParser;
    private final IDateTimeParser dateTimeParser;
    private final IDateTimeParser timePeriodParser;
    private final IDateTimeParser durationParser;
    private final IDateTimeParser timeZoneParser;

    private final Pattern pureNumberFromToRegex;
    private final Pattern pureNumberBetweenAndRegex;
    private final Pattern specificTimeOfDayRegex;
    private final Pattern timeOfDayRegex;
    private final Pattern pastRegex;
    private final Pattern futureRegex;
    private final Pattern futureSuffixRegex;
    private final Pattern numberCombinedWithUnitRegex;
    private final Pattern unitRegex;
    private final Pattern periodTimeOfDayWithDateRegex;
    private final Pattern relativeTimeUnitRegex;
    private final Pattern restOfDateTimeRegex;
    private final Pattern amDescRegex;
    private final Pattern pmDescRegex;
    private final Pattern withinNextPrefixRegex;
    private final Pattern prefixDayRegex;
    private final Pattern beforeRegex;
    private final Pattern afterRegex;

    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Integer> numbers;

    public static final Pattern MorningStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MorningStartEndRegex);
    public static final Pattern AfternoonStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AfternoonStartEndRegex);
    public static final Pattern EveningStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.EveningStartEndRegex);
    public static final Pattern NightStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NightStartEndRegex);
    
    public EnglishDateTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        tokenBeforeDate = EnglishDateTime.TokenBeforeDate;

        dateExtractor = config.getDateExtractor();
        timeExtractor = config.getTimeExtractor();
        dateTimeExtractor = config.getDateTimeExtractor();
        timePeriodExtractor = config.getTimePeriodExtractor();
        cardinalExtractor = config.getCardinalExtractor();
        durationExtractor = config.getDurationExtractor();
        numberParser = config.getNumberParser();
        dateParser = config.getDateParser();
        timeParser = config.getTimeParser();
        timePeriodParser = config.getTimePeriodParser();
        durationParser = config.getDurationParser();
        dateTimeParser = config.getDateTimeParser();
        timeZoneParser = config.getTimeZoneParser();

        pureNumberFromToRegex = EnglishTimePeriodExtractorConfiguration.PureNumFromTo;
        pureNumberBetweenAndRegex = EnglishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
        specificTimeOfDayRegex = EnglishDateTimePeriodExtractorConfiguration.PeriodSpecificTimeOfDayRegex;
        timeOfDayRegex = EnglishDateTimeExtractorConfiguration.TimeOfDayRegex;
        pastRegex = EnglishDatePeriodExtractorConfiguration.PreviousPrefixRegex;
        futureRegex = EnglishDatePeriodExtractorConfiguration.NextPrefixRegex;
        futureSuffixRegex = EnglishDatePeriodExtractorConfiguration.FutureSuffixRegex;
        numberCombinedWithUnitRegex = EnglishDateTimePeriodExtractorConfiguration.TimeNumberCombinedWithUnit;
        unitRegex = EnglishTimePeriodExtractorConfiguration.TimeUnitRegex;
        periodTimeOfDayWithDateRegex = EnglishDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex;
        relativeTimeUnitRegex = EnglishDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex;
        restOfDateTimeRegex = EnglishDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex;
        amDescRegex = EnglishDateTimePeriodExtractorConfiguration.AmDescRegex;
        pmDescRegex = EnglishDateTimePeriodExtractorConfiguration.PmDescRegex;
        withinNextPrefixRegex = EnglishDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex;
        prefixDayRegex = EnglishDateTimePeriodExtractorConfiguration.PrefixDayRegex;
        beforeRegex = EnglishDateTimePeriodExtractorConfiguration.BeforeRegex;
        afterRegex = EnglishDateTimePeriodExtractorConfiguration.AfterRegex;

        unitMap = config.getUnitMap();
        numbers = config.getNumbers();
    }

    @Override
    public String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    @Override
    public IDateTimeExtractor getDateExtractor() {
        return dateExtractor;
    }

    @Override
    public IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    @Override
    public IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
    }

    @Override
    public IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    @Override
    public IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    @Override
    public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override
    public IParser getNumberParser() {
        return numberParser;
    }

    @Override
    public IDateTimeParser getDateParser() {
        return dateParser;
    }

    @Override
    public IDateTimeParser getTimeParser() {
        return timeParser;
    }

    @Override
    public IDateTimeParser getDateTimeParser() {
        return dateTimeParser;
    }

    @Override
    public IDateTimeParser getTimePeriodParser() {
        return timePeriodParser;
    }

    @Override
    public IDateTimeParser getDurationParser() {
        return durationParser;
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
    public Pattern getSpecificTimeOfDayRegex() {
        return specificTimeOfDayRegex;
    }

    @Override
    public Pattern getTimeOfDayRegex() {
        return timeOfDayRegex;
    }

    @Override
    public Pattern getPastRegex() {
        return pastRegex;
    }

    @Override
    public Pattern getFutureRegex() {
        return futureRegex;
    }

    @Override
    public Pattern getFutureSuffixRegex() {
        return futureSuffixRegex;
    }

    @Override
    public Pattern getNumberCombinedWithUnitRegex() {
        return numberCombinedWithUnitRegex;
    }

    @Override
    public Pattern getUnitRegex() {
        return unitRegex;
    }

    @Override
    public Pattern getPeriodTimeOfDayWithDateRegex() {
        return periodTimeOfDayWithDateRegex;
    }

    @Override
    public Pattern getRelativeTimeUnitRegex() {
        return relativeTimeUnitRegex;
    }

    @Override
    public Pattern getRestOfDateTimeRegex() {
        return restOfDateTimeRegex;
    }

    @Override
    public Pattern getAmDescRegex() {
        return amDescRegex;
    }

    @Override
    public Pattern getPmDescRegex() {
        return pmDescRegex;
    }

    @Override
    public Pattern getWithinNextPrefixRegex() {
        return withinNextPrefixRegex;
    }

    @Override
    public Pattern getPrefixDayRegex() {
        return prefixDayRegex;
    }

    @Override
    public Pattern getBeforeRegex() {
        return beforeRegex;
    }

    @Override
    public Pattern getAfterRegex() {
        return afterRegex;
    }

    @Override
    public ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    @Override
    public ImmutableMap<String, Integer> getNumbers() {
        return numbers;
    }

    private boolean checkRegex(Pattern regex, String input) {
        return RegExpUtility.getMatches(regex, input).length > 0;
    }

    @Override
    public MatchedTimeRangeResult getMatchedTimeRange(String text, String timeStr, int beginHour, int endHour, int endMin) {

        String trimmedText = text.trim().toLowerCase();
        beginHour = 0;
        endHour = 0;
        endMin = 0;
        timeStr = null;
        boolean result = false;

        if (checkRegex(MorningStartEndRegex, trimmedText)) {
            timeStr = "TMO";
            beginHour = 8;
            endHour = Constants.HalfDayHourCount;
            result = true;
        } else if (checkRegex(AfternoonStartEndRegex, trimmedText)) {
            timeStr = "TAF";
            beginHour = Constants.HalfDayHourCount;
            endHour = 16;
            result = true;
        } else if (checkRegex(EveningStartEndRegex, trimmedText)) {
            timeStr = "TEV";
            beginHour = 16;
            endHour = 20;
            result = true;
        } else if (checkRegex(NightStartEndRegex, trimmedText)) {
            timeStr = "TNI";
            beginHour = 20;
            endHour = 23;
            endMin = 59;
            result = true;
        } else {
            timeStr = null;
        }

        return new MatchedTimeRangeResult(result, timeStr, beginHour, endHour, endMin);
    }

    @Override
    public int getSwiftPrefix(String text) {

        String trimmedText = text.trim().toLowerCase();

        int swift = 0;
        if (trimmedText.startsWith("next")) {
            swift = 1;
        } else if (trimmedText.startsWith("last")) {
            swift = -1;
        }

        return swift;
    }
}
