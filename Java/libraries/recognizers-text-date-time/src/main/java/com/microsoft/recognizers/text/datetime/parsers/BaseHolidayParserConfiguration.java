package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;

import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.time.YearMonth;
import java.util.HashMap;
import java.util.function.IntFunction;
import java.util.regex.Pattern;

public abstract class BaseHolidayParserConfiguration extends BaseOptionsConfiguration implements IHolidayParserConfiguration {

    private ImmutableMap<String, String> variableHolidaysTimexDictionary;

    public final ImmutableMap<String, String> getVariableHolidaysTimexDictionary() {
        return variableHolidaysTimexDictionary;
    }

    protected final void setVariableHolidaysTimexDictionary(ImmutableMap<String, String> value) {
        variableHolidaysTimexDictionary = value;
    }

    private ImmutableMap<String, IntFunction<LocalDateTime>> holidayFuncDictionary;

    public final ImmutableMap<String, IntFunction<LocalDateTime>> getHolidayFuncDictionary() {
        return holidayFuncDictionary;
    }

    protected final void setHolidayFuncDictionary(ImmutableMap<String, IntFunction<LocalDateTime>> value) {
        holidayFuncDictionary = value;
    }

    private ImmutableMap<String, Iterable<String>> holidayNames;

    public final ImmutableMap<String, Iterable<String>> getHolidayNames() {
        return holidayNames;
    }

    protected final void setHolidayNames(ImmutableMap<String, Iterable<String>> value) {
        holidayNames = value;
    }

    private Iterable<Pattern> holidayRegexList;

    public final Iterable<Pattern> getHolidayRegexList() {
        return holidayRegexList;
    }

    protected final void setHolidayRegexList(Iterable<Pattern> value) {
        holidayRegexList = value;
    }

    protected BaseHolidayParserConfiguration() {
        super(DateTimeOptions.None);
        this.variableHolidaysTimexDictionary = BaseDateTime.VariableHolidaysTimexDictionary;
        this.setHolidayFuncDictionary(ImmutableMap.copyOf(initHolidayFuncs()));
    }

    protected HashMap<String, IntFunction<LocalDateTime>> initHolidayFuncs() {
        HashMap<String, IntFunction<LocalDateTime>> holidays = new HashMap<>();
        holidays.put("labour", BaseHolidayParserConfiguration::labourDay);
        holidays.put("fathers", BaseHolidayParserConfiguration::fathersDay);
        holidays.put("mothers", BaseHolidayParserConfiguration::mothersDay);
        holidays.put("canberra", BaseHolidayParserConfiguration::canberraDay);
        holidays.put("columbus", BaseHolidayParserConfiguration::columbusDay);
        holidays.put("memorial", BaseHolidayParserConfiguration::memorialDay);
        holidays.put("thanksgiving", BaseHolidayParserConfiguration::thanksgivingDay);
        holidays.put("thanksgivingday", BaseHolidayParserConfiguration::thanksgivingDay);
        holidays.put("blackfriday", BaseHolidayParserConfiguration::blackFriday);
        holidays.put("martinlutherking", BaseHolidayParserConfiguration::martinLutherKingDay);
        holidays.put("washingtonsbirthday", BaseHolidayParserConfiguration::washingtonsBirthday);

        return holidays;
    }

    public abstract int getSwiftYear(String text);

    public abstract String sanitizeHolidayToken(String holiday);

    // @TODO auto-generate from YAML
    private static LocalDateTime canberraDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 3, getDay(year, 3, 0, DayOfWeek.MONDAY));
    }

    private static LocalDateTime martinLutherKingDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 1, getDay(year, 1, 2, DayOfWeek.MONDAY));
    }

    private static LocalDateTime washingtonsBirthday(int year) {
        return DateUtil.safeCreateFromMinValue(year, 2, getDay(year, 2, 2, DayOfWeek.MONDAY));
    }

    protected static LocalDateTime mothersDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, getDay(year, 5, 1, DayOfWeek.SUNDAY));
    }

    protected static LocalDateTime fathersDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 6, getDay(year, 6, 2, DayOfWeek.SUNDAY));
    }

    protected static LocalDateTime memorialDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, getLastDay(year, 5, DayOfWeek.MONDAY));
    }

    protected static LocalDateTime labourDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 9, getDay(year, 9, 0, DayOfWeek.MONDAY));
    }

    protected static LocalDateTime internationalWorkersDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 5, 1);
    }

    protected static LocalDateTime columbusDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 10, getDay(year, 10, 1, DayOfWeek.MONDAY));
    }

    protected static LocalDateTime thanksgivingDay(int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, getDay(year, 11, 3, DayOfWeek.THURSDAY));
    }

    protected static LocalDateTime blackFriday(int year) {
        return DateUtil.safeCreateFromMinValue(year, 11, getDay(year, 11, 3, DayOfWeek.FRIDAY));
    }

    protected static int getDay(int year, int month, int week, DayOfWeek dayOfWeek) {

        YearMonth yearMonthObject = YearMonth.of(year, month);
        int daysInMonth = yearMonthObject.lengthOfMonth();

        int weekCount = 0;
        for (int day = 1; day < daysInMonth + 1; day++) {
            if (DateUtil.safeCreateFromMinValue(year, month, day).getDayOfWeek() == dayOfWeek) {
                weekCount++;
                if (weekCount == week + 1) {
                    return day;
                }
            }
        }

        throw new Error("day out of bound.");
    }

    protected static int getLastDay(int year, int month, DayOfWeek dayOfWeek) {

        YearMonth yearMonthObject = YearMonth.of(year, month);
        int daysInMonth = yearMonthObject.lengthOfMonth();

        int lastDay = 0;
        for (int day = 1; day < daysInMonth + 1; day++) {
            if (DateUtil.safeCreateFromMinValue(year, month, day).getDayOfWeek() == dayOfWeek) {
                lastDay = day;
            }
        }

        return lastDay;
    }
}
