package com.microsoft.recognizers.text.datetime.utilities;

import java.time.LocalDateTime;

public class HolidayFunctions {

    public static LocalDateTime calculateHolidayByEaster(int year) {
        return calculateHolidayByEaster(year, 0);
    }

    public static LocalDateTime calculateHolidayByEaster(int year, int days) {

        int day = 0;
        int month = 3;

        int g = year % 19;
        int c = year / 100;
        int h = (c - (int)(c / 4) - (int)(((8 * c) + 13) / 25) + (19 * g) + 15) % 30;
        int i = h - ((int)(h / 28) * (1 - ((int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11))));

        day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;

        if (day > 31) {
            month++;
            day -= 31;
        }

        return DateUtil.safeCreateFromMinValue(year, month, day).plusDays(days);
    }

}
