package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.*;
import com.microsoft.recognizers.text.datetime.extractors.config.ISetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class EnglishSetExtractorConfiguration extends BaseOptionsConfiguration implements ISetExtractorConfiguration {

    public static final Pattern SetLastRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SetLastRegex);
    public static final Pattern EachDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.EachDayRegex);
    public static final Pattern SetEachRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SetEachRegex);
    public static final Pattern PeriodicRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodicRegex);
    public static final Pattern EachUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.EachUnitRegex);
    public static final Pattern SetUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DurationUnitRegex);
    public static final Pattern EachPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.EachPrefixRegex);
    public static final Pattern SetWeekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SetWeekDayRegex, Pattern.CASE_INSENSITIVE);

    public EnglishSetExtractorConfiguration()
    {
        super(DateTimeOptions.None);

        TimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        DateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
        DatePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
        TimePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
        DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration());
    }

    private IDateTimeExtractor TimeExtractor;
    public final IDateTimeExtractor getTimeExtractor() { return TimeExtractor; }

    private IDateTimeExtractor DateExtractor;
    public final IDateTimeExtractor getDateExtractor() { return DateExtractor; }

    private IDateTimeExtractor DurationExtractor;
    public final IDateTimeExtractor getDurationExtractor() { return DurationExtractor; }

    private IDateTimeExtractor DateTimeExtractor;
    public final IDateTimeExtractor getDateTimeExtractor() { return DateTimeExtractor; }

    private IDateTimeExtractor DatePeriodExtractor;
    public final IDateTimeExtractor getDatePeriodExtractor() { return DatePeriodExtractor; }

    private IDateTimeExtractor TimePeriodExtractor;
    public final IDateTimeExtractor getTimePeriodExtractor() { return TimePeriodExtractor; }

    private IDateTimeExtractor DateTimePeriodExtractor;
    public final IDateTimeExtractor getDateTimePeriodExtractor() { return DateTimePeriodExtractor; }

    public final Pattern getLastRegex() { return SetLastRegex; }
    public final Pattern getBeforeEachDayRegex() { return null; }
    public final Pattern getEachDayRegex() { return EachDayRegex; }
    public final Pattern getSetEachRegex() { return SetEachRegex; }
    public final Pattern getPeriodicRegex() { return PeriodicRegex; }
    public final Pattern getEachUnitRegex() { return EachUnitRegex; }
    public final Pattern getSetWeekDayRegex() { return SetWeekDayRegex; }
    public final Pattern getEachPrefixRegex() { return EachPrefixRegex; }
}
