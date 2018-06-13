package com.microsoft.recognizers.text.number;

public enum NumberOptions {
    None(0),
    PercentageMode(1);

    private final int value;

    NumberOptions(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }
}
