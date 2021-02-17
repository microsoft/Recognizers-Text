// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.HashMap;
import java.util.Map;
import java.util.Map.Entry;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class TimexRegex {
    private static final String DATE_TIME_COLLECTION_NAME = "datetime";
    private static final String DATE_COLLECTION_NAME = "date";
    private static final String TIME_COLLECTION_NAME = "time";
    private static final String PERIOD_COLLECTION_NAME = "period";

    private static Pattern[] DATE_COLLECTION_NAME_PATTERNS = {
            // date
            Pattern.compile("^(XXXX|(?<year>\\d\\d\\d\\d))-(?<month>\\d\\d)(-(?<dayOfMonth>\\d\\d))?"),
            Pattern.compile("^XXXX-WXX-(?<dayOfWeek>\\d)"),
            Pattern.compile("^XXXX-XX-(?<dayOfMonth>\\d\\d)"),

            // daterange
            Pattern.compile("^(?<year>\\d\\d\\d\\d)"),
            Pattern.compile("^(XXXX|(?<year>\\d\\d\\d\\d))-(?<month>\\d\\d)-W(?<weekOfMonth>\\d\\d)"),
            Pattern.compile("^(XXXX|(?<year>\\d\\d\\d\\d))-(?<month>\\d\\d)-WXX-(?<weekOfMonth>\\d{1,2})(-(?<dayOfWeek>\\d))?"),
            Pattern.compile("^(?<season>SP|SU|FA|WI)"),
            Pattern.compile("^(XXXX|(?<year>\\d\\d\\d\\d))-(?<season>SP|SU|FA|WI)"),
            Pattern.compile("^(XXXX|(?<year>\\d\\d\\d\\d))-W(?<weekOfYear>\\d\\d)(-(?<dayOfWeek>\\d)|-(?<weekend>WE))?"), };

    private static Pattern[] TIME_COLLECTION_NAME_PATTERNS = {
            // time
            Pattern.compile("T(?<hour>\\d\\d)Z?$"), Pattern.compile("T(?<hour>\\d\\d):(?<minute>\\d\\d)Z?$"),
            Pattern.compile("T(?<hour>\\d\\d):(?<minute>\\d\\d):(?<second>\\d\\d)Z?$"),

            // timerange
            Pattern.compile("^T(?<partOfDay>DT|NI|MO|AF|EV)$") };

    private static Pattern[] PERIOD_COLLECTION_NAME_PATTERNS = {
            Pattern.compile("^P(?<amount>\\d*\\.?\\d+)(?<dateUnit>Y|M|W|D)$"),
            Pattern.compile("^PT(?<hourAmount>\\d*\\.?\\d+)H(\\d*\\.?\\d+(M|S)){0,2}$"),
            Pattern.compile("^PT(\\d*\\.?\\d+H)?(?<minuteAmount>\\d*\\.?\\d+)M(\\d*\\.?\\d+S)?$"),
            Pattern.compile("^PT(\\d*\\.?\\d+(H|M)){0,2}(?<secondAmount>\\d*\\.?\\d+)S$"), };

    private static Map<String, Pattern[]> TIMEX_REGEX = new HashMap<String, Pattern[]>() {
        {
            put(DATE_COLLECTION_NAME, DATE_COLLECTION_NAME_PATTERNS);
            put(TIME_COLLECTION_NAME, TIME_COLLECTION_NAME_PATTERNS);
            put(PERIOD_COLLECTION_NAME, PERIOD_COLLECTION_NAME_PATTERNS);
        }
    };

    public static Boolean extract(String name, String timex, Map<String, String> result) {
        String lowerName = name.toLowerCase();
        String[] nameGroup = new String[lowerName == DATE_TIME_COLLECTION_NAME ? 2 : 1];

        if (lowerName == DATE_TIME_COLLECTION_NAME) {
            nameGroup[0] = DATE_COLLECTION_NAME;
            nameGroup[1] = TIME_COLLECTION_NAME;
        } else {
            nameGroup[0] = lowerName;
        }

        Boolean anyTrue = false;
        for (String nameItem : nameGroup) {
            for (Pattern entry : TIMEX_REGEX.get(nameItem)) {
                if (TimexRegex.tryExtract(entry, timex, result)) {
                    anyTrue = true;
                }
            }
        }

        return anyTrue;
    }

    private static Boolean tryExtract(Pattern regex, String timex, Map<String, String> result) {
        Matcher regexResult = regex.matcher(timex);
        if (!regexResult.find()) {
            return false;
        }

        Map<String, String> regexGroupNames = RegExpUtility.getNamedGroups(regexResult, true);

        for (Entry<String, String> entry : regexGroupNames.entrySet()) {
            result.put(entry.getKey(), entry.getValue());
        }

        return true;
    }
}
