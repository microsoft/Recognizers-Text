package com.microsoft.recognizers.text.datetime.french.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultTimex;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.regex.Pattern;

public class FrenchDateTimeParserConfiguration extends BaseOptionsConfiguration implements IDateTimeParserConfiguration {

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
    public final Pattern simpleTimeOfTodayAfterRegex;
    public final Pattern simpleTimeOfTodayBeforeRegex;
    public final Pattern specificTimeOfDayRegex;
    public final Pattern specificEndOfRegex;
    public final Pattern unspecificEndOfRegex;
    public final Pattern unitRegex;
    public final Pattern dateNumberConnectorRegex;

    public final IDateTimeUtilityConfiguration utilityConfiguration;

    public FrenchDateTimeParserConfiguration(final ICommonDateTimeParserConfiguration config) {
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

        tokenBeforeDate = FrenchDateTime.TokenBeforeDate;
        tokenBeforeTime = FrenchDateTime.TokenBeforeTime;

        nowRegex = FrenchDateTimeExtractorConfiguration.NowRegex;
        unitRegex = FrenchDateTimeExtractorConfiguration.UnitRegex;
        specificEndOfRegex = FrenchDateTimeExtractorConfiguration.SpecificEndOfRegex;
        unspecificEndOfRegex = FrenchDateTimeExtractorConfiguration.UnspecificEndOfRegex;
        specificTimeOfDayRegex = FrenchDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
        dateNumberConnectorRegex = FrenchDateTimeExtractorConfiguration.DateNumberConnectorRegex;
        simpleTimeOfTodayAfterRegex = FrenchDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex;
        simpleTimeOfTodayBeforeRegex = FrenchDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex;

        pmTimeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PmRegex);
        amTimeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AMTimeRegex);
    }

    @Override
    public int getHour(final String text, final int hour) {
        int result = hour;

        final String trimmedText = text.trim().toLowerCase();

        if (trimmedText.endsWith("matin") && hour >= Constants.HalfDayHourCount) {
            result -= Constants.HalfDayHourCount;
        } else if (!trimmedText.endsWith("matin") && hour < Constants.HalfDayHourCount) {
            result += Constants.HalfDayHourCount;
        }

        return result;
    }

    @Override
    public ResultTimex getMatchedNowTimex(final String text) {

        final String trimmedText = text.trim().toLowerCase();

        final String timex;
        if (trimmedText.endsWith("maintenant")) {
            timex = "PRESENT_REF";
        } else if (trimmedText.equals("récemment") || trimmedText.equals("précédemment") || trimmedText
            .equals("auparavant")) {
            timex = "PAST_REF";
        } else if (trimmedText.equals("dès que possible") || trimmedText.equals("dqp")) {
            timex = "FUTURE_REF";
        } else {
            timex = null;
            return new ResultTimex(false, null);
        }

        return new ResultTimex(true, timex);
    }

    @Override
    public int getSwiftDay(final String text) {
        int swift = 0;

        final String trimmedText = text.trim().toLowerCase();

        if (trimmedText.startsWith("prochain") || trimmedText.startsWith("prochain") ||
            trimmedText.startsWith("prochaine") || trimmedText.startsWith("prochaine")) {
            swift = 1;
        } else if (trimmedText.startsWith("dernier") || trimmedText.startsWith("dernière") ||
            trimmedText.startsWith("dernier") || trimmedText.startsWith("dernière")) {
            swift = -1;
        }

        return swift;

    }

    @Override
    public boolean containsAmbiguousToken(final String text, final String matchedText) {
        return false;
    }

    @Override
    public String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    @Override
    public String getTokenBeforeTime() {
        return tokenBeforeTime;
    }

    @Override
    public IDateTimeExtractor getDateExtractor() {
        return dateExtractor;
    }

    @Override
    public IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    @Override
    public IDateTimeParser getDateParser() {
        return dateParser;
    }

    @Override
    public IDateTimeParser getTimeParser() {
        return timeParser;
    }

    @Override
    public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
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
    public ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    @Override
    public ImmutableMap<String, Integer> getNumbers() {
        return numbers;
    }

    @Override
    public Pattern getNowRegex() {
        return nowRegex;
    }

    public Pattern getAMTimeRegex() {
        return amTimeRegex;
    }

    public Pattern getPMTimeRegex() {
        return pmTimeRegex;
    }

    @Override
    public Pattern getSimpleTimeOfTodayAfterRegex() {
        return simpleTimeOfTodayAfterRegex;
    }

    @Override
    public Pattern getSimpleTimeOfTodayBeforeRegex() {
        return simpleTimeOfTodayBeforeRegex;
    }

    @Override
    public Pattern getSpecificTimeOfDayRegex() {
        return specificTimeOfDayRegex;
    }

    @Override
    public Pattern getSpecificEndOfRegex() {
        return specificEndOfRegex;
    }

    @Override
    public Pattern getUnspecificEndOfRegex() {
        return unspecificEndOfRegex;
    }

    @Override
    public Pattern getUnitRegex() {
        return unitRegex;
    }

    @Override
    public Pattern getDateNumberConnectorRegex() {
        return dateNumberConnectorRegex;
    }

    @Override
    public IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }
}
