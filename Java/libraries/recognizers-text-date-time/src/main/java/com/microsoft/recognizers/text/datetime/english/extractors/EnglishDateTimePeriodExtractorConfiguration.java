package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.number.english.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Pattern;

public class EnglishDateTimePeriodExtractorConfiguration implements IDateTimePeriodExtractorConfiguration {

    private static final int flags = Pattern.CASE_INSENSITIVE;

    public static final Iterable<Pattern> SimpleCases = new ArrayList<Pattern>() {
        {
            add(EnglishTimePeriodExtractorConfiguration.PureNumFromTo);
            add(EnglishTimePeriodExtractorConfiguration.PureNumBetweenAnd);
        }
    };

    public static final Pattern PeriodTimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodTimeOfDayRegex, flags);
    public static final Pattern PeriodSpecificTimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodSpecificTimeOfDayRegex, flags);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex, flags);
    public static final Pattern TimeFollowedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeFollowedUnit, flags);
    public static final Pattern TimeNumberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeNumberCombinedWithUnit, flags);
    public static final Pattern PeriodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodTimeOfDayWithDateRegex, flags);
    public static final Pattern RelativeTimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeTimeUnitRegex, flags);
    public static final Pattern RestOfDateTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RestOfDateTimeRegex, flags);
    public static final Pattern GeneralEndingRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.GeneralEndingRegex, flags);
    public static final Pattern MiddlePauseRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MiddlePauseRegex, flags);
    public static final Pattern AmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmDescRegex, flags);
    public static final Pattern PmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PmDescRegex, flags);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WithinNextPrefixRegex, flags);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateUnitRegex, flags);
    public static final Pattern PrefixDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrefixDayRegex, flags);
    public static final Pattern SuffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixRegex, flags);
    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.BeforeRegex, flags);
    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AfterRegex, flags);

    private final DateTimeOptions options;
    private final String tokenBeforeDate;

    private final IExtractor cardinalExtractor;
    private final IDateTimeExtractor singleDateExtractor;
    private final IDateTimeExtractor singleTimeExtractor;
    private final IDateTimeExtractor singleDateTimeExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeExtractor timePeriodExtractor;

    private final Pattern weekDayRegex;
    private final Pattern rangeConnectorRegex;

    public EnglishDateTimePeriodExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public EnglishDateTimePeriodExtractorConfiguration(DateTimeOptions options) {

        super();

        //TODO add english implementations
        this.options = options;
        tokenBeforeDate = EnglishDateTime.TokenBeforeDate;

        cardinalExtractor = CardinalExtractor.getInstance();
        singleDateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        singleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(options));
        singleDateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(options));
        timePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(options));

        weekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayRegex, flags);
        rangeConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeConnectorRegex, flags);
    }

    @Override
    public DateTimeOptions getOptions() {
        return options;
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
        return EnglishTimePeriodExtractorConfiguration.PrepositionRegex;
    }

    @Override
    public Pattern getTillRegex() {
        return EnglishTimePeriodExtractorConfiguration.TillRegex;
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
        return EnglishDatePeriodExtractorConfiguration.PastPrefixRegex;
    }

    @Override
    public Pattern getNextPrefixRegex() {
        return EnglishDatePeriodExtractorConfiguration.NextPrefixRegex;
    }

    @Override
    public Pattern getFutureSuffixRegex() {
        return EnglishDatePeriodExtractorConfiguration.FutureSuffixRegex;
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

    // TODO: these three methods are the same in DatePeriod, should be abstracted
    @Override
    public ResultIndex getFromTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        if (text.endsWith("from")) {
            result = true;
            index = text.lastIndexOf("from");
        }

        return new ResultIndex(result, index);
    }

    @Override
    public ResultIndex getBetweenTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        if (text.endsWith("between")) {
            result = true;
            index = text.lastIndexOf("between");
        }

        return new ResultIndex(result, index);
    }

    @Override
    public boolean hasConnectorToken(String text) {
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(rangeConnectorRegex, text)).findFirst();
        return match.isPresent() && match.get().length == text.trim().length();
    }
}
