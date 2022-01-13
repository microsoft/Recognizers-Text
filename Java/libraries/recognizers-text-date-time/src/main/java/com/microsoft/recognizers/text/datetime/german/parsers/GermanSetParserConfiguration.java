// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanSetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ISetParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.MatchedTimexResult;

import java.util.Locale;
import java.util.regex.Pattern;

public class GermanSetParserConfiguration extends BaseOptionsConfiguration implements ISetParserConfiguration {

    private IDateTimeParser timeParser;
    private IDateTimeParser dateParser;
    private ImmutableMap<String, String> unitMap;
    private IDateTimeParser dateTimeParser;
    private IDateTimeParser durationParser;
    private IDateTimeExtractor timeExtractor;
    private IDateExtractor dateExtractor;
    private IDateTimeParser datePeriodParser;
    private IDateTimeParser timePeriodParser;
    private IDateTimeExtractor durationExtractor;
    private IDateTimeExtractor dateTimeExtractor;
    private IDateTimeParser dateTimePeriodParser;
    private IDateTimeExtractor datePeriodExtractor;
    private IDateTimeExtractor timePeriodExtractor;
    private IDateTimeExtractor dateTimePeriodExtractor;
    private Pattern eachDayRegex;
    private Pattern setEachRegex;
    private Pattern periodicRegex;
    private Pattern eachUnitRegex;
    private Pattern setWeekDayRegex;
    private Pattern eachPrefixRegex;

    public GermanSetParserConfiguration(ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        durationExtractor = config.getDurationExtractor();
        timeExtractor = config.getTimeExtractor();
        dateExtractor = config.getDateExtractor();
        dateTimeExtractor = config.getDateTimeExtractor();
        datePeriodExtractor = config.getDatePeriodExtractor();
        timePeriodExtractor = config.getTimePeriodExtractor();
        dateTimePeriodExtractor = config.getDateTimePeriodExtractor();

        durationParser = config.getDurationParser();
        timeParser = config.getTimeParser();
        dateParser = config.getDateParser();
        dateTimeParser = config.getDateTimeParser();
        datePeriodParser = config.getDatePeriodParser();
        timePeriodParser = config.getTimePeriodParser();
        dateTimePeriodParser = config.getDateTimePeriodParser();
        unitMap = config.getUnitMap();

        eachPrefixRegex = GermanSetExtractorConfiguration.EachPrefixRegex;
        periodicRegex = GermanSetExtractorConfiguration.PeriodicRegex;
        eachUnitRegex = GermanSetExtractorConfiguration.EachUnitRegex;
        eachDayRegex = GermanSetExtractorConfiguration.EachDayRegex;
        setWeekDayRegex = GermanSetExtractorConfiguration.SetWeekDayRegex;
        setEachRegex = GermanSetExtractorConfiguration.SetEachRegex;
    }

    public final IDateTimeParser getTimeParser() {
        return timeParser;
    }

    public final IDateTimeParser getDateParser() {
        return dateParser;
    }

    public final ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    public final IDateTimeParser getDateTimeParser() {
        return dateTimeParser;
    }

    public final IDateTimeParser getDurationParser() {
        return durationParser;
    }

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    public final IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    public final IDateTimeParser getDatePeriodParser() {
        return datePeriodParser;
    }

    public final IDateTimeParser getTimePeriodParser() {
        return timePeriodParser;
    }

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    public final IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
    }

    public final IDateTimeParser getDateTimePeriodParser() {
        return dateTimePeriodParser;
    }

    public final IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    public final IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    public final IDateTimeExtractor getDateTimePeriodExtractor() {
        return dateTimePeriodExtractor;
    }

    public final Pattern getEachDayRegex() {
        return eachDayRegex;
    }

    public final Pattern getSetEachRegex() {
        return setEachRegex;
    }

    public final Pattern getPeriodicRegex() {
        return periodicRegex;
    }

    public final Pattern getEachUnitRegex() {
        return eachUnitRegex;
    }

    public final Pattern getSetWeekDayRegex() {
        return setWeekDayRegex;
    }

    public final Pattern getEachPrefixRegex() {
        return eachPrefixRegex;
    }

    public MatchedTimexResult getMatchedDailyTimex(String text) {

        MatchedTimexResult result = new MatchedTimexResult();

        String trimmedText = text.trim().toLowerCase(Locale.ROOT);

        float durationLength = 1; // Default value
        float multiplier = 1;
        String durationType;

        if (trimmedText.equals("täglich") ||
                trimmedText.equals("täglicher") ||
                trimmedText.equals("tägliches") ||
                trimmedText.equals("tägliche") ||
                trimmedText.equals("täglichen") ||
                trimmedText.equals("alltäglich") ||
                trimmedText.equals("alltäglicher") ||
                trimmedText.equals("alltägliches") ||
                trimmedText.equals("alltägliche") ||
                trimmedText.equals("alltäglichen") ||
                trimmedText.equals("jeden tag")) {
            result.setTimex("P1D");
        } else if (trimmedText.equals("wöchentlich") ||
                trimmedText.equals("wöchentlicher") ||
                trimmedText.equals("wöchentliches") ||
                trimmedText.equals("wöchentliche") ||
                trimmedText.equals("wöchentlichen") ||
                trimmedText.equals("allwöchentlich") ||
                trimmedText.equals("allwöchentlicher") ||
                trimmedText.equals("allwöchentliches") ||
                trimmedText.equals("allwöchentliche") ||
                trimmedText.equals("allwöchentlichen")) {
            result.setTimex("P1W");
        } else if (trimmedText.equals("monatlich") ||
                trimmedText.equals("monatlicher") ||
                trimmedText.equals("monatliches") ||
                trimmedText.equals("monatliche") ||
                trimmedText.equals("monatlichen") ||
                trimmedText.equals("allmonatlich") ||
                trimmedText.equals("allmonatlicher") ||
                trimmedText.equals("allmonatliches") ||
                trimmedText.equals("allmonatliche") ||
                trimmedText.equals("allmonatlichen")) {
            result.setTimex("P1M");
        } else if (trimmedText.equals("jährlich") ||
                trimmedText.equals("jährlicher") ||
                trimmedText.equals("jährliches") ||
                trimmedText.equals("jährliche") ||
                trimmedText.equals("jährlichen") ||
                trimmedText.equals("alljährlich") ||
                trimmedText.equals("alljährlicher") ||
                trimmedText.equals("alljährliches") ||
                trimmedText.equals("alljährliche") ||
                trimmedText.equals("alljährlichen")) {
            result.setTimex("P1Y");
        }

        if (!"".equals(result.getTimex())) {
            result.setResult(true);
        }

        return result;
    }

    public MatchedTimexResult getMatchedUnitTimex(String text) {

        MatchedTimexResult result = new MatchedTimexResult();
        String trimmedText = text.trim().toLowerCase(Locale.ROOT);

        if (trimmedText.equals("tag")) {
            result.setTimex("P1D");
        } else if (trimmedText.equals("woche")) {
            result.setTimex("P1W");
        } else if (trimmedText.equals("monat")) {
            result.setTimex("P1M");
        } else if (trimmedText.equals("jahr")) {
            result.setTimex("P1Y");
        }

        if (!"".equals(result.getTimex())) {
            result.setResult(true);
        }

        return result;
    }
}
