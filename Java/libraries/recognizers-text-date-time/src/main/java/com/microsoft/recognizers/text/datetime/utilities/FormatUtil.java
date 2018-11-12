package com.microsoft.recognizers.text.datetime.utilities;

import java.time.LocalDateTime;

public class FormatUtil {

    private static final String dateDelimiter = "-";
    private static final String timeDelimiter = ":";

    public static String luisDate(Integer year, Integer month, Integer day)
    {
        if (year == -1)
        {
            if (month == -1)
            {
                if (day == -1)
                {
                    return String.join(dateDelimiter, "XXXX", "XX", "XX");
                }

                return String.join(dateDelimiter, "XXXX", "XX", String.format("%02d", day));
            }

            return String.join(dateDelimiter, "XXXX", String.format("%02d", month), String.format("%02d", day));
        }

        return String.join(dateDelimiter, String.format("%04d", year), String.format("%02d", month), String.format("%02d", day));
    }

    public static String luisDate(LocalDateTime date) {
        return luisDate(date.getYear(), date.getMonthValue(), date.getDayOfMonth());
    }

    public static String luisDateTime(LocalDateTime time) {
        return luisDate(time) + "T" + luisTime(time.getHour(), time.getMinute(), time.getSecond());
    }

    public static String luisTime(int hour, int min, int second) {
        return String.join(timeDelimiter, String.format("%02d", hour), String.format("%02d", min), String.format("%02d", second));
    }

    public static String formatDate(LocalDateTime date)
    {
        return String.join(dateDelimiter, String.format("%04d", date.getYear()),  String.format("%02d", date.getMonthValue()),  String.format("%02d", date.getDayOfMonth()));
    }

    public static String formatTime(LocalDateTime time)
    {
        return String.join(timeDelimiter, String.format("%02d", time.getHour()),  String.format("%02d", time.getMinute()),  String.format("%02d", time.getSecond()));
    }
}
