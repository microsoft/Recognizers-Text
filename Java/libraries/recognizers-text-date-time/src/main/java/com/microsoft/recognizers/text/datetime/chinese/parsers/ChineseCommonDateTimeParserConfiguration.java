package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.utilities.ChineseDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDatePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
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
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.chinese.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.chinese.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.number.chinese.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.chinese.parsers.ChineseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;

public class ChineseCommonDateTimeParserConfiguration extends BaseDateParserConfiguration implements ICommonDateTimeParserConfiguration {

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

    public ChineseCommonDateTimeParserConfiguration(DateTimeOptions options) {

        super(options);

        utilityConfiguration = new ChineseDatetimeUtilityConfiguration();

        unitMap = ChineseDateTime.ParserConfigurationUnitMap;
        unitValueMap = ChineseDateTime.ParserConfigurationUnitValueMap;
        seasonMap = ChineseDateTime.ParserConfigurationSeasonMap;
        specialYearPrefixesMap = ImmutableMap.of();
        cardinalMap = ChineseDateTime.ParserConfigurationCardinalMap;
        dayOfWeekMap = ChineseDateTime.ParserConfigurationDayOfWeek;
        dayOfMonth = ImmutableMap.<String, Integer>builder().putAll(super.getDayOfMonth()).putAll(ChineseDateTime.ParserConfigurationDayOfMonth).build();
        monthOfYear = ChineseDateTime.ParserConfigurationMonthOfYear;
        numbers = ImmutableMap.of();
        doubleNumbers = ImmutableMap.of();
        writtenDecades = ImmutableMap.of();
        specialDecadeCases = ImmutableMap.of();

        cardinalExtractor = new CardinalExtractor();
        integerExtractor = new IntegerExtractor();
        ordinalExtractor = new OrdinalExtractor();
        numberParser = new BaseNumberParser(new ChineseNumberParserConfiguration());

        durationExtractor = new BaseDurationExtractor(new ChineseDurationExtractorConfiguration());
        dateExtractor = new BaseDateExtractor(new ChineseDateExtractorConfiguration(this));
        timeExtractor = new BaseTimeExtractor(new ChineseTimeExtractorConfiguration(options));
        dateTimeExtractor = new BaseDateTimeExtractor(new ChineseDateTimeExtractorConfiguration(options));
        datePeriodExtractor = new BaseDatePeriodExtractor(new ChineseDatePeriodExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new ChineseTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new ChineseDateTimePeriodExtractorConfiguration(options));

        timeZoneParser = new BaseTimeZoneParser();
        durationParser = new BaseDurationParser(new ChineseDurationParserConfiguration(options));
        dateParser = new BaseDateParser(new ChineseDateParserConfiguration(this));
        timeParser = new ChineseTimeParser(new ChineseTimeParserConfiguration(options));
        dateTimeParser = new BaseDateTimeParser(new ChineseDateTimeParserConfiguration(this));
        datePeriodParser = new BaseDatePeriodParser(new ChineseDatePeriodParserConfiguration(options));
        timePeriodParser = new BaseTimePeriodParser(new ChineseTimePeriodParserConfiguration(this));
        dateTimePeriodParser = new BaseDateTimePeriodParser(new ChineseDateTimePeriodParserConfiguration(this));
        dateTimeAltParser = new BaseDateTimeAltParser(new ChineseDateTimeAltParserConfiguration(this));
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
