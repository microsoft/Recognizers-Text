// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDatePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.utilities.GermanDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseDateParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseDatePeriodParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseDateTimeAltParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseDateTimePeriodParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseDurationParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseTimePeriodParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseTimeZoneParser;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.BaseDateParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.german.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.german.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.number.german.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.german.parsers.GermanNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;

public class GermanCommonDateTimeParserConfiguration extends BaseDateParserConfiguration implements ICommonDateTimeParserConfiguration {

    private final IDateTimeUtilityConfiguration utilityConfiguration;

    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Long> unitValueMap;
    private final ImmutableMap<String, String> seasonMap;
    private final ImmutableMap<String, String> specialYearPrefixesMap;
    private final ImmutableMap<String, Integer> cardinalMap;
    private final ImmutableMap<String, Integer> dayOfWeekMap;
    private final ImmutableMap<String, Integer> dayOfMonth;
    private final ImmutableMap<String, Integer> monthOfYear;
    private final ImmutableMap<String, Integer> numbers;
    private final ImmutableMap<String, Double> doubleNumbers;
    private final ImmutableMap<String, Integer> writtenDecades;
    private final ImmutableMap<String, Integer> specialDecadeCases;

    private final IExtractor cardinalExtractor;
    private final IExtractor integerExtractor;
    private final IExtractor ordinalExtractor;
    private final IParser numberParser;

    private final IDateTimeExtractor durationExtractor;
    private final IDateExtractor dateExtractor;
    private final IDateTimeExtractor timeExtractor;
    private final IDateTimeExtractor dateTimeExtractor;
    private final IDateTimeExtractor datePeriodExtractor;
    private final IDateTimeExtractor timePeriodExtractor;
    private final IDateTimeExtractor dateTimePeriodExtractor;

    private final IDateTimeParser dateParser;
    private final IDateTimeParser timeParser;
    private final IDateTimeParser dateTimeParser;
    private final IDateTimeParser durationParser;
    private final IDateTimeParser datePeriodParser;
    private final IDateTimeParser timePeriodParser;
    private final IDateTimeParser dateTimePeriodParser;
    private final IDateTimeParser dateTimeAltParser;
    private final IDateTimeParser timeZoneParser;

    public GermanCommonDateTimeParserConfiguration(DateTimeOptions options) {

        super(options);

        utilityConfiguration = new GermanDatetimeUtilityConfiguration();

        unitMap = GermanDateTime.UnitMap;
        unitValueMap = GermanDateTime.UnitValueMap;
        seasonMap = GermanDateTime.SeasonMap;
        specialYearPrefixesMap = GermanDateTime.SpecialYearPrefixesMap;
        cardinalMap = GermanDateTime.CardinalMap;
        dayOfWeekMap = GermanDateTime.DayOfWeek;
        dayOfMonth = ImmutableMap.<String, Integer>builder().putAll(super.getDayOfMonth()).putAll(GermanDateTime.DayOfMonth).build();
        monthOfYear = GermanDateTime.MonthOfYear;
        numbers = GermanDateTime.Numbers;
        doubleNumbers = GermanDateTime.DoubleNumbers;
        writtenDecades = GermanDateTime.WrittenDecades;
        specialDecadeCases = GermanDateTime.SpecialDecadeCases;

        cardinalExtractor = CardinalExtractor.getInstance();
        integerExtractor = new IntegerExtractor();
        ordinalExtractor = new OrdinalExtractor();
        numberParser = new BaseNumberParser(new GermanNumberParserConfiguration());

        durationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration());
        dateExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
        timeExtractor = new BaseTimeExtractor(new GermanTimeExtractorConfiguration(options));
        dateTimeExtractor = new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration(options));
        datePeriodExtractor = new BaseDatePeriodExtractor(new GermanDatePeriodExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new GermanDateTimePeriodExtractorConfiguration(options));

        timeZoneParser = new BaseTimeZoneParser();
        durationParser = new BaseDurationParser(new GermanDurationParserConfiguration(this));
        dateParser = new BaseDateParser(new GermanDateParserConfiguration(this));
        timeParser = new TimeParser(new GermanTimeParserConfiguration(this));
        dateTimeParser = new BaseDateTimeParser(new GermanDateTimeParserConfiguration(this));
        datePeriodParser = new BaseDatePeriodParser(new GermanDatePeriodParserConfiguration(this));
        timePeriodParser = new BaseTimePeriodParser(new GermanTimePeriodParserConfiguration(this));
        dateTimePeriodParser = new BaseDateTimePeriodParser(new GermanDateTimePeriodParserConfiguration(this));
        dateTimeAltParser = new BaseDateTimeAltParser(new GermanDateTimeAltParserConfiguration(this));
    }

    @Override
    public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override
    public IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    @Override
    public IExtractor getOrdinalExtractor() {
        return ordinalExtractor;
    }

    @Override
    public IParser getNumberParser() {
        return numberParser;
    }

    @Override
    public IDateExtractor getDateExtractor() {
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
    public IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    @Override
    public IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    @Override
    public IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    @Override
    public IDateTimeExtractor getDateTimePeriodExtractor() {
        return dateTimePeriodExtractor;
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
    public IDateTimeParser getDurationParser() {
        return durationParser;
    }

    @Override
    public IDateTimeParser getDatePeriodParser() {
        return datePeriodParser;
    }

    @Override
    public IDateTimeParser getTimePeriodParser() {
        return timePeriodParser;
    }

    @Override
    public IDateTimeParser getDateTimePeriodParser() {
        return dateTimePeriodParser;
    }

    @Override
    public IDateTimeParser getDateTimeAltParser() {
        return dateTimeAltParser;
    }

    @Override
    public IDateTimeParser getTimeZoneParser() {
        return timeZoneParser;
    }

    @Override
    public ImmutableMap<String, Integer> getMonthOfYear() {
        return monthOfYear;
    }

    @Override
    public ImmutableMap<String, Integer> getNumbers() {
        return numbers;
    }

    @Override
    public ImmutableMap<String, Long> getUnitValueMap() {
        return unitValueMap;
    }

    @Override
    public ImmutableMap<String, String> getSeasonMap() {
        return seasonMap;
    }

    @Override
    public ImmutableMap<String, String> getSpecialYearPrefixesMap() {
        return specialYearPrefixesMap;
    }

    @Override
    public ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    @Override
    public ImmutableMap<String, Integer> getCardinalMap() {
        return cardinalMap;
    }

    @Override
    public ImmutableMap<String, Integer> getDayOfWeek() {
        return dayOfWeekMap;
    }

    @Override
    public ImmutableMap<String, Double> getDoubleNumbers() {
        return doubleNumbers;
    }

    @Override
    public ImmutableMap<String, Integer> getWrittenDecades() {
        return writtenDecades;
    }

    @Override
    public ImmutableMap<String, Integer> getSpecialDecadeCases() {
        return specialDecadeCases;
    }

    @Override
    public IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }

    @Override
    public ImmutableMap<String, Integer> getDayOfMonth() {
        return dayOfMonth;
    }
}
