// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.util.HashMap;
import java.util.Map;

public class TimexParsing {
    public static void parseString(String timex, TimexProperty timexProperty) {
        // a reference to the present
        if (timex == "PRESENT_REF") {
            timexProperty.setNow(true);
        } else if (timex.startsWith("P")) {
            // duration
            TimexParsing.extractDuration(timex, timexProperty);
        } else if (timex.startsWith("(") && timex.endsWith(")")) {
            // range indicated with start and end dates and a duration
            TimexParsing.extractStartEndRange(timex, timexProperty);
        } else {
            // date andt ime and their respective ranges
            TimexParsing.extractDateTime(timex, timexProperty);
        }
    }

    private static void extractDuration(String s, TimexProperty timexProperty) {
        Map<String, String> extracted = new HashMap<String, String>();
        TimexRegex.extract("period", s, extracted);
        timexProperty.assignProperties(extracted);
    }

    private static void extractStartEndRange(String s, TimexProperty timexProperty) {
        String[] parts = s.substring(1, s.length() - 1).split(",");

        if (parts.length == 3) {
            TimexParsing.extractDateTime(parts[0], timexProperty);
            TimexParsing.extractDuration(parts[2], timexProperty);
        }
    }

    private static void extractDateTime(String s, TimexProperty timexProperty) {
        Integer indexOfT = s.indexOf("T");

        if (indexOfT == -1) {
            Map<String, String> extracted = new HashMap<String, String>();
            TimexRegex.extract("date", s, extracted);
            timexProperty.assignProperties(extracted);

        } else {
            Map<String, String> extracted = new HashMap<String, String>();
            TimexRegex.extract("date", s.substring(0, indexOfT), extracted);
            TimexRegex.extract("time", s.substring(indexOfT), extracted);
            timexProperty.assignProperties(extracted);
        }
    }
}
