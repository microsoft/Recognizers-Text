package com.microsoft.recognizers.text.datetime.parsers.config;

public class MatchedTimeRangeResult {
    private boolean matched;
    private String timeStr;
    private int beginHour;
    private int endHour;
    private int endMin;

    public MatchedTimeRangeResult(boolean matched, String timeStr, int beginHour, int endHour, int endMin) {
        this.matched = matched;
        this.timeStr = timeStr;
        this.beginHour = beginHour;
        this.endHour = endHour;
        this.endMin = endMin;
    }

    public boolean getMatched() {
        return matched;
    }

    public String getTimeStr() {
        return timeStr;
    }

    public int getBeginHour() {
        return beginHour;
    }

    public int getEndHour() {
        return endHour;
    }

    public int getEndMin() {
        return endMin;
    }

    public void setMatched(boolean matched) {
        this.matched = matched;
    }

    public void setTimeStr(String timeStr) {
        this.timeStr = timeStr;
    }

    public void setBeginHour(int beginHour) {
        this.beginHour = beginHour;
    }

    public void setEndHour(int endHour) {
        this.endHour = endHour;
    }

    public void setEndMin(int endMin) {
        this.endMin = endMin;
    }
}
