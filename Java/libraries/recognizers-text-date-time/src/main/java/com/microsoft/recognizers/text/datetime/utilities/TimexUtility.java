package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.datetime.DatePeriodTimexType;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.temporal.ChronoUnit;
import java.time.temporal.IsoFields;

public class TimexUtility {

    public static String generateDatePeriodTimex(LocalDateTime begin, LocalDateTime end, DatePeriodTimexType timexType) {
        String datePeriodTimex;

        if (timexType == DatePeriodTimexType.ByDay)
        {
            datePeriodTimex = "P" + ChronoUnit.DAYS.between(begin,end) + "D";
        }
        else if (timexType == DatePeriodTimexType.ByWeek)
        {
            datePeriodTimex = "P" + (ChronoUnit.WEEKS.between(begin,end)) + "W";
        }
        else if (timexType == DatePeriodTimexType.ByMonth)
        {
            datePeriodTimex =  "P" + ChronoUnit.MONTHS.between(begin,end) + "M"; // ((end.getYear() - begin.getYear()) * 12) + (end.getMonthValue() - begin.getMonthValue());
        }
        else
        {
            double yearDiff = (end.getYear() - begin.getYear()) + (end.getMonthValue() - begin.getMonthValue()) / 12.0;
            datePeriodTimex = "P" + yearDiff + "Y";
        }

        return "(" + FormatUtil.luisDate(begin) + "," + FormatUtil.luisDate(end) + "," + datePeriodTimex + ")";
    }

    public static String generateWeekTimex() {
        return "XXXX-WXX";
    }

    public static String generateWeekTimex(LocalDateTime monday) {
        int isoWeek = LocalDate.of(monday.getYear(), monday.getMonthValue(), monday.getDayOfMonth()).get(IsoFields.WEEK_OF_WEEK_BASED_YEAR);
        return String.format("%04d-W%02d", monday.getYear(), isoWeek);
    }

    public static String generateWeekendTimex() {
        return "XXXX-WXX-WE";
    }

    public static String generateWeekendTimex(LocalDateTime date) {
        int isoWeek = LocalDate.of(date.getYear(), date.getMonthValue(), date.getDayOfMonth()).get(IsoFields.WEEK_OF_WEEK_BASED_YEAR);
        return String.format("%04d-W%02d-WE", date.getYear(), isoWeek);
    }
    public static String generateMonthTimex() {
        return "XXXX-XX";
    }

    public static String generateMonthTimex(LocalDateTime date) {
        return String.format("%04d-%02d",date.getYear(), date.getMonthValue());
    }
    public static String generateYearTimex() {
        return "XXXX";
    }

    public static String generateYearTimex(LocalDateTime date) {
        return String.format("%04d", date.getYear());
    }
}
