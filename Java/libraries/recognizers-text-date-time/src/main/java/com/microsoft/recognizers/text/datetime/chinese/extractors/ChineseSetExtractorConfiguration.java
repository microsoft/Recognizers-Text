package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ISetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class ChineseSetExtractorConfiguration extends BaseOptionsConfiguration implements ISetExtractorConfiguration {

    public static final Pattern SetEachUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetEachUnitRegex);
    public static final Pattern SetEachPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetEachPrefixRegex);
    public static final Pattern SetEachDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetEachDayRegex);
    public static final Pattern SetLastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetLastRegex);
    public static final Pattern SetUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetUnitRegex);

    public ChineseSetExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public ChineseSetExtractorConfiguration(DateTimeOptions options) {
        super(options);
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
        return SetEachDayRegex;
    }

    public final Pattern getSetEachRegex() {
        return SetEachUnitRegex;
    }

    public final Pattern getPeriodicRegex() {
        return null;
    }

    public final Pattern getEachUnitRegex() {
        return SetEachUnitRegex;
    }

    public final Pattern getSetWeekDayRegex() {
        return null;
    }

    public final Pattern getEachPrefixRegex() {
        return SetEachPrefixRegex;
    }
}
