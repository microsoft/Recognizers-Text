package com.microsoft.recognizers.text.datetime.french.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchSetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ISetParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.MatchedTimexResult;
import java.util.regex.Pattern;

public class FrenchSetParserConfiguration extends BaseOptionsConfiguration implements ISetParserConfiguration {

    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeParser durationParser;
    private final IDateTimeExtractor timeExtractor;
    private final IDateTimeParser timeParser;
    private final IDateExtractor dateExtractor;
    private final IDateTimeParser dateParser;
    private final IDateTimeExtractor dateTimeExtractor;
    private final IDateTimeParser dateTimeParser;
    private final IDateTimeExtractor datePeriodExtractor;
    private final IDateTimeParser datePeriodParser;
    private final IDateTimeExtractor timePeriodExtractor;
    private final IDateTimeParser timePeriodParser;
    private final IDateTimeExtractor dateTimePeriodExtractor;
    private final IDateTimeParser dateTimePeriodParser;
    private final ImmutableMap<String, String> unitMap;
    private final Pattern eachPrefixRegex;
    private final Pattern periodicRegex;
    private final Pattern eachUnitRegex;
    private final Pattern eachDayRegex;
    private final Pattern setWeekDayRegex;
    private final Pattern setEachRegex;

    public FrenchSetParserConfiguration(final ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        durationExtractor = config.getDurationExtractor();
        timeExtractor = config.getTimeExtractor();
        dateExtractor = config.getDateExtractor();
        dateTimeExtractor = config.getDateTimeExtractor();
        datePeriodExtractor = config.getDatePeriodExtractor();
        timePeriodExtractor = config.getTimePeriodExtractor();
        dateTimePeriodExtractor = config.getDateTimePeriodExtractor();

        durationParser = config.getDurationParser();
        timeParser = config.getTimeParser();
        dateParser = config.getDateParser();
        dateTimeParser = config.getDateTimeParser();
        datePeriodParser = config.getDatePeriodParser();
        timePeriodParser = config.getTimePeriodParser();
        dateTimePeriodParser = config.getDateTimePeriodParser();
        unitMap = config.getUnitMap();

        eachPrefixRegex = FrenchSetExtractorConfiguration.EachPrefixRegex;
        periodicRegex = FrenchSetExtractorConfiguration.PeriodicRegex;
        eachUnitRegex = FrenchSetExtractorConfiguration.EachUnitRegex;
        eachDayRegex = FrenchSetExtractorConfiguration.EachDayRegex;
        setWeekDayRegex = FrenchSetExtractorConfiguration.SetWeekDayRegex;
        setEachRegex = FrenchSetExtractorConfiguration.SetEachRegex;
    }

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    public final IDateTimeParser getDurationParser() {
        return durationParser;
    }

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    public final IDateTimeParser getTimeParser() {
        return timeParser;
    }

    public final IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    public final IDateTimeParser getDateParser() {
        return dateParser;
    }

    public final IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
    }

    public final IDateTimeParser getDateTimeParser() {
        return dateTimeParser;
    }

    public final IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    public final IDateTimeParser getDatePeriodParser() {
        return datePeriodParser;
    }

    public final IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    public final IDateTimeParser getTimePeriodParser() {
        return timePeriodParser;
    }

    public final IDateTimeExtractor getDateTimePeriodExtractor() {
        return dateTimePeriodExtractor;
    }

    public final IDateTimeParser getDateTimePeriodParser() {
        return dateTimePeriodParser;
    }

    public final ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    public final Pattern getEachPrefixRegex() {
        return eachPrefixRegex;
    }

    public final Pattern getPeriodicRegex() {
        return periodicRegex;
    }

    public final Pattern getEachUnitRegex() {
        return eachUnitRegex;
    }

    public final Pattern getEachDayRegex() {
        return eachDayRegex;
    }

    public final Pattern getSetWeekDayRegex() {
        return setWeekDayRegex;
    }

    public final Pattern getSetEachRegex() {
        return setEachRegex;
    }

    public MatchedTimexResult getMatchedDailyTimex(final String text) {
        final String trimmedText = text.trim();
        final String timex;
        if (trimmedText.equals("quotidien") || trimmedText.equals("quotidienne") ||
            trimmedText.equals("jours") || trimmedText.equals("journellement")) {
            // daily
            timex = "P1D";
        } else if (trimmedText.equals("hebdomadaire")) {
            // weekly
            timex = "P1W";
        } else if (trimmedText.equals("bihebdomadaire")) {
            // bi weekly
            timex = "P2W";
        } else if (trimmedText.equals("mensuel") || trimmedText.equals("mensuelle")) {
            // monthly
            timex = "P1M";
        } else if (trimmedText.equals("annuel") || trimmedText.equals("annuellement")) {
            // yearly/annually
            timex = "P1Y";
        } else {
            return new MatchedTimexResult(false, null);
        }

        return new MatchedTimexResult(true, timex);
    }

    public MatchedTimexResult getMatchedUnitTimex(final String text) {
        final String trimmedText = text.trim();
        final String timex;
        if (trimmedText.equals("jour") || trimmedText.equals("journee")) {
            timex = "P1D";
        } else if (trimmedText.equals("semaine")) {
            timex = "P1W";
        } else if (trimmedText.equals("mois")) {
            timex = "P1M";
        } else if (trimmedText.equals("an") || trimmedText.equals("annee")) {
            // year
            timex = "P1Y";
        } else {
            return new MatchedTimexResult(false, null);
        }

        return new MatchedTimexResult(true, timex);
    }
}
