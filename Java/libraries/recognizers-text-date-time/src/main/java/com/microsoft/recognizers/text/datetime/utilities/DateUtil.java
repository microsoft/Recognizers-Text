package com.microsoft.recognizers.text.datetime.utilities;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeFormatterBuilder;
import java.time.format.DateTimeParseException;
import java.time.temporal.ChronoField;
import java.time.temporal.ChronoUnit;
import java.time.temporal.TemporalField;
import java.time.temporal.WeekFields;
import java.util.Locale;

public class DateUtil {

    public static LocalDateTime safeCreateFromValue(LocalDateTime datetime, int year, int month, int day, int hour, int minute, int second) {
        if (isValidDate(year, month, day) && isValidTime(hour, minute, second)) {
            datetime = safeCreateFromValue(datetime, year, month, day);
            datetime = datetime.plusHours(hour - datetime.getHour());
            datetime = datetime.plusMinutes((minute - datetime.getMinute()));
            datetime = datetime.plusSeconds(second - datetime.getSecond());
        }

        return datetime;
    }

    public static LocalDateTime safeCreateFromValue(LocalDateTime datetime, int year, int month, int day) {
        if (isValidDate(year, month, day)) {
            datetime = datetime.plusYears(year - datetime.getYear());
            datetime = datetime.plusMonths((month - datetime.getMonthValue()));
            datetime = datetime.plusDays(day - datetime.getDayOfMonth());
        }

        return datetime;
    }

    public static LocalDateTime safeCreateFromMinValue(int year, int month, int day) {
        return safeCreateFromValue(minValue(), year, month, day, 0, 0, 0);
    }

    public static LocalDateTime safeCreateFromMinValue(int year, int month, int day, int hour, int minute, int second) {
        return safeCreateFromValue(minValue(), year, month, day, hour, minute, second);
    }

    public static LocalDateTime safeCreateFromMinValue(LocalDate date, LocalTime time) {
        return safeCreateFromValue(minValue(),
            date.getYear(), date.getMonthValue(), date.getDayOfMonth(),
            time.getHour(), time.getMinute(), time.getSecond()
        );
    }

    public static LocalDateTime minValue() {
        return LocalDateTime.of(1, 1, 1, 0, 0, 0, 0);
    }

    public static Boolean isValidDate(int year, int month, int day) {
        if (year < 1 || year > 9999) {
            return false;
        }

        Integer[] validDays = {
            31,
            year % 4 == 0 && year % 100 != 0 || year % 400 == 0 ? 29 : 28,
            31,
            30,
            31,
            30,
            31,
            31,
            30,
            31,
            30,
            31
        };

        return month >= 1 && month <= 12 && day >= 1 && day <= validDays[month - 1];
    }

    public static boolean isValidTime(int hour, int minute, int second) {
        return 0 <= hour && hour <= 23 &&
                0 <= minute && minute <= 59 &&
                0 <= second && second <= 59;
    }

    public static boolean isDefaultValue(LocalDateTime date) {
        return date.equals(DateUtil.minValue());
    }

    private static final DateTimeFormatter DATE_TIME_FORMATTER = new DateTimeFormatterBuilder()
            .append(DateTimeFormatter.ofPattern("yyyy-MM-dd"))
            .parseDefaulting(ChronoField.HOUR_OF_DAY, 0)
            .parseDefaulting(ChronoField.MINUTE_OF_HOUR, 0)
            .parseDefaulting(ChronoField.SECOND_OF_MINUTE, 0)
            .toFormatter();

    public static LocalDateTime tryParse(String date) {
        try {
            return LocalDateTime.parse(date, DATE_TIME_FORMATTER);
        } catch (DateTimeParseException ex) {
            return null;
        }
    }

    public static LocalDateTime next(LocalDateTime from, int dayOfWeek) {
        int start = from.getDayOfWeek().getValue();

        if (start == 0) {
            start = 7;
        }

        if (dayOfWeek == 0) {
            dayOfWeek = 7;
        }

        return from.plusDays(dayOfWeek - start + 7);
    }

    public static LocalDateTime thisDate(LocalDateTime from, int dayOfWeek) {
        int start = from.getDayOfWeek().getValue();

        if (start == 0) {
            start = 7;
        }

        if (dayOfWeek == 0) {
            dayOfWeek = 7;
        }

        return from.plusDays(dayOfWeek - start);
    }

    public static LocalDateTime last(LocalDateTime from, int dayOfWeek) {
        int start = from.getDayOfWeek().getValue();

        if (start == 0) {
            start = 7;
        }

        if (dayOfWeek == 0) {
            dayOfWeek = 7;
        }

        return from.plusDays(dayOfWeek - start - 7);
    }

    public static LocalDateTime plusPeriodInNanos(LocalDateTime reference, double period, ChronoUnit unit) {
        long nanos = unit.getDuration().toNanos();
        return reference.plusNanos(Math.round(nanos * period));
    }

    public static int weekOfYear(LocalDateTime date) {
        TemporalField woy = WeekFields.ISO.weekOfYear();
        return date.get(woy);
    }
}
