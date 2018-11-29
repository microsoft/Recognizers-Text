package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDatePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimeAltExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseHolidayExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseSetExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeListExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.number.english.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class EnglishMergedExtractorConfiguration implements IMergedExtractorConfiguration {

    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AfterRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern SinceRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SinceRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.BeforeRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern FromToRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.FromToRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern DateAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateAfterRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern NumberEndingPattern = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberEndingPattern, Pattern.CASE_INSENSITIVE);
    public static final Pattern PrepositionSuffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionSuffixRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern SingleAmbiguousMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SingleAmbiguousMonthRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern UnspecificDatePeriodRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.UnspecificDatePeriodRegex, Pattern.CASE_INSENSITIVE);

    public static final StringMatcher SuperfluousWordMatcher = new StringMatcher();
    private static final Iterable<Pattern> filterWordRegexList = new ArrayList<Pattern>() {
        {
            // one on one
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.OneOnOneRegex, Pattern.CASE_INSENSITIVE));

            // (the)? (day|week|month|year)
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.SingleAmbiguousTermsRegex, Pattern.CASE_INSENSITIVE));
        }
    };

    public final Iterable<Pattern> getFilterWordRegexList() {
        return filterWordRegexList;
    }

    public final StringMatcher getSuperfluousWordMatcher() {
        return SuperfluousWordMatcher;
    }

    private DateTimeOptions options;

    public final DateTimeOptions getOptions() {
        return options;
    }

    private IDateTimeExtractor setExtractor;

    public final IDateTimeExtractor getSetExtractor() {
        return setExtractor;
    }

    private IExtractor integerExtractor;

    public final IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    private IDateTimeExtractor dateExtractor;

    public final IDateTimeExtractor getDateExtractor() {
        return dateExtractor;
    }

    private IDateTimeExtractor timeExtractor;

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    private IDateTimeExtractor holidayExtractor;

    public final IDateTimeExtractor getHolidayExtractor() {
        return holidayExtractor;
    }

    private IDateTimeExtractor dateTimeExtractor;

    public final IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
    }

    private IDateTimeExtractor durationExtractor;

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    private IDateTimeExtractor datePeriodExtractor;

    public final IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    private IDateTimeExtractor timePeriodExtractor;

    public final IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    private IDateTimeZoneExtractor timeZoneExtractor;

    public final IDateTimeZoneExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }

    private IDateTimeListExtractor dateTimeAltExtractor;

    public final IDateTimeListExtractor getDateTimeAltExtractor() {
        return dateTimeAltExtractor;
    }

    private IDateTimeExtractor dateTimePeriodExtractor;

    public final IDateTimeExtractor getDateTimePeriodExtractor() {
        return dateTimePeriodExtractor;
    }

    public EnglishMergedExtractorConfiguration(DateTimeOptions options) {
        this.options = options;

        setExtractor = new BaseSetExtractor(new EnglishSetExtractorConfiguration(options));
        dateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        timeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(options));
        holidayExtractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
        datePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
        dateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(options));
        timeZoneExtractor = new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(options));
        dateTimeAltExtractor = new BaseDateTimeAltExtractor(new EnglishDateTimeAltExtractorConfiguration());
        timePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(options));
        integerExtractor = IntegerExtractor.getInstance();

        if (!this.options.match(DateTimeOptions.EnablePreview)) {
            getSuperfluousWordMatcher().init(EnglishDateTime.SuperfluousWordList);
        }
    }

    public final Pattern getAfterRegex() {
        return AfterRegex;
    }

    public final Pattern getSinceRegex() {
        return SinceRegex;
    }

    public final Pattern getBeforeRegex() {
        return BeforeRegex;
    }

    public final Pattern getFromToRegex() {
        return FromToRegex;
    }

    public final Pattern getDateAfterRegex() {
        return DateAfterRegex;
    }

    public final Pattern getNumberEndingPattern() {
        return NumberEndingPattern;
    }

    public final Pattern getPrepositionSuffixRegex() {
        return PrepositionSuffixRegex;
    }

    public final Pattern getSingleAmbiguousMonthRegex() {
        return SingleAmbiguousMonthRegex;
    }

    public final Pattern getUnspecificDatePeriodRegex() {
        return UnspecificDatePeriodRegex;
    }
}
