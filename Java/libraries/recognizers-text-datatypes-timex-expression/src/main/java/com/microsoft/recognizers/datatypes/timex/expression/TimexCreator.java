// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.math.BigDecimal;
import java.time.DayOfWeek;
import java.time.LocalDateTime;

public class TimexCreator {
    // The following constants are consistent with the Recognizer results
    public static final String MONDAY = "XXXX-WXX-1";
    public static final String TUESDAY = "XXXX-WXX-2";
    public static final String WEDNESDAY = "XXXX-WXX-3";
    public static final String THURSDAY = "XXXX-WXX-4";
    public static final String FRIDAY = "XXXX-WXX-5";
    public static final String SATURDAY = "XXXX-WXX-6";
    public static final String SUNDAY = "XXXX-WXX-7";
    public static final String MORNING = "(T08,T12,PT4H)";
    public static final String AFTERNOON = "(T12,T16,PT4H)";
    public static final String EVENING = "(T16,T20,PT4H)";
    public static final String DAYTIME = "(T08,T18,PT10H)";
    public static final String NIGHT = "(T20,T24,PT10H)";

    public static String today(LocalDateTime date) {
        return TimexProperty.fromDate(date == null ? LocalDateTime.now() : date).getTimexValue();
    }

    public static String tomorrow(LocalDateTime date) {
        LocalDateTime d = (date == null) ? LocalDateTime.now() : date;
        d = d.plusDays(1);
        return TimexProperty.fromDate(d).getTimexValue();
    }

    public static String yesterday(LocalDateTime date) {
        LocalDateTime d = (date == null) ? LocalDateTime.now() : date;
        d = d.plusDays(-1);
        return TimexProperty.fromDate(d).getTimexValue();
    }

    public static String weekFromToday(LocalDateTime date) {
        LocalDateTime d = (date == null) ? LocalDateTime.now() : date;
        TimexProperty t = TimexProperty.fromDate(d);
        t.setDays(new BigDecimal(7));
        return t.getTimexValue();
    }

    public static String weekBackFromToday(LocalDateTime date) {
        LocalDateTime d = (date == null) ? LocalDateTime.now() : date;
        d = d.plusDays(-7);
        TimexProperty t = TimexProperty.fromDate(d);
        t.setDays(new BigDecimal(7));
        return t.getTimexValue();
    }

    public static String thisWeek(LocalDateTime date) {
        LocalDateTime d = (date == null) ? LocalDateTime.now() : date;
        d = d.plusDays(-7);
        LocalDateTime start = TimexDateHelpers.dateOfNextDay(DayOfWeek.MONDAY, d);
        TimexProperty t = TimexProperty.fromDate(start);
        t.setDays(new BigDecimal(7));
        return t.getTimexValue();
    }

    public static String nextWeek(LocalDateTime date) {
        LocalDateTime d = (date == null) ? LocalDateTime.now() : date;
        LocalDateTime start = TimexDateHelpers.dateOfNextDay(DayOfWeek.MONDAY, d);
        TimexProperty t = TimexProperty.fromDate(start);
        t.setDays(new BigDecimal(7));
        return t.getTimexValue();
    }

    public static String lastWeek(LocalDateTime date) {
        LocalDateTime d = (date == null) ? LocalDateTime.now() : date;
        LocalDateTime start = TimexDateHelpers.dateOfLastDay(DayOfWeek.MONDAY, d);
        start = start.plusDays(-7);
        TimexProperty t = TimexProperty.fromDate(start);
        t.setDays(new BigDecimal(7));
        return t.getTimexValue();
    }

    public static String nextWeeksFromToday(Integer n, LocalDateTime date) {
        LocalDateTime d = (date == null) ? LocalDateTime.now() : date;
        TimexProperty t = TimexProperty.fromDate(d);
        t.setDays(new BigDecimal(n * 7));
        return t.getTimexValue();
    }
}
