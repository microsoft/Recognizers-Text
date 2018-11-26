package com.microsoft.recognizers.text.utilities;

public class MatchGroup {
    public final String value;
    public final int index;
    public final int length;
    public final Capture[] captures;

    public MatchGroup(String value, int index, int length, Capture[] captures) {
        this.value = value;
        this.index = index;
        this.length = length;
        this.captures = captures;
    }
}
