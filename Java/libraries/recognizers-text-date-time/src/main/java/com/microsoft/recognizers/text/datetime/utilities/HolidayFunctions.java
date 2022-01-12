// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.utilities;

import java.time.LocalDateTime;

public class HolidayFunctions {

    public static LocalDateTime calculateHolidayByEaster(int year) {
        return calculateHolidayByEaster(year, 0);
    }

    public static LocalDateTime calculateHolidayByEaster(int year, int days) {
        int day;
        int month = 3;

        int g = year % 19;
        int c = year / 100;
        int h = (c - (c / 4) - (((8 * c) + 13) / 25) + (19 * g) + 15) % 30;
        int i = h - ((h / 28) * (1 - ((h / 28) * (29 / (h + 1)) * ((21 - g) / 11))));

        day = i - ((year + (year / 4) + i + 2 - c + (c / 4)) % 7) + 28;

        if (day > 31) {
            month++;
            day -= 31;
        }

        return DateUtil.safeCreateFromMinValue(year, month, day).plusDays(days);
    }

    public static LocalDateTime calculateAdventDate(int year) {
        return calculateAdventDate(year, 0);
    }

    public static LocalDateTime calculateAdventDate(int year, int days) {
        LocalDateTime xmas = DateUtil.safeCreateFromMinValue(year, 12, 25);
        int weekday = xmas.getDayOfWeek().getValue();
        return xmas.minusDays(weekday).minusDays(days);
    }

}
