// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression.english;

import java.util.HashMap;
import java.util.Map;

public class TimexConstantsEnglish {
    public static final String[] DAYS = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday",
        "Sunday" };

    public static final String[] MONTHS = { "January", "February", "March", "April", "May", "June", "July", "August",
        "September", "October", "November", "December", };

    public static final String[] DATE_ABBREVIATION = { "th", "st", "nd", "rd", "th", "th", "th", "th", "th", "th", };

    public static final String[] HOURS = { "midnight", "1AM", "2AM", "3AM", "4AM", "5AM", "6AM", "7AM", "8AM", "9AM",
        "10AM", "11AM", "midday", "1PM", "2PM", "3PM", "4PM", "5PM", "6PM", "7PM", "8PM", "9PM", "10PM", "11PM", };

    public static final Map<String, String> SEASONS = new HashMap<String, String>() {
        {
            put("SP", "spring");
            put("SU", "summer");
            put("FA", "fall");
            put("WI", "winter");
        }
    };

    public static final String[] WEEKS = { "first", "second", "third", "forth", };

    public static final Map<String, String> DAY_PARTS = new HashMap<String, String>() {
        {
            put("DT", "daytime");
            put("NI", "night");
            put("MO", "morning");
            put("AF", "afternoon");
            put("EV", "evening");
        }
    };
}
