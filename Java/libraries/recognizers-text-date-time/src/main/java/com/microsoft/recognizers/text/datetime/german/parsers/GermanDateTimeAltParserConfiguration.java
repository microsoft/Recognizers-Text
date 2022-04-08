// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.parsers;

import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimeAltParserConfiguration;

public class GermanDateTimeAltParserConfiguration implements IDateTimeAltParserConfiguration {

    private final IDateTimeParser dateTimeParser;
    private final IDateTimeParser dateParser;
    private final IDateTimeParser timeParser;
    private final IDateTimeParser dateTimePeriodParser;
    private final IDateTimeParser timePeriodParser;
    private final IDateTimeParser datePeriodParser;

    public GermanDateTimeAltParserConfiguration(ICommonDateTimeParserConfiguration config) {
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
