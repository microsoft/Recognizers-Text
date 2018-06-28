package com.microsoft.recognizers.text.utilities;

public abstract class StringUtility {
    public static boolean isNullOrEmpty(String source) {
        return source == null || source.equals("");
    }

    public static boolean isNullOrWhiteSpace(String source) {
        return source == null || source.trim().equals("");
    }
}
