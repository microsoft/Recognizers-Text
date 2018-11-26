package com.microsoft.recognizers.text.utilities;

public class Capture {
    public final String value;
    public final int index;
    public final int length;

    public Capture(String value, int index, int length) {
        this.value = value;
        this.index = index;
        this.length = length;
    }
}
