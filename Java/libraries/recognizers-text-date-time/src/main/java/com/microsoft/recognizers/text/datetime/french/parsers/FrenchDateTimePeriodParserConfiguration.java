package com.microsoft.recognizers.text.datetime.french.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.MatchedTimeRangeResult;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.regex.Pattern;

public class FrenchDateTimePeriodParserConfiguration extends BaseOptionsConfiguration implements IDateTimePeriodParserConfiguration {

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

    public FrenchDateTimePeriodParserConfiguration(final ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        tokenBeforeDate = FrenchDateTime.TokenBeforeDate;

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

        pureNumberFromToRegex = FrenchTimePeriodExtractorConfiguration.PureNumFromTo;
        pureNumberBetweenAndRegex = FrenchTimePeriodExtractorConfiguration.PureNumBetweenAnd;
        specificTimeOfDayRegex = FrenchDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
        timeOfDayRegex = FrenchDateTimeExtractorConfiguration.TimeOfDayRegex;
        pastRegex = FrenchDatePeriodExtractorConfiguration.PastRegex;
        futureRegex = FrenchDatePeriodExtractorConfiguration.FutureRegex;
        futureSuffixRegex = FrenchDatePeriodExtractorConfiguration.FutureSuffixRegex;
        numberCombinedWithUnitRegex = FrenchDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit;
        unitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeUnitRegex);
        periodTimeOfDayWithDateRegex = FrenchDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex;
        relativeTimeUnitRegex = FrenchDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex;
        restOfDateTimeRegex = FrenchDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex;
        amDescRegex = FrenchDateTimePeriodExtractorConfiguration.AmDescRegex;
        pmDescRegex = FrenchDateTimePeriodExtractorConfiguration.PmDescRegex;
        withinNextPrefixRegex = FrenchDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex;
        prefixDayRegex = FrenchDateTimePeriodExtractorConfiguration.PrefixDayRegex;
        beforeRegex = FrenchDateTimePeriodExtractorConfiguration.BeforeRegex;
        afterRegex = FrenchDateTimePeriodExtractorConfiguration.AfterRegex;

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

    @Override
    public MatchedTimeRangeResult getMatchedTimeRange(final String text,
                                                      String timeStr,
                                                      int beginHour,
                                                      int endHour,
                                                      int endMin) {
        beginHour = 0;
        endHour = 0;
        endMin = 0;

        final String trimmedText = text.trim().toLowerCase();

        if (RegExpUtility
            .getMatches(RegExpUtility.getSafeRegExp(FrenchDateTime.MorningStartEndRegex), trimmedText).length > 0) {
            timeStr = "TMO";
            beginHour = 8;
            endHour = Constants.HalfDayHourCount;
        } else if (RegExpUtility
            .getMatches(RegExpUtility.getSafeRegExp(FrenchDateTime.AfternoonStartEndRegex), trimmedText).length
            > 0) {
            timeStr = "TAF";
            beginHour = Constants.HalfDayHourCount;
            endHour = 16;
        } else if (RegExpUtility
            .getMatches(RegExpUtility.getSafeRegExp(FrenchDateTime.EveningStartEndRegex), trimmedText).length > 0) {
            timeStr = "TEV";
            beginHour = 16;
            endHour = 20;
        } else if (RegExpUtility
            .getMatches(RegExpUtility.getSafeRegExp(FrenchDateTime.NightStartEndRegex), trimmedText).length > 0) {
            timeStr = "TNI";
            beginHour = 20;
            endHour = 23;
            endMin = 59;
        } else {
            return new MatchedTimeRangeResult(false, null, beginHour, endHour, endMin);
        }

        return new MatchedTimeRangeResult(true, timeStr, beginHour, endHour, endMin);
    }

    @Override
    public int getSwiftPrefix(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        int swift = 0;

        if (trimmedText.startsWith("prochain") || trimmedText.endsWith("prochain") ||
            trimmedText.startsWith("prochaine") || trimmedText.endsWith("prochaine")) {
            swift = 1;
        } else if (trimmedText.startsWith("derniere") || trimmedText.startsWith("dernier") ||
            trimmedText.endsWith("derniere") || trimmedText.endsWith("dernier")) {
            swift = -1;
        }

        return swift;
    }
}
