package com.microsoft.recognizers.text.datetime.utilities;

public class MatchedTimexResult {
    private boolean result;
    private String timex;

    public MatchedTimexResult(boolean result, String timex) {
        this.result = result;
        this.timex = timex;
    }

    public MatchedTimexResult() {
        this(false, "");
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
