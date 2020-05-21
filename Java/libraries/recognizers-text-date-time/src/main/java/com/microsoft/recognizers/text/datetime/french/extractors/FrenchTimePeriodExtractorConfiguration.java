package com.microsoft.recognizers.text.datetime.french.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.french.utilities.FrenchDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.french.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class FrenchTimePeriodExtractorConfiguration extends BaseOptionsConfiguration implements ITimePeriodExtractorConfiguration {

    public static final Pattern HourNumRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.HourNumRegex);
    public static final Pattern PureNumFromTo = RegExpUtility.getSafeRegExp(FrenchDateTime.PureNumFromTo);
    public static final Pattern PureNumBetweenAnd = RegExpUtility.getSafeRegExp(FrenchDateTime.PureNumBetweenAnd);
    public static final Pattern SpecificTimeFromTo = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecificTimeFromTo);
    public static final Pattern SpecificTimeBetweenAnd = RegExpUtility
        .getSafeRegExp(FrenchDateTime.SpecificTimeBetweenAnd);
    //    TODO: What are these?
    //    public static final Pattern UnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.UnitRegex);
    //    public static final Pattern FollowedUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.FollowedUnit);
    public static final Pattern NumberCombinedWithUnit = RegExpUtility
        .getSafeRegExp(FrenchDateTime.TimeNumberCombinedWithUnit);
    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeOfDayRegex);
    public static final Pattern GeneralEndingRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.GeneralEndingRegex);
    public static final Pattern TillRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TillRegex);
    private static final Pattern FromRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.FromRegex2);
    private static final Pattern RangeConnectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RangeConnectorRegex);
    private static final Pattern BetweenRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BeforeRegex2);
    public final IDateTimeExtractor timeZoneExtractor;
    public final Iterable<Pattern> getSimpleCasesRegex = new ArrayList<Pattern>() {
        {
            add(PureNumFromTo);
            add(PureNumBetweenAnd);
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.PmRegex));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.AmRegex));
        }
    };
    public final Iterable<Pattern> getPureNumberRegex = new ArrayList<Pattern>() {
        {
            add(PureNumFromTo);
            add(PureNumBetweenAnd);
        }
    };
    private final String tokenBeforeDate;
    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final IDateTimeExtractor singleTimeExtractor;
    private final IExtractor integerExtractor;

    public FrenchTimePeriodExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public FrenchTimePeriodExtractorConfiguration(final DateTimeOptions options) {

        super(options);

        tokenBeforeDate = FrenchDateTime.TokenBeforeDate;
        singleTimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration(options));
        utilityConfiguration = new FrenchDatetimeUtilityConfiguration();
        integerExtractor = new IntegerExtractor();
        timeZoneExtractor = new BaseTimeZoneExtractor(new FrenchTimeZoneExtractorConfiguration(options));
    }

    public final String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    public final IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }

    public final IDateTimeExtractor getSingleTimeExtractor() {
        return singleTimeExtractor;
    }

    public final IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    public IDateTimeExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }

    public Iterable<Pattern> getSimpleCasesRegex() {
        return getSimpleCasesRegex;
    }

    public Iterable<Pattern> getPureNumberRegex() {
        return getPureNumberRegex;
    }

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
        final Optional<Match> match = Arrays
            .stream(RegExpUtility.getMatches(RegExpUtility.getSafeRegExp(FrenchDateTime.ConnectorAndRegex), text))
            .findFirst();
        return match.isPresent() && match.get().length == text.trim().length();
    }
}
