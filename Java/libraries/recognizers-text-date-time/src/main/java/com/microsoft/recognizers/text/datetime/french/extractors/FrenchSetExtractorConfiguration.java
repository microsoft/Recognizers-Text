package com.microsoft.recognizers.text.datetime.french.extractors;

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
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.regex.Pattern;

public class FrenchSetExtractorConfiguration extends BaseOptionsConfiguration implements ISetExtractorConfiguration {

    public static final Pattern PeriodicRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PeriodicRegex);
    public static final Pattern EachUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.EachUnitRegex);
    public static final Pattern EachPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.EachPrefixRegex);
    public static final Pattern EachDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.EachDayRegex);
    // TODO
    public static final Pattern BeforeEachDayRegex = null;
    public static final Pattern SetWeekDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SetWeekDayRegex);
    public static final Pattern SetEachRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SetEachRegex);
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeExtractor timeExtractor;
    private final IDateExtractor dateExtractor;
    private final IDateTimeExtractor dateTimeExtractor;
    private final IDateTimeExtractor datePeriodExtractor;
    private final IDateTimeExtractor timePeriodExtractor;
    private final IDateTimeExtractor dateTimePeriodExtractor;

    public FrenchSetExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public FrenchSetExtractorConfiguration(final DateTimeOptions options) {
        super(options);

        durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        timeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration(options));
        dateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(this));
        dateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration(options));
        datePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(
            new FrenchDateTimePeriodExtractorConfiguration(options));
    }

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    public final IDateTimeExtractor getDateExtractor() {
        return dateExtractor;
    }

    public final IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
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

    public final Pattern getLastRegex() {
        return FrenchDateExtractorConfiguration.LastDateRegex;
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
