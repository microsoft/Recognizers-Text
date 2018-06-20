package com.microsoft.recognizers.text.number;

public abstract class NumberRangeConstants {
    // Number range regex type
    public static final String TWONUM = "TwoNum";
    public static final String TWONUMBETWEEN = "TwoNumBetween";
    public static final String TWONUMTILL = "TwoNumTill";
    public static final String MORE = "More";
    public static final String LESS = "Less";
    public static final String EQUAL = "Equal";

    // Brackets and comma for number range resolution value
    public static final char LEFT_OPEN = '(';
    public static final char RIGHT_OPEN = ')';
    public static final char LEFT_CLOSED = '[';
    public static final char RIGHT_CLOSED = ']';
    public static final char INTERVAL_SEPARATOR = ',';

    // Invalid number
    public static final int INVALID_NUM = -1;
}
