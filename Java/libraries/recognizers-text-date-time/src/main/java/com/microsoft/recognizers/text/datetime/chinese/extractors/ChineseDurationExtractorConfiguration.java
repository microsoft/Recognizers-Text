// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.number.chinese.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class ChineseDurationExtractorConfiguration extends BaseOptionsConfiguration implements IDurationExtractorConfiguration {

    public static final Pattern DurationUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DurationUnitRegex);
    public static final Pattern FollowedUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.FollowedUnit);
    public static final Pattern NumberCombinedWithUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.NumberCombinedWithUnit);
    public static final Pattern DurationConnectorRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DurationConnectorRegex);
    public static final Pattern DurationAllRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DurationAllRegex);
    public static final Pattern DurationHalfRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DurationHalfRegex);
    public static final Pattern DurationRelativeDurationUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DurationRelativeDurationUnitRegex);
    public static final Pattern AnUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.AnUnitRegex);
    public static final Pattern DurationDuringRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DurationDuringRegex);

    private final IExtractor cardinalExtractor;
    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Long> unitValueMap;

    public ChineseDurationExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public ChineseDurationExtractorConfiguration(DateTimeOptions options) {
        super(options);

        cardinalExtractor = new CardinalExtractor();
        unitMap = ChineseDateTime.ParserConfigurationUnitMap;
        unitValueMap = ChineseDateTime.ParserConfigurationUnitValueMap;
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
        return DurationDuringRegex;
    }

    @Override
    public Pattern getAllRegex() {
        return DurationAllRegex;
    }

    @Override
    public Pattern getHalfRegex() {
        return DurationHalfRegex;
    }

    @Override
    public Pattern getSuffixAndRegex() {
        return null;
    }

    @Override
    public Pattern getConjunctionRegex() {
        return null;
    }

    @Override
    public Pattern getInexactNumberRegex() {
        return null;
    }

    @Override
    public Pattern getInexactNumberUnitRegex() {
        return null;
    }

    @Override
    public Pattern getRelativeDurationUnitRegex() {
        return DurationRelativeDurationUnitRegex;
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
        return null;
    }

    @Override
    public Pattern getMoreThanRegex() {
        return null;
    }

    @Override
    public Pattern getSpecialNumberUnitRegex() {
        return null;
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
