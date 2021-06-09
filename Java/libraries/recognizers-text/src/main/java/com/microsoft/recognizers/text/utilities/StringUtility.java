package com.microsoft.recognizers.text.utilities;

public abstract class StringUtility {
    public static boolean isNullOrEmpty(String source) {
        return source == null || source.equals("");
    }

    public static boolean isNullOrWhiteSpace(String source) {
        return source == null || source.trim().equals("");
    }

    public static String trimStart(String source) {
        return trimStart(source, "^\\s+", "");
    }

    public static String trimStart(String source, String regex, String replacement) {
        return source.replaceFirst(regex, replacement);
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
