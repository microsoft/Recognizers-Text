package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.IDatePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.number.chinese.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.chinese.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.number.chinese.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.chinese.parsers.ChineseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Pattern;

public class ChineseDatePeriodParserConfiguration extends BaseOptionsConfiguration implements IDatePeriodParserConfiguration {

    private final String tokenBeforeDate;

    private final IDateExtractor dateExtractor;
    private final IExtractor cardinalExtractor;
    private final IExtractor ordinalExtractor;
    private final IExtractor integerExtractor;
    private final IParser numberParser;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeParser durationParser;
    private final IDateTimeParser dateParser;

    private final Pattern simpleCasesRegex;
    private final Pattern yearAndMonth;
    private final Pattern oneWordPeriodRegex;
    private final Pattern yearRegex;
    private final Pattern pastRegex;
    private final Pattern futureRegex;
    private final Pattern numberCombinedWithUnit;
    private final Pattern weekOfMonthRegex;
    private final Pattern weekOfYearRegex;
    private final Pattern quarterRegex;
    private final Pattern seasonRegex;
    private final Pattern decadeRegex;
    private final Pattern tillRegex;
    private final Pattern nowRegex;

    private final Pattern thisPrefixRegex;
    private final Pattern lastPrefixRegex;
    private final Pattern nextPrefixRegex;
    private final Pattern relativeRegex;

    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Integer> cardinalMap;
    private final ImmutableMap<String, Integer> dayOfMonth;
    private final ImmutableMap<String, Integer> monthOfYear;
    private final ImmutableMap<String, String> seasonMap;

    public ChineseDatePeriodParserConfiguration(DateTimeOptions options) {
        super(options);

        tokenBeforeDate = ChineseDateTime.ParserConfigurationDatePrefix;

        cardinalExtractor = new CardinalExtractor();
        ordinalExtractor = new OrdinalExtractor();
        integerExtractor = new IntegerExtractor();
        numberParser = new BaseNumberParser(new ChineseNumberParserConfiguration());
        dateExtractor = new BaseDateExtractor(new ChineseDateExtractorConfiguration(this));
        durationExtractor = new BaseDurationExtractor(new ChineseDurationExtractorConfiguration(options));
        durationParser = null;
        dateParser = null;

        simpleCasesRegex = ChineseDatePeriodExtractorConfiguration.SimpleCasesRegex;
        yearAndMonth = ChineseDatePeriodExtractorConfiguration.YearAndMonth;
        oneWordPeriodRegex = ChineseDatePeriodExtractorConfiguration.OneWordPeriodRegex;
        yearRegex = ChineseDatePeriodExtractorConfiguration.YearRegex;
        pastRegex = ChineseDatePeriodExtractorConfiguration.PastRegex;
        futureRegex = ChineseDatePeriodExtractorConfiguration.FutureRegex;
        numberCombinedWithUnit = ChineseDatePeriodExtractorConfiguration.NumberCombinedWithUnit;
        weekOfMonthRegex = ChineseDatePeriodExtractorConfiguration.WeekOfMonthRegex;
        weekOfYearRegex = ChineseDatePeriodExtractorConfiguration.WeekOfYearRegex;
        quarterRegex = ChineseDatePeriodExtractorConfiguration.QuarterRegex;
        seasonRegex = ChineseDatePeriodExtractorConfiguration.SeasonWithYear;
        decadeRegex = ChineseDatePeriodExtractorConfiguration.DecadeRegex;
        tillRegex = ChineseDatePeriodExtractorConfiguration.TillRegex;
        nowRegex = ChineseDatePeriodExtractorConfiguration.NowRegex;

        thisPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ThisPrefixRegex);
        lastPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.LastPrefixRegex);
        nextPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NextPrefixRegex);
        relativeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.RelativeRegex);

        unitMap = ChineseDateTime.ParserConfigurationUnitMap;
        cardinalMap = ChineseDateTime.ParserConfigurationCardinalMap;
        dayOfMonth = ChineseDateTime.ParserConfigurationDayOfMonth;
        monthOfYear = ChineseDateTime.ParserConfigurationMonthOfYear;
        seasonMap = ChineseDateTime.ParserConfigurationSeasonMap;
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
        return null;
    }

    @Override
    public Pattern getBetweenRegex() {
        return null;
    }

    @Override
    public Pattern getMonthFrontSimpleCasesRegex() {
        return null;
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
        return yearAndMonth;
    }

    @Override
    public Pattern getMonthNumWithYear() {
        return null;
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
        return null;
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
        return null;
    }

    @Override
    public Pattern getAllHalfYearRegex() {
        return null;
    }

    @Override
    public Pattern getSeasonRegex() {
        return seasonRegex;
    }

    @Override
    public Pattern getWhichWeekRegex() {
        return null;
    }

    @Override
    public Pattern getWeekOfRegex() {
        return null;
    }

    @Override
    public Pattern getMonthOfRegex() {
        return null;
    }

    @Override
    public Pattern getInConnectorRegex() {
        return null;
    }

    @Override
    public Pattern getWithinNextPrefixRegex() {
        return null;
    }

    @Override
    public Pattern getNextPrefixRegex() {
        return nextPrefixRegex;
    }

    @Override
    public Pattern getPastPrefixRegex() {
        return lastPrefixRegex;
    }

    @Override
    public Pattern getThisPrefixRegex() {
        return thisPrefixRegex;
    }

    @Override
    public Pattern getRestOfDateRegex() {
        return null;
    }

    @Override
    public Pattern getLaterEarlyPeriodRegex() {
        return null;
    }

    @Override
    public Pattern getWeekWithWeekDayRangeRegex() {
        return null;
    }

    @Override
    public Pattern getYearPlusNumberRegex() {
        return null;
    }

    @Override
    public Pattern getDecadeWithCenturyRegex() {
        return decadeRegex;
    }

    @Override
    public Pattern getYearPeriodRegex() {
        return null;
    }

    @Override
    public Pattern getComplexDatePeriodRegex() {
        return null;
    }

    @Override
    public Pattern getRelativeDecadeRegex() {
        return null;
    }

    @Override
    public Pattern getReferenceDatePeriodRegex() {
        return null;
    }

    @Override
    public Pattern getAgoRegex() {
        return null;
    }

    @Override
    public Pattern getLaterRegex() {
        return null;
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
    public Pattern getCenturySuffixRegex() {
        return null;
    }

    @Override
    public Pattern getRelativeRegex() {
        return relativeRegex;
    }

    @Override
    public Pattern getUnspecificEndOfRangeRegex() {
        return null;
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
        return null;
    }

    @Override
    public ImmutableMap<String, Integer> getWrittenDecades() {
        return null;
    }

    @Override
    public ImmutableMap<String, Integer> getNumbers() {
        return null;
    }

    @Override
    public ImmutableMap<String, Integer> getSpecialDecadeCases() {
        return null;
    }

    @Override
    public int getSwiftDayOrMonth(String text) {
        String trimmedText = text.trim().toLowerCase();
        int swift = 0;

        Optional<Match> matchNext = Arrays.stream(RegExpUtility.getMatches(nextPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchLast = Arrays.stream(RegExpUtility.getMatches(lastPrefixRegex, trimmedText)).findFirst();

        if (matchNext.isPresent()) {
            swift = 1;
        } else if (matchLast.isPresent()) {
            swift = -1;
        }

        return swift;
    }

    @Override
    public int getSwiftYear(String text) {
        String trimmedText = text.trim().toLowerCase();
        int swift = -10;

        Optional<Match> matchNext = Arrays.stream(RegExpUtility.getMatches(nextPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchLast = Arrays.stream(RegExpUtility.getMatches(lastPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchThis = Arrays.stream(RegExpUtility.getMatches(thisPrefixRegex, trimmedText)).findFirst();

        if (matchNext.isPresent()) {
            swift = 1;
        } else if (matchLast.isPresent()) {
            swift = -1;
        } else if (matchThis.isPresent()) {
            swift = 0;
        }

        return swift;
    }

    @Override
    public boolean isFuture(String text) {
        String trimmedText = text.trim().toLowerCase();
        return ChineseDateTime.ThisYearTerms.stream().anyMatch(trimmedText::contains) ||
               ChineseDateTime.NextYearTerms.stream().anyMatch(trimmedText::contains);
    }

    @Override
    public boolean isLastCardinal(String text) {
        String trimmedText = text.trim().toLowerCase();
        return trimmedText.contains("最后");
    }

    @Override
    public boolean isMonthOnly(String text) {
        String trimmedText = text.trim().toLowerCase();
        return ChineseDateTime.MonthTerms.stream().anyMatch(trimmedText::endsWith);
    }

    @Override
    public boolean isMonthToDate(String text) {
        return false;
    }

    @Override
    public boolean isWeekend(String text) {
        String trimmedText = text.trim().toLowerCase();
        return ChineseDateTime.WeekendTerms.stream().anyMatch(trimmedText::contains);
    }

    @Override
    public boolean isWeekOnly(String text) {
        String trimmedText = text.trim().toLowerCase();
        return ChineseDateTime.WeekTerms.stream().anyMatch(trimmedText::endsWith);
    }

    @Override
    public boolean isYearOnly(String text) {
        String trimmedText = text.trim().toLowerCase();
        return ChineseDateTime.YearTerms.stream().anyMatch(trimmedText::endsWith);
    }

    @Override
    public boolean isYearToDate(String text) {
        String trimmedText = text.trim().toLowerCase();
        return ChineseDateTime.YearToDateTerms.stream().anyMatch(trimmedText::contains);
    }
}
