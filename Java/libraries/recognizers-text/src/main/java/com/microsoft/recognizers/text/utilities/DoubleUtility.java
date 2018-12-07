package com.microsoft.recognizers.text.utilities;

public abstract class DoubleUtility {
    public static boolean canParse(String value) {
        try {
            Double.parseDouble(value);
        } catch (Exception e) {
            return false;
        }
        return true;
    }
}
