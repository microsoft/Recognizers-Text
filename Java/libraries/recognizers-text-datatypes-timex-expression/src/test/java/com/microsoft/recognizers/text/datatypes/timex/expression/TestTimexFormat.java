// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datatypes.timex.expression;

import com.microsoft.recognizers.datatypes.timex.expression.TimexProperty;

import java.math.BigDecimal;

import org.junit.Assert;
import org.junit.Test;

public class TestTimexFormat {

    @Test
    public void dataTypesFormatDate() {
        Assert.assertEquals("2017-09-27", new TimexProperty() {
            {
                setYear(2017);
                setMonth(9);
                setDayOfMonth(27);
            }
        }.getTimexValue());
        Assert.assertEquals("XXXX-WXX-3", new TimexProperty() {
            {
                setDayOfWeek(3);
            }
        }.getTimexValue());
        Assert.assertEquals("XXXX-12-05", new TimexProperty() {
            {
                setMonth(12);
                setDayOfMonth(5);
            }
        }.getTimexValue());
    }

    @Test
    public void dataTypesFormatTime() {
        Assert.assertEquals("T17:30:45", new TimexProperty() {
            {
                setHour(17);
                setMinute(30);
                setSecond(45);
            }
        }.getTimexValue());
        Assert.assertEquals("T05:06:07", new TimexProperty() {
            {
                setHour(5);
                setMinute(6);
                setSecond(7);
            }
        }.getTimexValue());
        Assert.assertEquals("T17:30", new TimexProperty() {
            {
                setHour(17);
                setMinute(30);
                setSecond(0);
            }
        }.getTimexValue());
        Assert.assertEquals("T23", new TimexProperty() {
            {
                setHour(23);
                setMinute(0);
                setSecond(0);
            }
        }.getTimexValue());
    }

    @Test
    public void dataTypesFormatDuration() {
        Assert.assertEquals("P50Y", new TimexProperty() {
            {
                setYears(new BigDecimal(50));
            }
        }.getTimexValue());
        Assert.assertEquals("P6M", new TimexProperty() {
            {
                setMonths(new BigDecimal(6));
            }
        }.getTimexValue());
        Assert.assertEquals("P3W", new TimexProperty() {
            {
                setWeeks(new BigDecimal(3));
            }
        }.getTimexValue());
        Assert.assertEquals("P5D", new TimexProperty() {
            {
                setDays(new BigDecimal(5));
            }
        }.getTimexValue());
        Assert.assertEquals("PT16H", new TimexProperty() {
            {
                setHours(new BigDecimal(16));
            }
        }.getTimexValue());
        Assert.assertEquals("PT32M", new TimexProperty() {
            {
                setMinutes(new BigDecimal(32));
            }
        }.getTimexValue());
        Assert.assertEquals("PT20S", new TimexProperty() {
            {
                setSeconds(new BigDecimal(20));
            }
        }.getTimexValue());
    }

    @Test
    public void dataTypesFormatPresent() {
        Assert.assertEquals("PRESENT_REF", new TimexProperty() {
            {
                setNow(true);
            }
        }.getTimexValue());
    }

    @Test
    public void dataTypesFormatDateTime() {
        Assert.assertEquals("XXXX-WXX-3T04", new TimexProperty() {
            {
                setDayOfWeek(3);
                setHour(4);
                setMinute(0);
                setSecond(0);
            }
        }.getTimexValue());
        Assert.assertEquals("2017-09-27T11:41:30", new TimexProperty() {
            {
                setYear(2017);
                setMonth(9);
                setDayOfMonth(27);
                setHour(11);
                setMinute(41);
                setSecond(30);
            }
        }.getTimexValue());
    }

    @Test
    public void dataTypesFormatDateRange() {
        Assert.assertEquals("2017", new TimexProperty() {
            {
                setYear(2017);
            }
        }.getTimexValue());
        Assert.assertEquals("SU", new TimexProperty() {
            {
                setSeason("SU");
            }
        }.getTimexValue());
        Assert.assertEquals("2017-WI", new TimexProperty() {
            {
                setYear(2017);
                setSeason("WI");
            }
        }.getTimexValue());
        Assert.assertEquals("2017-09", new TimexProperty() {
            {
                setYear(2017);
                setMonth(9);
            }
        }.getTimexValue());
        Assert.assertEquals("2017-W37", new TimexProperty() {
            {
                setYear(2017);
                setWeekOfYear(37);
            }
        }.getTimexValue());
        Assert.assertEquals("2017-W37-WE", new TimexProperty() {
            {
                setYear(2017);
                setWeekOfYear(37);
                setWeekend(true);
            }
        }.getTimexValue());
        Assert.assertEquals("XXXX-05", new TimexProperty() {
            {
                setMonth(5);
            }
        }.getTimexValue());
    }

    @Test
    public void dataTypesFormatTimeRange() {
        Assert.assertEquals("TEV", new TimexProperty() {
            {
                setPartOfDay("EV");
            }
        }.getTimexValue());
    }

    @Test
    public void dataTypesFormatDateTimeRange() {
        Assert.assertEquals("2017-09-27TEV", new TimexProperty() {
            {
                setYear(2017);
                setMonth(9);
                setDayOfMonth(27);
                setPartOfDay("EV");
            }
        }.getTimexValue());
    }
}
