// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datatypes.timex.expression;

import com.microsoft.recognizers.datatypes.timex.expression.Constants;
import com.microsoft.recognizers.datatypes.timex.expression.TimexProperty;

import java.math.BigDecimal;
import java.util.HashSet;

import org.junit.Assert;
import org.junit.Test;

public class TestTimexParsing {

    @Test
    public void dataTypesParsingCompleteDate() {
        TimexProperty timex = new TimexProperty("2017-05-29");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DEFINITE);
                add(Constants.TimexTypes.DATE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertEquals(2017, (int)timex.getYear());
        Assert.assertEquals(5, (int)timex.getMonth());
        Assert.assertEquals(29, (int)timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingMonthAndDayOfMonth() {
        TimexProperty timex = new TimexProperty("XXXX-12-05");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertEquals(12, (int)timex.getMonth());
        Assert.assertEquals(5, (int)timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDayOfWeek() {
        TimexProperty timex = new TimexProperty("XXXX-WXX-3");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertEquals(3, (int)timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingHoursMinutesAndSeconds() {
        TimexProperty timex = new TimexProperty("T17:30:05");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.TIME);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertEquals(17, (int)timex.getHour());
        Assert.assertEquals(30, (int)timex.getMinute());
        Assert.assertEquals(5, (int)timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingHoursAndMinutes() {
        TimexProperty timex = new TimexProperty("T17:30");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.TIME);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertEquals(17, (int)timex.getHour());
        Assert.assertEquals(30, (int)timex.getMinute());
        Assert.assertEquals(0, (int)timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingHours() {
        TimexProperty timex = new TimexProperty("T17");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.TIME);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertEquals(17, (int)timex.getHour());
        Assert.assertEquals(0, (int)timex.getMinute());
        Assert.assertEquals(0, (int)timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsing_Now() {
        TimexProperty timex = new TimexProperty("PRESENT_REF");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.PRESENT);
                add(Constants.TimexTypes.DATE);
                add(Constants.TimexTypes.TIME);
                add(Constants.TimexTypes.DATE_TIME);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertEquals(true, timex.getNow());
    }

    @Test
    public void dataTypesParsingFullDatetime() {
        TimexProperty timex = new TimexProperty("1984-01-03T18:30:45");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DEFINITE);
                add(Constants.TimexTypes.DATE);
                add(Constants.TimexTypes.TIME);
                add(Constants.TimexTypes.DATE_TIME);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertEquals(1984, (int)timex.getYear());
        Assert.assertEquals(1, (int)timex.getMonth());
        Assert.assertEquals(3, (int)timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertEquals(18, (int)timex.getHour());
        Assert.assertEquals(30, (int)timex.getMinute());
        Assert.assertEquals(45, (int)timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingParticularTimeOnParticularDayOfWeek() {
        TimexProperty timex = new TimexProperty("XXXX-WXX-3T16");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.TIME);
                add(Constants.TimexTypes.DATE);
                add(Constants.TimexTypes.DATE_TIME);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertEquals(3, (int)timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertEquals(16, (int)timex.getHour());
        Assert.assertEquals(0, (int)timex.getMinute());
        Assert.assertEquals(0, (int)timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingYear() {
        TimexProperty timex = new TimexProperty("2016");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertEquals(2016, (int)timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingSummerOf1999() {
        TimexProperty timex = new TimexProperty("1999-SU");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertEquals(1999, (int)timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertEquals("SU", timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingYearAndWeek() {
        TimexProperty timex = new TimexProperty("2017-W37");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertEquals(2017, (int)timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertEquals(37, (int)timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingSeasonSummer() {
        TimexProperty timex = new TimexProperty("SU");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertEquals("SU", timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingSeasonWinter() {
        TimexProperty timex = new TimexProperty("WI");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertEquals("WI", timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingYearAndWeekend() {
        TimexProperty timex = new TimexProperty("2017-W37-WE");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertEquals(2017, (int)timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertEquals(37, (int)timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertEquals(true, timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingMay() {
        TimexProperty timex = new TimexProperty("XXXX-05");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertEquals(5, (int)timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingJuly2020() {
        TimexProperty timex = new TimexProperty("2020-07");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertEquals(2020, (int)timex.getYear());
        Assert.assertEquals(7, (int)timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingWeekOfMonth() {
        TimexProperty timex = new TimexProperty("XXXX-01-W01");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertEquals(1, (int)timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertEquals(1, (int)timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingWednesdayToSaturday() {
        TimexProperty timex = new TimexProperty("(XXXX-WXX-3,XXXX-WXX-6,P3D)");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE);
                add(Constants.TimexTypes.DURATION);
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertEquals(3, (int)timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertEquals(3, timex.getDays().intValue());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingJan1ToAug5() {
        TimexProperty timex = new TimexProperty("(XXXX-01-01,XXXX-08-05,P216D)");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE);
                add(Constants.TimexTypes.DURATION);
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertEquals(1, (int)timex.getMonth());
        Assert.assertEquals(1, (int)timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertEquals(216, timex.getDays().intValue());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingJan1ToAug5Year2015() {
        TimexProperty timex = new TimexProperty("(2015-01-01,2015-08-05,P216D)");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DEFINITE);
                add(Constants.TimexTypes.DATE);
                add(Constants.TimexTypes.DURATION);
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertEquals(2015, (int)timex.getYear());
        Assert.assertEquals(1, (int)timex.getMonth());
        Assert.assertEquals(1, (int)timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertEquals(216, timex.getDays().intValue());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDayTime() {
        TimexProperty timex = new TimexProperty("TDT");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.TIME_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertEquals("DT", timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingNightTime() {
        TimexProperty timex = new TimexProperty("TNI");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.TIME_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertEquals("NI", timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingMorning() {
        TimexProperty timex = new TimexProperty("TMO");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.TIME_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertEquals("MO", timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingAfternoon() {
        TimexProperty timex = new TimexProperty("TAF");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.TIME_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertEquals("AF", timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingEvening() {
        TimexProperty timex = new TimexProperty("TEV");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.TIME_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertEquals("EV", timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingTimerange430pmTo445pm() {
        TimexProperty timex = new TimexProperty("(T16:30,T16:45,PT15M)");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.TIME);
                add(Constants.TimexTypes.DURATION);
                add(Constants.TimexTypes.TIME_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertEquals(16, (int)timex.getHour());
        Assert.assertEquals(30, (int)timex.getMinute());
        Assert.assertEquals(0, (int)timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertEquals(15, timex.getMinutes().intValue());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDateTimeRange() {
        TimexProperty timex = new TimexProperty("XXXX-WXX-5TEV");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE);
                add(Constants.TimexTypes.TIME_RANGE);
                add(Constants.TimexTypes.DATE_TIME_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertEquals(5, (int)timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertEquals("EV", timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingLastNight() {
        TimexProperty timex = new TimexProperty("2017-09-07TNI");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DEFINITE);
                add(Constants.TimexTypes.DATE);
                add(Constants.TimexTypes.TIME_RANGE);
                add(Constants.TimexTypes.DATE_TIME_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertEquals(2017, (int)timex.getYear());
        Assert.assertEquals(9, (int)timex.getMonth());
        Assert.assertEquals(7, (int)timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertEquals("NI", timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingLast5Minutes() {
        TimexProperty timex = new TimexProperty("(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE);
                add(Constants.TimexTypes.TIME_RANGE);
                add(Constants.TimexTypes.DATE_TIME_RANGE);
                add(Constants.TimexTypes.TIME);
                add(Constants.TimexTypes.DATE_TIME);
                add(Constants.TimexTypes.DURATION);
                add(Constants.TimexTypes.DATE_RANGE);
                add(Constants.TimexTypes.DEFINITE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertEquals(2017, (int)timex.getYear());
        Assert.assertEquals(9, (int)timex.getMonth());
        Assert.assertEquals(8, (int)timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertEquals(21, (int)timex.getHour());
        Assert.assertEquals(19, (int)timex.getMinute());
        Assert.assertEquals(29, (int)timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertEquals(5, timex.getMinutes().intValue());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingWed4PMToSat3PM() {
        TimexProperty timex = new TimexProperty("(XXXX-WXX-3T16,XXXX-WXX-6T15,PT71H)");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DATE);
                add(Constants.TimexTypes.TIME_RANGE);
                add(Constants.TimexTypes.DATE_TIME_RANGE);
                add(Constants.TimexTypes.TIME);
                add(Constants.TimexTypes.DATE_TIME);
                add(Constants.TimexTypes.DURATION);
                add(Constants.TimexTypes.DATE_RANGE);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertEquals(3, (int)timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertEquals(16, (int)timex.getHour());
        Assert.assertEquals(0, (int)timex.getMinute());
        Assert.assertEquals(0, (int)timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertEquals(71, timex.getHours().intValue());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDurationYears() {
        TimexProperty timex = new TimexProperty("P2Y");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DURATION);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertEquals(2, timex.getYears().intValue());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDurationMonths() {
        TimexProperty timex = new TimexProperty("P4M");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DURATION);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertEquals(4, timex.getMonths().intValue());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDurationWeeks() {
        TimexProperty timex = new TimexProperty("P6W");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DURATION);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertEquals(6, timex.getWeeks().intValue());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDurationWeeksFloatingPoint() {
        TimexProperty timex = new TimexProperty("P2.5W");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DURATION);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertEquals(2.5d, timex.getWeeks().doubleValue(), 0.5);
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDurationDays() {
        TimexProperty timex = new TimexProperty("P1D");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DURATION);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertEquals(1, timex.getDays().intValue());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDurationHours() {  
        TimexProperty timex = new TimexProperty("PT5H");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DURATION);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertEquals(5, timex.getHours().intValue());
        Assert.assertNull(timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDurationMinutes() {
        TimexProperty timex = new TimexProperty("PT30M");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DURATION);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertEquals(new BigDecimal(30), timex.getMinutes());
        Assert.assertNull(timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }

    @Test
    public void dataTypesParsingDurationSeconds() {
        TimexProperty timex = new TimexProperty("PT45S");
        HashSet<String> expected = new HashSet<String>() {
            {
                add(Constants.TimexTypes.DURATION);
            }
        };
        HashSet<String> actual = timex.getTypes();
        Assert.assertEquals(expected, actual);
        Assert.assertNull(timex.getYear());
        Assert.assertNull(timex.getMonth());
        Assert.assertNull(timex.getDayOfMonth());
        Assert.assertNull(timex.getDayOfWeek());
        Assert.assertNull(timex.getWeekOfYear());
        Assert.assertNull(timex.getWeekOfMonth());
        Assert.assertNull(timex.getSeason());
        Assert.assertNull(timex.getHour());
        Assert.assertNull(timex.getMinute());
        Assert.assertNull(timex.getSecond());
        Assert.assertNull(timex.getWeekend());
        Assert.assertNull(timex.getPartOfDay());
        Assert.assertNull(timex.getYears());
        Assert.assertNull(timex.getMonths());
        Assert.assertNull(timex.getWeeks());
        Assert.assertNull(timex.getDays());
        Assert.assertNull(timex.getHours());
        Assert.assertNull(timex.getMinutes());
        Assert.assertEquals(new BigDecimal(45), timex.getSeconds());
        Assert.assertNull(timex.getNow());
    }
}
