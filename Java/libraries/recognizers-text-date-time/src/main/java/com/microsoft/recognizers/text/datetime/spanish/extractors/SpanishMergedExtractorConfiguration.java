package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
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
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeListExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.number.spanish.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

import org.javatuples.Pair;

public class SpanishMergedExtractorConfiguration extends BaseOptionsConfiguration implements IMergedExtractorConfiguration {

    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BeforeRegex);
    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AfterRegex);
    public static final Pattern SinceRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SinceRegex);
    public static final Pattern AroundRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AroundRegex);
    public static final Pattern FromToRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FromToRegex);
    public static final Pattern SingleAmbiguousMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SingleAmbiguousMonthRegex);
    public static final Pattern PrepositionSuffixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PrepositionSuffixRegex);
    public static final Pattern AmbiguousRangeModifierPrefix = RegExpUtility.getSafeRegExp(SpanishDateTime.AmbiguousRangeModifierPrefix);
    public static final Pattern NumberEndingPattern = RegExpUtility.getSafeRegExp(SpanishDateTime.NumberEndingPattern);
    public static final Pattern SuffixAfterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SuffixAfterRegex);
    public static final Pattern UnspecificDatePeriodRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnspecificDatePeriodRegex);
    public final Iterable<Pair<Pattern, Pattern>> ambiguityFiltersDict = null;

    public static final StringMatcher SuperfluousWordMatcher = new StringMatcher();

    public SpanishMergedExtractorConfiguration(DateTimeOptions options) {
        super(options);

        setExtractor = new BaseSetExtractor(new SpanishSetExtractorConfiguration(options));
        dateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
        timeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(options));
        holidayExtractor = new BaseHolidayExtractor(new SpanishHolidayExtractorConfiguration());
        datePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration(this));
        dateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(options));
        timeZoneExtractor = new BaseTimeZoneExtractor(new SpanishTimeZoneExtractorConfiguration(options));
        dateTimeAltExtractor = new BaseDateTimeAltExtractor(new SpanishDateTimeAltExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration(options));
        integerExtractor = IntegerExtractor.getInstance();
    }

    public final StringMatcher getSuperfluousWordMatcher() {
        return SuperfluousWordMatcher;
    }

    private IDateExtractor dateExtractor;

    public final IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    private IDateTimeExtractor timeExtractor;

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
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

    private IDateTimeExtractor durationExtractor;

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    private IDateTimeExtractor setExtractor;

    public final IDateTimeExtractor getSetExtractor() {
        return setExtractor;
    }

    private IDateTimeExtractor holidayExtractor;

    public final IDateTimeExtractor getHolidayExtractor() {
        return holidayExtractor;
    }

    private IDateTimeZoneExtractor timeZoneExtractor;

    public final IDateTimeZoneExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }

    private IDateTimeListExtractor dateTimeAltExtractor;

    public final IDateTimeListExtractor getDateTimeAltExtractor() {
        return dateTimeAltExtractor;
    }

    private IExtractor integerExtractor;

    public final IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    public final Iterable<Pair<Pattern, Pattern>> getAmbiguityFiltersDict() {
        return ambiguityFiltersDict;
    }

    @Override
    public Iterable<Pattern> getFilterWordRegexList() {
        return null;
    }

    public final Pattern getAfterRegex() {
        return AfterRegex;
    }

    public final Pattern getBeforeRegex() {
        return BeforeRegex;
    }

    public final Pattern getSinceRegex() {
        return SinceRegex;
    }

    public final Pattern getAroundRegex() {
        return AroundRegex;
    }

    public final Pattern getFromToRegex() {
        return FromToRegex;
    }

    public final Pattern getSingleAmbiguousMonthRegex() {
        return SingleAmbiguousMonthRegex;
    }

    public final Pattern getPrepositionSuffixRegex() {
        return PrepositionSuffixRegex;
    }

    public final Pattern getAmbiguousRangeModifierPrefix() {
        return null;
    }

    public final Pattern getPotentialAmbiguousRangeRegex() {
        return null;
    }

    public final Pattern getNumberEndingPattern() {
        return NumberEndingPattern;
    }

    public final Pattern getSuffixAfterRegex() {
        return SuffixAfterRegex;
    }

    public final Pattern getUnspecificDatePeriodRegex() {
        return UnspecificDatePeriodRegex;
    }

}
