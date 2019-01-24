package com.microsoft.recognizers.text.numberwithunit.utilities;

import java.util.Comparator;

public class StringComparer implements Comparator<String> {
    @Override
    public int compare(String stringA, String stringB) {
        if (stringA.isEmpty() && stringB.isEmpty()) {
            return 0;
        } else {
            if (stringB.isEmpty()) {
                return -1;
            }
            if (stringA.isEmpty()) {
                return 1;
            } else {
                return stringB.compareToIgnoreCase(stringA);
            }
        }
    }
}

