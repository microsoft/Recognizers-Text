// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

public class Time {
    private Integer hour;

    private Integer minute;

    private Integer second;

    public Time(Integer withSeconds) {
        this.hour = (int)Math.floor(withSeconds / 3600000d);
        this.minute = (int)Math.floor((withSeconds - (this.hour * 3600000)) / 60000d);
        this.second = (withSeconds - (this.hour * 3600000) - (this.minute * 60000)) / 1000;
    }

    public Time(Integer withHour, Integer withMinute, Integer withSecond) {
        this.hour = withHour;
        this.minute = withMinute;
        this.second = withSecond;
    }

    public Integer getTime() {
        return (this.second * 1000) + (this.minute * 60000) + (this.hour * 3600000);
    }

    public Integer getHour() {
        return hour;
    }

    public void setHour(Integer withHour) {
        this.hour = withHour;
    }

    public Integer getMinute() {
        return minute;
    }

    public void setMinute(Integer withMinute) {
        this.minute = withMinute;
    }

    public Integer getSecond() {
        return second;
    }

    public void setSecond(Integer withSecond) {
        this.second = withSecond;
    }
}
