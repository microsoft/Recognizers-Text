package com.microsoft.recognizers.text.datetime.extractors.config;

public class ResultIndex {
    public final boolean result;
    public final int index;

    public ResultIndex(boolean result, int index) {
        this.result = result;
        this.index = index;
    }

    public ResultIndex withResult(boolean newResult) {
        return new ResultIndex(newResult, this.index);
    }

    public ResultIndex withIndex(int newIndex) {
        return new ResultIndex(this.result, newIndex);
    }
}
