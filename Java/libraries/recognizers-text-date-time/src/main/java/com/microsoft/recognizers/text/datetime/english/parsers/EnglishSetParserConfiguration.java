package com.microsoft.recognizers.text.datetime.english.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishSetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ISetParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.MatchedTimexResult;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Locale;
import java.util.regex.Pattern;

public class EnglishSetParserConfiguration extends BaseOptionsConfiguration implements ISetParserConfiguration {

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

    private static Pattern doubleMultiplierRegex =
            RegExpUtility.getSafeRegExp(EnglishDateTime.DoubleMultiplierRegex);

    private static Pattern halfMultiplierRegex =
            RegExpUtility.getSafeRegExp(EnglishDateTime.HalfMultiplierRegex);

    private static Pattern dayTypeRegex =
            RegExpUtility.getSafeRegExp(EnglishDateTime.DayTypeRegex);

    private static Pattern weekTypeRegex =
            RegExpUtility.getSafeRegExp(EnglishDateTime.WeekTypeRegex);

    private static Pattern weekendTypeRegex =
            RegExpUtility.getSafeRegExp(EnglishDateTime.WeekendTypeRegex);

    private static Pattern monthTypeRegex =
            RegExpUtility.getSafeRegExp(EnglishDateTime.MonthTypeRegex);

    private static Pattern quarterTypeRegex =
            RegExpUtility.getSafeRegExp(EnglishDateTime.QuarterTypeRegex);

    private static Pattern yearTypeRegex =
            RegExpUtility.getSafeRegExp(EnglishDateTime.YearTypeRegex);

    public EnglishSetParserConfiguration(ICommonDateTimeParserConfiguration config) {

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

        eachDayRegex = EnglishSetExtractorConfiguration.EachDayRegex;
        setEachRegex = EnglishSetExtractorConfiguration.SetEachRegex;
        eachUnitRegex = EnglishSetExtractorConfiguration.EachUnitRegex;
        periodicRegex = EnglishSetExtractorConfiguration.PeriodicRegex;
        eachPrefixRegex = EnglishSetExtractorConfiguration.EachPrefixRegex;
        setWeekDayRegex = EnglishSetExtractorConfiguration.SetWeekDayRegex;
    }

    public MatchedTimexResult getMatchedDailyTimex(String text) {

        MatchedTimexResult result = new MatchedTimexResult();

        String trimmedText = text.trim().toLowerCase(Locale.ROOT);

        float durationLength = 1; // Default value
        float multiplier = 1;
        String durationType;

        if (trimmedText.equals("daily")) {
            result.setTimex("P1D");
        } else if (trimmedText.equals("weekly")) {
            result.setTimex("P1W");
        } else if (trimmedText.equals("biweekly")) {
            result.setTimex("P2W");
        } else if (trimmedText.equals("monthly")) {
            result.setTimex("P1M");
        } else if (trimmedText.equals("quarterly")) {
            result.setTimex("P3M");
        } else if (trimmedText.equals("yearly") || trimmedText.equals("annually") || trimmedText.equals("annual")) {
            result.setTimex("P1Y");
        }

        if (result.getTimex() != "") {
            result.setResult(true);
        }

        return result;
    }

    public MatchedTimexResult getMatchedUnitTimex(String text) {

        MatchedTimexResult result = new MatchedTimexResult();
        String trimmedText = text.trim().toLowerCase(Locale.ROOT);

        if (trimmedText.equals("day")) {
            result.setTimex("P1D");
        } else if (trimmedText.equals("week")) {
            result.setTimex("P1W");
        } else if (trimmedText.equals("month")) {
            result.setTimex("P1M");
        } else if (trimmedText.equals("year")) {
            result.setTimex("P1Y");
        }

        if (result.getTimex() != "") {
            result.setResult(true);
        }

        return result;
    }
}
