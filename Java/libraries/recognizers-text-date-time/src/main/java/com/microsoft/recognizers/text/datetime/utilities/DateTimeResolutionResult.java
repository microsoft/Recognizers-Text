package com.microsoft.recognizers.text.datetime.utilities;

import java.util.List;
import java.util.Map;

public class DateTimeResolutionResult {

    private Boolean success;
    private String timex;
    private Boolean isLunar;
    private String mod;
    private Boolean hasRangeChangingMod;
    private String comment;

    private Map<String, String> futureResolution;
    private Map<String, String> pastResolution;

    private Object futureValue;
    private Object pastValue;

    private List<Object> subDateTimeEntities;

    private TimeZoneResolutionResult timeZoneResolution;

    private List<Object> list;

    public DateTimeResolutionResult() {
        success = hasRangeChangingMod = false;
    }

    public Boolean getSuccess() {
        return this.success;
    }

    public void setSuccess(Boolean success) {
        this.success = success;
    }

    public String getTimex() {
        return this.timex;
    }

    public void setTimex(String timex) {
        this.timex = timex;
    }

    public Boolean getIsLunar() {
        return this.isLunar;
    }

    public void setIsLunar(Boolean isLunar) {
        this.isLunar = isLunar;
    }

    public String getMod() {
        return this.mod;
    }

    public void setMod(String mod) {
        this.mod = mod;
    }

    public Boolean getHasRangeChangingMod() {
        return this.hasRangeChangingMod;
    }

    public void setHasRangeChangingMod(Boolean hasRangeChangingMod) {
        this.hasRangeChangingMod = hasRangeChangingMod;
    }

    public String getComment() {
        return this.comment;
    }

    public void setComment(String comment) {
        this.comment = comment;
    }

    public Map<String, String> getFutureResolution() {
        return this.futureResolution;
    }

    public void setFutureResolution(Map<String, String> futureResolution) {
        this.futureResolution = futureResolution;
    }

    public Map<String, String> getPastResolution() {
        return this.pastResolution;
    }

    public void setPastResolution(Map<String, String> pastResolution) {
        this.pastResolution = pastResolution;
    }

    public Object getFutureValue() {
        return this.futureValue;
    }

    public void setFutureValue(Object futureValue) {
        this.futureValue = futureValue;
    }

    public Object getPastValue() {
        return this.pastValue;
    }

    public void setPastValue(Object pastValue) {
        this.pastValue = pastValue;
    }

    public List<Object> getSubDateTimeEntities() {
        return this.subDateTimeEntities;
    }

    public void setSubDateTimeEntities(List<Object> subDateTimeEntities) {
        this.subDateTimeEntities = subDateTimeEntities;
    }

    public TimeZoneResolutionResult getTimeZoneResolution() {
        return this.timeZoneResolution;
    }

    public void setTimeZoneResolution(TimeZoneResolutionResult timeZoneResolution) {
        this.timeZoneResolution = timeZoneResolution;
    }

    public List<Object> getList() {
        return this.list;
    }

    public void setList(List<Object> list) {
        this.list = list;
    }
}