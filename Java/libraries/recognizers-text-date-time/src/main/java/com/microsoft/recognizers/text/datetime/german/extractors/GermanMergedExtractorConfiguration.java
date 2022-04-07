// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.extractors;

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
import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.number.german.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.ArrayList;
import java.util.regex.Pattern;
import java.util.stream.Collectors;
import org.javatuples.Pair;

public class GermanMergedExtractorConfiguration extends BaseOptionsConfiguration implements IMergedExtractorConfiguration {

    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(GermanDateTime.BeforeRegex);
    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AfterRegex);
    public static final Pattern SinceRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SinceRegex);
    public static final Pattern AroundRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AroundRegex);
    public static final Pattern EqualRegex = RegExpUtility.getSafeRegExp(BaseDateTime.EqualRegex);
    public static final Pattern FromToRegex = RegExpUtility.getSafeRegExp(GermanDateTime.FromToRegex);
    public static final Pattern SingleAmbiguousMonthRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SingleAmbiguousMonthRegex);
    public static final Pattern PrepositionSuffixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PrepositionSuffixRegex);
    public static final Pattern AmbiguousRangeModifierPrefix = RegExpUtility.getSafeRegExp(GermanDateTime.AmbiguousRangeModifierPrefix);
    public static final Pattern NumberEndingPattern = RegExpUtility.getSafeRegExp(GermanDateTime.NumberEndingPattern);
    public static final Pattern SuffixAfterRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SuffixAfterRegex);
    public static final Pattern UnspecificDatePeriodRegex = RegExpUtility.getSafeRegExp(GermanDateTime.UnspecificDatePeriodRegex);
    public static final StringMatcher SuperfluousWordMatcher = new StringMatcher();
    private final Iterable<Pair<Pattern, Pattern>> ambiguityFiltersDict;

    private static final Iterable<Pattern> filterWordRegexList = new ArrayList<Pattern>() {
        {
            // one on one
            add(RegExpUtility.getSafeRegExp(GermanDateTime.OneOnOneRegex));
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

    public GermanMergedExtractorConfiguration(DateTimeOptions options) {
        super(options);

        dateExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
        timeExtractor = new BaseTimeExtractor(new GermanTimeExtractorConfiguration(options));
        dateTimeExtractor = new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration(options));
        datePeriodExtractor = new BaseDatePeriodExtractor(new GermanDatePeriodExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new GermanDateTimePeriodExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(options));
        setExtractor = new BaseSetExtractor(new GermanSetExtractorConfiguration(options));
        holidayExtractor = new BaseHolidayExtractor(new GermanHolidayExtractorConfiguration());
        timeZoneExtractor = new BaseTimeZoneExtractor(new GermanTimeZoneExtractorConfiguration(options));
        dateTimeAltExtractor = new BaseDateTimeAltExtractor(new GermanDateTimeAltExtractorConfiguration(this));
        integerExtractor = new IntegerExtractor();

        ambiguityFiltersDict = GermanDateTime.AmbiguityFiltersDict.entrySet().stream().map(pair -> {
            Pattern key = RegExpUtility.getSafeRegExp(pair.getKey());
            Pattern val = RegExpUtility.getSafeRegExp(pair.getValue());
            return new Pair<>(key, val);
        }).collect(Collectors.toList());
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
