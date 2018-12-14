package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.datetime.Constants;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.temporal.IsoFields;

public class DateTimeFormatUtil {

    private static final String dateDelimiter = "-";
    private static final String timeDelimiter = ":";

    public static String luisDate(int year) {

        if (year == Constants.InvalidYear) {
            return Constants.TimexFuzzyYear;
        }
        return String.format("%04d", year);
    }

    public static String luisDate(int year, int month) {
        if (year == Constants.InvalidYear) {
            if (month == Constants.InvalidMonth) {
                return String.join(Constants.DateTimexConnector, Constants.TimexFuzzyYear, Constants.TimexFuzzyMonth);
            }

            return String.join(Constants.DateTimexConnector, Constants.TimexFuzzyYear, String.format("%02d", month));
        }

        return String.join(Constants.DateTimexConnector, String.format("%04d", year), String.format("%02d", month));
    }

    public static String luisDate(Integer year, Integer month, Integer day) {

        if (year == -1) {

            if (month == -1) {

                if (day == -1) {
                    return String.join(Constants.DateTimexConnector, Constants.TimexFuzzyYear, Constants.TimexFuzzyMonth, Constants.TimexFuzzyDay);
                }

                return String.join(Constants.DateTimexConnector, Constants.TimexFuzzyYear, Constants.TimexFuzzyMonth, String.format("%02d", day));
            }

            return String.join(Constants.DateTimexConnector, Constants.TimexFuzzyYear, String.format("%02d", month), String.format("%02d", day));
        }

        return String.join(Constants.DateTimexConnector, String.format("%04d", year), String.format("%02d", month), String.format("%02d", day));
    }

    public static String luisDate(LocalDateTime date) {
        return luisDate(date, null);
    }

    public static String luisDate(LocalDateTime date, LocalDateTime alternativeDate) {
        int year = date.getYear();
        int month = date.getMonthValue();
        int day = date.getDayOfMonth();

        if (alternativeDate != null) {
            if (alternativeDate.getYear() != year) {
                year = -1;
            }

            if (alternativeDate.getMonthValue() != month) {
                month = -1;
            }

            if (alternativeDate.getDayOfMonth() != day) {
                day = -1;
            }
        }

        return luisDate(year, month, day);
    }

    public static String luisDateTime(LocalDateTime time) {
        return luisDate(time) + "T" + luisTime(time.getHour(), time.getMinute(), time.getSecond());
    }

    public static String luisTime(int hour, int min, int second) {
        return String.join(timeDelimiter, String.format("%02d", hour), String.format("%02d", min), String.format("%02d", second));
    }

    public static String formatDate(LocalDateTime date) {
        return String.join(dateDelimiter, String.format("%04d", date.getYear()), String.format("%02d", date.getMonthValue()), String.format("%02d", date.getDayOfMonth()));
    }

    public static String toIsoWeekTimex(LocalDateTime date) {
        int weekNum = LocalDate.of(date.getYear(), date.getMonthValue(), date.getDayOfMonth()).get(IsoFields.WEEK_OF_WEEK_BASED_YEAR);
        return String.format("%04d-W%02d", date.getYear(), weekNum);
    }
}