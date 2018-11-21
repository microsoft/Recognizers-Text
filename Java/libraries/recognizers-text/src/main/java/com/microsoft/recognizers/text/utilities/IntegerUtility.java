package com.microsoft.recognizers.text.utilities;

public abstract class IntegerUtility {
    public static boolean canParse(String value) {
        try {
            Integer.parseInt(value);
        } catch (Exception e) {
            return false;
        }
        return true;
    }
}
