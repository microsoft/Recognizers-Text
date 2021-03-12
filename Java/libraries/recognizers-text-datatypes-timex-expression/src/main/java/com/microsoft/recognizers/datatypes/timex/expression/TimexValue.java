// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.math.BigDecimal;
import java.time.LocalDateTime;

public class TimexValue {
    public static String dateValue(TimexProperty timexProperty) {
        if (timexProperty.getYear() != null && timexProperty.getMonth() != null && timexProperty.getDayOfMonth() != null) {
            return String.format("%1$s-%2$s-%3$s", TimexDateHelpers.fixedFormatNumber(timexProperty.getYear(), 4),
                    TimexDateHelpers.fixedFormatNumber(timexProperty.getMonth(), 2),
                    TimexDateHelpers.fixedFormatNumber(timexProperty.getDayOfMonth(), 2));
        }

        return new String();
    }

    public static String timeValue(TimexProperty timexProperty, LocalDateTime date) {
        if (timexProperty.getHour() != null && timexProperty.getMinute() != null && timexProperty.getSecond() != null) {
            return String.format("%1$s:%2$s:%3$s", TimexDateHelpers.fixedFormatNumber(timexProperty.getHour(), 2),
                    TimexDateHelpers.fixedFormatNumber(timexProperty.getMinute(), 2),
                    TimexDateHelpers.fixedFormatNumber(timexProperty.getSecond(), 2));
        }

        return new String();
    }

    public static String datetimeValue(TimexProperty timexProperty, LocalDateTime date) {
        return String.format("%1$s %2$s", TimexValue.dateValue(timexProperty),
                TimexValue.timeValue(timexProperty, date));
    }

    public static String durationValue(TimexProperty timexProperty) {
        BigDecimal duration = new BigDecimal(0);
        if (timexProperty.getYears() != null) {
            double value = 31536000 * ((timexProperty.getYears() != null) ? timexProperty.getYears().doubleValue() : 0);
            duration = duration.add(BigDecimal.valueOf(value));
        }

        if (timexProperty.getMonths() != null) {
            double value = 2592000
                    * ((timexProperty.getMonths() != null) ? timexProperty.getMonths().doubleValue() : 0);
            duration = duration.add(BigDecimal.valueOf(value));
        }

        if (timexProperty.getWeeks() != null) {
            double value = 604800 * ((timexProperty.getWeeks() != null) ? timexProperty.getWeeks().doubleValue() : 0);
            duration = duration.add(BigDecimal.valueOf(value));
        }

        if (timexProperty.getDays() != null) {
            double value = 86400 * ((timexProperty.getDays() != null) ? timexProperty.getDays().doubleValue() : 0);
            duration = duration.add(BigDecimal.valueOf(value));
        }

        if (timexProperty.getHours() != null) {
            double value = 3600 * ((timexProperty.getHours() != null) ? timexProperty.getHours().doubleValue() : 0);
            duration = duration.add(BigDecimal.valueOf(value));
        }

        if (timexProperty.getMinutes() != null) {
            double value = 60 * ((timexProperty.getMinutes() != null) ? timexProperty.getMinutes().doubleValue() : 0);
            duration = duration.add(BigDecimal.valueOf(value));
        }

        if (timexProperty.getSeconds() != null) {
            duration = duration.add(BigDecimal.valueOf((timexProperty.getSeconds() != null) ? timexProperty.getSeconds().doubleValue() : 0));
        }

        duration = BigDecimal.valueOf(duration.intValue());
        return duration.toPlainString();
    }
}
