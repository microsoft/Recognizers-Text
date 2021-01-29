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
        if (timexProperty.getYears() != null) {
            return new BigDecimal(31536000 * timexProperty.getYears().doubleValue()).toPlainString();
        }

        if (timexProperty.getMonths() != null) {
            return new BigDecimal(2592000 * timexProperty.getMonths().doubleValue()).toPlainString();
        }

        if (timexProperty.getWeeks() != null) {
            return new BigDecimal(604800 * timexProperty.getWeeks().doubleValue()).toPlainString();
        }

        if (timexProperty.getDays() != null) {
            return new BigDecimal(86400 * timexProperty.getDays().doubleValue()).toPlainString();
        }

        if (timexProperty.getHours() != null) {
            return new BigDecimal(3600 * timexProperty.getHours().doubleValue()).toPlainString();
        }

        if (timexProperty.getMinutes() != null) {
            return new BigDecimal(60 * timexProperty.getMinutes().doubleValue()).toPlainString();
        }

        if (timexProperty.getSeconds() != null) {
            return timexProperty.getSeconds().toPlainString();
        }

        return new String();
    }
}
