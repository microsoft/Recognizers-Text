package com.microsoft.recognizers.text.datetime.english.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.utilities.EnglishDatetimeUtilityConfiguration;
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
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.english.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.english.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.number.english.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.english.parsers.EnglishNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;

public class EnglishCommonDateTimeParserConfiguration extends BaseDateParserConfiguration implements ICommonDateTimeParserConfiguration {

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

    public EnglishCommonDateTimeParserConfiguration(DateTimeOptions options) {

        super(options);

        utilityConfiguration = new EnglishDatetimeUtilityConfiguration();

        unitMap = EnglishDateTime.UnitMap;
        unitValueMap = EnglishDateTime.UnitValueMap;
        seasonMap = EnglishDateTime.SeasonMap;
        specialYearPrefixesMap = EnglishDateTime.SpecialYearPrefixesMap;
        cardinalMap = EnglishDateTime.CardinalMap;
        dayOfWeekMap = EnglishDateTime.DayOfWeek;
        dayOfMonth = ImmutableMap.<String, Integer>builder().putAll(super.getDayOfMonth()).putAll(EnglishDateTime.DayOfMonth).build();
        monthOfYear = EnglishDateTime.MonthOfYear;
        numbers = EnglishDateTime.Numbers;
        doubleNumbers = EnglishDateTime.DoubleNumbers;
        writtenDecades = EnglishDateTime.WrittenDecades;
        specialDecadeCases = EnglishDateTime.SpecialDecadeCases;

        cardinalExtractor = CardinalExtractor.getInstance();
        integerExtractor = IntegerExtractor.getInstance();
        ordinalExtractor = OrdinalExtractor.getInstance();
        numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());

        durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        dateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(this));
        timeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(options));
        dateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(options));
        datePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(options));

        timeZoneParser = new BaseTimeZoneParser();
        durationParser = new BaseDurationParser(new EnglishDurationParserConfiguration(this));
        dateParser = new BaseDateParser(new EnglishDateParserConfiguration(this));
        timeParser = new TimeParser(new EnglishTimeParserConfiguration(this));
        dateTimeParser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(this));
        datePeriodParser = new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(this));
        timePeriodParser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(this));
        dateTimePeriodParser = new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(this));
        dateTimeAltParser = new BaseDateTimeAltParser(new EnglishDateTimeAltParserConfiguration(this));
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
