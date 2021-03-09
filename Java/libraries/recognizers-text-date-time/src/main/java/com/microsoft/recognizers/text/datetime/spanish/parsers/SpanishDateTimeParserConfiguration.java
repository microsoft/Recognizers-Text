package com.microsoft.recognizers.text.datetime.spanish.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultTimex;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Locale;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class SpanishDateTimeParserConfiguration extends BaseOptionsConfiguration implements IDateTimeParserConfiguration {

    public final String tokenBeforeDate;
    public final String tokenBeforeTime;

    public final IDateTimeExtractor dateExtractor;
    public final IDateTimeExtractor timeExtractor;
    public final IDateTimeParser dateParser;
    public final IDateTimeParser timeParser;
    public final IExtractor cardinalExtractor;
    public final IExtractor integerExtractor;
    public final IParser numberParser;
    public final IDateTimeExtractor durationExtractor;
    public final IDateTimeParser durationParser;

    public final ImmutableMap<String, String> unitMap;
    public final ImmutableMap<String, Integer> numbers;

    public final Pattern nowRegex;
    public final Pattern amTimeRegex;
    public final Pattern pmTimeRegex;
    public final Pattern lastNightTimeRegex;
    public final Pattern simpleTimeOfTodayAfterRegex;
    public final Pattern simpleTimeOfTodayBeforeRegex;
    public final Pattern specificTimeOfDayRegex;
    public final Pattern specificEndOfRegex;
    public final Pattern unspecificEndOfRegex;
    public final Pattern unitRegex;
    public final Pattern dateNumberConnectorRegex;
    
    public final IDateTimeUtilityConfiguration utilityConfiguration;

    public SpanishDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config) {
        super(config.getOptions());

        unitMap = config.getUnitMap();
        numbers = config.getNumbers();
        dateParser = config.getDateParser();
        timeParser = config.getTimeParser();
        numberParser = config.getNumberParser();
        dateExtractor = config.getDateExtractor();
        timeExtractor = config.getTimeExtractor();
        durationParser = config.getDurationParser();
        integerExtractor = config.getIntegerExtractor();
        cardinalExtractor = config.getCardinalExtractor();
        durationExtractor = config.getDurationExtractor();
        utilityConfiguration = config.getUtilityConfiguration();

        tokenBeforeDate = SpanishDateTime.TokenBeforeDate;
        tokenBeforeTime = SpanishDateTime.TokenBeforeTime;

        nowRegex = SpanishDateTimeExtractorConfiguration.NowRegex;
        unitRegex = SpanishDateTimeExtractorConfiguration.UnitRegex;
        specificEndOfRegex = SpanishDateTimeExtractorConfiguration.SpecificEndOfRegex;
        unspecificEndOfRegex = SpanishDateTimeExtractorConfiguration.UnspecificEndOfRegex;
        specificTimeOfDayRegex = SpanishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
        dateNumberConnectorRegex = SpanishDateTimeExtractorConfiguration.DateNumberConnectorRegex;
        simpleTimeOfTodayAfterRegex = SpanishDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
        simpleTimeOfTodayBeforeRegex = SpanishDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;

        pmTimeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PmRegex);
        amTimeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AmTimeRegex);
        lastNightTimeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LastNightTimeRegex);
    }

    @Override
    public int getHour(String text, int hour) {

        String trimmedText = text.trim().toLowerCase();
        int result = hour;

        //TODO: Replace with a regex
        if ((trimmedText.endsWith("mañana") || trimmedText.endsWith("madrugada")) && hour >= Constants.HalfDayHourCount) {
            result -= Constants.HalfDayHourCount;
        } else if (!(trimmedText.endsWith("mañana") || trimmedText.endsWith("madrugada")) && hour < Constants.HalfDayHourCount) {
            result += Constants.HalfDayHourCount;
        }

        return result;
    }

    @Override
    public ResultTimex getMatchedNowTimex(String text) {

        String trimmedText = text.trim().toLowerCase();

        if (trimmedText.endsWith("ahora") || trimmedText.endsWith("mismo") || trimmedText.endsWith("momento")) {
            return new ResultTimex(true, "PRESENT_REF");
        } else if (trimmedText.endsWith("posible") || trimmedText.endsWith("pueda") || trimmedText.endsWith("puedas") ||
                trimmedText.endsWith("podamos") || trimmedText.endsWith("puedan")) {
            return new ResultTimex(true, "FUTURE_REF");
        } else if (trimmedText.endsWith("mente")) {
            return new ResultTimex(true, "PAST_REF");
        }

        return new ResultTimex(false, null);
    }

    @Override
    public int getSwiftDay(String text) {

        String trimmedText = text.trim().toLowerCase(Locale.ROOT);
        Matcher regexMatcher = SpanishDatePeriodParserConfiguration.previousPrefixRegex.matcher(trimmedText);

        int swift = 0;

        if (regexMatcher.find()) {
            swift = -1;
        } else {
            regexMatcher = this.lastNightTimeRegex.matcher(trimmedText);
            if (regexMatcher.find()) {
                swift = -1;
            } else {
                regexMatcher = SpanishDatePeriodParserConfiguration.nextPrefixRegex.matcher(trimmedText);
                if (regexMatcher.find()) {
                    swift = 1;
                }
            }
        }

        return swift;
    }

    @Override
    public boolean containsAmbiguousToken(String text, String matchedText) {
        return text.contains("esta mañana") && matchedText.contains("mañana");
    }

    @Override public String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    @Override public String getTokenBeforeTime() {
        return tokenBeforeTime;
    }

    @Override public IDateTimeExtractor getDateExtractor() {
        return dateExtractor;
    }

    @Override public IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    @Override public IDateTimeParser getDateParser() {
        return dateParser;
    }

    @Override public IDateTimeParser getTimeParser() {
        return timeParser;
    }

    @Override public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override public IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    @Override public IParser getNumberParser() {
        return numberParser;
    }

    @Override public IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    @Override public IDateTimeParser getDurationParser() {
        return durationParser;
    }

    @Override public ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    @Override public ImmutableMap<String, Integer> getNumbers() {
        return numbers;
    }

    @Override public Pattern getNowRegex() {
        return nowRegex;
    }

    public Pattern getAMTimeRegex() {
        return amTimeRegex;
    }

    public Pattern getPMTimeRegex() {
        return pmTimeRegex;
    }

    @Override public Pattern getSimpleTimeOfTodayAfterRegex() {
        return simpleTimeOfTodayAfterRegex;
    }

    @Override public Pattern getSimpleTimeOfTodayBeforeRegex() {
        return simpleTimeOfTodayBeforeRegex;
    }

    @Override public Pattern getSpecificTimeOfDayRegex() {
        return specificTimeOfDayRegex;
    }

    @Override public Pattern getSpecificEndOfRegex() {
        return specificEndOfRegex;
    }

    @Override public Pattern getUnspecificEndOfRegex() {
        return unspecificEndOfRegex;
    }

    @Override public Pattern getUnitRegex() {
        return unitRegex;
    }

    @Override public Pattern getDateNumberConnectorRegex() {
        return dateNumberConnectorRegex;
    }

    @Override public IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }
}
