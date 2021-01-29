// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression.english;

import com.microsoft.recognizers.datatypes.timex.expression.Constants;
import com.microsoft.recognizers.datatypes.timex.expression.TimexConvert;
import com.microsoft.recognizers.datatypes.timex.expression.TimexDateHelpers;
import com.microsoft.recognizers.datatypes.timex.expression.TimexInference;
import com.microsoft.recognizers.datatypes.timex.expression.TimexProperty;

import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.util.HashSet;

public class TimexRelativeConvertEnglish {
    public static String convertTimexToStringRelative(TimexProperty timex, LocalDateTime date) {
        HashSet<String> types = timex.getTypes().size() != 0 ? timex.getTypes() : TimexInference.infer(timex);

        if (types.contains(Constants.TimexTypes.DATE_TIME_RANGE)) {
            return TimexRelativeConvertEnglish.convertDateTimeRange(timex, date);
        }

        if (types.contains(Constants.TimexTypes.DATE_RANGE)) {
            return TimexRelativeConvertEnglish.convertDateRange(timex, date);
        }

        if (types.contains(Constants.TimexTypes.DATE_TIME)) {
            return TimexRelativeConvertEnglish.convertDateTime(timex, date);
        }

        if (types.contains(Constants.TimexTypes.DATE)) {
            return TimexRelativeConvertEnglish.convertDate(timex, date);
        }

        return TimexConvert.convertTimexToString(timex);
    }

    private static String getDateDay(DayOfWeek day) {
        Integer index = (day.getValue() == 0) ? 6 : day.getValue() - 1;
        return TimexConstantsEnglish.DAYS[index];
    }

    private static String convertDate(TimexProperty timex, LocalDateTime date) {
        if (timex.getYear() != null && timex.getMonth() != null && timex.getDayOfMonth() != null) {
            LocalDateTime timexDate = LocalDateTime.of(timex.getYear(), timex.getMonth(), timex.getDayOfMonth(), 0, 0);
            if (TimexDateHelpers.datePartEquals(timexDate, date)) {
                return "today";
            }

            LocalDateTime tomorrow = TimexDateHelpers.tomorrow(date);
            if (TimexDateHelpers.datePartEquals(timexDate, tomorrow)) {
                return "tomorrow";
            }

            LocalDateTime yesterday = TimexDateHelpers.yesterday(date);
            if (TimexDateHelpers.datePartEquals(timexDate, yesterday)) {
                return "yesterday";
            }

            if (TimexDateHelpers.isThisWeek(timexDate, date)) {
                return String.format("this %s",
                        TimexRelativeConvertEnglish.getDateDay(timexDate.getDayOfWeek()));
            }

            if (TimexDateHelpers.isNextWeek(timexDate, date)) {
                return String.format("next %s",
                        TimexRelativeConvertEnglish.getDateDay(timexDate.getDayOfWeek()));
            }

            if (TimexDateHelpers.isLastWeek(timexDate, date)) {
                return String.format("last %s",
                        TimexRelativeConvertEnglish.getDateDay(timexDate.getDayOfWeek()));
            }
        }

        return TimexConvertEnglish.convertDate(timex);
    }

    private static String convertDateTime(TimexProperty timex, LocalDateTime date) {
        return String.format("%1$s %2$s", TimexRelativeConvertEnglish.convertDate(timex, date),
                TimexConvertEnglish.convertTime(timex));
    }

    private static String convertDateRange(TimexProperty timex, LocalDateTime date) {
        if (timex.getYear() != null) {
            int year = date.getYear();
            if (timex.getYear() == year) {
                if (timex.getWeekOfYear() != null) {
                    Integer thisWeek = TimexDateHelpers.weekOfYear(date);
                    if (thisWeek == timex.getWeekOfYear()) {
                        return timex.getWeekend() != null ? "this weekend" : "this week";
                    }

                    if (thisWeek == timex.getWeekOfYear() + 1) {
                        return timex.getWeekend() != null ? "last weekend" : "last week";
                    }

                    if (thisWeek == timex.getWeekOfYear() - 1) {
                        return timex.getWeekend() != null ? "next weekend" : "next week";
                    }
                }

                if (timex.getMonth() != null) {
                    if (timex.getMonth() == date.getMonthValue()) {
                        return "this month";
                    }

                    if (timex.getMonth() == date.getMonthValue() + 1) {
                        return "next month";
                    }

                    if (timex.getMonth() == date.getMonthValue() - 1) {
                        return "last month";
                    }
                }


                return (timex.getSeason() != null) ? String.format("this %s", TimexConstantsEnglish.SEASONS.get(timex.getSeason()))
                            : "this year";
            }

            if (timex.getYear() == year + 1) {
                return (timex.getSeason() != null) ? String.format("next %s", TimexConstantsEnglish.SEASONS.get(timex.getSeason()))
                    : "next year";
            }

            if (timex.getYear() == year - 1) {
                return (timex.getSeason() != null) ? String.format("last %s", TimexConstantsEnglish.SEASONS.get(timex.getSeason()))
                    : "last year";
            }
        }

        return new String();
    }

    private static String convertDateTimeRange(TimexProperty timex, LocalDateTime date) {
        if (timex.getYear() != null && timex.getMonth() != null && timex.getDayOfMonth() != null) {
            LocalDateTime timexDate = LocalDateTime.of(timex.getYear(), timex.getMonth(), timex.getDayOfMonth(), 0,0);

            if (timex.getPartOfDay() != null) {
                if (TimexDateHelpers.datePartEquals(timexDate, date)) {
                    if (timex.getPartOfDay().equals("NI")) {
                        return "tonight";
                    } else {
                        return String.format("this %s", TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay()));
                    }
                }

                LocalDateTime tomorrow = TimexDateHelpers.tomorrow(date);
                if (TimexDateHelpers.datePartEquals(timexDate, tomorrow)) {
                    return String.format("tomorrow %s", TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay()));
                }

                LocalDateTime yesterday = TimexDateHelpers.yesterday(date);
                if (TimexDateHelpers.datePartEquals(timexDate, yesterday)) {
                    return String.format("yesterday %s", TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay()));
                }

                if (TimexDateHelpers.isNextWeek(timexDate, date)) {
                    return String.format("next %1$s %2$s",
                            TimexRelativeConvertEnglish.getDateDay(timexDate.getDayOfWeek()),
                            TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay()));
                }

                if (TimexDateHelpers.isLastWeek(timexDate, date)) {
                    return String.format("last %1$s %2$s",
                            TimexRelativeConvertEnglish.getDateDay(timexDate.getDayOfWeek()),
                            TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay()));
                }
            }
        }

        return new String();
    }
}
