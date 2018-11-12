package com.microsoft.recognizers.text.datetime.utilities;

public class MatchingUtilResult {
    public final boolean result;
    public final int index;

    public MatchingUtilResult(boolean result, int index) {
        this.result = result;
        this.index = index;
    }

    public MatchingUtilResult() {
        this(false, -1);
    }
}
