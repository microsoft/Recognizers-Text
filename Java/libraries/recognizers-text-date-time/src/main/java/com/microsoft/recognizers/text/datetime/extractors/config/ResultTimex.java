package com.microsoft.recognizers.text.datetime.extractors.config;

public class ResultTimex {
    private boolean result;
    private String timex;

    public ResultTimex(boolean result, String timex) {
        this.result = result;
        this.timex = timex;
    }

    public boolean getResult() {
        return result;
    }

    public String getTimex() {
        return timex;
    }

    public void setResult(boolean result) {
        this.result = result;
    }

    public void setTimex(String timex) {
        this.timex = timex;
    }
}
