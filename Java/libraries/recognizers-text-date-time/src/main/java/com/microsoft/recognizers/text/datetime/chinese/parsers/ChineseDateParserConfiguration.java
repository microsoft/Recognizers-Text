package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.Optional;
import java.util.regex.Pattern;

public class ChineseDateParserConfiguration extends BaseOptionsConfiguration implements IDateParserConfiguration {

    private final String dateTokenPrefix;
    private final IExtractor integerExtractor;
    private final IExtractor ordinalExtractor;
    private final IExtractor cardinalExtractor;
    private final IParser numberParser;
    private final IDateTimeExtractor durationExtractor;
    private final IDateExtractor dateExtractor;
    private final IDateTimeParser durationParser;
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

    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Integer> dayOfMonth;
    private final ImmutableMap<String, Integer> dayOfWeek;
    private final ImmutableMap<String, Integer> monthOfYear;
    private final ImmutableMap<String, Integer> cardinalMap;
    private final IDateTimeUtilityConfiguration utilityConfiguration;

    private final List<String> sameDayTerms;
    private final List<String> plusOneDayTerms;
    private final List<String> plusTwoDayTerms;
    private final List<String> minusOneDayTerms;
    private final List<String> minusTwoDayTerms;

    private final Pattern relativeDayRegex;
    private final Pattern nextPrefixRegex;
    private final Pattern previousPrefixRegex;

    public ChineseDateParserConfiguration(ICommonDateTimeParserConfiguration config) {
        super(config.getOptions());

        dateTokenPrefix = ChineseDateTime.ParserConfigurationDatePrefix;

        integerExtractor = config.getIntegerExtractor();
        ordinalExtractor = config.getOrdinalExtractor();
        cardinalExtractor = config.getCardinalExtractor();
        numberParser = config.getNumberParser();

        durationExtractor = config.getDurationExtractor();
        dateExtractor = config.getDateExtractor();
        durationParser = config.getDurationParser();

        dateRegexes = Collections.unmodifiableList(ChineseDateExtractorConfiguration.DateRegexList);
        onRegex = null;
        specialDayRegex = ChineseDateExtractorConfiguration.SpecialDayRegex;
        specialDayWithNumRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDayWithNumRegex);
        nextRegex = ChineseDateExtractorConfiguration.DateNextRegex;
        thisRegex = ChineseDateExtractorConfiguration.DateThisRegex;
        lastRegex = ChineseDateExtractorConfiguration.DateLastRegex;
        unitRegex = ChineseDateExtractorConfiguration.DateUnitRegex;
        weekDayRegex = ChineseDateExtractorConfiguration.WeekDayRegex;
        monthRegex = ChineseDateExtractorConfiguration.MonthRegex;
        weekDayOfMonthRegex = ChineseDateExtractorConfiguration.WeekDayOfMonthRegex;
        forTheRegex = null;
        weekDayAndDayOfMonthRegex = null;
        relativeMonthRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.RelativeMonthRegex);
        strictRelativeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.RelativeRegex);
        relativeWeekDayRegex = null;

        yearSuffix = ChineseDateExtractorConfiguration.YearRegex;
        unitMap = config.getUnitMap();
        dayOfMonth = config.getDayOfMonth();
        dayOfWeek = config.getDayOfWeek();
        monthOfYear = config.getMonthOfYear();
        cardinalMap = config.getCardinalMap();
        utilityConfiguration = config.getUtilityConfiguration();

        relativeDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDayRegex);
        nextPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NextPrefixRegex);
        previousPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.LastPrefixRegex);

        sameDayTerms = Collections.unmodifiableList(Arrays.asList("今天", "今日"));
        plusOneDayTerms = Collections.unmodifiableList(Arrays.asList("明天", "明日"));
        plusTwoDayTerms = Collections.unmodifiableList(Arrays.asList("后天", "後天"));
        minusOneDayTerms = Collections.unmodifiableList(Arrays.asList("昨天", "昨日"));
        minusTwoDayTerms = Collections.unmodifiableList(Arrays.asList("前天", "大前天"));
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
        String trimmedText = text.trim().toLowerCase();
        Integer swift = 0;

        Optional<Match> matchNext = Arrays.stream(RegExpUtility.getMatches(nextPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchPast = Arrays.stream(RegExpUtility.getMatches(previousPrefixRegex, trimmedText)).findFirst();

        if (matchNext.isPresent()) {
            swift = 1;
        } else if (matchPast.isPresent()) {
            swift = -1;
        }

        return swift;
    }

    @Override
    public Boolean isCardinalLast(String text) {
        String trimmedText = text.trim().toLowerCase();
        return trimmedText.equals("最后一个") || trimmedText.equals("最后");
    }

    @Override
    public String normalize(String text) {
        return text;
    }
}
