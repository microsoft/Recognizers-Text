package com.microsoft.recognizers.text.datetime.english.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDatePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Arrays;
import java.util.List;
import java.util.Optional;
import java.util.regex.Pattern;
import java.util.stream.Stream;

public class EnglishDatePeriodParserConfiguration extends BaseOptionsConfiguration implements IDatePeriodParserConfiguration {

    public EnglishDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        tokenBeforeDate = EnglishDateTime.TokenBeforeDate;

        cardinalExtractor = config.getCardinalExtractor();
        ordinalExtractor = config.getOrdinalExtractor();
        integerExtractor = config.getIntegerExtractor();
        numberParser = config.getNumberParser();
        dateExtractor = config.getDateExtractor();
        durationExtractor = config.getDurationExtractor();
        durationParser = config.getDurationParser();
        dateParser = config.getDateParser();

        monthFrontBetweenRegex = EnglishDatePeriodExtractorConfiguration.MonthFrontBetweenRegex;
        betweenRegex = EnglishDatePeriodExtractorConfiguration.BetweenRegex;
        monthFrontSimpleCasesRegex = EnglishDatePeriodExtractorConfiguration.MonthFrontSimpleCasesRegex;
        simpleCasesRegex = EnglishDatePeriodExtractorConfiguration.SimpleCasesRegex;
        oneWordPeriodRegex = EnglishDatePeriodExtractorConfiguration.OneWordPeriodRegex;
        monthWithYear = EnglishDatePeriodExtractorConfiguration.MonthWithYear;
        monthNumWithYear = EnglishDatePeriodExtractorConfiguration.MonthNumWithYear;
        yearRegex = EnglishDatePeriodExtractorConfiguration.YearRegex;
        pastRegex = EnglishDatePeriodExtractorConfiguration.PreviousPrefixRegex;
        futureRegex = EnglishDatePeriodExtractorConfiguration.NextPrefixRegex;
        futureSuffixRegex = EnglishDatePeriodExtractorConfiguration.FutureSuffixRegex;
        numberCombinedWithUnit = EnglishDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
        weekOfMonthRegex = EnglishDatePeriodExtractorConfiguration.WeekOfMonthRegex;
        weekOfYearRegex = EnglishDatePeriodExtractorConfiguration.WeekOfYearRegex;
        quarterRegex = EnglishDatePeriodExtractorConfiguration.QuarterRegex;
        quarterRegexYearFront = EnglishDatePeriodExtractorConfiguration.QuarterRegexYearFront;
        allHalfYearRegex = EnglishDatePeriodExtractorConfiguration.AllHalfYearRegex;
        seasonRegex = EnglishDatePeriodExtractorConfiguration.SeasonRegex;
        whichWeekRegex = EnglishDatePeriodExtractorConfiguration.WhichWeekRegex;
        weekOfRegex = EnglishDatePeriodExtractorConfiguration.WeekOfRegex;
        monthOfRegex = EnglishDatePeriodExtractorConfiguration.MonthOfRegex;
        restOfDateRegex = EnglishDatePeriodExtractorConfiguration.RestOfDateRegex;
        laterEarlyPeriodRegex = EnglishDatePeriodExtractorConfiguration.LaterEarlyPeriodRegex;
        weekWithWeekDayRangeRegex = EnglishDatePeriodExtractorConfiguration.WeekWithWeekDayRangeRegex;
        yearPlusNumberRegex = EnglishDatePeriodExtractorConfiguration.YearPlusNumberRegex;
        decadeWithCenturyRegex = EnglishDatePeriodExtractorConfiguration.DecadeWithCenturyRegex;
        yearPeriodRegex = EnglishDatePeriodExtractorConfiguration.YearPeriodRegex;
        complexDatePeriodRegex = EnglishDatePeriodExtractorConfiguration.ComplexDatePeriodRegex;
        relativeDecadeRegex = EnglishDatePeriodExtractorConfiguration.RelativeDecadeRegex;
        inConnectorRegex = config.getUtilityConfiguration().getInConnectorRegex();
        withinNextPrefixRegex = EnglishDatePeriodExtractorConfiguration.WithinNextPrefixRegex;
        referenceDatePeriodRegex = EnglishDatePeriodExtractorConfiguration.ReferenceDatePeriodRegex;
        agoRegex = EnglishDatePeriodExtractorConfiguration.AgoRegex;
        laterRegex = EnglishDatePeriodExtractorConfiguration.LaterRegex;
        lessThanRegex = EnglishDatePeriodExtractorConfiguration.LessThanRegex;
        moreThanRegex = EnglishDatePeriodExtractorConfiguration.MoreThanRegex;
        centurySuffixRegex = EnglishDatePeriodExtractorConfiguration.CenturySuffixRegex;
        relativeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeRegex);
        unspecificEndOfRangeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.UnspecificEndOfRangeRegex);
        nowRegex = EnglishDatePeriodExtractorConfiguration.NowRegex;

        unitMap = config.getUnitMap();
        cardinalMap = config.getCardinalMap();
        dayOfMonth = config.getDayOfMonth();
        monthOfYear = config.getMonthOfYear();
        seasonMap = config.getSeasonMap();
        writtenDecades = config.getWrittenDecades();
        numbers = config.getNumbers();
        specialDecadeCases = config.getSpecialDecadeCases();

        nextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex);
        previousPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PreviousPrefixRegex);
        thisPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ThisPrefixRegex);
        afterNextSuffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AfterNextSuffixRegex);
    }

    private final String tokenBeforeDate;

    // InternalParsers

    private final IDateExtractor dateExtractor;
    private final IExtractor cardinalExtractor;
    private final IExtractor ordinalExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IExtractor integerExtractor;
    private final IParser numberParser;
    private final IDateTimeParser dateParser;
    private final IDateTimeParser durationParser;

    // Regex

    private final Pattern monthFrontBetweenRegex;
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
    private final Pattern afterNextSuffixRegex;
    private final Pattern nowRegex;

    // Dictionaries
    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Integer> cardinalMap;
    private final ImmutableMap<String, Integer> dayOfMonth;
    private final ImmutableMap<String, Integer> monthOfYear;
    private final ImmutableMap<String, String> seasonMap;
    private final ImmutableMap<String, Integer> writtenDecades;
    private final ImmutableMap<String, Integer> numbers;
    private final ImmutableMap<String, Integer> specialDecadeCases;

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
    public int getSwiftDayOrMonth(String text) {

        String trimmedText = text.trim().toLowerCase();
        int swift = 0;

        Optional<Match> matchAfterNext = Arrays.stream(RegExpUtility.getMatches(afterNextSuffixRegex, trimmedText)).findFirst();
        Optional<Match> matchNext = Arrays.stream(RegExpUtility.getMatches(nextPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchPast = Arrays.stream(RegExpUtility.getMatches(previousPrefixRegex, trimmedText)).findFirst();

        if (matchAfterNext.isPresent()) {
            swift = 2;
        } else if (matchNext.isPresent()) {
            swift = 1;
        } else if (matchPast.isPresent()) {
            swift = -1;
        }

        return swift;
    }

    @Override
    public int getSwiftYear(String text) {

        String trimmedText = text.trim().toLowerCase();
        int swift = -10;

        Optional<Match> matchAfterNext = Arrays.stream(RegExpUtility.getMatches(afterNextSuffixRegex, trimmedText)).findFirst();
        Optional<Match> matchNext = Arrays.stream(RegExpUtility.getMatches(nextPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchPast = Arrays.stream(RegExpUtility.getMatches(previousPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchThisPresent = Arrays.stream(RegExpUtility.getMatches(thisPrefixRegex, trimmedText)).findFirst();

        if (matchAfterNext.isPresent()) {
            swift = 2;
        } else if (matchNext.isPresent()) {
            swift = 1;
        } else if (matchPast.isPresent()) {
            swift = -1;
        } else if (matchThisPresent.isPresent()) {
            swift = 0;
        }

        return swift;
    }

    @Override
    public boolean isFuture(String text) {
        String trimmedText = text.trim().toLowerCase();
        return (trimmedText.startsWith("this") || trimmedText.startsWith("next"));
    }

    @Override
    public boolean isLastCardinal(String text) {
        String trimmedText = text.trim().toLowerCase();
        return trimmedText.equals("last");
    }

    @Override
    public boolean isMonthOnly(String text) {
        String trimmedText = text.trim().toLowerCase();
        Optional<Match> matchAfterNext = Arrays.stream(RegExpUtility.getMatches(afterNextSuffixRegex, trimmedText)).findFirst();
        return trimmedText.endsWith("month") || trimmedText.contains(" month ") && matchAfterNext.isPresent();
    }

    @Override
    public boolean isMonthToDate(String text) {
        String trimmedText = text.trim().toLowerCase();
        return trimmedText.equals("month to date");
    }

    @Override
    public boolean isWeekend(String text) {
        String trimmedText = text.trim().toLowerCase();
        Optional<Match> matchAfterNext = Arrays.stream(RegExpUtility.getMatches(afterNextSuffixRegex, trimmedText)).findFirst();
        return trimmedText.endsWith("weekend") || trimmedText.contains(" weekend ") && matchAfterNext.isPresent();
    }

    @Override
    public boolean isWeekOnly(String text) {
        String trimmedText = text.trim().toLowerCase();
        Optional<Match> matchAfterNext = Arrays.stream(RegExpUtility.getMatches(afterNextSuffixRegex, trimmedText)).findFirst();
        return trimmedText.endsWith("week") || trimmedText.contains(" week ") && matchAfterNext.isPresent();
    }

    @Override
    public boolean isYearOnly(String text) {
        String trimmedText = text.trim().toLowerCase();
        return EnglishDateTime.YearTerms.stream().anyMatch(o -> trimmedText.endsWith(o)) ||
            (getYearTermsPadded().anyMatch(o -> trimmedText.contains(o)) && RegExpUtility.getMatches(afterNextSuffixRegex, trimmedText).length > 0) ||
            (EnglishDateTime.GenericYearTerms.stream().anyMatch(o -> trimmedText.endsWith(o)) && RegExpUtility.getMatches(unspecificEndOfRangeRegex, trimmedText).length > 0);
    }

    @Override
    public boolean isYearToDate(String text) {
        String trimmedText = text.trim().toLowerCase();
        return trimmedText.equals("year to date");
    }

    private Stream<String> getYearTermsPadded() {
        return EnglishDateTime.YearTerms.stream().map(i -> String.format(" %s ", i));
    }
}
