// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.extractors;

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
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class GermanSetExtractorConfiguration extends BaseOptionsConfiguration implements ISetExtractorConfiguration {

    public static final Pattern SetLastRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SetLastRegex);
    public static final Pattern EachDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.EachDayRegex);
    public static final Pattern SetEachRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SetEachRegex);
    public static final Pattern PeriodicRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PeriodicRegex);
    public static final Pattern EachUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.EachUnitRegex);
    public static final Pattern SetUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DurationUnitRegex);
    public static final Pattern EachPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.EachPrefixRegex);
    public static final Pattern SetWeekDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SetWeekDayRegex);

    public GermanSetExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public GermanSetExtractorConfiguration(DateTimeOptions options) {
        super(options);

        timeExtractor = new BaseTimeExtractor(new GermanTimeExtractorConfiguration(options));
        dateExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
        durationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration());
        dateTimeExtractor = new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration(options));
        datePeriodExtractor = new BaseDatePeriodExtractor(new GermanDatePeriodExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new GermanDateTimePeriodExtractorConfiguration(options));
    }

    private IDateTimeExtractor timeExtractor;

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    private IDateExtractor dateExtractor;

    public final IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    private IDateTimeExtractor durationExtractor;

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
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
        return SetLastRegex;
    }

    public final Pattern getBeforeEachDayRegex() {
        return null;
    }

    public final Pattern getEachDayRegex() {
        return EachDayRegex;
    }

    public final Pattern getSetEachRegex() {
        return SetEachRegex;
    }

    public final Pattern getPeriodicRegex() {
        return PeriodicRegex;
    }

    public final Pattern getEachUnitRegex() {
        return EachUnitRegex;
    }

    public final Pattern getSetWeekDayRegex() {
        return SetWeekDayRegex;
    }

    public final Pattern getEachPrefixRegex() {
        return EachPrefixRegex;
    }
}
