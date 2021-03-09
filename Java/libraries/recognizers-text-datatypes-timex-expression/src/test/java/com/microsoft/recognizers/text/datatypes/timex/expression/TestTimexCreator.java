// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datatypes.timex.expression;

import com.microsoft.recognizers.datatypes.timex.expression.TimexCreator;
import com.microsoft.recognizers.datatypes.timex.expression.TimexDateHelpers;
import com.microsoft.recognizers.datatypes.timex.expression.TimexFormat;
import com.microsoft.recognizers.datatypes.timex.expression.TimexProperty;

import java.math.BigDecimal;
import java.time.DayOfWeek;
import java.time.LocalDateTime;

import org.junit.Assert;
import org.junit.Test;

public class TestTimexCreator {

    @Test
    public void dataTypesCreatorToday() {
        LocalDateTime d = LocalDateTime.now();
        String expected = TimexFormat.format(new TimexProperty() {
            {
                setYear(d.getYear());
                setMonth(d.getMonthValue());
                setDayOfMonth(d.getDayOfMonth());
            }
        });
        Assert.assertEquals(expected, TimexCreator.today(d));
    }

    @Test
    public void dataTypesCreatorTodayRelative() {
        LocalDateTime d = LocalDateTime.of(2017, 10, 5, 0, 0);
        Assert.assertEquals("2017-10-05", TimexCreator.today(d));
    }

    @Test
    public void dataTypesCreatorTomorrow() {
        LocalDateTime d = LocalDateTime.now().plusDays(1);
        String expected = TimexFormat.format(new TimexProperty() {
            {
                setYear(d.getYear());
                setMonth(d.getMonthValue());
                setDayOfMonth(d.getDayOfMonth());
            }
        });
        Assert.assertEquals(expected, TimexCreator.tomorrow(null));
    }

    @Test
    public void dataTypesCreatorTomorrowRelative() {
        LocalDateTime d = LocalDateTime.of(2017, 10, 5, 0, 0);
        Assert.assertEquals("2017-10-06", TimexCreator.tomorrow(d));
    }

    @Test
    public void dataTypesCreatorYesterday() {
        LocalDateTime d = LocalDateTime.now().plusDays(-1);
        String expected = TimexFormat.format(new TimexProperty() {
            {
                setYear(d.getYear());
                setMonth(d.getMonthValue());
                setDayOfMonth(d.getDayOfMonth());
            }
        });
        Assert.assertEquals(expected, TimexCreator.yesterday(null));
    }

    @Test
    public void dataTypesCreatorYesterdayRelative() {
        LocalDateTime d = LocalDateTime.of(2017, 10, 5, 0, 0);
        Assert.assertEquals("2017-10-04", TimexCreator.yesterday(d));
    }

    @Test
    public void dataTypesCreatorWeekFromToday() {
        LocalDateTime d = LocalDateTime.now();
        String expected = TimexFormat.format(new TimexProperty() {
            {
                setYear(d.getYear());
                setMonth(d.getMonthValue());
                setDayOfMonth(d.getDayOfMonth());
                setDays(new BigDecimal(7));
            }
        });
        Assert.assertEquals(expected, TimexCreator.weekFromToday(d));
    }

    @Test
    public void dataTypesCreatorWeekFromTodayRelative() {
        LocalDateTime d = LocalDateTime.of(2017, 10, 5, 0, 0);
        Assert.assertEquals("(2017-10-05,2017-10-12,P7D)", TimexCreator.weekFromToday(d));
    }

    @Test
    public void dataTypesCreatorWeekBackFromToday() {
        LocalDateTime d = LocalDateTime.now().plusDays(-7);
        String expected = TimexFormat.format(new TimexProperty() {
            {
                setYear(d.getYear());
                setMonth(d.getMonthValue());
                setDayOfMonth(d.getDayOfMonth());
                setDays(new BigDecimal(7));
            }
        });
        Assert.assertEquals(expected, TimexCreator.weekBackFromToday(null));
    }

    @Test
    public void dataTypesCreatorWeekBackFromTodayRelative() {
        LocalDateTime d = LocalDateTime.of(2017, 10, 5, 0, 0);
        Assert.assertEquals("(2017-09-28,2017-10-05,P7D)", TimexCreator.weekBackFromToday(d));
    }

    @Test
    public void dataTypesCreatorNextWeek() {
        LocalDateTime start = TimexDateHelpers.dateOfNextDay(DayOfWeek.MONDAY, LocalDateTime.now());
        TimexProperty t = TimexProperty.fromDate(start);
        t.setDays(new BigDecimal(7));
        String expected = t.getTimexValue();
        Assert.assertEquals(expected, TimexCreator.nextWeek(null));
    }

    @Test
    public void dataTypesCreatorNextWeekRelative() {
        LocalDateTime d = LocalDateTime.of(2017, 10, 5, 0, 0);
        Assert.assertEquals("(2017-10-09,2017-10-16,P7D)", TimexCreator.nextWeek(d));
    }

    @Test
    public void dataTypesCreatorLastWeek() {
        LocalDateTime start = TimexDateHelpers.dateOfLastDay(DayOfWeek.MONDAY, LocalDateTime.now());
        start = start.plusDays(-7);
        TimexProperty t = TimexProperty.fromDate(start);
        t.setDays(new BigDecimal(7));
        String expected = t.getTimexValue();
        Assert.assertEquals(expected, TimexCreator.lastWeek(null));
    }

    @Test
    public void dataTypesCreatorLastWeekRelative() {
        LocalDateTime d = LocalDateTime.of(2017, 10, 5, 0, 0);
        Assert.assertEquals("(2017-09-25,2017-10-02,P7D)", TimexCreator.lastWeek(d));
    }

    @Test
    public void dataTypesCreatorNextWeeksFromToday() {
        LocalDateTime d = LocalDateTime.now();
        String expected = TimexFormat.format(new TimexProperty() {
            {
                setYear(d.getYear());
                setMonth(d.getMonthValue());
                setDayOfMonth(d.getDayOfMonth());
                setDays(new BigDecimal(14));
            }
        });
        Assert.assertEquals(expected, TimexCreator.nextWeeksFromToday(2, d));
    }

    @Test
    public void dataTypesCreatorNextWeeksFromTodayRelative() {
        LocalDateTime d = LocalDateTime.of(2017, 10, 5, 0, 0);
        Assert.assertEquals("(2017-10-05,2017-10-19,P14D)", TimexCreator.nextWeeksFromToday(2, d));
    }
}
