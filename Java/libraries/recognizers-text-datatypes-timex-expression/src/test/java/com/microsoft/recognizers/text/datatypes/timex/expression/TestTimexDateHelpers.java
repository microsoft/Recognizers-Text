// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datatypes.timex.expression;

import com.microsoft.recognizers.datatypes.timex.expression.TimexDateHelpers;

import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.util.List;

import org.junit.Assert;
import org.junit.Test;

public class TestTimexDateHelpers {
    @Test
    public void dataTypesDateHelpersTomorrow() {
        LocalDateTime dateExpected = LocalDateTime.of(2017, 1, 1, 0, 0);
        LocalDateTime dateActual = LocalDateTime.of(2016, 12, 31, 0, 0);

        Assert.assertEquals(dateExpected, TimexDateHelpers.tomorrow(dateActual));

        dateExpected = LocalDateTime.of(2017, 1, 2, 0, 0);
        dateActual = LocalDateTime.of(2017, 1, 1, 0, 0);
        Assert.assertEquals(dateExpected, TimexDateHelpers.tomorrow(dateActual));

        dateExpected = LocalDateTime.of(2017, 3, 1, 0, 0);
        dateActual = LocalDateTime.of(2017, 2, 28, 0, 0);
        Assert.assertEquals(dateExpected, TimexDateHelpers.tomorrow(dateActual));

        dateExpected = LocalDateTime.of(2016, 2, 29, 0, 0);
        dateActual = LocalDateTime.of(2016, 2, 28, 0, 0);
        Assert.assertEquals(dateExpected, TimexDateHelpers.tomorrow(dateActual));
    }

    @Test
    public void dataTypesDateHelpersYesterday() {
        LocalDateTime dateExpected = LocalDateTime.of(2016, 12, 31, 0, 0);
        LocalDateTime dateActual = LocalDateTime.of(2017, 1, 1, 0, 0);
        Assert.assertEquals(dateExpected, TimexDateHelpers.yesterday(dateActual));

        dateExpected = LocalDateTime.of(2017, 1, 1, 0, 0);
        dateActual = LocalDateTime.of(2017, 1, 2, 0, 0);
        Assert.assertEquals(dateExpected, TimexDateHelpers.yesterday(dateActual));

        dateExpected = LocalDateTime.of(2017, 2, 28, 0, 0);
        dateActual = LocalDateTime.of(2017, 3, 1, 0, 0);
        Assert.assertEquals(dateExpected, TimexDateHelpers.yesterday(dateActual));

        dateExpected = LocalDateTime.of(2016, 2, 28, 0, 0);
        dateActual = LocalDateTime.of(2016, 2, 29, 0, 0);
        Assert.assertEquals(dateExpected, TimexDateHelpers.yesterday(dateActual));
    }

    @Test
    public void dataTypesDateHelpersDatePartEquals() {
        LocalDateTime dateExpected = LocalDateTime.of(2017, 5, 29, 0, 0);
        LocalDateTime dateActual = LocalDateTime.of(2017, 5, 29, 0, 0);

        Assert.assertTrue(TimexDateHelpers.datePartEquals(dateExpected, dateActual));

        dateExpected = LocalDateTime.of(2017, 5, 29, 19, 30, 0);
        dateActual = LocalDateTime.of(2017, 5, 29, 0, 0);

        Assert.assertTrue(TimexDateHelpers.datePartEquals(dateExpected, dateActual));

        dateExpected = LocalDateTime.of(2017, 5, 29, 0, 0);
        dateActual = LocalDateTime.of(2017, 11, 15, 0, 0);
        Assert.assertFalse(TimexDateHelpers.datePartEquals(dateExpected, dateActual));
    }

    @Test
    public void dataTypesDateHelpersIsNextWeek() {
        LocalDateTime today = LocalDateTime.of(2017, 9, 25, 0, 0);

        LocalDateTime dateExpected = LocalDateTime.of(2017, 10, 4, 0, 0);
        Assert.assertTrue(TimexDateHelpers.isNextWeek(dateExpected, today));

        dateExpected = LocalDateTime.of(2017, 9, 27, 0, 0);
        Assert.assertFalse(TimexDateHelpers.isNextWeek(dateExpected, today));

        Assert.assertFalse(TimexDateHelpers.isNextWeek(today, today));
    }

    @Test
    public void dataTypesDateHelpersIsLastWeek() {
        LocalDateTime today = LocalDateTime.of(2017, 9, 25, 0, 0);

        LocalDateTime dateExpected = LocalDateTime.of(2017, 9, 20, 0, 0);
        Assert.assertTrue(TimexDateHelpers.isLastWeek(dateExpected, today));

        dateExpected = LocalDateTime.of(2017, 9, 4, 0, 0);
        Assert.assertFalse(TimexDateHelpers.isLastWeek(dateExpected, today));
        Assert.assertFalse(TimexDateHelpers.isLastWeek(today, today));
    }

    @Test
    public void dataTypesDateHelpersWeekOfyear() {
        LocalDateTime dateExpected = LocalDateTime.of(2017, 1, 1, 0, 0);
        Assert.assertEquals(1, (int)TimexDateHelpers.weekOfYear(dateExpected));

        dateExpected = LocalDateTime.of(2017, 1, 2, 0, 0);
        Assert.assertEquals(2, (int)TimexDateHelpers.weekOfYear(dateExpected));

        dateExpected = LocalDateTime.of(2017, 2, 23, 0, 0);
        Assert.assertEquals(9, (int)TimexDateHelpers.weekOfYear(dateExpected));

        dateExpected = LocalDateTime.of(2017, 3, 15, 0, 0);
        Assert.assertEquals(12, (int)TimexDateHelpers.weekOfYear(dateExpected));

        dateExpected = LocalDateTime.of(2017, 9, 25, 0, 0);
        Assert.assertEquals(40, (int)TimexDateHelpers.weekOfYear(dateExpected));

        dateExpected = LocalDateTime.of(2017, 12, 31, 0, 0);
        Assert.assertEquals(53, (int)TimexDateHelpers.weekOfYear(dateExpected));

        dateExpected = LocalDateTime.of(2018, 1, 1, 0, 0);
        Assert.assertEquals(1, (int)TimexDateHelpers.weekOfYear(dateExpected));

        dateExpected = LocalDateTime.of(2018, 1, 1, 0, 0);
        Assert.assertEquals(1, (int)TimexDateHelpers.weekOfYear(dateExpected));

        dateExpected = LocalDateTime.of(2018, 1, 7, 0, 0);
        Assert.assertEquals(1, (int)TimexDateHelpers.weekOfYear(dateExpected));

        dateExpected = LocalDateTime.of(2018, 1, 8, 0, 0);
        Assert.assertEquals(2, (int)TimexDateHelpers.weekOfYear(dateExpected));
    }

    @Test
    public void dataTypesDateHelpersInvariance() {
        LocalDateTime d = LocalDateTime.of(2017, 8, 25, 0, 0);
        LocalDateTime before = d;
        TimexDateHelpers.tomorrow(d);
        TimexDateHelpers.yesterday(d);
        TimexDateHelpers.datePartEquals(LocalDateTime.now(), d);
        TimexDateHelpers.datePartEquals(d, LocalDateTime.now());
        TimexDateHelpers.isNextWeek(d, LocalDateTime.now());
        TimexDateHelpers.isNextWeek(LocalDateTime.now(), d);
        TimexDateHelpers.isLastWeek(LocalDateTime.now(), d);
        TimexDateHelpers.weekOfYear(d);
        LocalDateTime after = d;
        Assert.assertEquals(after, before);
    }

    @Test
    public void dataTypesDateHelpersDateOfLastDayFridayLastWeek() {
        DayOfWeek day = DayOfWeek.FRIDAY;
        LocalDateTime date = LocalDateTime.of(2017, 9, 28, 0, 0);

        LocalDateTime dateActual = LocalDateTime.of(2017, 9, 22, 0, 0);
        Assert.assertTrue(TimexDateHelpers.datePartEquals(TimexDateHelpers.dateOfLastDay(day, date), dateActual));
    }

    @Test
    public void dataTypesDateHelpersDateOfNextDayWednesdayNextWeek() {
        DayOfWeek day = DayOfWeek.WEDNESDAY;
        LocalDateTime date = LocalDateTime.of(2017, 9, 28, 0, 0);

        LocalDateTime dateActual = LocalDateTime.of(2017, 10, 4, 0, 0);
        Assert.assertTrue(TimexDateHelpers.datePartEquals(TimexDateHelpers.dateOfNextDay(day, date), dateActual));
    }

    @Test
    public void dataTypesDateHelpersDateOfNextDayToday() {
        DayOfWeek day = DayOfWeek.THURSDAY;
        LocalDateTime date = LocalDateTime.of(2017, 9, 28, 0, 0);
        Assert.assertFalse(TimexDateHelpers.datePartEquals(TimexDateHelpers.dateOfNextDay(day, date), date));
    }

    @Test
    public void dataTypesDateHelpersDatesMatchingDay() {
        DayOfWeek day = DayOfWeek.THURSDAY;
        LocalDateTime start = LocalDateTime.of(2017, 3, 1, 0, 0);
        LocalDateTime end = LocalDateTime.of(2017, 4, 1, 0, 0);
        List<LocalDateTime> result = TimexDateHelpers.datesMatchingDay(day, start, end);
        Assert.assertEquals(5, result.size());

        LocalDateTime dateActual = LocalDateTime.of(2017, 3, 2, 0, 0);
        Assert.assertTrue(TimexDateHelpers.datePartEquals(result.get(0), dateActual));

        dateActual = LocalDateTime.of(2017, 3, 9, 0, 0);
        Assert.assertTrue(TimexDateHelpers.datePartEquals(result.get(1), dateActual));

        dateActual = LocalDateTime.of(2017, 3, 16, 0, 0);
        Assert.assertTrue(TimexDateHelpers.datePartEquals(result.get(2), dateActual));

        dateActual = LocalDateTime.of(2017, 3, 23, 0, 0);
        Assert.assertTrue(TimexDateHelpers.datePartEquals(result.get(3), dateActual));

        dateActual = LocalDateTime.of(2017, 3, 30, 0, 0);
        Assert.assertTrue(TimexDateHelpers.datePartEquals(result.get(4), dateActual));
    }

}
