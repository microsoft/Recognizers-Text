package com.microsoft.recognizers.text.datetime.english.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.number.english.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class EnglishDurationExtractorConfiguration extends BaseOptionsConfiguration implements IDurationExtractorConfiguration {

    public static final Pattern DurationUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DurationUnitRegex);
    public static final Pattern SuffixAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixAndRegex);
    public static final Pattern DurationFollowedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.DurationFollowedUnit);
    public static final Pattern NumberCombinedWithDurationUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberCombinedWithDurationUnit);
    public static final Pattern AnUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AnUnitRegex);
    public static final Pattern DuringRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DuringRegex);
    public static final Pattern AllRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AllRegex);
    public static final Pattern HalfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.HalfRegex);
    public static final Pattern ConjunctionRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ConjunctionRegex);
    public static final Pattern InexactNumberRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InexactNumberRegex);
    public static final Pattern InexactNumberUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InexactNumberUnitRegex);
    public static final Pattern RelativeDurationUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeDurationUnitRegex);
    public static final Pattern DurationConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DurationConnectorRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MoreThanRegex);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LessThanRegex);

    private final IExtractor cardinalExtractor;
    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Long> unitValueMap;

    public EnglishDurationExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public EnglishDurationExtractorConfiguration(DateTimeOptions options) {

        super(options);

        cardinalExtractor = CardinalExtractor.getInstance();
        unitMap = EnglishDateTime.UnitMap;
        unitValueMap = EnglishDateTime.UnitValueMap;
    }

    @Override
    public Pattern getFollowedUnit() {
        return DurationFollowedUnit;
    }

    @Override
    public Pattern getNumberCombinedWithUnit() {
        return NumberCombinedWithDurationUnit;
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