package com.microsoft.recognizers.text.numberwithunit.utilities;

import java.util.Comparator;

public class DinoComparer implements Comparator<String> {
    @Override
    public int compare(String x, String y) {
        if (x == null) {
            if (y == null) {
                // If x is null and y is null, they're
                // equal.
                return 0;
            } else {
                // If x is null and y is not null, y
                // is greater.
                return 1;
            }
        } else {
            // If x is not null...
            //
            if (y == null)
            // ...and y is null, x is greater.
            {
                return -1;
            } else {
                // ...and y is not null, compare the
                // lengths of the two strings.
                //
                int retval = Integer.compare(y.length(), x.length());

                if (retval != 0) {
                    // If the strings are not of equal length,
                    // the longer string is greater.
                    //
                    return retval;
                } else {
                    // If the strings are of equal length,
                    // sort them with ordinary string comparison.
                    //
                    return x.compareToIgnoreCase(y);
                }
            }
        }
    }
}
