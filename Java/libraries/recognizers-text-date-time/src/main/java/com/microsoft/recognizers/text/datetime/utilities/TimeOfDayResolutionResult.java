package com.microsoft.recognizers.text.datetime.utilities;

public class TimeOfDayResolutionResult {
    private String timex;
    private int beginHour;
    private int endHour;
    private int endMin;

    public TimeOfDayResolutionResult(String timex, int beginHour, int endHour, int endMin) {
        this.timex = timex;
        this.beginHour = beginHour;
        this.endHour = endHour;
        this.endMin = endMin;
    }

    public TimeOfDayResolutionResult() {
    }

    public String getTimex() {
        return timex;
    }

    public void setTimex(String timex) {
        this.timex = timex;
    }

    public int getBeginHour() {
        return beginHour;
    }

    public void setBeginHour(int beginHour) {
        this.beginHour = beginHour;
    }

    public int getEndHour() {
        return endHour;
    }

    public void setEndHour(int endHour) {
        this.endHour = endHour;
    }

    public int getEndMin() {
        return endMin;
    }

    public void setEndMin(int endMin) {
        this.endMin = endMin;
    }
}
