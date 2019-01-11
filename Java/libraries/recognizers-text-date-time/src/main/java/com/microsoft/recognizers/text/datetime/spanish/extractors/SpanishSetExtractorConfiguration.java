package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDatePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ISetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class SpanishSetExtractorConfiguration extends BaseOptionsConfiguration implements ISetExtractorConfiguration {

    public static final Pattern PeriodicRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PeriodicRegex);
    public static final Pattern EachUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.EachUnitRegex);
    public static final Pattern EachPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.EachPrefixRegex);
    public static final Pattern EachDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.EachDayRegex);
    public static final Pattern BeforeEachDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BeforeEachDayRegex);
    public static final Pattern SetWeekDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SetWeekDayRegex);
    public static final Pattern SetEachRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SetEachRegex);

    public SpanishSetExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public SpanishSetExtractorConfiguration(DateTimeOptions options) {
        super(options);

        durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        timeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(options));
        dateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
        dateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(options));
        datePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration(options));
    }

    private IDateTimeExtractor durationExtractor;

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    private IDateTimeExtractor timeExtractor;

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    private IDateExtractor dateExtractor;

    public final IDateTimeExtractor getDateExtractor() {
        return dateExtractor;
    }

    private IDateTimeExtractor dateTimeExtractor;

    public final IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
    }

    private IDateTimeExtractor datePeriodExtractor;

    public final IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    private IDateTimeExtractor timePeriodExtractor;

    public final IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    private IDateTimeExtractor dateTimePeriodExtractor;

    public final IDateTimeExtractor getDateTimePeriodExtractor() {
        return dateTimePeriodExtractor;
    }

    public final Pattern getLastRegex() {
        return SpanishDateExtractorConfiguration.LastDateRegex;
    }

    public final Pattern getEachPrefixRegex() {
        return EachPrefixRegex;
    }

    public final Pattern getPeriodicRegex() {
        return PeriodicRegex;
    }

    public final Pattern getEachUnitRegex() {
        return EachUnitRegex;
    }

    public final Pattern getEachDayRegex() {
        return EachDayRegex;
    }

    public final Pattern getBeforeEachDayRegex() {
        return BeforeEachDayRegex;
    }

    public final Pattern getSetWeekDayRegex() {
        return SetWeekDayRegex;
    }

    public final Pattern getSetEachRegex() {
        return SetEachRegex;
    }
}
