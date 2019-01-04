package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.number.spanish.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class SpanishDurationExtractorConfiguration extends BaseOptionsConfiguration implements IDurationExtractorConfiguration {

    //public static final Pattern UnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnitRegex);
    public static final Pattern SuffixAndRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SuffixAndRegex);
    public static final Pattern FollowedUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.FollowedUnit);
    public static final Pattern NumberCombinedWithUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.DurationNumberCombinedWithUnit);
    public static final Pattern AnUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AnUnitRegex);
    public static final Pattern DuringRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DuringRegex);
    public static final Pattern AllRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AllRegex);
    public static final Pattern HalfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.HalfRegex);
    public static final Pattern ConjunctionRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ConjunctionRegex);
    public static final Pattern InexactNumberRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.InexactNumberRegex);
    public static final Pattern InexactNumberUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.InexactNumberUnitRegex);
    public static final Pattern RelativeDurationUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeDurationUnitRegex);
    public static final Pattern DurationUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DurationUnitRegex);
    public static final Pattern DurationConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DurationConnectorRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MoreThanRegex);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LessThanRegex);

    private final IExtractor cardinalExtractor;
    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Long> unitValueMap;

    public SpanishDurationExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public SpanishDurationExtractorConfiguration(DateTimeOptions options) {

        super(options);

        cardinalExtractor = CardinalExtractor.getInstance();
        unitMap = SpanishDateTime.UnitMap;
        unitValueMap = SpanishDateTime.UnitValueMap;
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
