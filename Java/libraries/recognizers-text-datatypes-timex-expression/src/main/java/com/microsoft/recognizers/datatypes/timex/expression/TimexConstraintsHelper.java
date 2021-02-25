// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.util.List;

public class TimexConstraintsHelper {
    public static List<TimeRange> collapseTimeRanges(List<TimeRange> ranges) {
        List<TimeRange> r = ranges;

        while (TimexConstraintsHelper.innerCollapseTimeRanges(r)) {

        }

        r.sort((a, b) -> a.getStart().getTime() - b.getStart().getTime());

        return r;
    }

    public static List<DateRange> collapseDateRanges(List<DateRange> ranges) {
        List<DateRange> r = ranges;

        while (TimexConstraintsHelper.innerCollapseDateRanges(r)) {

        }

        r.sort((a, b) -> a.getStart().compareTo(b.getStart()));
        return r;
    }

    public static Boolean isOverlapping(TimeRange r1, TimeRange r2) {
        return (r1.getEnd().getTime() > r2.getStart().getTime() && r1.getStart().getTime() <= r2.getStart().getTime()) ||
            (r1.getStart().getTime() < r2.getEnd().getTime() &&
            r1.getStart().getTime() >= r2.getStart().getTime());
    }

    private static Boolean isOverlapping(DateRange r1, DateRange r2) {
        return (r1.getEnd().isAfter(r2.getStart()) && (r1.getStart().isBefore(r2.getStart()) || r1.getStart().isEqual(r2.getStart()))) ||
            (r1.getStart().isBefore(r2.getEnd()) && (r1.getStart().isAfter(r2.getStart()) || r1.getStart().isEqual(r2.getStart())));
    }

    private static TimeRange collapseOverlapping(TimeRange r1, TimeRange r2) {
        return new TimeRange() {
            {
                setStart(new Time(Math.max(r1.getStart().getTime(), r2.getStart().getTime())));
                setEnd(new Time(Math.min(r1.getEnd().getTime(), r2.getEnd().getTime())));
            }
        };
    }

    private static DateRange collapseOverlapping(DateRange r1, DateRange r2) {
        return new DateRange() {
            {
                setStart(r1.getStart().compareTo(r2.getStart()) > 0 ? r1.getStart() : r2.getStart());
                setEnd(r1.getEnd().compareTo(r2.getEnd()) < 0 ? r1.getEnd() : r2.getEnd());
            }
        };
    }

    private static Boolean innerCollapseTimeRanges(List<TimeRange> ranges) {
        if (ranges.size() == 1) {
            return false;
        }

        for (int i = 0; i < ranges.size(); i++) {
            TimeRange r1 = ranges.get(i);
            for (int j = i + 1; j < ranges.size(); j++) {
                TimeRange r2 = ranges.get(j);
                if (TimexConstraintsHelper.isOverlapping(r1, r2)) {
                    ranges.subList(i, 1).clear();
                    ranges.subList(j - 1, 1).clear();
                    ranges.add(TimexConstraintsHelper.collapseOverlapping(r1, r2));
                    return true;
                }
            }
        }

        return false;
    }

    private static Boolean innerCollapseDateRanges(List<DateRange> ranges) {
        if (ranges.size() == 1) {
            return false;
        }

        for (int i = 0; i < ranges.size(); i++) {
            DateRange r1 = ranges.get(i);
            for (int j = i + 1; j < ranges.size(); j++) {
                DateRange r2 = ranges.get(j);
                if (TimexConstraintsHelper.isOverlapping(r1, r2)) {
                    ranges.subList(i, 1).clear();
                    ranges.subList(j - 1, 1).clear();
                    ranges.add(TimexConstraintsHelper.collapseOverlapping(r1, r2));
                    return true;
                }
            }
        }

        return false;
    }
}
