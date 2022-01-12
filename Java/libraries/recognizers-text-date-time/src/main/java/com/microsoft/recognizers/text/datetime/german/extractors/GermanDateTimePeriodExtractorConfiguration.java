// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.number.german.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class GermanDateTimePeriodExtractorConfiguration extends BaseOptionsConfiguration implements IDateTimePeriodExtractorConfiguration {

    public static final Iterable<Pattern> SimpleCases = new ArrayList<Pattern>() {
        {
            add(GermanTimePeriodExtractorConfiguration.PureNumFromTo);
            add(GermanTimePeriodExtractorConfiguration.PureNumBetweenAnd);
        }
    };

    public static final Pattern PeriodTimeOfDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PeriodTimeOfDayRegex);
    public static final Pattern PeriodSpecificTimeOfDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PeriodSpecificTimeOfDayRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.TimeUnitRegex);
    public static final Pattern TimeFollowedUnit = RegExpUtility.getSafeRegExp(GermanDateTime.TimeFollowedUnit);
    public static final Pattern TimeNumberCombinedWithUnit = RegExpUtility.getSafeRegExp(GermanDateTime.TimeNumberCombinedWithUnit);
    public static final Pattern PeriodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PeriodTimeOfDayWithDateRegex);
    public static final Pattern RelativeTimeUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RelativeTimeUnitRegex);
    public static final Pattern RestOfDateTimeRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RestOfDateTimeRegex);
    public static final Pattern GeneralEndingRegex = RegExpUtility.getSafeRegExp(GermanDateTime.GeneralEndingRegex);
    public static final Pattern MiddlePauseRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MiddlePauseRegex);
    public static final Pattern AmDescRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AmDescRegex);
    public static final Pattern PmDescRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PmDescRegex);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WithinNextPrefixRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DateUnitRegex);
    public static final Pattern PrefixDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PrefixDayRegex);
    public static final Pattern SuffixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SuffixRegex);
    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(GermanDateTime.BeforeRegex);
    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AfterRegex);
    public static final Pattern HyphenDateRegex = RegExpUtility.getSafeRegExp(BaseDateTime.HyphenDateRegex);
    public static final Pattern FromTokenRegex = RegExpUtility.getSafeRegExp(GermanDateTime.FromRegex);
    public static final Pattern BetweenTokenRegex = RegExpUtility.getSafeRegExp(GermanDateTime.BetweenTokenRegex);

    private final String tokenBeforeDate;

    private final IExtractor cardinalExtractor;
    private final IDateTimeExtractor singleDateExtractor;
    private final IDateTimeExtractor singleTimeExtractor;
    private final IDateTimeExtractor singleDateTimeExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeExtractor timePeriodExtractor;
    private final IDateTimeExtractor timeZoneExtractor;

    private final Pattern weekDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WeekDayRegex);
    private final Pattern rangeConnectorRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RangeConnectorRegex);

    public GermanDateTimePeriodExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public GermanDateTimePeriodExtractorConfiguration(DateTimeOptions options) {
        super(options);

        tokenBeforeDate = GermanDateTime.TokenBeforeDate;

        cardinalExtractor = CardinalExtractor.getInstance();
        singleDateExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
        singleTimeExtractor = new BaseTimeExtractor(new GermanTimeExtractorConfiguration(options));
        singleDateTimeExtractor = new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(options));
        timePeriodExtractor = new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration(options));
        timeZoneExtractor = new BaseTimeZoneExtractor(new GermanTimeZoneExtractorConfiguration(options));
    }

    @Override
    public String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    @Override
    public Iterable<Pattern> getSimpleCasesRegex() {
        return SimpleCases;
    }

    @Override
    public Pattern getPrepositionRegex() {
        return GermanTimePeriodExtractorConfiguration.PrepositionRegex;
    }

    @Override
    public Pattern getTillRegex() {
        return GermanTimePeriodExtractorConfiguration.TillRegex;
    }

    @Override
    public Pattern getSpecificTimeOfDayRegex() {
        return PeriodSpecificTimeOfDayRegex;
    }

    @Override
    public Pattern getTimeOfDayRegex() {
        return PeriodTimeOfDayRegex;
    }

    @Override
    public Pattern getFollowedUnit() {
        return TimeFollowedUnit;
    }

    @Override
    public Pattern getNumberCombinedWithUnit() {
        return TimeNumberCombinedWithUnit;
    }

    @Override
    public Pattern getTimeUnitRegex() {
        return TimeUnitRegex;
    }

    @Override
    public Pattern getPastPrefixRegex() {
        return GermanDatePeriodExtractorConfiguration.PreviousPrefixRegex;
    }

    @Override
    public Pattern getNextPrefixRegex() {
        return GermanDatePeriodExtractorConfiguration.NextPrefixRegex;
    }

    @Override
    public Pattern getFutureSuffixRegex() {
        return GermanDatePeriodExtractorConfiguration.FutureSuffixRegex;
    }

    @Override
    public Pattern getWeekDayRegex() {
        return weekDayRegex;
    }

    @Override
    public Pattern getPeriodTimeOfDayWithDateRegex() {
        return PeriodTimeOfDayWithDateRegex;
    }

    @Override
    public Pattern getRelativeTimeUnitRegex() {
        return RelativeTimeUnitRegex;
    }

    @Override
    public Pattern getRestOfDateTimeRegex() {
        return RestOfDateTimeRegex;
    }

    @Override
    public Pattern getGeneralEndingRegex() {
        return GeneralEndingRegex;
    }

    @Override
    public Pattern getMiddlePauseRegex() {
        return MiddlePauseRegex;
    }

    @Override
    public Pattern getAmDescRegex() {
        return AmDescRegex;
    }

    @Override
    public Pattern getPmDescRegex() {
        return PmDescRegex;
    }

    @Override
    public Pattern getWithinNextPrefixRegex() {
        return WithinNextPrefixRegex;
    }

    @Override
    public Pattern getDateUnitRegex() {
        return DateUnitRegex;
    }

    @Override
    public Pattern getPrefixDayRegex() {
        return PrefixDayRegex;
    }

    @Override
    public Pattern getSuffixRegex() {
        return SuffixRegex;
    }

    @Override
    public Pattern getBeforeRegex() {
        return BeforeRegex;
    }

    @Override
    public Pattern getAfterRegex() {
        return AfterRegex;
    }

    @Override
    public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override
    public IDateTimeExtractor getSingleDateExtractor() {
        return singleDateExtractor;
    }

    @Override
    public IDateTimeExtractor getSingleTimeExtractor() {
        return singleTimeExtractor;
    }

    @Override
    public IDateTimeExtractor getSingleDateTimeExtractor() {
        return singleDateTimeExtractor;
    }

    @Override
    public IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    @Override
    public IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    @Override
    public IDateTimeExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }

    @Override
    public ResultIndex getFromTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        Matcher matcher = FromTokenRegex.matcher(text);
        if (matcher.find()) {
            result = true;
            index = matcher.start();
        }

        return new ResultIndex(result, index);
    }

    @Override
    public ResultIndex getBetweenTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        Matcher matcher = BetweenTokenRegex.matcher(text);
        if (matcher.find()) {
            result = true;
            index = matcher.start();
        }

        return new ResultIndex(result, index);
    }

    @Override
    public boolean hasConnectorToken(String text) {
        return RegexExtension.isExactMatch(rangeConnectorRegex, text, true);
    }
}
