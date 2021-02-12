// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.util.HashSet;

public class TimexInference {
    public static HashSet<String> infer(TimexProperty timexProperty) {
        HashSet<String> types = new HashSet<String>();

        if (TimexInference.isPresent(timexProperty)) {
            types.add(Constants.TimexTypes.PRESENT);
        }

        if (TimexInference.isDefinite(timexProperty)) {
            types.add(Constants.TimexTypes.DEFINITE);
        }

        if (TimexInference.isDate(timexProperty)) {
            types.add(Constants.TimexTypes.DATE);
        }

        if (TimexInference.isDateRange(timexProperty)) {
            types.add(Constants.TimexTypes.DATE_RANGE);
        }

        if (TimexInference.isDuration(timexProperty)) {
            types.add(Constants.TimexTypes.DURATION);
        }

        if (TimexInference.isTime(timexProperty)) {
            types.add(Constants.TimexTypes.TIME);
        }

        if (TimexInference.isTimeRange(timexProperty)) {
            types.add(Constants.TimexTypes.TIME_RANGE);
        }

        if (types.contains(Constants.TimexTypes.PRESENT)) {
            types.add(Constants.TimexTypes.DATE);
            types.add(Constants.TimexTypes.TIME);
        }

        if (types.contains(Constants.TimexTypes.TIME) && types.contains(Constants.TimexTypes.DURATION)) {
            types.add(Constants.TimexTypes.TIME_RANGE);
        }

        if (types.contains(Constants.TimexTypes.DATE) && types.contains(Constants.TimexTypes.TIME)) {
            types.add(Constants.TimexTypes.DATE_TIME);
        }

        if (types.contains(Constants.TimexTypes.DATE) && types.contains(Constants.TimexTypes.DURATION)) {
            types.add(Constants.TimexTypes.DATE_RANGE);
        }

        if (types.contains(Constants.TimexTypes.DATE_TIME) && types.contains(Constants.TimexTypes.DURATION)) {
            types.add((Constants.TimexTypes.DATE_TIME_RANGE));
        }

        if (types.contains(Constants.TimexTypes.DATE) && types.contains(Constants.TimexTypes.TIME_RANGE)) {
            types.add(Constants.TimexTypes.DATE_TIME_RANGE);
        }

        return types;
    }

    private static Boolean isPresent(TimexProperty timexProperty) {
        return timexProperty.getNow() != null && timexProperty.getNow() == true;
    }

    private static Boolean isDuration(TimexProperty timexProperty) {
        return timexProperty.getYears() != null || timexProperty.getMonths() != null || timexProperty.getWeeks() != null ||
            timexProperty.getDays() != null | timexProperty.getHours() != null ||
            timexProperty.getMinutes() != null || timexProperty.getSeconds() != null;
    }

    private static Boolean isTime(TimexProperty timexProperty) {
        return timexProperty.getHour() != null && timexProperty.getMinute() != null && timexProperty.getSecond() != null;
    }

    private static Boolean isDate(TimexProperty timexProperty) {
        return timexProperty.getDayOfMonth() != null || timexProperty.getDayOfWeek() != null;
    }

    private static Boolean isTimeRange(TimexProperty timexProperty) {
        return timexProperty.getPartOfDay() != null;
    }

    private static Boolean isDateRange(TimexProperty timexProperty) {
        return (timexProperty.getDayOfMonth() == null && timexProperty.getDayOfWeek() == null) &&
                (timexProperty.getYear() != null || timexProperty.getMonth() != null ||
                        timexProperty.getSeason() != null || timexProperty.getWeekOfYear() != null ||
                        timexProperty.getWeekOfMonth() != null);
    }

    private static Boolean isDefinite(TimexProperty timexProperty) {
        return timexProperty.getYear() != null & timexProperty.getMonth() != null && timexProperty.getDayOfMonth() != null;
    }
}
