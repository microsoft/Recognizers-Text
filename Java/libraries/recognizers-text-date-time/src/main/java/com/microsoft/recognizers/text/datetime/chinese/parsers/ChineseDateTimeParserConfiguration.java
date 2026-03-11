package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultTimex;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class ChineseDateTimeParserConfiguration extends BaseOptionsConfiguration implements IDateTimeParserConfiguration {

    private final String tokenBeforeDate;
    private final String tokenBeforeTime;

    private final IDateTimeExtractor dateExtractor;
    private final IDateTimeExtractor timeExtractor;
    private final IDateTimeParser dateParser;
    private final IDateTimeParser timeParser;
    private final IExtractor cardinalExtractor;
    private final IExtractor integerExtractor;
    private final IParser numberParser;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeParser durationParser;

    private final Pattern nowRegex;
    private final Pattern amTimeRegex;
    private final Pattern pmTimeRegex;
    private final Pattern simpleTimeOfTodayAfterRegex;
    private final Pattern simpleTimeOfTodayBeforeRegex;
    private final Pattern specificTimeOfDayRegex;
    private final Pattern specificEndOfRegex;
    private final Pattern unspecificEndOfRegex;
    private final Pattern unitRegex;
    private final Pattern dateNumberConnectorRegex;

    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Integer> numbers;
    private final IDateTimeUtilityConfiguration utilityConfiguration;

    public ChineseDateTimeParserConfiguration(ICommonDateTimeParserConfiguration config) {
        super(config.getOptions());

        tokenBeforeDate = ChineseDateTime.ParserConfigurationDatePrefix;
        tokenBeforeTime = ChineseDateTime.ParserConfigurationDatePrefix;

        cardinalExtractor = config.getCardinalExtractor();
        integerExtractor = config.getIntegerExtractor();
        numberParser = config.getNumberParser();
        dateExtractor = config.getDateExtractor();
        timeExtractor = config.getTimeExtractor();
        durationExtractor = config.getDurationExtractor();
        dateParser = config.getDateParser();
        timeParser = config.getTimeParser();
        durationParser = config.getDurationParser();

        nowRegex = ChineseDateTimeExtractorConfiguration.NowRegex;
        amTimeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodMORegex);
        pmTimeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodAFRegex);
        simpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfSpecialDayRegex);
        simpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfSpecialDayRegex);
        specificTimeOfDayRegex = ChineseDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
        specificEndOfRegex = null;
        unspecificEndOfRegex = null;
        unitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodUnitRegex);
        dateNumberConnectorRegex = null;

        unitMap = config.getUnitMap();
        numbers = config.getNumbers();
        utilityConfiguration = config.getUtilityConfiguration();
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
    public Pattern getNowRegex() {
        return nowRegex;
    }

    @Override
    public Pattern getAMTimeRegex() {
        return amTimeRegex;
    }

    @Override
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
    public ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    @Override
    public ImmutableMap<String, Integer> getNumbers() {
        return numbers;
    }

    @Override
    public IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }

    @Override
    public boolean containsAmbiguousToken(String text, String matchedText) {
        return false;
    }

    @Override
    public ResultTimex getMatchedNowTimex(String text) {
        String trimmedText = text.trim();

        if (trimmedText.equals("现在") || trimmedText.equals("此刻") || trimmedText.equals("当下")) {
            return new ResultTimex(true, "PRESENT_REF");
        } else if (trimmedText.equals("刚刚") || trimmedText.equals("刚才")) {
            return new ResultTimex(true, "PAST_REF");
        } else if (trimmedText.equals("马上") || trimmedText.equals("立刻")) {
            return new ResultTimex(true, "FUTURE_REF");
        }

        return new ResultTimex(false, null);
    }

    @Override
    public int getSwiftDay(String text) {
        String trimmedText = text.trim();

        int swift = 0;
        if (trimmedText.startsWith("明") || trimmedText.startsWith("下")) {
            swift = 1;
        } else if (trimmedText.startsWith("昨") || trimmedText.startsWith("上")) {
            swift = -1;
        }

        return swift;
    }

    @Override
    public int getHour(String text, int hour) {
        String trimmedText = text.trim();
        int result = hour;

        if ((trimmedText.contains("早") || trimmedText.contains("上午") || trimmedText.contains("凌晨") || 
             trimmedText.contains("清晨")) && hour >= Constants.HalfDayHourCount) {
            result -= Constants.HalfDayHourCount;
        } else if ((trimmedText.contains("下午") || trimmedText.contains("午后") || trimmedText.contains("晚") || 
                    trimmedText.contains("傍晚") || trimmedText.contains("夜")) && hour < Constants.HalfDayHourCount) {
            result += Constants.HalfDayHourCount;
        }

        return result;
    }
}
