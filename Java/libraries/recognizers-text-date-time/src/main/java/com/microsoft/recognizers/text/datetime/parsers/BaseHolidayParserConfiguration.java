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
    private ImmutableMap<String, String> VariableHolidaysTimexDictionary;
    public final ImmutableMap<String, String> getVariableHolidaysTimexDictionary() { return VariableHolidaysTimexDictionary; }
    protected final void setVariableHolidaysTimexDictionary(ImmutableMap<String, String> value) { VariableHolidaysTimexDictionary = value; }

    private ImmutableMap<String, IntFunction<LocalDateTime>> HolidayFuncDictionary;
    public final ImmutableMap<String, IntFunction<LocalDateTime>> getHolidayFuncDictionary() { return HolidayFuncDictionary; }
    protected final void setHolidayFuncDictionary(ImmutableMap<String, IntFunction<LocalDateTime>> value) { HolidayFuncDictionary = value; }


    private ImmutableMap<String, Iterable<String>> HolidayNames;
    public final ImmutableMap<String, Iterable<String>> getHolidayNames() { return HolidayNames; }
    protected final void setHolidayNames(ImmutableMap<String, Iterable<String>> value) { HolidayNames = value; }

    private Iterable<Pattern> HolidayRegexList;
    public final Iterable<Pattern> getHolidayRegexList() { return HolidayRegexList; }
    protected final void setHolidayRegexList(Iterable<Pattern> value) { HolidayRegexList = value; }

    protected BaseHolidayParserConfiguration()
    {
        super(DateTimeOptions.None);
        this.VariableHolidaysTimexDictionary = BaseDateTime.VariableHolidaysTimexDictionary;
        this.setHolidayFuncDictionary(ImmutableMap.copyOf(InitHolidayFuncs()));
    }

    protected HashMap<String, IntFunction<LocalDateTime>> InitHolidayFuncs()
    {
        HashMap<String, IntFunction<LocalDateTime>> holidays = new HashMap<>();
        holidays.put("labour", BaseHolidayParserConfiguration::LabourDay);
        holidays.put("fathers", BaseHolidayParserConfiguration::FathersDay);
        holidays.put("mothers", BaseHolidayParserConfiguration::MothersDay);
        holidays.put("canberra", BaseHolidayParserConfiguration::CanberraDay);
        holidays.put("columbus", BaseHolidayParserConfiguration::ColumbusDay);
        holidays.put("memorial", BaseHolidayParserConfiguration::MemorialDay);
        holidays.put("thanksgiving", BaseHolidayParserConfiguration::ThanksgivingDay);
        holidays.put("thanksgivingday", BaseHolidayParserConfiguration::ThanksgivingDay);
        holidays.put("martinlutherking", BaseHolidayParserConfiguration::MartinLutherKingDay);
        holidays.put("washingtonsbirthday", BaseHolidayParserConfiguration::WashingtonsBirthday);

        return holidays;
    }


    public abstract int getSwiftYear(String text);
    public abstract String sanitizeHolidayToken(String holiday);

    private static LocalDateTime CanberraDay(int year) { return DateUtil.safeCreateFromMinValue(year, 3, GetDay(year, 3, 0, DayOfWeek.MONDAY)); }
    private static LocalDateTime MartinLutherKingDay(int year) { return DateUtil.safeCreateFromMinValue(year, 1, GetDay(year, 1, 2, DayOfWeek.MONDAY)); }
    private static LocalDateTime WashingtonsBirthday(int year) { return DateUtil.safeCreateFromMinValue(year, 2, GetDay(year, 2, 2, DayOfWeek.MONDAY)); }
    protected static LocalDateTime MothersDay(int year) { return DateUtil.safeCreateFromMinValue(year, 5, GetDay(year, 5, 1, DayOfWeek.SUNDAY)); }
    protected static LocalDateTime FathersDay(int year) { return DateUtil.safeCreateFromMinValue(year, 6, GetDay(year, 6, 2, DayOfWeek.SUNDAY)); }
    protected static LocalDateTime MemorialDay(int year) { return DateUtil.safeCreateFromMinValue(year, 5, GetLastDay(year, 5, DayOfWeek.MONDAY)); }
    protected static LocalDateTime LabourDay(int year) { return DateUtil.safeCreateFromMinValue(year, 9, GetDay(year,9, 0, DayOfWeek.MONDAY)); }
    protected static LocalDateTime ColumbusDay(int year) { return DateUtil.safeCreateFromMinValue(year, 10, GetDay(year,10, 1, DayOfWeek.MONDAY)); }
    protected static LocalDateTime ThanksgivingDay(int year) { return DateUtil.safeCreateFromMinValue(year, 11, GetDay(year,11, 3, DayOfWeek.THURSDAY)); }

    protected static int GetDay(int year, int month, int week, DayOfWeek dayOfWeek)
    {
        YearMonth yearMonthObject = YearMonth.of(year, month);
        int daysInMonth = yearMonthObject.lengthOfMonth();

        int weekCount = 0;
        for (int day = 1; day < daysInMonth + 1; day++) {
            if (DateUtil.safeCreateFromMinValue(year,month,day).getDayOfWeek() == dayOfWeek){
                weekCount++;
                if (weekCount == week + 1) {
                    return day;
                }
            }
        }

        throw new Error("day out of bound.");
    }

    protected static int GetLastDay(int year, int month, DayOfWeek dayOfWeek)
    {
        YearMonth yearMonthObject = YearMonth.of(year, month);
        int daysInMonth = yearMonthObject.lengthOfMonth();

        int lastDay = 0;
        for (int day = 1; day < daysInMonth + 1; day++) {
            if (DateUtil.safeCreateFromMinValue(year,month,day).getDayOfWeek() == dayOfWeek){
                lastDay = day;
            }
        }

        return lastDay;
    }
}
