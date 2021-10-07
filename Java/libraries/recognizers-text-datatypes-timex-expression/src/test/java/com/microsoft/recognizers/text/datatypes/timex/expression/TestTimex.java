// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datatypes.timex.expression;

import com.microsoft.recognizers.datatypes.timex.expression.Time;
import com.microsoft.recognizers.datatypes.timex.expression.TimexProperty;

import java.time.LocalDateTime;

import org.junit.Assert;
import org.junit.Test;

public class TestTimex {
    @Test
    public void dataTypesTimexFromDate() {
        LocalDateTime date = LocalDateTime.of(2017, 12, 5, 0, 0);
        Assert.assertEquals("2017-12-05", TimexProperty.fromDate(date).getTimexValue());
    }

    @Test
    public void dataTypesTimexFromDateTime() {
        LocalDateTime date = LocalDateTime.of(2017, 12, 5, 23, 57, 35);
        Assert.assertEquals("2017-12-05T23:57:35", TimexProperty.fromDateTime(date).getTimexValue());
    }

    @Test
    public void dataTypesTimexRoundtripDate() {
        TestTimex.roundtrip("2017-09-27");
        TestTimex.roundtrip("XXXX-WXX-3");
        TestTimex.roundtrip("XXXX-12-05");
    }

    @Test
    public void dataTypesTimexRoundtripTime() {
        TestTimex.roundtrip("T17:30:45");
        TestTimex.roundtrip("T05:06:07");
        TestTimex.roundtrip("T17:30");
        TestTimex.roundtrip("T23");
    }

    @Test
    public void dataTypesTimexRoundtripDuration() {
        TestTimex.roundtrip("P50Y");
        TestTimex.roundtrip("P6M");
        TestTimex.roundtrip("P3W");
        TestTimex.roundtrip("P5D");
        TestTimex.roundtrip("PT16H");
        TestTimex.roundtrip("PT32M");
        TestTimex.roundtrip("PT20S");
    }

    @Test
    public void dataTypesTimexRoundTripNow() {
        TestTimex.roundtrip("PRESENT_REF");
    }

    @Test
    public void dataTypesTimexRoundtripDateTime() {
        TestTimex.roundtrip("XXXX-WXX-3T04");
        TestTimex.roundtrip("2017-09-27T11:41:30");
    }

    @Test
    public void dataTypesTimeRoundtripDateRange() {
        TestTimex.roundtrip("2017");
        TestTimex.roundtrip("SU");
        TestTimex.roundtrip("2017-WI");
        TestTimex.roundtrip("2017-09");
        TestTimex.roundtrip("2017-W37");
        TestTimex.roundtrip("2017-W37-WE");
        TestTimex.roundtrip("XXXX-05");
    }

    @Test
    public void dataTypesTimexRoundtripDateRangeStartEndDuration() {
        TestTimex.roundtrip("(XXXX-WXX-3,XXXX-WXX-6,P3D)");
        TestTimex.roundtrip("(XXXX-01-01,XXXX-08-05,P216D)");
        TestTimex.roundtrip("(2017-01-01,2017-08-05,P216D)");
        TestTimex.roundtrip("(2016-01-01,2016-08-05,P217D)");
    }

    @Test
    public void dataTypesTimexRoundtripTimeRange() {
        TestTimex.roundtrip("TEV");
    }

    @Test
    public void dataTypesTimexRoundtripTimeRangeStartEndDuration() {
        TestTimex.roundtrip("(T16,T19,PT3H)");
    }

    @Test
    public void dataTypesTimexRoundtripDateTimeRange() {
        TestTimex.roundtrip("2017-09-27TEV");
    }

    @Test
    public void dataTypesTimexRoundtripDateTimeRangeStartEndDuration() {
        TestTimex.roundtrip("(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)");
        TestTimex.roundtrip("(XXXX-WXX-3T16,XXXX-WXX-6T15,PT71H)");
    }

    @Test
    public void dataTypesTimexToString() {
        Assert.assertEquals("5th May", new TimexProperty("XXXX-05-05").toString());
    }

    @Test
    public void dataTypesTimexToNaturalLanguage() {
        LocalDateTime today = LocalDateTime.of(2017, 10, 16, 0, 0);
        Assert.assertEquals("tomorrow", new TimexProperty("2017-10-17").toNaturalLanguage(today));
    }

    @Test
    public void dataTypesTimexFromTime() {
        Time time = new Time(23, 59, 30);
        Assert.assertEquals("T23:59:30", TimexProperty.fromTime(time).getTimexValue());
    }

    private static void roundtrip(String timex) {
        Assert.assertEquals(timex, new TimexProperty(timex).getTimexValue());
    }
}
