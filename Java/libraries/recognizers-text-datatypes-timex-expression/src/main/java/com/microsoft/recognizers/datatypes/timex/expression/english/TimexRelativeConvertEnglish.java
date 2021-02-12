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
                return TimexConstantsEnglish.TODAY;
            }

            LocalDateTime tomorrow = TimexDateHelpers.tomorrow(date);
            if (TimexDateHelpers.datePartEquals(timexDate, tomorrow)) {
                return TimexConstantsEnglish.TOMORROW;
            }

            LocalDateTime yesterday = TimexDateHelpers.yesterday(date);
            if (TimexDateHelpers.datePartEquals(timexDate, yesterday)) {
                return TimexConstantsEnglish.YESTERDAY;
            }

            if (TimexDateHelpers.isThisWeek(timexDate, date)) {
                return String.format("%1$s %2$s", TimexConstantsEnglish.THIS,
                        TimexRelativeConvertEnglish.getDateDay(timexDate.getDayOfWeek()));
            }

            if (TimexDateHelpers.isNextWeek(timexDate, date)) {
                return String.format("%1$s %2$s", TimexConstantsEnglish.NEXT,
                        TimexRelativeConvertEnglish.getDateDay(timexDate.getDayOfWeek()));
            }

            if (TimexDateHelpers.isLastWeek(timexDate, date)) {
                return String.format("%1$s %2$s", TimexConstantsEnglish.LAST,
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
                        return timex.getWeekend() != null ? String.format("%1$s %2$s", TimexConstantsEnglish.THIS, TimexConstantsEnglish.WEEKEND)
                                : String.format("%1$s %2$s", TimexConstantsEnglish.THIS, Constants.WEEK_UNIT);
                    }

                    if (thisWeek == timex.getWeekOfYear() + 1) {
                        return timex.getWeekend() != null ? String.format("%1$s %2$s", TimexConstantsEnglish.LAST, TimexConstantsEnglish.WEEKEND)
                                : String.format("%1$s %2$s", TimexConstantsEnglish.LAST, Constants.WEEK_UNIT);
                    }

                    if (thisWeek == timex.getWeekOfYear() - 1) {
                        return timex.getWeekend() != null ? String.format("%1$s %2$s", TimexConstantsEnglish.NEXT, TimexConstantsEnglish.WEEKEND)
                                : String.format("%1$s %2$s", TimexConstantsEnglish.NEXT, Constants.WEEK_UNIT);
                    }
                }

                if (timex.getMonth() != null) {
                    if (timex.getMonth() == date.getMonthValue()) {
                        return String.format("%1$s %2$s", TimexConstantsEnglish.THIS, Constants.MONTH_UNIT);
                    }

                    if (timex.getMonth() == date.getMonthValue() + 1) {
                        return String.format("%1$s %2$s", TimexConstantsEnglish.NEXT, Constants.MONTH_UNIT);
                    }

                    if (timex.getMonth() == date.getMonthValue() - 1) {
                        return String.format("%1$s %2$s", TimexConstantsEnglish.LAST, Constants.MONTH_UNIT);
                    }
                }

                return (timex.getSeason() != null) ? String.format("%1$s %2$s", TimexConstantsEnglish.THIS,
                                TimexConstantsEnglish.SEASONS.get(timex.getSeason()))
                        : String.format("%1$s %2$s", TimexConstantsEnglish.THIS, Constants.YEAR_UNIT);
            }

            if (timex.getYear() == year + 1) {
                return (timex.getSeason() != null) ? String.format("%1$s %2$s", TimexConstantsEnglish.NEXT,
                                TimexConstantsEnglish.SEASONS.get(timex.getSeason()))
                        : String.format("%1$s %2$s", TimexConstantsEnglish.NEXT, Constants.YEAR_UNIT);
            }

            if (timex.getYear() == year - 1) {
                return (timex.getSeason() != null) ? String.format("%1$s %2$s", TimexConstantsEnglish.LAST,
                                TimexConstantsEnglish.SEASONS.get(timex.getSeason()))
                        : String.format("%1$s %2$s", TimexConstantsEnglish.LAST, Constants.YEAR_UNIT);
            }
        }

        return new String();
    }

    private static String convertDateTimeRange(TimexProperty timex, LocalDateTime date) {
        if (timex.getYear() != null && timex.getMonth() != null && timex.getDayOfMonth() != null) {
            LocalDateTime timexDate = LocalDateTime.of(timex.getYear(), timex.getMonth(), timex.getDayOfMonth(), 0, 0);

            if (timex.getPartOfDay() != null) {
                if (TimexDateHelpers.datePartEquals(timexDate, date)) {
                    if (timex.getPartOfDay().equals(Constants.TIMEX_NIGHT)) {
                        return TimexConstantsEnglish.TONIGHT;
                    } else {
                        return String.format("%1$s %2$s", TimexConstantsEnglish.THIS,
                                TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay()));
                    }
                }

                LocalDateTime tomorrow = TimexDateHelpers.tomorrow(date);
                if (TimexDateHelpers.datePartEquals(timexDate, tomorrow)) {
                    return String.format("%1$s %2$s", TimexConstantsEnglish.TOMORROW,
                            TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay()));
                }

                LocalDateTime yesterday = TimexDateHelpers.yesterday(date);
                if (TimexDateHelpers.datePartEquals(timexDate, yesterday)) {
                    return String.format("%1$s %2$s", TimexConstantsEnglish.YESTERDAY,
                            TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay()));
                }

                if (TimexDateHelpers.isNextWeek(timexDate, date)) {
                    return String.format("%1$s %2$s %3$s", TimexConstantsEnglish.NEXT,
                            TimexRelativeConvertEnglish.getDateDay(timexDate.getDayOfWeek()),
                            TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay()));
                }

                if (TimexDateHelpers.isLastWeek(timexDate, date)) {
                    return String.format("%1$s %2$s", TimexConstantsEnglish.LAST,
                            TimexRelativeConvertEnglish.getDateDay(timexDate.getDayOfWeek()),
                            TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay()));
                }
            }
        }

        return new String();
    }
}
