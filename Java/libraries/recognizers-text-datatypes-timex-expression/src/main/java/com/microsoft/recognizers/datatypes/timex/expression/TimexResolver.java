// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.time.temporal.TemporalField;
import java.time.temporal.WeekFields;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Locale;

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
        return new ArrayList<Resolution.Entry>() {
            {
                add(new Resolution.Entry() {
                    {
                        setTimex(timex.getTimexValue());
                        setType("date");
                        setValue(TimexResolver.lastDateValue(timex, date));
                    }
                });
                add(new Resolution.Entry() {
                    {
                        setTimex(timex.getTimexValue());
                        setType("date");
                        setValue(TimexResolver.nextDateValue(timex, date));
                    }
                });
            }
        };
    }

    private static String lastDateValue(TimexProperty timex, LocalDateTime date) {
        if (timex.getMonth() != null && timex.getDayOfMonth() != null) {
            return TimexValue.dateValue(new TimexProperty() {
                {
                    setYear(date.getYear() - 1);
                    setMonth(timex.getMonth());
                    setDayOfMonth(timex.getDayOfMonth());
                }
            });
        }

        if (timex.getDayOfWeek() != null) {
            DayOfWeek day = timex.getDayOfWeek() == 7 ? DayOfWeek.SUNDAY : DayOfWeek.of(timex.getDayOfWeek());
            LocalDateTime result = TimexDateHelpers.dateOfLastDay(day, date);
            return TimexValue.dateValue(new TimexProperty() {
                {
                    setYear(result.getYear());
                    setMonth(result.getMonthValue());
                    setDayOfMonth(result.getDayOfMonth());
                }
            });
        }

        return new String();
    }

    private static String nextDateValue(TimexProperty timex, LocalDateTime date) {
        if (timex.getMonth() != null && timex.getDayOfMonth() != null) {
            return TimexValue.dateValue(new TimexProperty() {
                {
                    setYear(date.getYear());
                    setMonth(timex.getMonth());
                    setDayOfMonth(timex.getDayOfMonth());
                }
            });
        }

        if (timex.getDayOfWeek() != null) {
            DayOfWeek day = timex.getDayOfWeek() == 7 ? DayOfWeek.SUNDAY : DayOfWeek.of(timex.getDayOfWeek());
            LocalDateTime result = TimexDateHelpers.dateOfNextDay(day, date);
            return TimexValue.dateValue(new TimexProperty() {
                {
                    setYear(result.getYear());
                    setMonth(result.getMonthValue());
                    setDayOfMonth(result.getDayOfMonth());
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
        Integer daysOffset = DayOfWeek.MONDAY.getValue() - jan1.getDayOfWeek().getValue();
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

    // TODO: research about Pair
    private static Pair<String, String> monthWeekDateRange(Integer year, Integer month, Integer weekOfYear) {
        LocalDateTime dateInWeek = LocalDateTime.of(year, month, 1 + ((weekOfYear - 1) * 7), 0, 0);
        if (dateInWeek.getDayOfWeek() == DayOfWeek.SUNDAY) {
            dateInWeek = dateInWeek.plusDays(1);
        } else if (dateInWeek.getDayOfWeek().getValue() > DayOfWeek.MONDAY.getValue()) {
            dateInWeek = dateInWeek.plusDays(1 - dateInWeek.getDayOfWeek().getValue());
        }

        LocalDateTime start = dateInWeek;
        LocalDateTime end = start;
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
                Pair<String, String> dateRange = TimexResolver.yearWeekDateRange(timex.getYear(), timex.getWeekOfYear(),
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

            if (timex.getMonth() != null && timex.getWeekOfMonth() != null) {
                Pair<String, String> lastYearDateRange = TimexResolver.monthWeekDateRange(date.getYear() - 1,
                        timex.getMonth(), timex.getWeekOfMonth());
                Pair<String, String> thisYearDateRange = TimexResolver.monthWeekDateRange(date.getYear(),
                        timex.getMonth(), timex.getWeekOfMonth());

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

    // TODO: research about Pair
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

    private static List<Resolution.Entry> resolveDateTimeRange(TimexProperty timex, LocalDateTime date) {
        if (timex.getPartOfDay() != null) {
            String dateValue = TimexValue.dateValue(timex);
            Pair<String, String> timeRange = TimexResolver.partOfDayTimeRange(timex);
            return new ArrayList<Resolution.Entry>() {
                {
                    add(new Resolution.Entry() {
                        {
                            setTimex(timex.getTimexValue());
                            setType("datetimerange");
                            setStart(String.format("%1$s %2$s", dateValue, timeRange.getLeft()));
                            setEnd(String.format("%1$s %2$s", dateValue, timeRange.getRight()));
                        }
                    });
                }
            };
        } else {
            TimexRange range = TimexHelpers.expandDateTimeRange(timex);
            return new ArrayList<Resolution.Entry>() {
                {
                    add(new Resolution.Entry() {
                        {
                            setTimex(timex.getTimexValue());
                            setType("datetimerange");
                            setStart(String.format("%1$s %2$s", TimexValue.dateValue(range.getStart()),
                                    TimexValue.timeValue(range.getStart(), date)));
                            setEnd(String.format("%1$s %2$s", TimexValue.dateValue(range.getEnd()),
                                    TimexValue.timeValue(range.getEnd(), date)));
                        }
                    });
                }
            };
        }
    }
}
