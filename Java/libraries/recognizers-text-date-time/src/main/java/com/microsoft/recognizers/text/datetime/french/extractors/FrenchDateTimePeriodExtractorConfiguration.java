package com.microsoft.recognizers.text.datetime.french.extractors;

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
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.number.french.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class FrenchDateTimePeriodExtractorConfiguration extends BaseOptionsConfiguration implements IDateTimePeriodExtractorConfiguration {

    public static final Pattern weekDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayRegex);
    public static final Pattern NumberCombinedWithUnit = RegExpUtility
        .getSafeRegExp(FrenchDateTime.TimeNumberCombinedWithUnit);
    public static final Pattern RestOfDateTimeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RestOfDateTimeRegex);
    public static final Pattern PeriodTimeOfDayWithDateRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.PeriodTimeOfDayWithDateRegex);
    public static final Pattern RelativeTimeUnitRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.RelativeTimeUnitRegex);
    public static final Pattern GeneralEndingRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.GeneralEndingRegex);
    public static final Pattern MiddlePauseRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MiddlePauseRegex);
    public static final Pattern AmDescRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AmDescRegex);
    public static final Pattern PmDescRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PmDescRegex);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.WithinNextPrefixRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DateUnitRegex);
    public static final Pattern PrefixDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PrefixDayRegex);
    public static final Pattern SuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SuffixRegex);
    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BeforeRegex);
    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AfterRegex);
    public static final Pattern FromRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.FromRegex2);
    public static final Pattern RangeConnectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RangeConnectorRegex);
    public static final Pattern BetweenRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BetweenRegex);
    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeOfDayRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeUnitRegex);
    public static final Pattern TimeFollowedUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeFollowedUnit);
    public static final Iterable<Pattern> SimpleCases = new ArrayList<Pattern>() {
        {
            add(FrenchTimePeriodExtractorConfiguration.PureNumFromTo);
            add(FrenchTimePeriodExtractorConfiguration.PureNumBetweenAnd);
            add(FrenchDateTimeExtractorConfiguration.SpecificTimeOfDayRegex);
        }
    };
    private final String tokenBeforeDate;
    private final IExtractor cardinalExtractor;
    private final IDateTimeExtractor singleDateExtractor;
    private final IDateTimeExtractor singleTimeExtractor;
    private final IDateTimeExtractor singleDateTimeExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeExtractor timePeriodExtractor;
    private final IDateTimeExtractor timeZoneExtractor;

    public FrenchDateTimePeriodExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public FrenchDateTimePeriodExtractorConfiguration(final DateTimeOptions options) {

        super(options);
        tokenBeforeDate = FrenchDateTime.TokenBeforeDate;

        cardinalExtractor = CardinalExtractor.getInstance();

        singleDateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(this));
        singleTimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration(options));
        singleDateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(options));
        timePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration(options));
        timeZoneExtractor = new BaseTimeZoneExtractor(new FrenchTimeZoneExtractorConfiguration(options));
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
        return FrenchDateTimeExtractorConfiguration.PrepositionRegex;
    }

    @Override
    public Pattern getTillRegex() {
        return FrenchTimePeriodExtractorConfiguration.TillRegex;
    }

    @Override
    public Pattern getTimeOfDayRegex() {
        return FrenchDateTimeExtractorConfiguration.TimeOfDayRegex;
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
        return FrenchDatePeriodExtractorConfiguration.PastRegex;
    }

    @Override
    public Pattern getNextPrefixRegex() {
        return FrenchDatePeriodExtractorConfiguration.FutureRegex;
    }

    @Override
    public Pattern getFutureSuffixRegex() {
        return FrenchDatePeriodExtractorConfiguration.FutureSuffixRegex;
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
        return FrenchDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
    }

    @Override
    public ResultIndex getFromTokenIndex(final String text) {
        int index = -1;
        boolean result = false;
        final Matcher matcher = FromRegex.matcher(text);
        if (matcher.find()) {
            result = true;
            index = matcher.start();
        }

        return new ResultIndex(result, index);
    }

    @Override
    public ResultIndex getBetweenTokenIndex(final String text) {
        int index = -1;
        boolean result = false;
        final Matcher matcher = BetweenRegex.matcher(text);
        if (matcher.find()) {
            result = true;
            index = matcher.start();
        }

        return new ResultIndex(result, index);
    }

    @Override
    public boolean hasConnectorToken(final String text) {
        final Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(RegExpUtility.getSafeRegExp(FrenchDateTime.ConnectorAndRegex), text)).findFirst();
        return match.isPresent() && match.get().length == text.trim().length();
    }
}
