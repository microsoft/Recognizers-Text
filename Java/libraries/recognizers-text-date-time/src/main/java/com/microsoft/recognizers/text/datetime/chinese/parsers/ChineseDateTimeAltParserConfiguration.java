package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimeAltParserConfiguration;

public class ChineseDateTimeAltParserConfiguration implements IDateTimeAltParserConfiguration {

    private final IDateExtractor dateExtractor;
    private final IDateTimeExtractor datePeriodExtractor;
    private final IDateTimeExtractor dateTimeExtractor;
    private final IDateTimeExtractor dateTimePeriodExtractor;
    private final IDateTimeParser dateParser;
    private final IDateTimeParser datePeriodParser;
    private final IDateTimeParser dateTimeParser;
    private final IDateTimeParser dateTimePeriodParser;
    private final IDateTimeParser timeParser;
    private final IDateTimeParser timePeriodParser;

    public ChineseDateTimeAltParserConfiguration(ICommonDateTimeParserConfiguration config) {
        dateExtractor = config.getDateExtractor();
        datePeriodExtractor = config.getDatePeriodExtractor();
        dateTimeExtractor = config.getDateTimeExtractor();
        dateTimePeriodExtractor = config.getDateTimePeriodExtractor();
        dateParser = config.getDateParser();
        datePeriodParser = config.getDatePeriodParser();
        dateTimeParser = config.getDateTimeParser();
        dateTimePeriodParser = config.getDateTimePeriodParser();
        timeParser = config.getTimeParser();
        timePeriodParser = config.getTimePeriodParser();
    }

    public IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    public IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    public IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
    }

    public IDateTimeExtractor getDateTimePeriodExtractor() {
        return dateTimePeriodExtractor;
    }

    public IDateTimeParser getDateParser() {
        return dateParser;
    }

    public IDateTimeParser getDatePeriodParser() {
        return datePeriodParser;
    }

    public IDateTimeParser getDateTimeParser() {
        return dateTimeParser;
    }

    public IDateTimeParser getDateTimePeriodParser() {
        return dateTimePeriodParser;
    }

    public IDateTimeParser getTimeParser() {
        return timeParser;
    }

    public IDateTimeParser getTimePeriodParser() {
        return timePeriodParser;
    }
}
