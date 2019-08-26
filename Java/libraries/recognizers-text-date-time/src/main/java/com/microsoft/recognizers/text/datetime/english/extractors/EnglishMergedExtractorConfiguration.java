package com.microsoft.recognizers.text.datetime.english.extractors;

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
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.number.english.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

import org.javatuples.Pair;

import org.javatuples.Pair;

public class EnglishMergedExtractorConfiguration extends BaseOptionsConfiguration implements IMergedExtractorConfiguration {

    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AfterRegex);
    public static final Pattern SinceRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SinceRegex);
    public static final Pattern AroundRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AroundRegex);
    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.BeforeRegex);
    public static final Pattern FromToRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.FromToRegex);
    public static final Pattern SuffixAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixAfterRegex);
    public static final Pattern NumberEndingPattern = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberEndingPattern);
    public static final Pattern PrepositionSuffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionSuffixRegex);
    public static final Pattern AmbiguousRangeModifierPrefix = RegExpUtility.getSafeRegExp(EnglishDateTime.AmbiguousRangeModifierPrefix);
    public static final Pattern SingleAmbiguousMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SingleAmbiguousMonthRegex);
    public static final Pattern UnspecificDatePeriodRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.UnspecificDatePeriodRegex);
    private final Iterable<Pair<Pattern, Pattern>> ambiguityFiltersDict;

    public static final StringMatcher SuperfluousWordMatcher = new StringMatcher();
    private static final Iterable<Pattern> filterWordRegexList = new ArrayList<Pattern>() {
        {
            // one on one
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.OneOnOneRegex));

            // (the)? (day|week|month|year)
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.SingleAmbiguousTermsRegex));
        }
    };

    public final Iterable<Pattern> getFilterWordRegexList() {
        return filterWordRegexList;
    }

    public final StringMatcher getSuperfluousWordMatcher() {
        return SuperfluousWordMatcher;
    }

    private IDateTimeExtractor setExtractor;

    public final IDateTimeExtractor getSetExtractor() {
        return setExtractor;
    }

    private IExtractor integerExtractor;

    public final IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    private IDateExtractor dateExtractor;

    public final IDateExtractor getDateExtractor() {
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
        super(options);

        setExtractor = new BaseSetExtractor(new EnglishSetExtractorConfiguration(options));
        dateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(this));
        timeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(options));
        holidayExtractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
        datePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(this));
        dateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(options));
        timeZoneExtractor = new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(options));
        dateTimeAltExtractor = new BaseDateTimeAltExtractor(new EnglishDateTimeAltExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(options));
        integerExtractor = IntegerExtractor.getInstance();

        ambiguityFiltersDict = EnglishDateTime.AmbiguityFiltersDict.entrySet().stream().map(pair -> {
            Pattern key = RegExpUtility.getSafeRegExp(pair.getKey());
            Pattern val = RegExpUtility.getSafeRegExp(pair.getValue());
            return new Pair<Pattern, Pattern>(key, val);
        }).collect(Collectors.toList());

        if (!this.getOptions().match(DateTimeOptions.EnablePreview)) {
            getSuperfluousWordMatcher().init(EnglishDateTime.SuperfluousWordList);
        }
    }

    public final Pattern getAfterRegex() {
        return AfterRegex;
    }

    public final Pattern getSinceRegex() {
        return SinceRegex;
    }


    public final Pattern getAroundRegex() {
        return AroundRegex;
    }

    public final Pattern getBeforeRegex() {
        return BeforeRegex;
    }

    public final Pattern getFromToRegex() {
        return FromToRegex;
    }

    public final Pattern getSuffixAfterRegex() {
        return SuffixAfterRegex;
    }

    public final Pattern getNumberEndingPattern() {
        return NumberEndingPattern;
    }

    public final Pattern getPrepositionSuffixRegex() {
        return PrepositionSuffixRegex;
    }

    public final Pattern getAmbiguousRangeModifierPrefix() {
        return AmbiguousRangeModifierPrefix;
    }

    public final Pattern getPotentialAmbiguousRangeRegex() {
        return FromToRegex;
    }

    public final Pattern getSingleAmbiguousMonthRegex() {
        return SingleAmbiguousMonthRegex;
    }

    public final Pattern getUnspecificDatePeriodRegex() {
        return UnspecificDatePeriodRegex;
    }

    public final Iterable<Pair<Pattern, Pattern>> getAmbiguityFiltersDict() {
        return ambiguityFiltersDict;
    }
}
