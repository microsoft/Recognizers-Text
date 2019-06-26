package com.microsoft.recognizers.text.datetime.spanish.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.english.parsers.TimeParser;
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
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.utilities.SpanishDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.number.spanish.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.spanish.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.number.spanish.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.spanish.parsers.SpanishNumberParserConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class SpanishCommonDateTimeParserConfiguration extends BaseDateParserConfiguration implements ICommonDateTimeParserConfiguration {

    private final IDateTimeUtilityConfiguration utilityConfiguration;

    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Long> unitValueMap;
    private final ImmutableMap<String, String> seasonMap;
    private final ImmutableMap<String, String> specialYearPrefixesMap;
    private final ImmutableMap<String, Integer> cardinalMap;
    private final ImmutableMap<String, Integer> dayOfWeek;
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

    private final IDateTimeParser timeZoneParser;
    private final IDateTimeParser dateParser;
    private final IDateTimeParser timeParser;
    private final IDateTimeParser dateTimeParser;
    private final IDateTimeParser durationParser;
    private final IDateTimeParser datePeriodParser;
    private final IDateTimeParser timePeriodParser;
    private final IDateTimeParser dateTimePeriodParser;
    private final IDateTimeParser dateTimeAltParser;

    public SpanishCommonDateTimeParserConfiguration(DateTimeOptions options) {

        super(options);

        utilityConfiguration = new SpanishDatetimeUtilityConfiguration();

        unitMap = SpanishDateTime.UnitMap;
        unitValueMap = SpanishDateTime.UnitValueMap;
        seasonMap = SpanishDateTime.SeasonMap;
        specialYearPrefixesMap = SpanishDateTime.SpecialYearPrefixesMap;
        cardinalMap = SpanishDateTime.CardinalMap;
        dayOfWeek = SpanishDateTime.DayOfWeek;
        monthOfYear = SpanishDateTime.MonthOfYear;
        numbers = SpanishDateTime.Numbers;
        doubleNumbers = SpanishDateTime.DoubleNumbers;
        writtenDecades = SpanishDateTime.WrittenDecades;
        specialDecadeCases = SpanishDateTime.SpecialDecadeCases;

        cardinalExtractor = CardinalExtractor.getInstance();
        integerExtractor = IntegerExtractor.getInstance();
        ordinalExtractor = OrdinalExtractor.getInstance();

        numberParser = new BaseNumberParser(new SpanishNumberParserConfiguration());

        dateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
        timeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(options));
        dateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        datePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration(options));

        timeZoneParser = new BaseTimeZoneParser();
        dateParser = new BaseDateParser(new SpanishDateParserConfiguration(this));
        timeParser = new TimeParser(new SpanishTimeParserConfiguration(this));
        dateTimeParser = new BaseDateTimeParser(new SpanishDateTimeParserConfiguration(this));
        durationParser = new BaseDurationParser(new SpanishDurationParserConfiguration(this));
        datePeriodParser = new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(this));
        timePeriodParser = new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(this));
        dateTimePeriodParser = new BaseDateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(this));
        dateTimeAltParser = new BaseDateTimeAltParser(new SpanishDateTimeAltParserConfiguration(this));
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

    @Override public IDateTimeParser getTimeZoneParser() {
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
        return dayOfWeek;
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
}
