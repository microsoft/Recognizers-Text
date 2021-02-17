// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.HashSet;
import java.util.Map;
import java.util.Map.Entry;

import org.apache.commons.lang3.StringUtils;

public class TimexProperty {
    private Time time;

    private String timexValue;

    private HashSet<String> types;

    private Boolean now;

    private BigDecimal years;

    private BigDecimal months;

    private BigDecimal weeks;

    private BigDecimal days;

    private BigDecimal hours;

    private BigDecimal minutes;

    private BigDecimal seconds;

    private Integer year;

    private Integer month;

    private Integer dayOfMonth;

    private Integer dayOfWeek;

    private String season;

    private Integer weekOfYear;

    private Boolean weekend;

    public Integer weekOfMonth;

    private Integer hour;

    private Integer minute;

    private Integer second;

    private String partOfDay;

    public TimexProperty() {

    }

    public TimexProperty(String timex) {
        TimexParsing.parseString(timex, this);
    }

    public String getTimexValue() {
        return TimexFormat.format(this);
    }

    public void setTimexValue(String withTimexValue) {
        this.timexValue = withTimexValue;
    }

    public HashSet<String> getTypes() {
        return TimexInference.infer(this);
    }

    public void setTypes(HashSet<String> withTypes) {
        this.types = withTypes;
    }

    public Boolean getNow() {
        return now;
    }

    public void setNow(Boolean withNow) {
        this.now = withNow;
    }

    public BigDecimal getYears() {
        return years;
    }

    public void setYears(BigDecimal withYears) {
        this.years = withYears;
    }

    public BigDecimal getMonths() {
        return months;
    }

    public void setMonths(BigDecimal withMonths) {
        this.months = withMonths;
    }

    public BigDecimal getWeeks() {
        return weeks;
    }

    public void setWeeks(BigDecimal withWeeks) {
        this.weeks = withWeeks;
    }

    public BigDecimal getDays() {
        return days;
    }

    public void setDays(BigDecimal withDays) {
        this.days = withDays;
    }

    public BigDecimal getHours() {
        return hours;
    }

    public void setHours(BigDecimal withHours) {
        this.hours = withHours;
    }

    public BigDecimal getMinutes() {
        return minutes;
    }

    public void setMinutes(BigDecimal withMinutes) {
        this.minutes = withMinutes;
    }

    public BigDecimal getSeconds() {
        return seconds;
    }

    public void setSeconds(BigDecimal withSeconds) {
        this.seconds = withSeconds;
    }

    public Integer getYear() {
        return year;
    }

    public void setYear(Integer withYear) {
        this.year = withYear;
    }

    public Integer getMonth() {
        return month;
    }

    public void setMonth(Integer withMonth) {
        this.month = withMonth;
    }

    public Integer getDayOfMonth() {
        return dayOfMonth;
    }

    public void setDayOfMonth(Integer withDayOfMonth) {
        this.dayOfMonth = withDayOfMonth;
    }

    public Integer getDayOfWeek() {
        return dayOfWeek;
    }

    public void setDayOfWeek(Integer withDayOfWeek) {
        this.dayOfWeek = withDayOfWeek;
    }

    public String getSeason() {
        return season;
    }

    public void setSeason(String withSeason) {
        this.season = withSeason;
    }

    public Integer getWeekOfYear() {
        return weekOfYear;
    }

    public void setWeekOfYear(Integer withWeekOfYear) {
        this.weekOfYear = withWeekOfYear;
    }

    public Boolean getWeekend() {
        return weekend;
    }

    public void setWeekend(Boolean withWeekend) {
        this.weekend = withWeekend;
    }

    public Integer getWeekOfMonth() {
        return weekOfMonth;
    }

    public void setWeekOfMonth(Integer withWeekOfMonth) {
        this.weekOfMonth = withWeekOfMonth;
    }

    public Integer getHour() {
        if (this.time != null) {
            return this.time.getHour();
        }

        return null;
    }

    public void setHour(Integer withHour) {
        if (withHour != null) {
            if (this.time == null) {
                this.time = new Time(withHour, 0, 0);
            } else {
                this.time.setHour(withHour);
            }
        } else {
            this.time = null;
        }
    }

    public Integer getMinute() {
        if (this.time != null) {
            return this.time.getMinute();
        }

        return null;
    }

    public void setMinute(Integer withMinute) {
        if (withMinute != null) {
            if (this.time == null) {
                time = new Time(0, withMinute, 0);
            } else {
                time.setMinute(withMinute);
            }
        } else {
            this.time = null;
        }
    }

    public Integer getSecond() {
        if (this.time != null) {
            return this.time.getSecond();
        }

        return null;
    }

    public void setSecond(Integer withSecond) {
        if (withSecond != null) {
            if (this.time == null) {
                this.time = new Time(0, 0, withSecond);
            } else {
                this.time.setSecond(withSecond);
            }
        } else {
            this.time = null;
        }
    }

    public String getPartOfDay() {
        return partOfDay;
    }

    public void setPartOfDay(String wthPartOfDay) {
        this.partOfDay = wthPartOfDay;
    }

    public static TimexProperty fromDate(LocalDateTime date) {
        TimexProperty timex = new TimexProperty() {
            {
                setYear(date.getYear());
                setMonth(date.getMonthValue());
                setDayOfMonth(date.getDayOfMonth());
            }
        };
        return timex;
    }

    public static TimexProperty fromDateTime(LocalDateTime datetime) {
        TimexProperty timex = TimexProperty.fromDate(datetime);
        timex.setHour(datetime.getHour());
        timex.setMinute(datetime.getMinute());
        timex.setSecond(datetime.getSecond());
        return timex;
    }

    public static TimexProperty fromTime(Time time) {
        return new TimexProperty() {
            {
                setHour(time.getHour());
                setMinute(time.getMinute());
                setSecond(time.getSecond());
            }
        };
    }

    @Override
    public String toString() {
        return TimexConvert.convertTimexToString(this);
    }

    public String toNaturalLanguage(LocalDateTime referenceDate) {
        return TimexRelativeConvert.convertTimexToStringRelative(this, referenceDate);
    }

    public TimexProperty clone() {
        Boolean now = this.getNow();
        BigDecimal years = this.getYears();
        BigDecimal months = this.getMonths();
        BigDecimal weeks = this.getWeeks();
        BigDecimal days = this.getDays();
        BigDecimal hours = this.getHours();
        BigDecimal minutes = this.getMinutes();
        BigDecimal seconds = this.getSeconds();
        Integer year = this.getYear();
        Integer month = this.getMonth();
        Integer dayOfMonth = this.getDayOfMonth();
        Integer dayOfWeek = this.getDayOfWeek();
        String season = this.getSeason();
        Integer weekOfYear = this.getWeekOfYear();
        Boolean weekend = this.getWeekend();
        Integer innerWeekOfMonth = this.getWeekOfMonth();
        Integer hour = this.getHour();
        Integer minute = this.getMinute();
        Integer second = this.getSecond();
        String partOfDay = this.getPartOfDay();

        return new TimexProperty() {
            {
                setNow(now);
                setYears(years);
                setMonths(months);
                setWeeks(weeks);
                setDays(days);
                setHours(hours);
                setMinutes(minutes);
                setSeconds(seconds);
                setYear(year);
                setMonth(month);
                setDayOfMonth(dayOfMonth);
                setDayOfWeek(dayOfWeek);
                setSeason(season);
                setWeekOfYear(weekOfYear);
                setWeekend(weekend);
                setWeekOfMonth(innerWeekOfMonth);
                setHour(hour);
                setMinute(minute);
                setSecond(second);
                setPartOfDay(partOfDay);
            }
        };
    }

    public void assignProperties(Map<String, String> source) {
        for (Entry<String, String> item : source.entrySet()) {

            if (StringUtils.isBlank(item.getValue())) {
                continue;
            }

            switch (item.getKey()) {
                case "year":
                    setYear(Integer.parseInt(item.getValue()));
                    break;
                case "month":
                    setMonth(Integer.parseInt(item.getValue()));
                    break;
                case "dayOfMonth":
                    setDayOfMonth(Integer.parseInt(item.getValue()));
                    break;
                case "dayOfWeek":
                    setDayOfWeek(Integer.parseInt(item.getValue()));
                    break;
                case "season":
                    setSeason(item.getValue());
                    break;
                case "weekOfYear":
                    setWeekOfYear(Integer.parseInt(item.getValue()));
                    break;
                case "weekend":
                    setWeekend(true);
                    break;
                case "weekOfMonth":
                    setWeekOfMonth(Integer.parseInt(item.getValue()));
                    break;
                case "hour":
                    setHour(Integer.parseInt(item.getValue()));
                    break;
                case "minute":
                    setMinute(Integer.parseInt(item.getValue()));
                    break;
                case "second":
                    setSecond(Integer.parseInt(item.getValue()));
                    break;
                case "partOfDay":
                    setPartOfDay(item.getValue());
                    break;
                case "dateUnit":
                    this.assignDateDuration(source);
                    break;
                case "hourAmount":
                    setHours(new BigDecimal(item.getValue()));
                    break;
                case "minuteAmount":
                    setMinutes(new BigDecimal(item.getValue()));
                    break;
                case "secondAmount":
                    setSeconds(new BigDecimal(item.getValue()));
                    break;
                default:
            }
        }
    }

    private void assignDateDuration(Map<String, String> source) {
        switch (source.get("dateUnit")) {
            case "Y":
                this.years = new BigDecimal(source.get("amount"));
                break;
            case "M":
                this.months = new BigDecimal(source.get("amount"));
                break;
            case "W":
                this.weeks = new BigDecimal(source.get("amount"));
                break;
            case "D":
                this.days = new BigDecimal(source.get("amount"));
                break;
            default:
        }
    }
}
