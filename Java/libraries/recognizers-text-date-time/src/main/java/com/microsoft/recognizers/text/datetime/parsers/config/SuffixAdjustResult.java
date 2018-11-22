package com.microsoft.recognizers.text.datetime.parsers.config;

public class SuffixAdjustResult {
    public final int hour;
    public final int minute;
    public final boolean hasMin;
    public final boolean hasAm;
    public final boolean hasPm;

    public SuffixAdjustResult(int hour, int minute, boolean hasMin, boolean hasAm, boolean hasPm) {
        this.hour = hour;
        this.minute = minute;
        this.hasMin = hasMin;
        this.hasAm = hasAm;
        this.hasPm = hasPm;
    }
}
