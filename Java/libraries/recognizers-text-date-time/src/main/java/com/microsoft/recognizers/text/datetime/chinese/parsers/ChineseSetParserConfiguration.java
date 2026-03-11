package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseSetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ISetParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.MatchedTimexResult;

import java.util.regex.Pattern;

public class ChineseSetParserConfiguration extends BaseOptionsConfiguration implements ISetParserConfiguration {

    private IDateTimeParser timeParser;

    public final IDateTimeParser getTimeParser() {
        return timeParser;
    }

    private IDateTimeParser dateParser;

    public final IDateTimeParser getDateParser() {
        return dateParser;
    }

    private ImmutableMap<String, String> unitMap;

    public final ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    private IDateTimeParser dateTimeParser;

    public final IDateTimeParser getDateTimeParser() {
        return dateTimeParser;
    }

    private IDateTimeParser durationParser;

    public final IDateTimeParser getDurationParser() {
        return durationParser;
    }

    private IDateTimeExtractor timeExtractor;

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    private IDateExtractor dateExtractor;

    public final IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    private IDateTimeParser datePeriodParser;

    public final IDateTimeParser getDatePeriodParser() {
        return datePeriodParser;
    }

    private IDateTimeParser timePeriodParser;

    public final IDateTimeParser getTimePeriodParser() {
        return timePeriodParser;
    }

    private IDateTimeExtractor durationExtractor;

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    private IDateTimeExtractor dateTimeExtractor;

    public final IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
    }

    private IDateTimeParser dateTimePeriodParser;

    public final IDateTimeParser getDateTimePeriodParser() {
        return dateTimePeriodParser;
    }

    private IDateTimeExtractor datePeriodExtractor;

    public final IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    private IDateTimeExtractor timePeriodExtractor;

    public final IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    private IDateTimeExtractor dateTimePeriodExtractor;

    public final IDateTimeExtractor getDateTimePeriodExtractor() {
        return dateTimePeriodExtractor;
    }

    private Pattern eachDayRegex;

    public final Pattern getEachDayRegex() {
        return eachDayRegex;
    }

    private Pattern setEachRegex;

    public final Pattern getSetEachRegex() {
        return setEachRegex;
    }

    private Pattern periodicRegex;

    public final Pattern getPeriodicRegex() {
        return periodicRegex;
    }

    private Pattern eachUnitRegex;

    public final Pattern getEachUnitRegex() {
        return eachUnitRegex;
    }

    private Pattern setWeekDayRegex;

    public final Pattern getSetWeekDayRegex() {
        return setWeekDayRegex;
    }

    private Pattern eachPrefixRegex;

    public final Pattern getEachPrefixRegex() {
        return eachPrefixRegex;
    }

    public ChineseSetParserConfiguration(ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        timeExtractor = config.getTimeExtractor();
        dateExtractor = config.getDateExtractor();
        dateTimeExtractor = config.getDateTimeExtractor();
        durationExtractor = config.getDurationExtractor();
        datePeriodExtractor = config.getDatePeriodExtractor();
        timePeriodExtractor = config.getTimePeriodExtractor();
        dateTimePeriodExtractor = config.getDateTimePeriodExtractor();

        unitMap = config.getUnitMap();
        timeParser = config.getTimeParser();
        dateParser = config.getDateParser();
        dateTimeParser = config.getDateTimeParser();
        durationParser = config.getDurationParser();
        datePeriodParser = config.getDatePeriodParser();
        timePeriodParser = config.getTimePeriodParser();
        dateTimePeriodParser = config.getDateTimePeriodParser();

        eachDayRegex = ChineseSetExtractorConfiguration.SetEachDayRegex;
        setEachRegex = ChineseSetExtractorConfiguration.SetEachUnitRegex;
        eachUnitRegex = ChineseSetExtractorConfiguration.SetEachUnitRegex;
        periodicRegex = null;
        eachPrefixRegex = ChineseSetExtractorConfiguration.SetEachPrefixRegex;
        setWeekDayRegex = null;
    }

    public MatchedTimexResult getMatchedDailyTimex(String text) {

        MatchedTimexResult result = new MatchedTimexResult();

        String trimmedText = text.trim();

        if (trimmedText.equals("每天") || trimmedText.equals("每日")) {
            result.setTimex("P1D");
        } else if (trimmedText.equals("每周") || trimmedText.equals("每星期")) {
            result.setTimex("P1W");
        } else if (trimmedText.equals("每月")) {
            result.setTimex("P1M");
        } else if (trimmedText.equals("每年")) {
            result.setTimex("P1Y");
        }

        if (result.getTimex() != "") {
            result.setResult(true);
        }

        return result;
    }

    public MatchedTimexResult getMatchedUnitTimex(String text) {

        MatchedTimexResult result = new MatchedTimexResult();
        String trimmedText = text.trim();

        if (trimmedText.equals("天") || trimmedText.equals("日")) {
            result.setTimex("P1D");
        } else if (trimmedText.equals("周") || trimmedText.equals("星期")) {
            result.setTimex("P1W");
        } else if (trimmedText.equals("月")) {
            result.setTimex("P1M");
        } else if (trimmedText.equals("年")) {
            result.setTimex("P1Y");
        } else if (trimmedText.equals("小时") || trimmedText.equals("时")) {
            result.setTimex("PT1H");
        } else if (trimmedText.equals("分钟") || trimmedText.equals("分")) {
            result.setTimex("PT1M");
        } else if (trimmedText.equals("秒钟") || trimmedText.equals("秒")) {
            result.setTimex("PT1S");
        }

        if (result.getTimex() != "") {
            result.setResult(true);
        }

        return result;
    }
}
