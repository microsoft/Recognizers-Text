package com.microsoft.recognizers.text.datetime.extractors.config;

public class ResultIndex {
    private boolean result;
    private int index;

    public ResultIndex(boolean result, int index) {
        this.result = result;
        this.index = index;
    }

    public boolean getResult() {
        return result;
    }

    public int getIndex() {
        return index;
    }

    public void setResult(boolean result) {
        this.result = result;
    }

    public void setIndex(int index) {
        this.index = index;
    }
}
