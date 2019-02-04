package com.microsoft.recognizers.text.datetime.spanish.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ISetParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishSetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.MatchedTimexResult;

import java.util.Locale;
import java.util.regex.Pattern;

public class SpanishSetParserConfiguration extends BaseOptionsConfiguration implements ISetParserConfiguration {

    private IDateTimeExtractor durationExtractor;

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    private IDateTimeParser durationParser;

    public final IDateTimeParser getDurationParser() {
        return durationParser;
    }

    private IDateTimeExtractor timeExtractor;

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    private IDateTimeParser timeParser;

    public final IDateTimeParser getTimeParser() {
        return timeParser;
    }

    private IDateExtractor dateExtractor;

    public final IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    private IDateTimeParser dateParser;

    public final IDateTimeParser getDateParser() {
        return dateParser;
    }

    private IDateTimeExtractor dateTimeExtractor;

    public final IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
    }

    private IDateTimeParser dateTimeParser;

    public final IDateTimeParser getDateTimeParser() {
        return dateTimeParser;
    }

    private IDateTimeExtractor datePeriodExtractor;

    public final IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    private IDateTimeParser datePeriodParser;

    public final IDateTimeParser getDatePeriodParser() {
        return datePeriodParser;
    }

    private IDateTimeExtractor timePeriodExtractor;

    public final IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    private IDateTimeParser timePeriodParser;

    public final IDateTimeParser getTimePeriodParser() {
        return timePeriodParser;
    }

    private IDateTimeExtractor dateTimePeriodExtractor;

    public final IDateTimeExtractor getDateTimePeriodExtractor() {
        return dateTimePeriodExtractor;
    }

    private IDateTimeParser dateTimePeriodParser;

    public final IDateTimeParser getDateTimePeriodParser() {
        return dateTimePeriodParser;
    }

    private ImmutableMap<String, String> unitMap;

    public final ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    private Pattern eachPrefixRegex;

    public final Pattern getEachPrefixRegex() {
        return eachPrefixRegex;
    }

    private Pattern periodicRegex;

    public final Pattern getPeriodicRegex() {
        return periodicRegex;
    }

    private Pattern eachUnitRegex;

    public final Pattern getEachUnitRegex() {
        return eachUnitRegex;
    }

    private Pattern eachDayRegex;

    public final Pattern getEachDayRegex() {
        return eachDayRegex;
    }

    private Pattern setWeekDayRegex;

    public final Pattern getSetWeekDayRegex() {
        return setWeekDayRegex;
    }

    private Pattern setEachRegex;

    public final Pattern getSetEachRegex() {
        return setEachRegex;
    }

    public SpanishSetParserConfiguration(ICommonDateTimeParserConfiguration config) {

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

        eachPrefixRegex = SpanishSetExtractorConfiguration.EachPrefixRegex;
        periodicRegex = SpanishSetExtractorConfiguration.PeriodicRegex;
        eachUnitRegex = SpanishSetExtractorConfiguration.EachUnitRegex;
        eachDayRegex = SpanishSetExtractorConfiguration.EachDayRegex;
        setWeekDayRegex = SpanishSetExtractorConfiguration.SetWeekDayRegex;
        setEachRegex = SpanishSetExtractorConfiguration.SetEachRegex;
    }

    public MatchedTimexResult getMatchedDailyTimex(String text) {

        MatchedTimexResult result = new MatchedTimexResult();
        String trimmedText = text.trim().toLowerCase(Locale.ROOT);

        if (trimmedText.endsWith("diario") || trimmedText.endsWith("diariamente")) {
            result.setTimex("P1D");
        } else if (trimmedText.equals("semanalmente")) {
            result.setTimex("P1W");
        } else if (trimmedText.equals("quincenalmente")) {
            result.setTimex("P2W");
        } else if (trimmedText.equals("mensualmente")) {
            result.setTimex("P1M");
        } else if (trimmedText.equals("anualmente")) {
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

        if (trimmedText.equals("día") || trimmedText.equals("dia")  || trimmedText.equals("días")  || trimmedText.equals("dias")) {
            result.setTimex("P1D");
        } else if (trimmedText.equals("semana") || trimmedText.equals("semanas")) {
            result.setTimex("P1W");
        } else if (trimmedText.equals("mes") || trimmedText.equals("meses")) {
            result.setTimex("P1M");
        } else if (trimmedText.equals("año") || trimmedText.equals("años")) {
            result.setTimex("P1Y");
        }

        if (result.getTimex() != "") {
            result.setResult(true);
        }

        return result;
    }
}
