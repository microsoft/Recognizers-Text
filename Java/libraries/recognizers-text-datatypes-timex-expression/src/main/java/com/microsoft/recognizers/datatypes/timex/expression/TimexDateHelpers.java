// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

public class TimexDateHelpers {
    public static LocalDateTime tomorrow(LocalDateTime date) {
        date = date.plusDays(1);
        return date;
    }

    public static LocalDateTime yesterday(LocalDateTime date) {
        date = date.plusDays(-1);
        return date;
    }

    public static Boolean datePartEquals(LocalDateTime dateX, LocalDateTime dateY) {
        return (dateX.getYear() == dateY.getYear()) &&
            (dateX.getMonthValue() == dateY.getMonthValue()) &&
            (dateX.getDayOfMonth() == dateY.getDayOfMonth());
    }

    public static boolean isDateInWeek(LocalDateTime date, LocalDateTime startOfWeek) {
        LocalDateTime d = startOfWeek;
        for (int i = 0; i < 7; i++) {
            if (TimexDateHelpers.datePartEquals(date, d)) {
                return true;
            }

            d = d.plusDays(1);
        }

        return false;
    }

    public static Boolean isThisWeek(LocalDateTime date, LocalDateTime referenceDate) {
        // Note ISO 8601 week starts on a Monday
        LocalDateTime startOfWeek = referenceDate;
        while (TimexDateHelpers.getUSDayOfWeek(startOfWeek.getDayOfWeek()) > TimexDateHelpers.getUSDayOfWeek(DayOfWeek.MONDAY)) {
            startOfWeek = startOfWeek.plusDays(-1);
        }

        return TimexDateHelpers.isDateInWeek(date, startOfWeek);
    }

    public static Boolean isNextWeek(LocalDateTime date, LocalDateTime referenceDate) {
        LocalDateTime nextWeekDate = referenceDate;
        nextWeekDate = nextWeekDate.plusDays(7);
        return TimexDateHelpers.isThisWeek(date, nextWeekDate);
    }

    public static Boolean isLastWeek(LocalDateTime date, LocalDateTime referenceDate) {
        LocalDateTime nextWeekDate = referenceDate;
        nextWeekDate = nextWeekDate.plusDays(-7);
        return TimexDateHelpers.isThisWeek(date, nextWeekDate);
    }

    public static Integer weekOfYear(LocalDateTime date) {
        LocalDateTime ds = LocalDateTime.of(date.getYear(), 1, 1, 0, 0);
        LocalDateTime de = LocalDateTime.of(date.getYear(), date.getMonthValue(), date.getDayOfMonth(), 0, 0);
        Integer weeks = 1;

        while (ds.compareTo(de) < 0) {
            Integer dayOfWeek = TimexDateHelpers.getUSDayOfWeek(ds.getDayOfWeek());

            Integer isoDayOfWeek = (dayOfWeek == 0) ? 7 : dayOfWeek;
            if (isoDayOfWeek == 7) {
                weeks++;
            }

            ds = ds.plusDays(1);
        }

        return weeks;
    }

    public static String fixedFormatNumber(Integer n, Integer size) {
        return String.format("%1$" + size + "s", n.toString()).replace(' ', '0');
    }

    public static LocalDateTime dateOfLastDay(DayOfWeek day, LocalDateTime referenceDate) {
        LocalDateTime result = referenceDate;
        result = result.plusDays(-1);

        while (result.getDayOfWeek() != day) {
            result = result.plusDays(-1);
        }

        return result;
    }

    public static LocalDateTime dateOfNextDay(DayOfWeek day, LocalDateTime referenceDate) {
        LocalDateTime result = referenceDate;
        result = result.plusDays(1);

        while (result.getDayOfWeek() != day) {
            result = result.plusDays(1);
        }

        return result;
    }

    public static List<LocalDateTime> datesMatchingDay(DayOfWeek day, LocalDateTime start, LocalDateTime end) {
        List<LocalDateTime> result = new ArrayList<LocalDateTime>();
        LocalDateTime d = start;

        while (!TimexDateHelpers.datePartEquals(d, end)) {
            if (d.getDayOfWeek() == day) {
                result.add(d);
            }

            d = d.plusDays(1);
        }

        return result;
    }

    public static Integer getUSDayOfWeek(DayOfWeek dayOfWeek) {
        return dayOfWeek.getValue() % 7;
    }
}
