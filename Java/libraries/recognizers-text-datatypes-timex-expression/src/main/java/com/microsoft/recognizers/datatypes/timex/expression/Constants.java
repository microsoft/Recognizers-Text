// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

public class Constants {

    // Timex
    public static final String TIMEX_YEAR = "Y";
    public static final String TIMEX_MONTH = "M";
    public static final String TIMEX_MONTH_FULL = "MON";
    public static final String TIMEX_WEEK = "W";
    public static final String TIMEX_DAY = "D";
    public static final String TIMEX_BUSINESS_DAY = "BD";
    public static final String TIMEX_WEEKEND = "WE";
    public static final String TIMEX_HOUR = "H";
    public static final String TIMEX_MINUTE = "M";
    public static final String TIMEX_SECOND = "S";
    public static final String TIMEX_NIGHT = "NI";
    public static final Character TIMEX_FUZZY = 'X';
    public static final String TIMEX_FUZZY_YEAR = "XXXX";
    public static final String TIMEX_FUZZY_MONTH = "XX";
    public static final String TIMEX_FUZZY_WEEK = "WXX";
    public static final String TIMEX_FUZZY_DAY = "XX";
    public static final String DATE_TIMEX_CONNECTOR = "-";
    public static final String TIME_TIMEX_CONNECTOR = ":";
    public static final String GENERAL_PERIOD_PREFIX = "P";
    public static final String TIME_TIMEX_PREFIX = "T";

    public static final String YEAR_UNIT = "year";
    public static final String MONTH_UNIT = "month";
    public static final String WEEK_UNIT = "week";
    public static final String DAY_UNIT = "day";
    public static final String HOUR_UNIT = "hour";
    public static final String MINUTE_UNIT = "minute";
    public static final String SECOND_UNIT = "second";
    public static final String TIME_DURATION_UNIT = "s";

    public static final String AM = "AM";
    public static final String PM = "PM";

    public static final int INVALID_VALUE = -1;

    public static class TimexTypes {
        public static final String PRESENT = "present";
        public static final String DEFINITE = "definite";
        public static final String DATE = "date";
        public static final String DATE_TIME = "datetime";
        public static final String DATE_RANGE = "daterange";
        public static final String DURATION = "duration";
        public static final String TIME = "time";
        public static final String TIME_RANGE = "timerange";
        public static final String DATE_TIME_RANGE = "datetimerange";
    }
}
