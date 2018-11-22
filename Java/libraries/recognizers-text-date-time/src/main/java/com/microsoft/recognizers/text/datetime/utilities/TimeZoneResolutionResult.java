package com.microsoft.recognizers.text.datetime.utilities;

public class TimeZoneResolutionResult {

    private final String value;
    private final Integer utcOffsetMins;
    private final String timeZoneText;

    public TimeZoneResolutionResult(String value, Integer utcOffsetMins, String timeZoneText) {
        this.value = value;
        this.utcOffsetMins = utcOffsetMins;
        this.timeZoneText = timeZoneText;
    }

    public String getValue() {
        return this.value;
    }

    public Integer getUtcOffsetMins() {
        return this.utcOffsetMins;
    }

    public String getTimeZoneText() {
        return this.timeZoneText;
    }
}
