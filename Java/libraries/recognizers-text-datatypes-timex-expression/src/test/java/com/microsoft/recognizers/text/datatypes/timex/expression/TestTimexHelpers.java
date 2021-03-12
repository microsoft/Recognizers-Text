// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datatypes.timex.expression;

import com.microsoft.recognizers.datatypes.timex.expression.DateRange;
import com.microsoft.recognizers.datatypes.timex.expression.Time;
import com.microsoft.recognizers.datatypes.timex.expression.TimeRange;
import com.microsoft.recognizers.datatypes.timex.expression.TimexHelpers;
import com.microsoft.recognizers.datatypes.timex.expression.TimexProperty;
import com.microsoft.recognizers.datatypes.timex.expression.TimexRange;

import java.time.LocalDateTime;

import org.junit.Assert;
import org.junit.Test;

public class TestTimexHelpers {
    
    @Test
    public void dataTypesHelpersExpandDateTimeRangeShort() {
        TimexProperty timex = new TimexProperty("(2017-09-27,2017-09-29,P2D)");
        TimexRange range = TimexHelpers.expandDateTimeRange(timex);
        Assert.assertEquals("2017-09-27", range.getStart().getTimexValue());
        Assert.assertEquals("2017-09-29", range.getEnd().getTimexValue());
    }

    @Test
    public void dataTypesHelpersExpandDateTimeRangeLong() {
        TimexProperty timex = new TimexProperty("(2006-01-01,2008-06-01,P882D)");
        TimexRange range = TimexHelpers.expandDateTimeRange(timex);
        Assert.assertEquals("2006-01-01", range.getStart().getTimexValue());
        Assert.assertEquals("2008-06-01", range.getEnd().getTimexValue());
    }

    @Test
    public void dataTypesHelpersExpandDateTimeRangeIncludeTime() {
        TimexProperty timex = new TimexProperty("(2017-10-10T16:02:04,2017-10-10T16:07:04,PT5M)");
        TimexRange range = TimexHelpers.expandDateTimeRange(timex);
        Assert.assertEquals("2017-10-10T16:02:04", range.getStart().getTimexValue());
        Assert.assertEquals("2017-10-10T16:07:04", range.getEnd().getTimexValue());
    }

    @Test
    public void dataTypesHelpersExpandDateTimeRangeMonth() {
        TimexProperty timex = new TimexProperty("2017-05");
        TimexRange range = TimexHelpers.expandDateTimeRange(timex);
        Assert.assertEquals("2017-05-01", range.getStart().getTimexValue());
        Assert.assertEquals("2017-06-01", range.getEnd().getTimexValue());
    }

    @Test
    public void dataTypesHelpersExpandDateTimeRangeYear() {
        TimexProperty timex = new TimexProperty("1999");
        TimexRange range = TimexHelpers.expandDateTimeRange(timex);
        Assert.assertEquals("1999-01-01", range.getStart().getTimexValue());
        Assert.assertEquals("2000-01-01", range.getEnd().getTimexValue());
    }

    @Test
    public void dataTypesHelpersExpandTimeRange() {
        TimexProperty timex = new TimexProperty("(T14,T16,PT2H)");
        TimexRange range = TimexHelpers.expandTimeRange(timex);
        Assert.assertEquals("T14", range.getStart().getTimexValue());
        Assert.assertEquals("T16", range.getEnd().getTimexValue());
    }

    @Test
    public void dataTypesHelpersDateRangeFromTimex() {
        TimexProperty timex = new TimexProperty("(2017-09-27,2017-09-29,P2D)");
        DateRange range = TimexHelpers.dateRangeFromTimex(timex);

        LocalDateTime dateExpected = LocalDateTime.of(2017, 9, 27,0,0);
        Assert.assertEquals(dateExpected, range.getStart());

        dateExpected = LocalDateTime.of(2017, 9, 29,0,0);
        Assert.assertEquals(dateExpected, range.getEnd());
    }

    @Test
    public void dataTypesHelpersDateRangeFromTimexWeek23() {
        TimexProperty timex = new TimexProperty("2020-W23");
        DateRange range = TimexHelpers.dateRangeFromTimex(timex);

        LocalDateTime dateExpected = LocalDateTime.of(2020, 6, 1, 0, 0);
        Assert.assertEquals(dateExpected, range.getStart());

        dateExpected = LocalDateTime.of(2020, 6, 8, 0, 0);
        Assert.assertEquals(dateExpected, range.getEnd());
    }

    @Test
    public void dataTypesHelpersTimeRangeFromTimex() {
        TimexProperty timex = new TimexProperty("(T14,T16,PT2H)");
        TimeRange range = TimexHelpers.timeRangeFromTimex(timex);
        Assert.assertEquals(new Time(14, 0, 0).getTime(), range.getStart().getTime());
        Assert.assertEquals(new Time(16, 0, 0).getTime(), range.getEnd().getTime());
    }

    @Test
    public void dataTypesHelpersDateFromTimex() {
        TimexProperty timex = new TimexProperty("2017-09-27");
        LocalDateTime date = TimexHelpers.dateFromTimex(timex);

        LocalDateTime dateExpected = LocalDateTime.of(2017, 9, 27,0,0);
        Assert.assertEquals(dateExpected, date);
    }

    @Test
    public void dataTypesHelpersTimeFromTimex() {
        TimexProperty timex = new TimexProperty("T00:05:00");
        Time time = TimexHelpers.timeFromTimex(timex);
        Assert.assertEquals(new Time(0, 5, 0).getTime(), time.getTime());
    }
}
