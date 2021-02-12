// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression.english;

import com.microsoft.recognizers.datatypes.timex.expression.Constants;
import com.microsoft.recognizers.datatypes.timex.expression.TimexInference;
import com.microsoft.recognizers.datatypes.timex.expression.TimexProperty;
import com.microsoft.recognizers.datatypes.timex.expression.TimexSet;

import java.math.BigDecimal;
import java.util.HashSet;

public class TimexConvertEnglish {
    public static String convertTimexToString(TimexProperty timex) {
        HashSet<String> types = timex.getTypes().size() != 0 ? timex.getTypes() : TimexInference.infer(timex);

        if (types.contains(Constants.TimexTypes.PRESENT)) {
            return TimexConstantsEnglish.NOW;
        }

        if (types.contains(Constants.TimexTypes.DATE_TIME_RANGE)) {
            return TimexConvertEnglish.convertDateTimeRange(timex);
        }

        if (types.contains(Constants.TimexTypes.DATE_RANGE)) {
            return TimexConvertEnglish.convertDateRange(timex);
        }

        if (types.contains(Constants.TimexTypes.DURATION)) {
            return TimexConvertEnglish.convertDuration(timex);
        }

        if (types.contains(Constants.TimexTypes.TIME_RANGE)) {
            return TimexConvertEnglish.convertTimeRange(timex);
        }

        // TODO: where appropriate delegate most the formatting delegate to
        // Date.toLocaleString(options)
        if (types.contains(Constants.TimexTypes.DATE_TIME)) {
            return TimexConvertEnglish.convertDateTime(timex);
        }

        if (types.contains(Constants.TimexTypes.DATE)) {
            return TimexConvertEnglish.convertDate(timex);
        }

        if (types.contains(Constants.TimexTypes.TIME)) {
            return TimexConvertEnglish.convertTime(timex);
        }

        return new String();
    }

    public static String convertTimexSetToString(TimexSet timexSet) {
        TimexProperty timex = timexSet.getTimex();
        if (timex.getTypes().contains(Constants.TimexTypes.DURATION)) {
            return String.format("%1$s %2$s", TimexConstantsEnglish.EVERY,
                    TimexConvertEnglish.convertTimexDurationToString(timex, false));
        } else {
            return String.format("%1$s %2$s", TimexConstantsEnglish.EVERY,
                    TimexConvertEnglish.convertTimexToString(timex));
        }
    }

    public static String convertTime(TimexProperty timex) {
        if (timex.getHour() == 0 && timex.getMinute() == 0 && timex.getSecond() == 0) {
            return TimexConstantsEnglish.MIDNIGHT;
        }

        if (timex.getHour() == 12 && timex.getMinute() == 0 && timex.getSecond() == 0) {
            return TimexConstantsEnglish.MIDDAY;
        }

        String hour = (timex.getHour() == 0) ? "12"
                : (timex.getHour() > 12) ? String.valueOf(timex.getHour() - 12) : String.valueOf(timex.getHour());
        String minute = (timex.getMinute() == 0 && timex.getSecond() == 0) ? new String()
                : Constants.TIME_TIMEX_CONNECTOR
                        + String.format("%1$2s", String.valueOf(timex.getMinute())).replace(' ', '0');
        String second = (timex.getSecond() == 0) ? new String()
                : Constants.TIME_TIMEX_CONNECTOR
                        + String.format("%1$2s", String.valueOf(timex.getSecond())).replace(' ', '0');
        String period = timex.getHour() < 12 ? Constants.AM : Constants.PM;

        return String.format("%1$s%2$s%3$s%4$s", hour, minute, second, period);
    }

    public static String convertDate(TimexProperty timex) {
        if (timex.getDayOfWeek() != null) {
            return TimexConstantsEnglish.DAYS[timex.getDayOfWeek() - 1];
        }

        String date = String.valueOf(timex.getDayOfMonth());

        String abbreviation = TimexConstantsEnglish.DATE_ABBREVIATION[Integer
                .parseInt(String.valueOf(date.charAt(date.length() - 1)))];

        if (timex.getMonth() != null) {
            String month = TimexConstantsEnglish.MONTHS[timex.getMonth() - 1];
            if (timex.getYear() != null) {
                return String.format("%1$s%2$s %3$s %4$s", date, abbreviation, month, timex.getYear()).trim();
            }
            return String.format("%1$s%2$s %3$s", date, abbreviation, month);
        }
        return date.concat(abbreviation);
    }

    private static String convertDurationPropertyToString(BigDecimal value, String property,
            Boolean includeSingleCount) {
        if (value.intValue() == 1) {
            return includeSingleCount ? "1 " + property : property;
        } else {
            return String.format("%1$s %2$s%3$s", value, property, Constants.TIME_DURATION_UNIT);
        }
    }

    private static String convertTimexDurationToString(TimexProperty timex, Boolean includeSingleCount) {
        String result = new String();
        if (timex.getYears() != null) {
            result += TimexConvertEnglish.convertDurationPropertyToString(timex.getYears(), Constants.YEAR_UNIT,
                    includeSingleCount);
        }

        if (timex.getMonths() != null) {
            result += TimexConvertEnglish.convertDurationPropertyToString(timex.getMonths(), Constants.MONTH_UNIT,
                    includeSingleCount);
        }

        if (timex.getWeeks() != null) {
            result += TimexConvertEnglish.convertDurationPropertyToString(timex.getWeeks(), Constants.WEEK_UNIT,
                    includeSingleCount);
        }

        if (timex.getDays() != null) {
            result += TimexConvertEnglish.convertDurationPropertyToString(timex.getDays(), Constants.DAY_UNIT,
                    includeSingleCount);
        }

        if (timex.getHours() != null) {
            result += TimexConvertEnglish.convertDurationPropertyToString(timex.getHours(), Constants.HOUR_UNIT,
                    includeSingleCount);
        }

        if (timex.getMinutes() != null) {
            result += TimexConvertEnglish.convertDurationPropertyToString(timex.getMinutes(), Constants.MINUTE_UNIT,
                    includeSingleCount);
        }

        if (timex.getSeconds() != null) {
            result += TimexConvertEnglish.convertDurationPropertyToString(timex.getSeconds(), Constants.SECOND_UNIT,
                    includeSingleCount);
        }

        return result;
    }

    private static String convertDuration(TimexProperty timex) {
        return TimexConvertEnglish.convertTimexDurationToString(timex, true);
    }

    private static String convertDateRange(TimexProperty timex) {
        String season = (timex.getSeason() != null) ? TimexConstantsEnglish.SEASONS.get(timex.getSeason())
                : new String();

        String year = (timex.getYear() != null) ? timex.getYear().toString() : new String();

        if (timex.getWeekOfYear() != null) {
            if (timex.getWeekend() != null) {
                throw new UnsupportedOperationException();
            }
        }

        if (timex.getMonth() != null) {
            String month = TimexConstantsEnglish.MONTHS[timex.getMonth() - 1];
            if (timex.getWeekOfMonth() != null) {
                return String.format("%1$s week of %2$s", TimexConstantsEnglish.WEEKS[timex.getWeekOfMonth() - 1],
                        month);
            } else {
                return String.format("%1$s %2$s", month, year).trim();
            }
        }

        return String.format("%1$s %2$s", season, year).trim();
    }

    private static String convertTimeRange(TimexProperty timex) {
        return TimexConstantsEnglish.DAY_PARTS.get(timex.getPartOfDay());
    }

    private static String convertDateTime(TimexProperty timex) {
        return String.format("%1$s %2$s", TimexConvertEnglish.convertTime(timex),
                TimexConvertEnglish.convertDate(timex));
    }

    private static String convertDateTimeRange(TimexProperty timex) {
        if (timex.getTypes().contains(Constants.TimexTypes.TIME_RANGE)) {
            return String.format("%1$s %2$s", TimexConvertEnglish.convertDate(timex),
                    TimexConvertEnglish.convertTimeRange(timex));
        }

        // date + time + duration
        // - OR -
        // date + duration
        return new String();
    }
}
