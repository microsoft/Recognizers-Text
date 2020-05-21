package com.microsoft.recognizers.text.datetime.french.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.number.french.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.regex.Pattern;

public class FrenchDurationExtractorConfiguration extends BaseOptionsConfiguration implements IDurationExtractorConfiguration {

    // TODO: Investigate if required
    //    public static final Pattern UnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.UnitRegex);
    public static final Pattern SuffixAndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SuffixAndRegex);
    public static final Pattern FollowedUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.DurationFollowedUnit);
    public static final Pattern NumberCombinedWithUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.NumberCombinedWithDurationUnit);
    public static final Pattern AnUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AnUnitRegex);
    public static final Pattern DuringRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DuringRegex);
    public static final Pattern AllRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AllRegex);
    public static final Pattern HalfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.HalfRegex);
    public static final Pattern ConjunctionRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ConjunctionRegex);
    public static final Pattern InexactNumberRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.InexactNumberRegex);
    public static final Pattern InexactNumberUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.InexactNumberUnitRegex);
    public static final Pattern RelativeDurationUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeDurationUnitRegex);
    public static final Pattern DurationUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DurationUnitRegex);
    public static final Pattern DurationConnectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DurationConnectorRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MoreThanRegex);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.LessThanRegex);

    private final IExtractor cardinalExtractor;
    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Long> unitValueMap;

    public FrenchDurationExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public FrenchDurationExtractorConfiguration(final DateTimeOptions options) {

        super(options);

        cardinalExtractor = CardinalExtractor.getInstance();
        unitMap = FrenchDateTime.UnitMap;
        unitValueMap = FrenchDateTime.UnitValueMap;
    }

    @Override
    public Pattern getFollowedUnit() {
        return FollowedUnit;
    }

    @Override
    public Pattern getNumberCombinedWithUnit() {
        return NumberCombinedWithUnit;
    }

    @Override
    public Pattern getAnUnitRegex() {
        return AnUnitRegex;
    }

    @Override
    public Pattern getDuringRegex() {
        return DuringRegex;
    }

    @Override
    public Pattern getAllRegex() {
        return AllRegex;
    }

    @Override
    public Pattern getHalfRegex() {
        return HalfRegex;
    }

    @Override
    public Pattern getSuffixAndRegex() {
        return SuffixAndRegex;
    }

    @Override
    public Pattern getConjunctionRegex() {
        return ConjunctionRegex;
    }

    @Override
    public Pattern getInexactNumberRegex() {
        return InexactNumberRegex;
    }

    @Override
    public Pattern getInexactNumberUnitRegex() {
        return InexactNumberUnitRegex;
    }

    @Override
    public Pattern getRelativeDurationUnitRegex() {
        return RelativeDurationUnitRegex;
    }

    @Override
    public Pattern getDurationUnitRegex() {
        return DurationUnitRegex;
    }

    @Override
    public Pattern getDurationConnectorRegex() {
        return DurationConnectorRegex;
    }

    @Override
    public Pattern getLessThanRegex() {
        return LessThanRegex;
    }

    @Override
    public Pattern getMoreThanRegex() {
        return MoreThanRegex;
    }

    @Override
    public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override
    public ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    @Override
    public ImmutableMap<String, Long> getUnitValueMap() {
        return unitValueMap;
    }
}
