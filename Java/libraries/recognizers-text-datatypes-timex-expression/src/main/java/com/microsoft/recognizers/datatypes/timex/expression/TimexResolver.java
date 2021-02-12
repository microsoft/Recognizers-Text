// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import com.google.common.collect.Streams;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.time.temporal.TemporalField;
import java.time.temporal.WeekFields;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Locale;
import java.util.stream.Collectors;

import org.apache.commons.lang3.tuple.Pair;

public class TimexResolver {
    public static Resolution resolve(String[] timexArray, LocalDateTime date) {
        date = date != null ? date : LocalDateTime.now();
        Resolution resolution = new Resolution();
        for (String timex : timexArray) {
            TimexProperty t = new TimexProperty(timex);
            List<Resolution.Entry> r = TimexResolver.resolveTimex(t, date);
            resolution.getValues().addAll(r);
        }

        return resolution;
    }

    private static List<Resolution.Entry> resolveTimex(TimexProperty timex, LocalDateTime date) {
        HashSet<String> types = timex.getTypes().size() != 0 ? timex.getTypes() : TimexInference.infer(timex);

        if (types.contains(Constants.TimexTypes.DATE_TIME_RANGE)) {
            return TimexResolver.resolveDateTimeRange(timex, date);
        }

        if (types.contains(Constants.TimexTypes.DEFINITE) && types.contains(Constants.TimexTypes.TIME)) {
            return TimexResolver.resolveDefiniteTime(timex, date);
        }

        if (types.contains(Constants.TimexTypes.DEFINITE) && types.contains(Constants.TimexTypes.DATE_RANGE)) {
            return TimexResolver.resolveDefiniteDateRange(timex, date);
        }

        if (types.contains(Constants.TimexTypes.DATE_RANGE)) {
            return TimexResolver.resolveDateRange(timex, date);
        }

        if (types.contains(Constants.TimexTypes.DEFINITE)) {
            return TimexResolver.resolveDefinite(timex);
        }

        if (types.contains(Constants.TimexTypes.TIME_RANGE)) {
            return TimexResolver.resolveTimeRange(timex, date);
        }

        if (types.contains(Constants.TimexTypes.DATE_TIME)) {
            return TimexResolver.resolveDateTime(timex, date);
        }

        if (types.contains(Constants.TimexTypes.DURATION)) {
            return TimexResolver.resolveDuration(timex);
        }

        if (types.contains(Constants.TimexTypes.DATE)) {
            return TimexResolver.resolveDate(timex, date);
        }

        if (types.contains(Constants.TimexTypes.TIME)) {
            return TimexResolver.resolveTime(timex, date);
        }

        return new ArrayList<Resolution.Entry>();
    }

    private static List<Resolution.Entry> resolveDefiniteTime(TimexProperty timex, LocalDateTime date) {
        return new ArrayList<Resolution.Entry>() {
            {
                add(new Resolution.Entry() {
                    {
                        setTimex(timex.getTimexValue());
                        setType("datetime");
                        setValue(String.format("%1$s %2$s", TimexValue.dateValue(timex),
                                TimexValue.timeValue(timex, date)));
                    }
                });
            }
        };
    }

    private static List<Resolution.Entry> resolveDefinite(TimexProperty timex) {
        return new ArrayList<Resolution.Entry>() {
            {
                add(new Resolution.Entry() {
                    {
                        setTimex(timex.getTimexValue());
                        setType("date");
                        setValue(TimexValue.dateValue(timex));
                    }
                });
            }
        };
    }

    private static List<Resolution.Entry> resolveDefiniteDateRange(TimexProperty timex, LocalDateTime date) {
        TimexRange range = TimexHelpers.expandDateTimeRange(timex);
        return new ArrayList<Resolution.Entry>() {
            {
                add(new Resolution.Entry() {
                    {
                        setTimex(timex.getTimexValue());
                        setType("daterange");
                        setStart(TimexValue.dateValue(range.getStart()));
                        setEnd(TimexValue.dateValue(range.getEnd()));
                    }
                });
            }
        };
    }

    private static List<Resolution.Entry> resolveDate(TimexProperty timex, LocalDateTime date) {
        List<String> dateValueList = TimexResolver.getDateValues(timex, date);
        List<Resolution.Entry> result = new ArrayList<Resolution.Entry>();
        for (String dateValue : dateValueList) {
            result.add(new Resolution.Entry() {
                {
                    setTimex(timex.getTimexValue());
                    setType("date");
                    setValue(dateValue);
                }
            });
        }

        return result;
    }

    private static String lastDateValue(TimexProperty timex, LocalDateTime date) {
        if (timex.getDayOfMonth() != null) {
            Integer year = date.getYear();
            Integer month = date.getMonth().getValue();
            if (timex.getMonth() != null) {
                month = timex.getMonth();
                if (date.getMonthValue() <= month || (date.getMonth().getValue() == month && TimexDateHelpers.getUSDayOfWeek(date.getDayOfWeek()) <= timex.getDayOfMonth())) {
                    year--;
                }
            } else {
                if (date.getDayOfMonth() <= timex.getDayOfMonth()) {
                    month--;
                    if (month < 1) {
                        month = (month + 12) % 12;
                        year--;
                    }
                }
            }
            Integer finalYear = year;
            Integer finalMonth = month;
            return TimexValue.dateValue(new TimexProperty() {
                {
                    setYear(finalYear);
                    setMonth(finalMonth);
                    setDayOfMonth(timex.getDayOfMonth());
                }
            });
        }

        if (timex.getDayOfWeek() != null) {
            LocalDateTime start = generateWeekDate(timex, date, true);
            return TimexValue.dateValue(new TimexProperty() {
                {
                    setYear(start.getYear());
                    setMonth(start.getMonthValue());
                    setDayOfMonth(start.getDayOfMonth());
                }
            });
        }

        return new String();
    }

    private static String nextDateValue(TimexProperty timex, LocalDateTime date) {
        if (timex.getDayOfMonth() != null) {
            Integer year = date.getYear();
            Integer month = date.getMonth().getValue();
            if (timex.getMonth() != null) {
                month = timex.getMonth();
                if (date.getMonthValue() > month ||
                    (date.getMonthValue() == month && date.getDayOfMonth() > timex.getDayOfMonth())) {
                    year++;
                }
            } else {
                if (date.getDayOfMonth() > timex.getDayOfMonth()) {
                    month++;
                    if (month > 12) {
                        month = month % 12;
                        year--;
                    }
                }
            }
            Integer finalYear = year;
            Integer finalMonth = month;
            return TimexValue.dateValue(new TimexProperty() {
                {
                    setYear(finalYear);
                    setMonth(finalMonth);
                    setDayOfMonth(timex.getDayOfMonth());
                }
            });
        }

        if (timex.getDayOfWeek() != null) {
            LocalDateTime start = generateWeekDate(timex, date, false);
            return TimexValue.dateValue(new TimexProperty() {
                {
                    setYear(start.getYear());
                    setMonth(start.getMonthValue());
                    setDayOfMonth(start.getDayOfMonth());
                }
            });
        }

        return new String();
    }

    private static List<Resolution.Entry> resolveTime(TimexProperty timex, LocalDateTime date) {
        return new ArrayList<Resolution.Entry>() {
            {
                add(new Resolution.Entry() {
                    {
                        setTimex(timex.getTimexValue());
                        setType("time");
                        setValue(TimexValue.timeValue(timex, date));
                    }
                });
            }
        };
    }

    private static List<Resolution.Entry> resolveDuration(TimexProperty timex) {
        return new ArrayList<Resolution.Entry>() {
            {
                add(new Resolution.Entry() {
                    {
                        setTimex(timex.getTimexValue());
                        setType("duration");
                        setValue(TimexValue.durationValue(timex));
                    }
                });
            }
        };
    }

    private static Pair<String, String> yearDateRange(Integer year) {
        return Pair.of(TimexValue.dateValue(new TimexProperty() {
            {
                setYear(year);
                setMonth(1);
                setDayOfMonth(1);
            }
        }), TimexValue.dateValue(new TimexProperty() {
            {
                setYear(year + 1);
                setMonth(1);
                setDayOfMonth(1);
            }
        }));
    }

    private static Pair<String, String> monthDateRange(Integer year, Integer month) {
        return Pair.of(TimexValue.dateValue(new TimexProperty() {
            {
                setYear(year);
                setMonth(month);
                setDayOfMonth(1);
            }
        }), TimexValue.dateValue(new TimexProperty() {
            {
                setYear(month == 12 ? year + 1 : year);
                setMonth(month == 12 ? 1 : month + 1);
                setDayOfMonth(1);
            }
        }));
    }

    private static Pair<String, String> yearWeekDateRange(Integer year, Integer weekOfYear, Boolean isWeekend) {
        LocalDateTime firstMondayInWeek = firstDateOfWeek(year, weekOfYear, Locale.ROOT);

        LocalDateTime start = (isWeekend == null || isWeekend == false) ? firstMondayInWeek
                : TimexDateHelpers.dateOfNextDay(DayOfWeek.SATURDAY, firstMondayInWeek);
        LocalDateTime end = firstMondayInWeek;
        LocalDateTime end2 = end.plusDays(7);

        return Pair.of(TimexValue.dateValue(new TimexProperty() {
            {
                setYear(start.getYear());
                setMonth(start.getMonthValue());
                setDayOfMonth(start.getDayOfMonth());
            }
        }), TimexValue.dateValue(new TimexProperty() {
            {
                setYear(end2.getYear());
                setMonth(end2.getMonthValue());
                setDayOfMonth(end2.getDayOfMonth());
            }
        }));
    }

    // this is based on
    // https://stackoverflow.com/questions/19901666/get-date-of-first-and-last-day-of-week-knowing-week-number/34727270
    private static LocalDateTime firstDateOfWeek(Integer year, Integer weekOfYear, Locale cultureInfo) {
        // ISO uses FirstFourDayWeek, and Monday as first day of week, according to
        // https://en.wikipedia.org/wiki/ISO_8601
        LocalDateTime jan1 = LocalDateTime.of(year, 1, 1, 0, 0);
        Integer daysOffset = DayOfWeek.MONDAY.getValue() - TimexDateHelpers.getUSDayOfWeek(jan1.getDayOfWeek());
        LocalDateTime firstWeekDay = jan1;
        firstWeekDay = firstWeekDay.plusDays(daysOffset);

        TemporalField woy = WeekFields.ISO.weekOfYear();
        Integer firstWeek = jan1.get(woy);

        if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3) {
            weekOfYear -= 1;
        }

        firstWeekDay = firstWeekDay.plusDays(weekOfYear * 7);

        return firstWeekDay;
    }

    private static Pair<String, String> monthWeekDateRange(Integer year, Integer month, Integer weekOfMonth) {
        LocalDateTime start = generateMonthWeekDateStart(year, month, weekOfMonth);
        LocalDateTime end = start.plusDays(7);

        return Pair.of(TimexValue.dateValue(new TimexProperty() {
            {
                setYear(start.getYear());
                setMonth(start.getMonthValue());
                setDayOfMonth(start.getDayOfMonth());
            }
        }), TimexValue.dateValue(new TimexProperty() {
            {
                setYear(end.getYear());
                setMonth(end.getMonthValue());
                setDayOfMonth(end.getDayOfMonth());
            }
        }));
    }

    private static LocalDateTime generateMonthWeekDateStart(Integer year, Integer month, Integer weekOfMonth) {
        LocalDateTime dateInWeek = LocalDateTime.of(year, month, 1 + (weekOfMonth - 1) * 7, 0, 0);

        // Align the date of the week according to Thursday, base on ISO 8601,
        // https://en.wikipedia.org/wiki/ISO_8601
        if (TimexDateHelpers.getUSDayOfWeek(dateInWeek.getDayOfWeek()) > TimexDateHelpers.getUSDayOfWeek(DayOfWeek.THURSDAY)) {
            dateInWeek = dateInWeek.plusDays(7 - TimexDateHelpers.getUSDayOfWeek(dateInWeek.getDayOfWeek()) + 1);
        } else {
            dateInWeek = dateInWeek.plusDays(1 - TimexDateHelpers.getUSDayOfWeek(dateInWeek.getDayOfWeek()));
        }

        return dateInWeek;
    }

    private static LocalDateTime generateWeekDate(TimexProperty timex, LocalDateTime date, boolean isBefore) {
        LocalDateTime start;
        if (timex.getWeekOfMonth() == null && timex.getWeekOfYear() == null) {
            DayOfWeek day = timex.getDayOfWeek() == 7 ? DayOfWeek.SUNDAY : DayOfWeek.of(timex.getDayOfWeek());
            if (isBefore) {
                start = TimexDateHelpers.dateOfLastDay(day, date);
            } else {
                start = TimexDateHelpers.dateOfNextDay(day, date);
            }
        } else {
            Integer dayOfWeek = timex.getDayOfWeek() - 1;
            Integer year = timex.getYear() != null ? timex.getYear() : date.getYear();
            if (timex.getWeekOfYear() != null) {
                Integer weekOfYear = timex.getWeekOfYear();
                start = firstDateOfWeek(year, weekOfYear, Locale.getDefault()).plusDays(dayOfWeek);
                if (timex.getYear() == null) {
                    if (isBefore && start.isAfter(date)) {
                        start = firstDateOfWeek(year - 1, weekOfYear, Locale.getDefault()).plusDays(dayOfWeek);
                    } else if (!isBefore && start.isBefore(date)) {
                        start = firstDateOfWeek(year + 1, weekOfYear, Locale.getDefault()).plusDays(dayOfWeek);
                    }
                }
            } else {
                Integer month = timex.getMonth() != null ? timex.getMonth() : date.getMonthValue();
                Integer weekOfMonth = timex.getWeekOfMonth();
                start = generateMonthWeekDateStart(year, month, weekOfMonth).plusDays(dayOfWeek);
                if (timex.getYear() == null || timex.getMonth() == null) {
                    if (isBefore && start.isAfter(date)) {
                        start = generateMonthWeekDateStart(timex.getMonth() != null ? year - 1 : year,
                                timex.getMonth() == null ? month - 1 : month, weekOfMonth).plusDays(dayOfWeek);
                    } else if (!isBefore && start.isBefore(date)) {
                        start = generateMonthWeekDateStart(timex.getMonth() != null ? year + 1 : year,
                                timex.getMonth() == null ? month + 1 : month, weekOfMonth).plusDays(dayOfWeek);
                    }
                }
            }
        }

        return start;
    }

    private static List<Resolution.Entry> resolveDateRange(TimexProperty timex, LocalDateTime date) {
        if (timex.getSeason() != null) {
            return new ArrayList<Resolution.Entry>() {
                {
                    add(new Resolution.Entry() {
                        {
                            setTimex(timex.getTimexValue());
                            setType("daterange");
                            setValue("not resolved");
                        }
                    });
                }
            };
        } else {
            if (timex.getMonth() != null && timex.getWeekOfMonth() != null) {
                List<Pair<String, String>> yearDateRangeList = getMonthWeekDateRange(
                        timex.getYear() != null ? timex.getYear() : Constants.INVALID_VALUE,
                        timex.getMonth(), timex.getWeekOfMonth(), date.getYear());
                List<Resolution.Entry> result = new ArrayList<Resolution.Entry>();
                for (Pair<String, String> yearDateRange : yearDateRangeList) {
                    result.add(new Resolution.Entry() {
                        {
                            setTimex(timex.getTimexValue());
                            setType("daterange");
                            setStart(yearDateRange.getLeft());
                            setEnd(yearDateRange.getRight());
                        }
                    });
                }

                return result;
            }

            if (timex.getYear() != null && timex.getMonth() != null) {
                Pair<String, String> dateRange = TimexResolver.monthDateRange(timex.getYear(), timex.getMonth());

                return new ArrayList<Resolution.Entry>() {
                    {
                        add(new Resolution.Entry() {
                            {
                                setTimex(timex.getTimexValue());
                                setType("daterange");
                                setStart(dateRange.getLeft());
                                setEnd(dateRange.getRight());
                            }
                        });
                    }
                };
            }

            if (timex.getYear() != null && timex.getWeekOfYear() != null) {
                Pair<String, String> dateRange = TimexResolver.yearWeekDateRange(date.getYear(), timex.getWeekOfYear(),
                        timex.getWeekend());

                return new ArrayList<Resolution.Entry>() {
                    {
                        add(new Resolution.Entry() {
                            {
                                setTimex(timex.getTimexValue());
                                setType("daterange");
                                setStart(dateRange.getLeft());
                                setEnd(dateRange.getRight());
                            }
                        });
                    }
                };
            }

            if (timex.getMonth() != null) {
                Integer y = date.getYear();
                Pair<String, String> lastYearDateRange = TimexResolver.monthDateRange(y - 1, timex.getMonth());
                Pair<String, String> thisYearDateRange = TimexResolver.monthDateRange(y, timex.getMonth());

                return new ArrayList<Resolution.Entry>() {
                    {
                        add(new Resolution.Entry() {
                            {
                                setTimex(timex.getTimexValue());
                                setType("daterange");
                                setStart(lastYearDateRange.getLeft());
                                setEnd(lastYearDateRange.getRight());
                            }
                        });
                        add(new Resolution.Entry() {
                            {
                                setTimex(timex.getTimexValue());
                                setType("daterange");
                                setStart(thisYearDateRange.getLeft());
                                setEnd(thisYearDateRange.getRight());
                            }
                        });
                    }
                };
            }

            if (timex.getYear() != null) {
                Pair<String, String> dateRange = TimexResolver.yearDateRange(timex.getYear());

                return new ArrayList<Resolution.Entry>() {
                    {
                        add(new Resolution.Entry() {
                            {
                                setTimex(timex.getTimexValue());
                                setType("daterange");
                                setStart(dateRange.getLeft());
                                setEnd(dateRange.getRight());
                            }
                        });
                    }
                };
            }

            return new ArrayList<Resolution.Entry>();
        }
    }

    private static Pair<String, String> partOfDayTimeRange(TimexProperty timex) {
        switch (timex.getPartOfDay()) {
            case "MO":
                return Pair.of("08:00:00", "12:00:00");
            case "AF":
                return Pair.of("12:00:00", "16:00:00");
            case "EV":
                return Pair.of("16:00:00", "20:00:00");
            case "NI":
                return Pair.of("20:00:00", "24:00:00");
            default:
        }

        return Pair.of("not resolved", "not resolved");
    }

    private static List<Resolution.Entry> resolveTimeRange(TimexProperty timex, LocalDateTime date) {
        if (timex.getPartOfDay() != null) {
            Pair<String, String> range = TimexResolver.partOfDayTimeRange(timex);
            return new ArrayList<Resolution.Entry>() {
                {
                    add(new Resolution.Entry() {
                        {
                            setTimex(timex.getTimexValue());
                            setType("timerange");
                            setStart(range.getLeft());
                            setEnd(range.getRight());
                        }
                    });
                }
            };
        } else {
            TimexRange range = TimexHelpers.expandTimeRange(timex);
            return new ArrayList<Resolution.Entry>() {
                {
                    add(new Resolution.Entry() {
                        {
                            setTimex(timex.getTimexValue());
                            setType("timerange");
                            setStart(TimexValue.timeValue(range.getStart(), date));
                            setEnd(TimexValue.timeValue(range.getEnd(), date));
                        }
                    });
                }
            };
        }
    }

    private static List<Resolution.Entry> resolveDateTime(TimexProperty timex, LocalDateTime date) {
        List<Resolution.Entry> resolvedDates = TimexResolver.resolveDate(timex, date);
        for (Resolution.Entry resolved : resolvedDates) {
            resolved.setType("datetime");
            resolved.setValue(String.format("%1$s %2$s", resolved.getValue(), TimexValue.timeValue(timex, date)));
        }

        return resolvedDates;
    }

    private static List<String> getDateValues(TimexProperty timex, LocalDateTime date) {
        ArrayList<String> result = new ArrayList<String>();
        if (timex.getYear() != null && timex.getMonth() != null && timex.getDayOfMonth() != null) {
            result.add(TimexValue.dateValue(timex));
        } else {
            result.add(lastDateValue(timex, date));
            if (timex.getYear() == null) {
                result.add(nextDateValue(timex, date));
            }
        }

        return result;
    }

    private static List<Pair<String, String>> getMonthWeekDateRange(Integer year, Integer month, Integer weekOfMonth,
        Integer referYear) {
        List<Pair<String, String>> result = new ArrayList<Pair<String, String>>();
        if (year == Constants.INVALID_VALUE) {
            result.add(monthWeekDateRange(referYear - 1, month, weekOfMonth));
            result.add(monthWeekDateRange(referYear, month, weekOfMonth));
        } else {
            result.add(monthWeekDateRange(year, month, weekOfMonth));
        }

        return result;
    }

    private static List<Resolution.Entry> resolveDateTimeRange(TimexProperty timex, LocalDateTime date) {
        if (timex.getPartOfDay() != null) {
            List<String> dateValues = getDateValues(timex, date);
            Pair<String, String> timeRange = partOfDayTimeRange(timex);
            ArrayList<Resolution.Entry> result = new ArrayList<Resolution.Entry>();
            for (String dateValue : dateValues) {
                result.add(new Resolution.Entry() {
                    {
                        setTimex(timex.getTimexValue());
                        setType("datetimerange");
                        setStart(TimexHelpers.formatResolvedDateValue(dateValue, timeRange.getLeft()));
                        setEnd(TimexHelpers.formatResolvedDateValue(dateValue, timeRange.getRight()));
                    }
                });
            }

            return result;

        } else {
            TimexRange range = TimexHelpers.expandDateTimeRange(timex);
            List<String> startDateValues = getDateValues(range.getStart(), date);
            List<String> endDateValues = getDateValues(range.getEnd(), date);
            List<Resolution.Entry> result = new ArrayList<Resolution.Entry>();
            DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd");
            LocalTime defaultTime = LocalDateTime.MIN.toLocalTime();
            List<DateRange> dateRanges = Streams
                    .zip(startDateValues.stream(), endDateValues.stream(), (n, w) -> new DateRange() {
                        {
                            setStart(LocalDateTime.of(LocalDate.parse(n, formatter), defaultTime));
                            setEnd(LocalDateTime.of(LocalDate.parse(w, formatter), defaultTime));
                        }
                    }).collect(Collectors.toList());
            for (DateRange dateRange : dateRanges) {
                {
                    result.add(new Resolution.Entry() {
                        {
                            setTimex(timex.getTimexValue());
                            setType("datetimerange");
                            setStart(TimexHelpers.formatResolvedDateValue(dateRange.getStart().toLocalDate().toString(),
                                    TimexValue.timeValue(range.getStart(), date)));
                            setEnd(TimexHelpers.formatResolvedDateValue(dateRange.getEnd().toLocalDate().toString(),
                                    TimexValue.timeValue(range.getEnd(), date)));
                        }
                    });
                }
            }

            return result;
        }
    }
}
