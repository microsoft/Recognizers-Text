// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datatypes.timex.expression;

import com.microsoft.recognizers.datatypes.timex.expression.TimexConvert;
import com.microsoft.recognizers.datatypes.timex.expression.TimexProperty;
import com.microsoft.recognizers.datatypes.timex.expression.TimexSet;
import org.junit.Assert;
import org.junit.Test;

public class TestTimexConvert {

    @Test
    public void dataTypesConvertCompleteDate() {
        Assert.assertEquals("29th May 2017", TimexConvert.convertTimexToString(new TimexProperty("2017-05-29")));
    }

    @Test
    public void dataTypesConvertMonthAndDayOfMonth() {
        Assert.assertEquals("5th January", TimexConvert.convertTimexToString(new TimexProperty("XXXX-01-05")));
        Assert.assertEquals("5th February", TimexConvert.convertTimexToString(new TimexProperty("XXXX-02-05")));
        Assert.assertEquals("5th March", TimexConvert.convertTimexToString(new TimexProperty("XXXX-03-05")));
        Assert.assertEquals("5th April", TimexConvert.convertTimexToString(new TimexProperty("XXXX-04-05")));
        Assert.assertEquals("5th May", TimexConvert.convertTimexToString(new TimexProperty("XXXX-05-05")));
        Assert.assertEquals("5th June", TimexConvert.convertTimexToString(new TimexProperty("XXXX-06-05")));
        Assert.assertEquals("5th July", TimexConvert.convertTimexToString(new TimexProperty("XXXX-07-05")));
        Assert.assertEquals("5th August", TimexConvert.convertTimexToString(new TimexProperty("XXXX-08-05")));
        Assert.assertEquals("5th September", TimexConvert.convertTimexToString(new TimexProperty("XXXX-09-05")));
        Assert.assertEquals("5th October", TimexConvert.convertTimexToString(new TimexProperty("XXXX-10-05")));
        Assert.assertEquals("5th November", TimexConvert.convertTimexToString(new TimexProperty("XXXX-11-05")));
        Assert.assertEquals("5th December", TimexConvert.convertTimexToString(new TimexProperty("XXXX-12-05")));
    }

    @Test
    public void dataTypesConvertMonthAndDayOfMonthWithCorrectAbbreviation() {
        Assert.assertEquals("1st June", TimexConvert.convertTimexToString(new TimexProperty("XXXX-06-01")));
        Assert.assertEquals("2nd June", TimexConvert.convertTimexToString(new TimexProperty("XXXX-06-02")));
        Assert.assertEquals("3rd June", TimexConvert.convertTimexToString(new TimexProperty("XXXX-06-03")));
        Assert.assertEquals("4th June", TimexConvert.convertTimexToString(new TimexProperty("XXXX-06-04")));
    }

    @Test
    public void dataTypesConvertDayOfWeek() {
        Assert.assertEquals("Monday", TimexConvert.convertTimexToString(new TimexProperty("XXXX-WXX-1")));
        Assert.assertEquals("Tuesday", TimexConvert.convertTimexToString(new TimexProperty("XXXX-WXX-2")));
        Assert.assertEquals("Wednesday", TimexConvert.convertTimexToString(new TimexProperty("XXXX-WXX-3")));
        Assert.assertEquals("Thursday", TimexConvert.convertTimexToString(new TimexProperty("XXXX-WXX-4")));
        Assert.assertEquals("Friday", TimexConvert.convertTimexToString(new TimexProperty("XXXX-WXX-5")));
        Assert.assertEquals("Saturday", TimexConvert.convertTimexToString(new TimexProperty("XXXX-WXX-6")));
        Assert.assertEquals("Sunday", TimexConvert.convertTimexToString(new TimexProperty("XXXX-WXX-7")));
    }

    @Test
    public void dataTypesConvertTime() {
        Assert.assertEquals("5:30:05PM", TimexConvert.convertTimexToString(new TimexProperty("T17:30:05")));
        Assert.assertEquals("2:30:30AM", TimexConvert.convertTimexToString(new TimexProperty("T02:30:30")));
        Assert.assertEquals("12:30:30AM", TimexConvert.convertTimexToString(new TimexProperty("T00:30:30")));
        Assert.assertEquals("12:30:30PM", TimexConvert.convertTimexToString(new TimexProperty("T12:30:30")));
    }

    @Test
    public void dataTypesConvertHourAndMinute() {
        Assert.assertEquals("5:30PM", TimexConvert.convertTimexToString(new TimexProperty("T17:30")));
        Assert.assertEquals("5PM", TimexConvert.convertTimexToString(new TimexProperty("T17:00")));
        Assert.assertEquals("1:30AM", TimexConvert.convertTimexToString(new TimexProperty("T01:30")));
        Assert.assertEquals("1AM", TimexConvert.convertTimexToString(new TimexProperty("T01:00")));
    }

    @Test
    public void dataTypesConvertHour() {
        Assert.assertEquals("midnight", TimexConvert.convertTimexToString(new TimexProperty("T00")));
        Assert.assertEquals("1AM", TimexConvert.convertTimexToString(new TimexProperty("T01")));
        Assert.assertEquals("2AM", TimexConvert.convertTimexToString(new TimexProperty("T02")));
        Assert.assertEquals("3AM", TimexConvert.convertTimexToString(new TimexProperty("T03")));
        Assert.assertEquals("4AM", TimexConvert.convertTimexToString(new TimexProperty("T04")));
        Assert.assertEquals("midday", TimexConvert.convertTimexToString(new TimexProperty("T12")));
        Assert.assertEquals("1PM", TimexConvert.convertTimexToString(new TimexProperty("T13")));
        Assert.assertEquals("2PM", TimexConvert.convertTimexToString(new TimexProperty("T14")));
        Assert.assertEquals("11PM", TimexConvert.convertTimexToString(new TimexProperty("T23")));
    }

    @Test
    public void dataTypesConvertNow() {
        Assert.assertEquals("now", TimexConvert.convertTimexToString(new TimexProperty("PRESENT_REF")));
    }

    @Test
    public void dataTypesConvertFullDatetime() {
        Assert.assertEquals("6:30:45PM 3rd January 1984",
                TimexConvert.convertTimexToString(new TimexProperty("1984-01-03T18:30:45")));
        Assert.assertEquals("midnight 1st January 2000",
                TimexConvert.convertTimexToString(new TimexProperty("2000-01-01T00")));
        Assert.assertEquals("7:30PM 29th May 1967",
                TimexConvert.convertTimexToString(new TimexProperty("1967-05-29T19:30:00")));
    }

    @Test
    public void dataTypesConvertParticularTimeOnParticularDayOfWeek() {
        Assert.assertEquals("4PM Wednesday", TimexConvert.convertTimexToString(new TimexProperty("XXXX-WXX-3T16")));
        Assert.assertEquals("6:30PM Friday", TimexConvert.convertTimexToString(new TimexProperty("XXXX-WXX-5T18:30")));
    }

    @Test
    public void dataTypesConvertYear() {
        Assert.assertEquals("2016", TimexConvert.convertTimexToString(new TimexProperty("2016")));
    }

    @Test
    public void dataTypesConvertYearSeason() {
        Assert.assertEquals("summer 1999", TimexConvert.convertTimexToString(new TimexProperty("1999-SU")));
    }

    @Test
    public void dataTypesConvertSeason() {
        Assert.assertEquals("summer", TimexConvert.convertTimexToString(new TimexProperty("SU")));
        Assert.assertEquals("winter", TimexConvert.convertTimexToString(new TimexProperty("WI")));
    }

    @Test
    public void dataTypesConvertMonth() {
        Assert.assertEquals("January", TimexConvert.convertTimexToString(new TimexProperty("XXXX-01")));
        Assert.assertEquals("May", TimexConvert.convertTimexToString(new TimexProperty("XXXX-05")));
        Assert.assertEquals("December", TimexConvert.convertTimexToString(new TimexProperty("XXXX-12")));
    }

    @Test
    public void dataTypesConvertMonthAndYear() {
        Assert.assertEquals("May 2018", TimexConvert.convertTimexToString(new TimexProperty("2018-05")));
    }

    @Test
    public void dataTypesConvertWeekOfMonth() {
        Assert.assertEquals("first week of January",
                TimexConvert.convertTimexToString(new TimexProperty("XXXX-01-W01")));
        Assert.assertEquals("third week of August",
                TimexConvert.convertTimexToString(new TimexProperty("XXXX-08-W03")));
    }

    @Test
    public void dataTypesConvertPartOfTheDay() {
        Assert.assertEquals("daytime", TimexConvert.convertTimexToString(new TimexProperty("TDT")));
        Assert.assertEquals("night", TimexConvert.convertTimexToString(new TimexProperty("TNI")));
        Assert.assertEquals("morning", TimexConvert.convertTimexToString(new TimexProperty("TMO")));
        Assert.assertEquals("afternoon", TimexConvert.convertTimexToString(new TimexProperty("TAF")));
        Assert.assertEquals("evening", TimexConvert.convertTimexToString(new TimexProperty("TEV")));
    }

    @Test
    public void dataTypesConvertFridayEvening() {
        Assert.assertEquals("Friday evening", TimexConvert.convertTimexToString(new TimexProperty("XXXX-WXX-5TEV")));
    }

    @Test
    public void dataTypesConvertDateAndPartOfDay() {
        Assert.assertEquals("7th September 2017 night",
                TimexConvert.convertTimexToString(new TimexProperty("2017-09-07TNI")));
    }

    @Test
    public void dataTypesConvertLast5Minutes() {
        // date + time + duration
        TimexProperty timex = new TimexProperty("(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)");

        // TODO
    }

    @Test
    public void dataTypesConvertWednesdayToSaturday() {
        // date + duration
        TimexProperty timex = new TimexProperty("(XXXX-WXX-3,XXXX-WXX-6,P3D)");

        // TODO
    }

    @Test
    public void dataTypesConvertYears() {
        Assert.assertEquals("2 years", TimexConvert.convertTimexToString(new TimexProperty("P2Y")));
        Assert.assertEquals("1 year", TimexConvert.convertTimexToString(new TimexProperty("P1Y")));
    }

    @Test
    public void dataTypesConvertMonths() {
        Assert.assertEquals("4 months", TimexConvert.convertTimexToString(new TimexProperty("P4M")));
        Assert.assertEquals("1 month", TimexConvert.convertTimexToString(new TimexProperty("P1M")));
        Assert.assertEquals("0 months", TimexConvert.convertTimexToString(new TimexProperty("P0M")));
    }

    @Test
    public void dataTypesConvertWeeks() {
        Assert.assertEquals("6 weeks", TimexConvert.convertTimexToString(new TimexProperty("P6W")));
        Assert.assertEquals("9.5 weeks", TimexConvert.convertTimexToString(new TimexProperty("P9.5W")));
    }

    @Test
    public void dataTypesConvertDays() {
        Assert.assertEquals("5 days", TimexConvert.convertTimexToString(new TimexProperty("P5D")));
        Assert.assertEquals("1 day", TimexConvert.convertTimexToString(new TimexProperty("P1D")));
    }

    @Test
    public void dataTypesConvertHours() {
        Assert.assertEquals("5 hours", TimexConvert.convertTimexToString(new TimexProperty("PT5H")));
        Assert.assertEquals("1 hour", TimexConvert.convertTimexToString(new TimexProperty("PT1H")));
    }

    @Test
    public void dataTypesConvertMinutes() {
        Assert.assertEquals("30 minutes", TimexConvert.convertTimexToString(new TimexProperty("PT30M")));
        Assert.assertEquals("1 minute", TimexConvert.convertTimexToString(new TimexProperty("PT1M")));
    }

    @Test
    public void dataTypesConvertSeconds() {
        Assert.assertEquals("45 seconds", TimexConvert.convertTimexToString(new TimexProperty("PT45S")));
    }

    @Test
    public void dataTypesConvertEvery2Days() {
        Assert.assertEquals("every 2 days", TimexConvert.convertTimexSetToString(new TimexSet("P2D")));
    }

    @Test
    public void dataTypesConvertEveryWeek() {
        Assert.assertEquals("every week", TimexConvert.convertTimexSetToString(new TimexSet("P1W")));
    }

    @Test
    public void dataTypesConvertEveryOctober() {
        Assert.assertEquals("every October", TimexConvert.convertTimexSetToString(new TimexSet("XXXX-10")));
    }

    @Test
    public void dataTypesConvertEverySunday() {
        Assert.assertEquals("every Sunday", TimexConvert.convertTimexSetToString(new TimexSet("XXXX-WXX-7")));
    }

    @Test
    public void dataTypesConvertEveryDay() {
        Assert.assertEquals("every day", TimexConvert.convertTimexSetToString(new TimexSet("P1D")));
    }

    @Test
    public void dataTypesConvertEveryYear() {
        Assert.assertEquals("every year", TimexConvert.convertTimexSetToString(new TimexSet("P1Y")));
    }

    @Test
    public void dataTypesConvertEverySpring() {
        Assert.assertEquals("every spring", TimexConvert.convertTimexSetToString(new TimexSet("SP")));
    }

    @Test
    public void dataTypesConvertEveryWinter() {
        Assert.assertEquals("every winter", TimexConvert.convertTimexSetToString(new TimexSet("WI")));
    }

    @Test
    public void dataTypesConvertEveryEvening() {
        Assert.assertEquals("every evening", TimexConvert.convertTimexSetToString(new TimexSet("TEV")));
    }
}
