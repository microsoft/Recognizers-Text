// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.number.german.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class GermanDurationExtractorConfiguration extends BaseOptionsConfiguration implements IDurationExtractorConfiguration {

    public static final Pattern DurationUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DurationUnitRegex);
    public static final Pattern SuffixAndRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SuffixAndRegex);
    public static final Pattern DurationFollowedUnit = RegExpUtility.getSafeRegExp(GermanDateTime.DurationFollowedUnit);
    public static final Pattern NumberCombinedWithDurationUnit = RegExpUtility.getSafeRegExp(GermanDateTime.NumberCombinedWithDurationUnit);
    public static final Pattern AnUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AnUnitRegex);
    public static final Pattern DuringRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DuringRegex);
    public static final Pattern AllRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AllRegex);
    public static final Pattern HalfRegex = RegExpUtility.getSafeRegExp(GermanDateTime.HalfRegex);
    public static final Pattern ConjunctionRegex = RegExpUtility.getSafeRegExp(GermanDateTime.ConjunctionRegex);
    public static final Pattern InexactNumberRegex = RegExpUtility.getSafeRegExp(GermanDateTime.InexactNumberRegex);
    public static final Pattern InexactNumberUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.InexactNumberUnitRegex);
    public static final Pattern RelativeDurationUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RelativeDurationUnitRegex);
    public static final Pattern DurationConnectorRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DurationConnectorRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MoreThanRegex);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(GermanDateTime.LessThanRegex);

    private final IExtractor cardinalExtractor;
    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Long> unitValueMap;

    public GermanDurationExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public GermanDurationExtractorConfiguration(DateTimeOptions options) {

        super(options);

        cardinalExtractor = CardinalExtractor.getInstance();
        unitMap = GermanDateTime.UnitMap;
        unitValueMap = GermanDateTime.UnitValueMap;
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