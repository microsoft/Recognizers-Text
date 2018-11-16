package com.microsoft.recognizers.text.utilities;

public abstract class StringUtility {
    public static boolean isNullOrEmpty(String source) {
        return source == null || source.equals("");
    }

    public static boolean isNullOrWhiteSpace(String source) {
        return source == null || source.trim().equals("");
    }

    public static String trimStart(String source) {
        return source.replaceFirst("^\\s+", "");
    }

    public static String trimEnd(String source) {
        return source.replaceFirst("\\s+$", "");
    }

    public static String format(double d) {
        if (d == (long)d) {
            return String.format("%d", (long)d);
        }

        return String.format("%s", d);
    }
}
