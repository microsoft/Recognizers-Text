// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.parsers;

import static com.microsoft.recognizers.text.datetime.resources.GermanDateTime.TodayNowRegex;

import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Pattern;
import java.util.stream.Stream;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDatePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

public class GermanDatePeriodParserConfiguration
        extends BaseOptionsConfiguration
        implements IDatePeriodParserConfiguration {
    private final String tokenBeforeDate;
    private final IDateExtractor dateExtractor;

    // InternalParsers
    private final IExtractor cardinalExtractor;
    private final IExtractor ordinalExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IExtractor integerExtractor;
    private final IParser numberParser;
    private final IDateTimeParser dateParser;
    private final IDateTimeParser durationParser;
    private final Pattern monthFrontBetweenRegex;

    // Regex
    private final Pattern betweenRegex;
    private final Pattern monthFrontSimpleCasesRegex;
    private final Pattern simpleCasesRegex;
    private final Pattern oneWordPeriodRegex;
    private final Pattern monthWithYear;
    private final Pattern monthNumWithYear;
    private final Pattern yearRegex;
    private final Pattern pastRegex;
    private final Pattern futureRegex;
    private final Pattern futureSuffixRegex;
    private final Pattern numberCombinedWithUnit;
    private final Pattern weekOfMonthRegex;
    private final Pattern weekOfYearRegex;
    private final Pattern quarterRegex;
    private final Pattern quarterRegexYearFront;
    private final Pattern allHalfYearRegex;
    private final Pattern seasonRegex;
    private final Pattern whichWeekRegex;
    private final Pattern weekOfRegex;
    private final Pattern monthOfRegex;
    private final Pattern inConnectorRegex;
    private final Pattern withinNextPrefixRegex;
    private final Pattern restOfDateRegex;
    private final Pattern laterEarlyPeriodRegex;
    private final Pattern weekWithWeekDayRangeRegex;
    private final Pattern yearPlusNumberRegex;
    private final Pattern decadeWithCenturyRegex;
    private final Pattern yearPeriodRegex;
    private final Pattern complexDatePeriodRegex;
    private final Pattern relativeDecadeRegex;
    private final Pattern referenceDatePeriodRegex;
    private final Pattern agoRegex;
    private final Pattern laterRegex;
    private final Pattern lessThanRegex;
    private final Pattern moreThanRegex;
    private final Pattern centurySuffixRegex;
    private final Pattern relativeRegex;
    private final Pattern unspecificEndOfRangeRegex;
    private final Pattern nextPrefixRegex;
    private final Pattern previousPrefixRegex;
    private final Pattern thisPrefixRegex;
    private final Pattern afterNextPrefixRegex;
    private final Pattern nowRegex;
    private final Pattern firstLastRegex;
    private final Pattern ofYearRegex;
    private final Pattern specialDayRegex;
    private final Pattern todayNowRegex;
    private final Pattern penultimatePrefixRegex;

    // Dictionaries
    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Integer> cardinalMap;
    private final ImmutableMap<String, Integer> dayOfMonth;
    private final ImmutableMap<String, Integer> monthOfYear;
    private final ImmutableMap<String, String> seasonMap;
    private final ImmutableMap<String, String> specialYearPrefixesMap;
    private final ImmutableMap<String, Integer> writtenDecades;
    private final ImmutableMap<String, Integer> numbers;
    private final ImmutableMap<String, Integer> specialDecadeCases;

    public GermanDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        tokenBeforeDate = GermanDateTime.TokenBeforeDate;

        cardinalExtractor = config.getCardinalExtractor();
        ordinalExtractor = config.getOrdinalExtractor();
        integerExtractor = config.getIntegerExtractor();
        numberParser = config.getNumberParser();
        dateExtractor = config.getDateExtractor();
        durationExtractor = config.getDurationExtractor();
        durationParser = config.getDurationParser();
        dateParser = config.getDateParser();

        monthFrontBetweenRegex = GermanDatePeriodExtractorConfiguration.MonthFrontBetweenRegex;
        betweenRegex = GermanDatePeriodExtractorConfiguration.BetweenRegex;
        monthFrontSimpleCasesRegex = GermanDatePeriodExtractorConfiguration.MonthFrontSimpleCasesRegex;
        simpleCasesRegex = GermanDatePeriodExtractorConfiguration.SimpleCasesRegex;
        oneWordPeriodRegex = GermanDatePeriodExtractorConfiguration.OneWordPeriodRegex;
        monthWithYear = GermanDatePeriodExtractorConfiguration.MonthWithYear;
        monthNumWithYear = GermanDatePeriodExtractorConfiguration.MonthNumWithYear;
        yearRegex = GermanDatePeriodExtractorConfiguration.YearRegex;
        pastRegex = GermanDatePeriodExtractorConfiguration.PreviousPrefixRegex;
        futureRegex = GermanDatePeriodExtractorConfiguration.NextPrefixRegex;
        futureSuffixRegex = GermanDatePeriodExtractorConfiguration.FutureSuffixRegex;
        numberCombinedWithUnit = GermanDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
        weekOfMonthRegex = GermanDatePeriodExtractorConfiguration.WeekOfMonthRegex;
        weekOfYearRegex = GermanDatePeriodExtractorConfiguration.WeekOfYearRegex;
        quarterRegex = GermanDatePeriodExtractorConfiguration.QuarterRegex;
        quarterRegexYearFront = GermanDatePeriodExtractorConfiguration.QuarterRegexYearFront;
        allHalfYearRegex = GermanDatePeriodExtractorConfiguration.AllHalfYearRegex;
        seasonRegex = GermanDatePeriodExtractorConfiguration.SeasonRegex;
        whichWeekRegex = GermanDatePeriodExtractorConfiguration.WhichWeekRegex;
        weekOfRegex = GermanDatePeriodExtractorConfiguration.WeekOfRegex;
        monthOfRegex = GermanDatePeriodExtractorConfiguration.MonthOfRegex;
        restOfDateRegex = GermanDatePeriodExtractorConfiguration.RestOfDateRegex;
        laterEarlyPeriodRegex = GermanDatePeriodExtractorConfiguration.LaterEarlyPeriodRegex;
        weekWithWeekDayRangeRegex = GermanDatePeriodExtractorConfiguration.WeekWithWeekDayRangeRegex;
        yearPlusNumberRegex = GermanDatePeriodExtractorConfiguration.YearPlusNumberRegex;
        decadeWithCenturyRegex = GermanDatePeriodExtractorConfiguration.DecadeWithCenturyRegex;
        yearPeriodRegex = GermanDatePeriodExtractorConfiguration.YearPeriodRegex;
        complexDatePeriodRegex = GermanDatePeriodExtractorConfiguration.ComplexDatePeriodRegex;
        relativeDecadeRegex = GermanDatePeriodExtractorConfiguration.RelativeDecadeRegex;
        inConnectorRegex = config.getUtilityConfiguration().getInConnectorRegex();
        withinNextPrefixRegex = GermanDatePeriodExtractorConfiguration.WithinNextPrefixRegex;
        referenceDatePeriodRegex = GermanDatePeriodExtractorConfiguration.ReferenceDatePeriodRegex;
        agoRegex = GermanDatePeriodExtractorConfiguration.AgoRegex;
        laterRegex = GermanDatePeriodExtractorConfiguration.LaterRegex;
        lessThanRegex = GermanDatePeriodExtractorConfiguration.LessThanRegex;
        moreThanRegex = GermanDatePeriodExtractorConfiguration.MoreThanRegex;
        centurySuffixRegex = GermanDatePeriodExtractorConfiguration.CenturySuffixRegex;
        nowRegex = GermanDatePeriodExtractorConfiguration.NowRegex;
        firstLastRegex = GermanDatePeriodExtractorConfiguration.FirstLastRegex;
        ofYearRegex = GermanDatePeriodExtractorConfiguration.OfYearRegex;
        specialDayRegex = GermanDateExtractorConfiguration.SpecialDayRegex;
        todayNowRegex = RegExpUtility.getSafeRegExp(TodayNowRegex);
        relativeRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RelativeRegex);
        unspecificEndOfRangeRegex = RegExpUtility.getSafeRegExp(GermanDateTime.UnspecificEndOfRangeRegex);
        penultimatePrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PenultimatePrefixRegex);

        unitMap = config.getUnitMap();
        cardinalMap = config.getCardinalMap();
        dayOfMonth = config.getDayOfMonth();
        monthOfYear = config.getMonthOfYear();
        seasonMap = config.getSeasonMap();
        specialYearPrefixesMap = config.getSpecialYearPrefixesMap();
        writtenDecades = config.getWrittenDecades();
        numbers = config.getNumbers();
        specialDecadeCases = config.getSpecialDecadeCases();

        nextPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.NextPrefixRegex);
        previousPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PreviousPrefixRegex);
        thisPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.ThisPrefixRegex);
        afterNextPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AfterNextPrefixRegex);
    }

    @Override
    public String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    @Override
    public IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    @Override
    public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override
    public IExtractor getOrdinalExtractor() {
        return ordinalExtractor;
    }

    @Override
    public IExtractor getIntegerExtractor() {
        return integerExtractor;
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
    public IDateTimeParser getDurationParser() {
        return durationParser;
    }

    @Override
    public IDateTimeParser getDateParser() {
        return dateParser;
    }

    @Override
    public Pattern getMonthFrontBetweenRegex() {
        return monthFrontBetweenRegex;
    }

    @Override
    public Pattern getBetweenRegex() {
        return betweenRegex;
    }

    @Override
    public Pattern getMonthFrontSimpleCasesRegex() {
        return monthFrontSimpleCasesRegex;
    }

    @Override
    public Pattern getSimpleCasesRegex() {
        return simpleCasesRegex;
    }

    @Override
    public Pattern getOneWordPeriodRegex() {
        return oneWordPeriodRegex;
    }

    @Override
    public Pattern getMonthWithYear() {
        return monthWithYear;
    }

    @Override
    public Pattern getMonthNumWithYear() {
        return monthNumWithYear;
    }

    @Override
    public Pattern getYearRegex() {
        return yearRegex;
    }

    @Override
    public Pattern getPastRegex() {
        return pastRegex;
    }

    @Override
    public Pattern getFutureRegex() {
        return futureRegex;
    }

    @Override
    public Pattern getFutureSuffixRegex() {
        return futureSuffixRegex;
    }

    @Override
    public Pattern getNumberCombinedWithUnit() {
        return numberCombinedWithUnit;
    }

    @Override
    public Pattern getWeekOfMonthRegex() {
        return weekOfMonthRegex;
    }

    @Override
    public Pattern getWeekOfYearRegex() {
        return weekOfYearRegex;
    }

    @Override
    public Pattern getQuarterRegex() {
        return quarterRegex;
    }

    @Override
    public Pattern getQuarterRegexYearFront() {
        return quarterRegexYearFront;
    }

    @Override
    public Pattern getAllHalfYearRegex() {
        return allHalfYearRegex;
    }

    @Override
    public Pattern getSeasonRegex() {
        return seasonRegex;
    }

    @Override
    public Pattern getWhichWeekRegex() {
        return whichWeekRegex;
    }

    @Override
    public Pattern getWeekOfRegex() {
        return weekOfRegex;
    }

    @Override
    public Pattern getMonthOfRegex() {
        return monthOfRegex;
    }

    @Override
    public Pattern getInConnectorRegex() {
        return inConnectorRegex;
    }

    @Override
    public Pattern getWithinNextPrefixRegex() {
        return withinNextPrefixRegex;
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
    public Pattern getThisPrefixRegex() {
        return thisPrefixRegex;
    }

    @Override
    public Pattern getRestOfDateRegex() {
        return restOfDateRegex;
    }

    @Override
    public Pattern getLaterEarlyPeriodRegex() {
        return laterEarlyPeriodRegex;
    }

    @Override
    public Pattern getWeekWithWeekDayRangeRegex() {
        return weekWithWeekDayRangeRegex;
    }

    @Override
    public Pattern getYearPlusNumberRegex() {
        return yearPlusNumberRegex;
    }

    @Override
    public Pattern getDecadeWithCenturyRegex() {
        return decadeWithCenturyRegex;
    }

    @Override
    public Pattern getYearPeriodRegex() {
        return yearPeriodRegex;
    }

    @Override
    public Pattern getComplexDatePeriodRegex() {
        return complexDatePeriodRegex;
    }

    @Override
    public Pattern getRelativeDecadeRegex() {
        return relativeDecadeRegex;
    }

    @Override
    public Pattern getReferenceDatePeriodRegex() {
        return referenceDatePeriodRegex;
    }

    @Override
    public Pattern getAgoRegex() {
        return agoRegex;
    }

    @Override
    public Pattern getLaterRegex() {
        return laterRegex;
    }

    @Override
    public Pattern getLessThanRegex() {
        return lessThanRegex;
    }

    @Override
    public Pattern getMoreThanRegex() {
        return moreThanRegex;
    }

    @Override
    public Pattern getCenturySuffixRegex() {
        return centurySuffixRegex;
    }

    @Override
    public Pattern getRelativeRegex() {
        return relativeRegex;
    }

    @Override
    public Pattern getUnspecificEndOfRangeRegex() {
        return unspecificEndOfRangeRegex;
    }

    @Override
    public Pattern getNowRegex() {
        return nowRegex;
    }

    @Override
    public ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    @Override
    public ImmutableMap<String, Integer> getCardinalMap() {
        return cardinalMap;
    }

    @Override
    public ImmutableMap<String, Integer> getDayOfMonth() {
        return dayOfMonth;
    }

    @Override
    public ImmutableMap<String, Integer> getMonthOfYear() {
        return monthOfYear;
    }

    @Override
    public ImmutableMap<String, String> getSeasonMap() {
        return seasonMap;
    }

    @Override
    public ImmutableMap<String, String> getSpecialYearPrefixesMap() {
        return specialYearPrefixesMap;
    }

    @Override
    public ImmutableMap<String, Integer> getWrittenDecades() {
        return writtenDecades;
    }

    @Override
    public ImmutableMap<String, Integer> getNumbers() {
        return numbers;
    }

    @Override
    public ImmutableMap<String, Integer> getSpecialDecadeCases() {
        return specialDecadeCases;
    }

    @Override
    public int getSwiftDayOrMonth(final String text) {

        final String trimmedText = text.trim().toLowerCase();

        if (afterNextPrefixRegex.matcher(trimmedText).find()) {
            return 2;
        }
        else if (nextPrefixRegex.matcher(trimmedText).find()) {
            return 1;
        }
        else if (previousPrefixRegex.matcher(trimmedText).find()) {
            return -1;
        }
        else if (penultimatePrefixRegex.matcher(trimmedText).find()) {
            return -2;
        }

        return 0;
    }

    @Override
    public int getSwiftYear(final String text) {

        final String trimmedText = text.trim().toLowerCase();

        if (nextPrefixRegex.matcher(trimmedText).find()) {
            return 1;
        }
        else if (previousPrefixRegex.matcher(trimmedText).find()) {
            return -1;
        }
        else if (penultimatePrefixRegex.matcher(trimmedText).find()) {
            return -2;
        }
        else if (thisPrefixRegex.matcher(trimmedText).find()) {
            return 0;
        }

        return -10;
    }

    @Override
    public boolean isFuture(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return thisPrefixRegex.matcher(trimmedText).find() || nextPrefixRegex.matcher(trimmedText).find();
    }

    @Override
    public boolean isLastCardinal(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return previousPrefixRegex.matcher(trimmedText).find();
    }

    @Override
    public boolean isMonthOnly(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return GermanDateTime.MonthTerms.stream().anyMatch(trimmedText::endsWith);
    }

    @Override
    public boolean isMonthToDate(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return GermanDateTime.MonthToDateTerms.stream().anyMatch(trimmedText::equals);
    }

    @Override
    public boolean isWeekend(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return GermanDateTime.WeekendTerms.stream().anyMatch(trimmedText::endsWith);
    }

    @Override
    public boolean isWeekOnly(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return GermanDateTime.WeekTerms.stream().anyMatch(trimmedText::endsWith);
    }

    @Override
    public boolean isYearOnly(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return GermanDateTime.YearTerms.stream().anyMatch(trimmedText::endsWith);
    }

    @Override
    public boolean isYearToDate(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return GermanDateTime.YearToDateTerms.stream().anyMatch(trimmedText::equals);
    }

    public boolean IsFortnight(final String text)
    {
        return false;
    }
}
