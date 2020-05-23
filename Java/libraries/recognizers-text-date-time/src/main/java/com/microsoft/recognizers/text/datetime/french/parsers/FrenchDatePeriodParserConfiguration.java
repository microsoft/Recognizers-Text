package com.microsoft.recognizers.text.datetime.french.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDatePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Pattern;

public class FrenchDatePeriodParserConfiguration extends BaseOptionsConfiguration implements IDatePeriodParserConfiguration {

    public static final Pattern nextPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextPrefixRegex);
    public static final Pattern previousPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PreviousPrefixRegex);
    public static final Pattern thisPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ThisPrefixRegex);
    public static final Pattern nextSuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextSuffixRegex);
    public static final Pattern pastSuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PastSuffixRegex);
    public static final Pattern relativeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeRegex);
    public static final Pattern unspecificEndOfRangeRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.UnspecificEndOfRangeRegex);
    private final String tokenBeforeDate;

    // Regex
    private final IDateExtractor dateExtractor;
    private final IExtractor cardinalExtractor;
    private final IExtractor ordinalExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IExtractor integerExtractor;
    private final IParser numberParser;
    private final IDateTimeParser dateParser;
    private final IDateTimeParser durationParser;
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
    private final Pattern nowRegex;
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

    public FrenchDatePeriodParserConfiguration(final ICommonDateTimeParserConfiguration config) {
        super(config.getOptions());

        tokenBeforeDate = FrenchDateTime.TokenBeforeDate;
        cardinalExtractor = config.getCardinalExtractor();
        ordinalExtractor = config.getOrdinalExtractor();
        integerExtractor = config.getIntegerExtractor();
        numberParser = config.getNumberParser();
        durationExtractor = config.getDurationExtractor();
        dateExtractor = config.getDateExtractor();
        durationParser = config.getDurationParser();
        dateParser = config.getDateParser();
        monthFrontBetweenRegex = FrenchDatePeriodExtractorConfiguration.MonthFrontBetweenRegex;
        betweenRegex = FrenchDatePeriodExtractorConfiguration.BetweenRegex;
        monthFrontSimpleCasesRegex = FrenchDatePeriodExtractorConfiguration.MonthFrontSimpleCasesRegex;
        simpleCasesRegex = FrenchDatePeriodExtractorConfiguration.SimpleCasesRegex;
        oneWordPeriodRegex = FrenchDatePeriodExtractorConfiguration.OneWordPeriodRegex;
        monthWithYear = FrenchDatePeriodExtractorConfiguration.MonthWithYearRegex;
        monthNumWithYear = FrenchDatePeriodExtractorConfiguration.MonthNumWithYearRegex;
        yearRegex = FrenchDatePeriodExtractorConfiguration.YearRegex;
        pastRegex = FrenchDatePeriodExtractorConfiguration.PastRegex;
        futureRegex = FrenchDatePeriodExtractorConfiguration.FutureRegex;
        futureSuffixRegex = FrenchDatePeriodExtractorConfiguration.FutureSuffixRegex;
        numberCombinedWithUnit = FrenchDurationExtractorConfiguration.NumberCombinedWithUnit;
        weekOfMonthRegex = FrenchDatePeriodExtractorConfiguration.WeekOfMonthRegex;
        weekOfYearRegex = FrenchDatePeriodExtractorConfiguration.WeekOfYearRegex;
        quarterRegex = FrenchDatePeriodExtractorConfiguration.QuarterRegex;
        quarterRegexYearFront = FrenchDatePeriodExtractorConfiguration.QuarterRegexYearFront;
        allHalfYearRegex = FrenchDatePeriodExtractorConfiguration.AllHalfYearRegex;
        seasonRegex = FrenchDatePeriodExtractorConfiguration.SeasonRegex;
        whichWeekRegex = FrenchDatePeriodExtractorConfiguration.WhichWeekRegex;
        weekOfRegex = FrenchDatePeriodExtractorConfiguration.WeekOfRegex;
        monthOfRegex = FrenchDatePeriodExtractorConfiguration.MonthOfRegex;
        restOfDateRegex = FrenchDatePeriodExtractorConfiguration.RestOfDateRegex;
        laterEarlyPeriodRegex = FrenchDatePeriodExtractorConfiguration.LaterEarlyPeriodRegex;
        weekWithWeekDayRangeRegex = FrenchDatePeriodExtractorConfiguration.WeekWithWeekDayRangeRegex;
        yearPlusNumberRegex = FrenchDatePeriodExtractorConfiguration.YearPlusNumberRegex;
        decadeWithCenturyRegex = FrenchDatePeriodExtractorConfiguration.DecadeWithCenturyRegex;
        yearPeriodRegex = FrenchDatePeriodExtractorConfiguration.YearPeriodRegex;
        complexDatePeriodRegex = FrenchDatePeriodExtractorConfiguration.ComplexDatePeriodRegex;
        relativeDecadeRegex = FrenchDatePeriodExtractorConfiguration.RelativeDecadeRegex;
        inConnectorRegex = config.getUtilityConfiguration().getInConnectorRegex();
        withinNextPrefixRegex = FrenchDatePeriodExtractorConfiguration.WithinNextPrefixRegex;
        referenceDatePeriodRegex = FrenchDatePeriodExtractorConfiguration.ReferenceDatePeriodRegex;
        agoRegex = FrenchDatePeriodExtractorConfiguration.AgoRegex;
        laterRegex = FrenchDatePeriodExtractorConfiguration.LaterRegex;
        lessThanRegex = FrenchDatePeriodExtractorConfiguration.LessThanRegex;
        moreThanRegex = FrenchDatePeriodExtractorConfiguration.MoreThanRegex;
        centurySuffixRegex = FrenchDatePeriodExtractorConfiguration.CenturySuffixRegex;
        nowRegex = FrenchDatePeriodExtractorConfiguration.NowRegex;

        unitMap = config.getUnitMap();
        cardinalMap = config.getCardinalMap();
        dayOfMonth = config.getDayOfMonth();
        monthOfYear = config.getMonthOfYear();
        seasonMap = config.getSeasonMap();
        specialYearPrefixesMap = config.getSpecialYearPrefixesMap();
        numbers = config.getNumbers();
        writtenDecades = config.getWrittenDecades();
        specialDecadeCases = config.getSpecialDecadeCases();
    }

    @Override
    public final String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    @Override
    public final IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    @Override
    public final IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override
    public final IExtractor getOrdinalExtractor() {
        return ordinalExtractor;
    }

    @Override
    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    @Override
    public final IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    @Override
    public final IParser getNumberParser() {
        return numberParser;
    }

    @Override
    public final IDateTimeParser getDateParser() {
        return dateParser;
    }

    @Override
    public final IDateTimeParser getDurationParser() {
        return durationParser;
    }

    @Override
    public final Pattern getMonthFrontBetweenRegex() {
        return monthFrontBetweenRegex;
    }

    @Override
    public final Pattern getBetweenRegex() {
        return betweenRegex;
    }

    @Override
    public final Pattern getMonthFrontSimpleCasesRegex() {
        return monthFrontSimpleCasesRegex;
    }

    @Override
    public final Pattern getSimpleCasesRegex() {
        return simpleCasesRegex;
    }

    @Override
    public final Pattern getOneWordPeriodRegex() {
        return oneWordPeriodRegex;
    }

    @Override
    public final Pattern getMonthWithYear() {
        return monthWithYear;
    }

    @Override
    public final Pattern getMonthNumWithYear() {
        return monthNumWithYear;
    }

    @Override
    public final Pattern getYearRegex() {
        return yearRegex;
    }

    @Override
    public final Pattern getPastRegex() {
        return pastRegex;
    }

    @Override
    public final Pattern getFutureRegex() {
        return futureRegex;
    }

    @Override
    public final Pattern getFutureSuffixRegex() {
        return futureSuffixRegex;
    }

    @Override
    public final Pattern getNumberCombinedWithUnit() {
        return numberCombinedWithUnit;
    }

    @Override
    public final Pattern getWeekOfMonthRegex() {
        return weekOfMonthRegex;
    }

    @Override
    public final Pattern getWeekOfYearRegex() {
        return weekOfYearRegex;
    }

    @Override
    public final Pattern getQuarterRegex() {
        return quarterRegex;
    }

    @Override
    public final Pattern getQuarterRegexYearFront() {
        return quarterRegexYearFront;
    }

    @Override
    public final Pattern getAllHalfYearRegex() {
        return allHalfYearRegex;
    }

    @Override
    public final Pattern getSeasonRegex() {
        return seasonRegex;
    }

    @Override
    public final Pattern getWhichWeekRegex() {
        return whichWeekRegex;
    }

    @Override
    public final Pattern getWeekOfRegex() {
        return weekOfRegex;
    }

    @Override
    public final Pattern getMonthOfRegex() {
        return monthOfRegex;
    }

    @Override
    public final Pattern getInConnectorRegex() {
        return inConnectorRegex;
    }

    @Override
    public final Pattern getWithinNextPrefixRegex() {
        return withinNextPrefixRegex;
    }

    @Override
    public final Pattern getRestOfDateRegex() {
        return restOfDateRegex;
    }

    @Override
    public final Pattern getLaterEarlyPeriodRegex() {
        return laterEarlyPeriodRegex;
    }

    @Override
    public final Pattern getWeekWithWeekDayRangeRegex() {
        return laterEarlyPeriodRegex;
    }

    @Override
    public final Pattern getYearPlusNumberRegex() {
        return yearPlusNumberRegex;
    }

    @Override
    public final Pattern getDecadeWithCenturyRegex() {
        return decadeWithCenturyRegex;
    }

    @Override
    public final Pattern getYearPeriodRegex() {
        return yearPeriodRegex;
    }

    @Override
    public final Pattern getComplexDatePeriodRegex() {
        return complexDatePeriodRegex;
    }

    @Override
    public final Pattern getRelativeDecadeRegex() {
        return complexDatePeriodRegex;
    }

    @Override
    public final Pattern getReferenceDatePeriodRegex() {
        return referenceDatePeriodRegex;
    }

    @Override
    public final Pattern getAgoRegex() {
        return agoRegex;
    }

    @Override
    public final Pattern getLaterRegex() {
        return laterRegex;
    }

    @Override
    public final Pattern getLessThanRegex() {
        return lessThanRegex;
    }

    @Override
    public final Pattern getMoreThanRegex() {
        return moreThanRegex;
    }

    @Override
    public final Pattern getCenturySuffixRegex() {
        return centurySuffixRegex;
    }

    @Override
    public final Pattern getNextPrefixRegex() {
        return nextPrefixRegex;
    }

    @Override
    public final Pattern getPastPrefixRegex() {
        return previousPrefixRegex;
    }

    @Override
    public final Pattern getThisPrefixRegex() {
        return thisPrefixRegex;
    }

    @Override
    public final Pattern getRelativeRegex() {
        return relativeRegex;
    }

    @Override
    public final Pattern getUnspecificEndOfRangeRegex() {
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
        int swift = 0;

        if (trimmedText.endsWith("prochain") || trimmedText.endsWith("prochaine")) {
            swift = 1;
        }

        if (trimmedText.endsWith("dernière") ||
            trimmedText.endsWith("dernières") ||
            trimmedText.endsWith("derniere") ||
            trimmedText.endsWith("dernieres")
        ) {
            swift = -1;
        }

        return swift;
    }

    @Override
    public int getSwiftYear(final String text) {

        final String trimmedText = text.trim().toLowerCase();
        int swift = -10;

        if (trimmedText.endsWith("prochain") || trimmedText.endsWith("prochaine")) {
            swift = 1;
        }

        if (trimmedText.endsWith("dernière") || trimmedText.endsWith("dernières") || trimmedText
            .endsWith("derniere") || trimmedText.endsWith("dernieres")) {
            swift = -1;
        } else if (trimmedText.startsWith("cette")) {
            swift = 0;
        }

        return swift;
    }

    @Override
    public boolean isFuture(final String text) {
        final String trimmedText = text.trim().toLowerCase();

        return FrenchDateTime.FutureStartTerms.stream().anyMatch(o -> trimmedText.startsWith(o)) || FrenchDateTime.FutureEndTerms.stream().anyMatch(o -> trimmedText.endsWith(o));
    }

    @Override
    public boolean isLastCardinal(final String text) {
        final String trimmedText = text.trim().toLowerCase();

        final Optional<Match> matchLast = Arrays.stream(RegExpUtility.getMatches(previousPrefixRegex, trimmedText))
            .findFirst();
        return matchLast.isPresent();
    }

    @Override
    public boolean isMonthOnly(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return FrenchDateTime.MonthTerms.stream().anyMatch(o -> trimmedText.endsWith(o));
    }

    @Override
    public boolean isMonthToDate(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return FrenchDateTime.MonthToDateTerms.stream().anyMatch(o -> trimmedText.endsWith(o));
    }

    @Override
    public boolean isWeekend(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return FrenchDateTime.WeekendTerms.stream().anyMatch(o -> trimmedText.endsWith(o));
    }

    @Override
    public boolean isWeekOnly(final String text) {
        final String trimmedText = text.trim().toLowerCase();

        final boolean nextSuffix = Arrays.stream(RegExpUtility.getMatches(nextSuffixRegex, trimmedText))
            .findFirst().isPresent();
        final boolean pastSuffix = Arrays.stream(RegExpUtility.getMatches(pastSuffixRegex, trimmedText))
            .findFirst().isPresent();

        return (FrenchDateTime.WeekTerms.stream().anyMatch(o -> trimmedText.endsWith(o)) ||
            (FrenchDateTime.WeekTerms.stream().anyMatch(o -> trimmedText.contains(o)) && (nextSuffix || pastSuffix))) &&
            !FrenchDateTime.WeekendTerms.stream().anyMatch(o -> trimmedText.endsWith(o));
    }

    @Override
    public boolean isYearOnly(final String text) {
        final String trimmedText = text.trim().toLowerCase();
        return FrenchDateTime.YearTerms.stream().anyMatch(o -> trimmedText.endsWith(o));
    }

    @Override
    public boolean isYearToDate(final String text) {
        final String trimmedText = text.trim().toLowerCase();

        return FrenchDateTime.YearToDateTerms.stream().anyMatch(o -> trimmedText.endsWith(o));
    }
}
