package com.microsoft.recognizers.text.datetime.french.parsers;

import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimeAltParserConfiguration;

public class FrenchDateTimeAltParserConfiguration implements IDateTimeAltParserConfiguration {

    private final IDateTimeParser dateTimeParser;
    private final IDateTimeParser dateParser;
    private final IDateTimeParser timeParser;
    private final IDateTimeParser dateTimePeriodParser;
    private final IDateTimeParser timePeriodParser;
    private final IDateTimeParser datePeriodParser;

    public FrenchDateTimeAltParserConfiguration(final ICommonDateTimeParserConfiguration config) {
        dateTimeParser = config.getDateTimeParser();
        dateParser = config.getDateParser();
        timeParser = config.getTimeParser();
        dateTimePeriodParser = config.getDateTimePeriodParser();
        timePeriodParser = config.getTimePeriodParser();
        datePeriodParser = config.getDatePeriodParser();
    }

    public IDateTimeParser getDateTimeParser() {
        return dateTimeParser;
    }

    public IDateTimeParser getDateParser() {
        return dateParser;
    }

    public IDateTimeParser getTimeParser() {
        return timeParser;
    }

    public IDateTimeParser getDateTimePeriodParser() {
        return dateTimePeriodParser;
    }

    public IDateTimeParser getTimePeriodParser() {
        return timePeriodParser;
    }

    public IDateTimeParser getDatePeriodParser() {
        return datePeriodParser;
    }
}
