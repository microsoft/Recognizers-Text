// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.spanish.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.StringExtension;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Collections;
import java.util.List;
import java.util.Locale;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class SpanishDateParserConfiguration  extends BaseOptionsConfiguration implements IDateParserConfiguration {

    private final String dateTokenPrefix;
    private final IExtractor integerExtractor;
    private final IExtractor ordinalExtractor;
    private final IExtractor cardinalExtractor;
    private final IParser numberParser;
    private final IDateTimeExtractor durationExtractor;
    private final IDateExtractor dateExtractor;
    private final IDateTimeParser durationParser;
    private final ImmutableMap<String, String> unitMap;
    private final Iterable<Pattern> dateRegexes;
    private final Pattern onRegex;
    private final Pattern specialDayRegex;
    private final Pattern specialDayWithNumRegex;
    private final Pattern nextRegex;
    private final Pattern thisRegex;
    private final Pattern lastRegex;
    private final Pattern unitRegex;
    private final Pattern weekDayRegex;
    private final Pattern monthRegex;
    private final Pattern weekDayOfMonthRegex;
    private final Pattern forTheRegex;
    private final Pattern weekDayAndDayOfMonthRegex;
    private final Pattern relativeMonthRegex;
    private final Pattern strictRelativeRegex;
    private final Pattern yearSuffix;
    private final Pattern relativeWeekDayRegex;
    private final Pattern relativeDayRegex;
    private final Pattern nextPrefixRegex;
    private final Pattern previousPrefixRegex;

    private final ImmutableMap<String, Integer> dayOfMonth;
    private final ImmutableMap<String, Integer> dayOfWeek;
    private final ImmutableMap<String, Integer> monthOfYear;
    private final ImmutableMap<String, Integer> cardinalMap;
    private final List<String> sameDayTerms;
    private final List<String> plusOneDayTerms;
    private final List<String> plusTwoDayTerms;
    private final List<String> minusOneDayTerms;
    private final List<String> minusTwoDayTerms;
    private final IDateTimeUtilityConfiguration utilityConfiguration;

    public SpanishDateParserConfiguration(ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        dateTokenPrefix = SpanishDateTime.DateTokenPrefix;
        integerExtractor = config.getIntegerExtractor();
        ordinalExtractor = config.getOrdinalExtractor();
        cardinalExtractor = config.getCardinalExtractor();
        numberParser = config.getNumberParser();
        durationExtractor = config.getDurationExtractor();
        dateExtractor = config.getDateExtractor();
        durationParser = config.getDurationParser();
        dateRegexes = Collections.unmodifiableList(SpanishDateExtractorConfiguration.DateRegexList);
        onRegex = SpanishDateExtractorConfiguration.OnRegex;
        specialDayRegex = SpanishDateExtractorConfiguration.SpecialDayRegex;
        specialDayWithNumRegex = SpanishDateExtractorConfiguration.SpecialDayWithNumRegex;
        nextRegex = SpanishDateExtractorConfiguration.NextDateRegex;
        thisRegex = SpanishDateExtractorConfiguration.ThisRegex;
        lastRegex = SpanishDateExtractorConfiguration.LastDateRegex;
        unitRegex = SpanishDateExtractorConfiguration.DateUnitRegex;
        weekDayRegex = SpanishDateExtractorConfiguration.WeekDayRegex;
        monthRegex = SpanishDateExtractorConfiguration.MonthRegex;
        weekDayOfMonthRegex = SpanishDateExtractorConfiguration.WeekDayOfMonthRegex;
        forTheRegex = SpanishDateExtractorConfiguration.ForTheRegex;
        weekDayAndDayOfMonthRegex = SpanishDateExtractorConfiguration.WeekDayAndDayOfMonthRegex;
        relativeMonthRegex = SpanishDateExtractorConfiguration.RelativeMonthRegex;
        strictRelativeRegex = SpanishDateExtractorConfiguration.StrictRelativeRegex;
        yearSuffix = SpanishDateExtractorConfiguration.YearSuffix;
        relativeWeekDayRegex = SpanishDateExtractorConfiguration.RelativeWeekDayRegex;
        relativeDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeDayRegex);
        nextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NextPrefixRegex);
        previousPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PreviousPrefixRegex);
        dayOfMonth = config.getDayOfMonth();
        dayOfWeek = config.getDayOfWeek();
        monthOfYear = config.getMonthOfYear();
        cardinalMap = config.getCardinalMap();
        unitMap = config.getUnitMap();
        utilityConfiguration = config.getUtilityConfiguration();
        sameDayTerms = Collections.unmodifiableList(SpanishDateTime.SameDayTerms);
        plusOneDayTerms = Collections.unmodifiableList(SpanishDateTime.PlusOneDayTerms);
        plusTwoDayTerms = Collections.unmodifiableList(SpanishDateTime.PlusTwoDayTerms);
        minusOneDayTerms = Collections.unmodifiableList(SpanishDateTime.MinusOneDayTerms);
        minusTwoDayTerms = Collections.unmodifiableList(SpanishDateTime.MinusTwoDayTerms);
    }

    @Override
    public String getDateTokenPrefix() {
        return dateTokenPrefix;
    }

    @Override
    public IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    @Override
    public IExtractor getOrdinalExtractor() {
        return ordinalExtractor;
    }

    @Override
    public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override
    public IParser getNumberParser() {
        return numberParser;
    }

    @Override
    public IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    @Override
    public IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    @Override
    public IDateTimeParser getDurationParser() {
        return durationParser;
    }

    @Override
    public Iterable<Pattern> getDateRegexes() {
        return dateRegexes;
    }

    @Override
    public Pattern getOnRegex() {
        return onRegex;
    }

    @Override
    public Pattern getSpecialDayRegex() {
        return specialDayRegex;
    }

    @Override
    public Pattern getSpecialDayWithNumRegex() {
        return specialDayWithNumRegex;
    }

    @Override
    public Pattern getNextRegex() {
        return nextRegex;
    }

    @Override
    public Pattern getThisRegex() {
        return thisRegex;
    }

    @Override
    public Pattern getLastRegex() {
        return lastRegex;
    }

    @Override
    public Pattern getUnitRegex() {
        return unitRegex;
    }

    @Override
    public Pattern getWeekDayRegex() {
        return weekDayRegex;
    }

    @Override
    public Pattern getMonthRegex() {
        return monthRegex;
    }

    @Override
    public Pattern getWeekDayOfMonthRegex() {
        return weekDayOfMonthRegex;
    }

    @Override
    public Pattern getForTheRegex() {
        return forTheRegex;
    }

    @Override
    public Pattern getWeekDayAndDayOfMonthRegex() {
        return weekDayAndDayOfMonthRegex;
    }

    @Override
    public Pattern getRelativeMonthRegex() {
        return relativeMonthRegex;
    }

    @Override
    public Pattern getStrictRelativeRegex() {
        return strictRelativeRegex;
    }

    @Override
    public Pattern getYearSuffix() {
        return yearSuffix;
    }

    @Override
    public Pattern getRelativeWeekDayRegex() {
        return relativeWeekDayRegex;
    }

    @Override
    public Pattern getRelativeDayRegex() {
        return relativeDayRegex;
    }

    @Override
    public Pattern getNextPrefixRegex() {
        return nextPrefixRegex;
    }

    @Override
    public Pattern getPastPrefixRegex() {
        return previousPrefixRegex;
    }

    @Override
    public ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    @Override
    public ImmutableMap<String, Integer> getDayOfMonth() {
        return dayOfMonth;
    }

    @Override
    public ImmutableMap<String, Integer> getDayOfWeek() {
        return dayOfWeek;
    }

    @Override
    public ImmutableMap<String, Integer> getMonthOfYear() {
        return monthOfYear;
    }

    @Override
    public ImmutableMap<String, Integer> getCardinalMap() {
        return cardinalMap;
    }

    @Override
    public List<String> getSameDayTerms() {
        return sameDayTerms;
    }

    @Override
    public List<String> getPlusOneDayTerms() {
        return plusOneDayTerms;
    }

    @Override
    public List<String> getMinusOneDayTerms() {
        return minusOneDayTerms;
    }

    @Override
    public List<String> getPlusTwoDayTerms() {
        return plusTwoDayTerms;
    }

    @Override
    public List<String> getMinusTwoDayTerms() {
        return minusTwoDayTerms;
    }

    @Override
    public IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }

    @Override
    public Integer getSwiftMonthOrYear(String text) {
        String trimmedText = text.trim().toLowerCase(Locale.ROOT);
        int swift = 0;

        Matcher regexMatcher = nextPrefixRegex.matcher(trimmedText);
        if (regexMatcher.find()) {
            swift = 1;
        }

        regexMatcher = previousPrefixRegex.matcher(trimmedText);
        if (regexMatcher.find()) {
            swift = -1;
        }

        return swift;
    }

    @Override
    public Boolean isCardinalLast(String text) {
        String trimmedText = text.trim().toLowerCase();
        return trimmedText.equals("last");
    }

    @Override
    public String normalize(String text) {
        return StringExtension.normalize(text, SpanishDateTime.SpecialCharactersEquivalent);
    }
}
