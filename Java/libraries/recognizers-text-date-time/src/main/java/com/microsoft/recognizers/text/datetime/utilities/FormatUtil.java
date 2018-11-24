package com.microsoft.recognizers.text.datetime.utilities;

import java.time.Duration;
import java.time.LocalDateTime;
import java.time.temporal.ChronoUnit;

public class FormatUtil {

    private static final String dateDelimiter = "-";
    private static final String timeDelimiter = ":";

    public static String luisDate(Integer year, Integer month, Integer day) {
        if (year == -1) {
            if (month == -1) {
                if (day == -1) {
                    return String.join(dateDelimiter, "XXXX", "XX", "XX");
                }

                return String.join(dateDelimiter, "XXXX", "XX", String.format("%02d", day));
            }

            return String.join(dateDelimiter, "XXXX", String.format("%02d", month), String.format("%02d", day));
        }

        return String.join(dateDelimiter, String.format("%04d", year), String.format("%02d", month),
                String.format("%02d", day));
    }

    public static String luisDate(LocalDateTime date) {
        return luisDate(date.getYear(), date.getMonthValue(), date.getDayOfMonth());
    }

    public static String luisDateTime(LocalDateTime time) {
        return luisDate(time) + "T" + luisTime(time.getHour(), time.getMinute(), time.getSecond());
    }

    public static String luisTime(int hour, int min, int second) {
        return String.join(timeDelimiter, String.format("%02d", hour), String.format("%02d", min),
                String.format("%02d", second));
    }

    public static String luisTime(LocalDateTime time) {
        return luisTime(time.getHour(), time.getMinute(), time.getSecond());
    }

    public static String formatDate(LocalDateTime date) {
        return String.join(dateDelimiter, String.format("%04d", date.getYear()), String.format("%02d", date.getMonthValue()), String.format("%02d", date.getDayOfMonth()));
    }
    
    public static String formatTime(LocalDateTime time) {
        return String.join(timeDelimiter, String.format("%02d", time.getHour()), String.format("%02d", time.getMinute()), String.format("%02d", time.getSecond()));
    }

    public static String formatDateTime(LocalDateTime datetime) {
        return String.join(" ", formatDate(datetime), formatTime(datetime));
    }

    public static String shortTime(int hour, int min, int second) {
        if (min < 0 && second < 0) {
            return String.format("T%02d", hour);
        } else if (second < 0) {
            return String.format("T%02d:%02d", hour, min);
        }

        return String.format("T%02d:%02d:%02d", hour, min, second);
    }

    // Only handle TimeSpan which is less than one day
    public static String luisTimeSpan(Duration timeSpan) {
        String result = "PT";

        if (timeSpan.toHours() % 24 > 0) {
            result = String.format("%s%sH", result, timeSpan.toHours() % 24);
        }

        if (timeSpan.toMinutes() % 60 > 0) {
            result = String.format("%s%sM", result, timeSpan.toMinutes() % 60);
        }

        if (timeSpan.get(ChronoUnit.SECONDS) % 60 > 0) {
            result = String.format("%s%sS", result, timeSpan.get(ChronoUnit.SECONDS) % 60);
        }

        if (timeSpan.toMinutes() % 60 > 0) {
            result = String.format("%s%sM", result, timeSpan.toMinutes() % 60);
        }

        if (timeSpan.get(ChronoUnit.SECONDS) % 60 > 0) {
            result = String.format("%s%sS", result, timeSpan.get(ChronoUnit.SECONDS) % 60);
        }

        return timeSpan.toString();
    }
}
