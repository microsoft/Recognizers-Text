// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.math.BigDecimal;
import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Locale;

public class TimexFormat {
    public static String format(TimexProperty timex) {
        HashSet<String> types = timex.getTypes().size() != 0 ? timex.getTypes() : TimexInference.infer(timex);

        if (types.contains(Constants.TimexTypes.PRESENT)) {
            return "PRESENT_REF";
        }

        if ((types.contains(Constants.TimexTypes.DATE_TIME_RANGE) || types.contains(Constants.TimexTypes.DATE_RANGE) ||
            types.contains(Constants.TimexTypes.TIME_RANGE)) && types.contains(Constants.TimexTypes.DURATION)) {
            TimexRange range = TimexHelpers.expandDateTimeRange(timex);
            return String.format("(%1$s,%2$s,%3$s)", TimexFormat.format(range.getStart()),
                    TimexFormat.format(range.getEnd()), TimexFormat.format(range.getDuration()));
        }

        if (types.contains(Constants.TimexTypes.DATE_TIME_RANGE)) {
            return String.format("%1$s%2$s", TimexFormat.formatDate(timex), TimexFormat.formatTimeRange(timex));
        }

        if (types.contains(Constants.TimexTypes.DATE_RANGE)) {
            return TimexFormat.formatDateRange(timex);
        }

        if (types.contains(Constants.TimexTypes.TIME_RANGE)) {
            return TimexFormat.formatTimeRange(timex);
        }

        if (types.contains(Constants.TimexTypes.DATE_TIME)) {
            return String.format("%1$s%2$s", TimexFormat.formatDate(timex), TimexFormat.formatTime(timex));
        }

        if (types.contains(Constants.TimexTypes.DURATION)) {
            return TimexFormat.formatDuration(timex);
        }

        if (types.contains(Constants.TimexTypes.DATE)) {
            return TimexFormat.formatDate(timex);
        }

        if (types.contains(Constants.TimexTypes.TIME)) {
            return TimexFormat.formatTime(timex);
        }

        return new String();
    }

    private static String formatDuration(TimexProperty timex) {
        List<String> timexList = new ArrayList<String>();
        NumberFormat nf = NumberFormat.getInstance(Locale.getDefault());

        if (timex.getYears() != null) {
            nf.setMaximumFractionDigits(timex.getYears().scale());
            timexList.add(TimexHelpers.generateDurationTimex(TimexUnit.Year,
                    timex.getYears() != null ? timex.getYears() : BigDecimal.valueOf(Constants.INVALID_VALUE)));
        }

        if (timex.getMonths() != null) {
            nf.setMaximumFractionDigits(timex.getMonths().scale());
            timexList.add(TimexHelpers.generateDurationTimex(TimexUnit.Month,
                    timex.getMonths() != null ? timex.getMonths() : BigDecimal.valueOf(Constants.INVALID_VALUE)));
        }

        if (timex.getWeeks() != null) {
            nf.setMaximumFractionDigits(timex.getWeeks().scale());
            timexList.add(TimexHelpers.generateDurationTimex(TimexUnit.Week,
                    timex.getWeeks() != null ? timex.getWeeks() : BigDecimal.valueOf(Constants.INVALID_VALUE)));
        }

        if (timex.getDays() != null) {
            nf.setMaximumFractionDigits(timex.getDays().scale());
            timexList.add(TimexHelpers.generateDurationTimex(TimexUnit.Day,
                    timex.getDays() != null ? timex.getDays() : BigDecimal.valueOf(Constants.INVALID_VALUE)));
        }

        if (timex.getHours() != null) {
            nf.setMaximumFractionDigits(timex.getHours().scale());
            timexList.add(TimexHelpers.generateDurationTimex(TimexUnit.Hour,
                    timex.getHours() != null ? timex.getHours() : BigDecimal.valueOf(Constants.INVALID_VALUE)));
        }

        if (timex.getMinutes() != null) {
            nf.setMaximumFractionDigits(timex.getMinutes().scale());
            timexList.add(TimexHelpers.generateDurationTimex(TimexUnit.Minute,
                    timex.getMinutes() != null ? timex.getMinutes() : BigDecimal.valueOf(Constants.INVALID_VALUE)));
        }

        if (timex.getSeconds() != null) {
            nf.setMaximumFractionDigits(timex.getSeconds().scale());
            timexList.add(TimexHelpers.generateDurationTimex(TimexUnit.Second,
                    timex.getSeconds() != null ? timex.getSeconds() : BigDecimal.valueOf(Constants.INVALID_VALUE)));
        }

        return TimexHelpers.generateCompoundDurationTimex(timexList);
    }

    private static String formatTime(TimexProperty timex) {
        if (timex.getMinute() == 0 && timex.getSecond() == 0) {
            return String.format("T%s", TimexDateHelpers.fixedFormatNumber(timex.getHour(), 2));
        }

        if (timex.getSecond() == 0) {
            return String.format("T%1$s:%2$s", TimexDateHelpers.fixedFormatNumber(timex.getHour(), 2),
                    TimexDateHelpers.fixedFormatNumber(timex.getMinute(), 2));
        }

        return String.format("T%1$s:%2$s:%3$s", TimexDateHelpers.fixedFormatNumber(timex.getHour(), 2),
                TimexDateHelpers.fixedFormatNumber(timex.getMinute(), 2),
                TimexDateHelpers.fixedFormatNumber(timex.getSecond(), 2));
    }

    private static String formatDate(TimexProperty timex) {
        Integer year = timex.getYear() != null ? timex.getYear() : Constants.INVALID_VALUE;
        Integer month = timex.getWeekOfYear() != null ? timex.getWeekOfYear()
                : (timex.getMonth() != null ? timex.getMonth() : Constants.INVALID_VALUE);
        Integer day = timex.getDayOfWeek() != null ? timex.getDayOfWeek()
                : timex.getDayOfMonth() != null ? timex.getDayOfMonth() : Constants.INVALID_VALUE;
        Integer weekOfMonth = timex.getWeekOfMonth() != null ? timex.getWeekOfMonth() : Constants.INVALID_VALUE;

        return TimexHelpers.generateDateTimex(year, month, day, weekOfMonth, timex.getDayOfWeek() != null);
    }

    private static String formatDateRange(TimexProperty timex) {
        if (timex.getYear() != null && timex.getWeekOfYear() != null && timex.getWeekend() != null) {
            return String.format("%1$s-W%2$s-WE", TimexDateHelpers.fixedFormatNumber(timex.getYear(), 4),
                    TimexDateHelpers.fixedFormatNumber(timex.getWeekOfYear(), 2));
        }

        if (timex.getYear() != null && timex.getWeekOfYear() != null) {
            return String.format("%1$s-W%2$s", TimexDateHelpers.fixedFormatNumber(timex.getYear(), 4),
                    TimexDateHelpers.fixedFormatNumber(timex.getWeekOfYear(), 2));
        }

        if (timex.getYear() != null && timex.getMonth() != null && timex.getWeekOfMonth() != null) {
            return String.format("%1$s-%2$s-W%3$s", TimexDateHelpers.fixedFormatNumber(timex.getYear(), 4),
                    TimexDateHelpers.fixedFormatNumber(timex.getMonth(), 2),
                    TimexDateHelpers.fixedFormatNumber(timex.getWeekOfMonth(), 2));
        }

        if (timex.getYear() != null && timex.getSeason() != null) {
            return String.format("%1$s-%2$s", TimexDateHelpers.fixedFormatNumber(timex.getYear(), 4),
                    timex.getSeason());
        }

        if (timex.getSeason() != null) {
            return timex.getSeason();
        }

        if (timex.getYear() != null && timex.getMonth() != null) {
            return String.format("%1$s-%2$s", TimexDateHelpers.fixedFormatNumber(timex.getYear(), 4),
                    TimexDateHelpers.fixedFormatNumber(timex.getMonth(), 2));
        }

        if (timex.getYear() != null) {
            return TimexDateHelpers.fixedFormatNumber(timex.getYear(), 4);
        }

        if (timex.getMonth() != null && timex.getWeekOfMonth() != null && timex.getDayOfWeek() != null) {
            return String.format("%1$s-%2$s-%3$s-%4$s-%5$s", Constants.TIMEX_FUZZY_YEAR,
                    TimexDateHelpers.fixedFormatNumber(timex.getMonth(), 2), Constants.TIMEX_FUZZY_WEEK,
                    timex.getWeekOfMonth(), timex.getDayOfWeek());
        }

        if (timex.getMonth() != null && timex.getWeekOfMonth() != null) {
            return String.format("%1$s-%2$s-W%3$02d", Constants.TIMEX_FUZZY_YEAR,
                    TimexDateHelpers.fixedFormatNumber(timex.getMonth(), 2), timex.getWeekOfMonth());
        }

        if (timex.getMonth() != null) {
            return String.format("%1$s-%2$s", Constants.TIMEX_FUZZY_YEAR,
                    TimexDateHelpers.fixedFormatNumber(timex.getMonth(), 2));
        }

        return new String();
    }

    private static String formatTimeRange(TimexProperty timex) {
        if (timex.getPartOfDay() != null) {
            return String.format("T%s", timex.getPartOfDay());
        }

        return new String();
    }
}
