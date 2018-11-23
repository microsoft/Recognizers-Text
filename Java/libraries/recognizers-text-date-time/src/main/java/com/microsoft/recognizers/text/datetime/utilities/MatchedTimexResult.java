package com.microsoft.recognizers.text.datetime.utilities;

public class MatchedTimexResult {
    public final boolean result;
    public final String timex;

    public MatchedTimexResult(boolean result, String timex) {
        this.result = result;
        this.timex = timex;
    }

    public MatchedTimexResult() {
        this(false, "");
    }

    public MatchedTimexResult withTimex(String timex) {
        return new MatchedTimexResult(this.result, timex);
    }

    public MatchedTimexResult withResult(boolean result) {
        return new MatchedTimexResult(result, this.timex);
    }
}
