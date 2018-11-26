package com.microsoft.recognizers.text.datetime.extractors.config;

public class ResultTimex {
    public final boolean result;
    public final String timex;

    public ResultTimex(boolean result, String timex) {
        this.result = result;
        this.timex = timex;
    }

    public ResultTimex withResult(boolean newResult) {
        return new ResultTimex(newResult, this.timex);
    }

    public ResultTimex withTimex(String newTimex) {
        return new ResultTimex(this.result, newTimex);
    }
}
