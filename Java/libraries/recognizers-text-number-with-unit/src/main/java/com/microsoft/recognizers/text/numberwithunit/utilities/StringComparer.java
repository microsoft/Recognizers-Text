package com.microsoft.recognizers.text.numberwithunit.utilities;

import com.microsoft.recognizers.text.utilities.StringUtility;
import java.util.Comparator;

public class StringComparer implements Comparator<String> {
    @Override
    public int compare(String stringA, String stringB) {
        if (StringUtility.isNullOrEmpty(stringA) && StringUtility.isNullOrEmpty(stringB)) {
            return 0;
        } else {
            if (StringUtility.isNullOrEmpty(stringB)) {
                return -1;
            }
            if (StringUtility.isNullOrEmpty(stringA)) {
                return 1;
            }
            int stringComparedLength = stringB.length() - stringA.length();

            if (stringComparedLength != 0) {
                return stringComparedLength;
            } else {
                return stringA.compareToIgnoreCase(stringB);
            }
        }
    }
}