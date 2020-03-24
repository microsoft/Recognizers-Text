package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.spanish.utilities.SpanishDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.spanish.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class SpanishTimePeriodExtractorConfiguration extends BaseOptionsConfiguration implements ITimePeriodExtractorConfiguration {

    private String tokenBeforeDate;

    public final String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    public static final Pattern HourNumRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.HourNumRegex);
    public static final Pattern PureNumFromTo = RegExpUtility.getSafeRegExp(SpanishDateTime.PureNumFromTo);
    public static final Pattern PureNumBetweenAnd = RegExpUtility.getSafeRegExp(SpanishDateTime.PureNumBetweenAnd);
    public static final Pattern SpecificTimeFromTo = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificTimeFromTo);
    public static final Pattern SpecificTimeBetweenAnd = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificTimeBetweenAnd);
    public static final Pattern UnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnitRegex);
    public static final Pattern FollowedUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.FollowedUnit);
    public static final Pattern NumberCombinedWithUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeNumberCombinedWithUnit);

    private static final Pattern FromRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FromRegex);
    private static final Pattern RangeConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeConnectorRegex);
    private static final Pattern BetweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BetweenRegex);

    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfDayRegex);
    public static final Pattern GeneralEndingRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.GeneralEndingRegex);
    public static final Pattern TillRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TillRegex);

    public SpanishTimePeriodExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public SpanishTimePeriodExtractorConfiguration(DateTimeOptions options) {

        super(options);

        tokenBeforeDate = SpanishDateTime.TokenBeforeDate;
        singleTimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(options));
        utilityConfiguration = new SpanishDatetimeUtilityConfiguration();
        integerExtractor = IntegerExtractor.getInstance();
        timeZoneExtractor = new BaseTimeZoneExtractor(new SpanishTimeZoneExtractorConfiguration(options));
    }

    private IDateTimeUtilityConfiguration utilityConfiguration;

    public final IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }

    private IDateTimeExtractor singleTimeExtractor;

    public final IDateTimeExtractor getSingleTimeExtractor() {
        return singleTimeExtractor;
    }

    private IExtractor integerExtractor;

    public final IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    public final IDateTimeExtractor timeZoneExtractor;

    public IDateTimeExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }


    public Iterable<Pattern> getSimpleCasesRegex() {
        return getSimpleCasesRegex;
    }

    public final Iterable<Pattern> getSimpleCasesRegex = new ArrayList<Pattern>() {
        {
            add(PureNumFromTo);
            add(PureNumBetweenAnd);
        }
    };

    public Iterable<Pattern> getPureNumberRegex() {
        return getPureNumberRegex;
    }

    public final Iterable<Pattern> getPureNumberRegex = new ArrayList<Pattern>() {
        {
            add(PureNumFromTo);
            add(PureNumBetweenAnd);
        }
    };

    public final Pattern getTillRegex() {
        return TillRegex;
    }

    public final Pattern getTimeOfDayRegex() {
        return TimeOfDayRegex;
    }

    public final Pattern getGeneralEndingRegex() {
        return GeneralEndingRegex;
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
