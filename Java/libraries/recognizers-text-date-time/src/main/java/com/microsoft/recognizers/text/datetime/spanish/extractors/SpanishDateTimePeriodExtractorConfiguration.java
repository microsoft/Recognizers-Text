package com.microsoft.recognizers.text.datetime.spanish.extractors;

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
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.number.spanish.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class SpanishDateTimePeriodExtractorConfiguration extends BaseOptionsConfiguration
    implements IDateTimePeriodExtractorConfiguration {

    public static final Pattern weekDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayRegex);
    public static final Pattern NumberCombinedWithUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.DateTimePeriodNumberCombinedWithUnit);
    public static final Pattern RestOfDateTimeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RestOfDateTimeRegex);
    public static final Pattern PeriodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PeriodTimeOfDayWithDateRegex);
    public static final Pattern RelativeTimeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeTimeUnitRegex);
    public static final Pattern GeneralEndingRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.GeneralEndingRegex);
    public static final Pattern MiddlePauseRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MiddlePauseRegex);
    public static final Pattern AmDescRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AmDescRegex);
    public static final Pattern PmDescRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PmDescRegex);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WithinNextPrefixRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DateUnitRegex);
    public static final Pattern PrefixDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PrefixDayRegex);
    public static final Pattern SuffixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SuffixRegex);
    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BeforeRegex);
    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AfterRegex);
    public static final Pattern FromRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FromRegex);
    public static final Pattern RangeConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeConnectorRegex);
    public static final Pattern BetweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BetweenRegex);
    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfDayRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeUnitRegex);
    public static final Pattern TimeFollowedUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeFollowedUnit);

    private final String tokenBeforeDate;

    private final IExtractor cardinalExtractor;
    private final IDateTimeExtractor singleDateExtractor;
    private final IDateTimeExtractor singleTimeExtractor;
    private final IDateTimeExtractor singleDateTimeExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeExtractor timePeriodExtractor;
    private final IDateTimeExtractor timeZoneExtractor;

    public static final Iterable<Pattern> SimpleCases = new ArrayList<Pattern>() {
        {
            add(SpanishTimePeriodExtractorConfiguration.PureNumFromTo);
            add(SpanishTimePeriodExtractorConfiguration.PureNumBetweenAnd);
        }
    };

    public SpanishDateTimePeriodExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public SpanishDateTimePeriodExtractorConfiguration(DateTimeOptions options) {

        super(options);
        tokenBeforeDate = SpanishDateTime.TokenBeforeDate;

        cardinalExtractor = CardinalExtractor.getInstance();

        singleDateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
        singleTimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(options));
        singleDateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(options));
        timePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration(options));
        timeZoneExtractor = new BaseTimeZoneExtractor(new SpanishTimeZoneExtractorConfiguration(options));
    }

    @Override
    public String getTokenBeforeDate() {
        return tokenBeforeDate;
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
    public Iterable<Pattern> getSimpleCasesRegex() {
        return SimpleCases;
    }

    @Override
    public Pattern getPrepositionRegex() {
        return SpanishDateTimeExtractorConfiguration.PrepositionRegex;
    }

    @Override
    public Pattern getTillRegex() {
        return SpanishTimePeriodExtractorConfiguration.TillRegex;
    }

    @Override
    public Pattern getTimeOfDayRegex() {
        return SpanishDateTimeExtractorConfiguration.TimeOfDayRegex;
    }

    @Override
    public Pattern getFollowedUnit() {
        return TimeFollowedUnit;
    }

    @Override
    public Pattern getTimeUnitRegex() {
        return TimeUnitRegex;
    }

    @Override
    public Pattern getPastPrefixRegex() {
        return SpanishDatePeriodExtractorConfiguration.PastRegex;
    }

    @Override
    public Pattern getNextPrefixRegex() {
        return SpanishDatePeriodExtractorConfiguration.FutureRegex;
    }

    @Override
    public Pattern getFutureSuffixRegex() {
        return SpanishDatePeriodExtractorConfiguration.FutureSuffixRegex;
    }

    @Override
    public Pattern getPrefixDayRegex() {
        return PrefixDayRegex;
    }

    @Override
    public Pattern getDateUnitRegex() {
        return DateUnitRegex;
    }

    @Override
    public Pattern getNumberCombinedWithUnit() {
        return NumberCombinedWithUnit;
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
    public Pattern getSpecificTimeOfDayRegex() {
        return SpanishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
    }

    @Override
    public ResultIndex getFromTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        Matcher matcher = FromRegex.matcher(text);
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
        Matcher matcher = BetweenRegex.matcher(text);
        if (matcher.find()) {
            result = true;
            index = matcher.start();
        }

        return new ResultIndex(result, index);
    }

    @Override
    public boolean hasConnectorToken(String text) {
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(RangeConnectorRegex, text)).findFirst();
        return match.isPresent() && match.get().length == text.trim().length();
    }
}
