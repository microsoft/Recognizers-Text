// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;

public class TimexHelpers {
    public static final HashMap<TimexUnit, String> TIMEX_UNIT_TO_STRING_MAP = new HashMap<TimexUnit, String>() {
        {
            put(TimexUnit.Year, Constants.TIMEX_YEAR);
            put(TimexUnit.Month, Constants.TIMEX_MONTH);
            put(TimexUnit.Week, Constants.TIMEX_WEEK);
            put(TimexUnit.Day, Constants.TIMEX_DAY);
            put(TimexUnit.Hour, Constants.TIMEX_HOUR);
            put(TimexUnit.Minute, Constants.TIMEX_MINUTE);
            put(TimexUnit.Second, Constants.TIMEX_SECOND);
        }
    };

    public static final List<TimexUnit> TimeTimexUnitList = Arrays.asList(TimexUnit.Hour, TimexUnit.Minute,
            TimexUnit.Second);

    public static TimexRange expandDateTimeRange(TimexProperty timex) {
        HashSet<String> types = timex.getTypes().size() != 0 ? timex.getTypes() : TimexInference.infer(timex);

        if (types.contains(Constants.TimexTypes.DURATION)) {
            TimexProperty start = TimexHelpers.cloneDateTime(timex);
            TimexProperty duration = TimexHelpers.cloneDuration(timex);
            return new TimexRange() {
                {
                    setStart(start);
                    setEnd(TimexHelpers.timexDateTimeAdd(start, duration));
                    setDuration(duration);
                }
            };
        } else {
            if (timex.getYear() != null) {
                TimexRange range = new TimexRange() {
                    {
                        setStart(new TimexProperty() {
                            {
                                setYear(timex.getYear());
                            }
                        });
                        setEnd(new TimexProperty());
                    }
                };
                if (timex.getMonth() != null) {
                    range.getStart().setMonth(timex.getMonth());
                    range.getStart().setDayOfMonth(1);
                    range.getEnd().setYear(timex.getYear());
                    range.getEnd().setMonth(timex.getMonth() + 1);
                    range.getEnd().setDayOfMonth(1);
                } else {
                    range.getStart().setMonth(1);
                    range.getStart().setDayOfMonth(1);
                    range.getEnd().setYear(timex.getYear() + 1);
                    range.getEnd().setMonth(1);
                    range.getEnd().setDayOfMonth(1);
                }

                return range;
            }
        }

        return new TimexRange() {
            {
                setStart(new TimexProperty());
                setEnd(new TimexProperty());
            }
        };
    }

    public static TimexRange expandTimeRange(TimexProperty timex) {
        if (!timex.getTypes().contains(Constants.TimexTypes.TIME_RANGE)) {
            throw new IllegalArgumentException("argument must be a timerange: timex");
        }

        if (timex.getPartOfDay() != null) {
            switch (timex.getPartOfDay()) {
                case "DT":
                    timex = new TimexProperty(TimexCreator.DAYTIME);
                    break;
                case "MO":
                    timex = new TimexProperty(TimexCreator.MORNING);
                    break;
                case "AF":
                    timex = new TimexProperty(TimexCreator.AFTERNOON);
                    break;
                case "EV":
                    timex = new TimexProperty(TimexCreator.EVENING);
                    break;
                case "NI":
                    timex = new TimexProperty(TimexCreator.NIGHT);
                    break;
                default:
                    throw new IllegalArgumentException("unrecognized part of day timerange: timex");
            }
        }

        Integer hour = timex.getHour();
        Integer minute = timex.getMinute();
        Integer second = timex.getSecond();
        TimexProperty start = new TimexProperty() {
            {
                setHour(hour);
                setMinute(minute);
                setSecond(second);
            }
        };
        TimexProperty duration = TimexHelpers.cloneDuration(timex);

        return new TimexRange() {
            {
                setStart(start);
                setEnd(TimexHelpers.timeAdd(start, duration));
                setDuration(duration);
            }
        };
    }

    public static TimexProperty timexDateAdd(TimexProperty start, TimexProperty duration) {
        if (start.getDayOfWeek() != null) {
            TimexProperty end = start.clone();
            if (duration.getDays() != null) {
                Integer newDayOfWeek = end.getDayOfWeek() + (int)Math.round(duration.getDays().doubleValue());
                end.setDayOfWeek(newDayOfWeek);
            }

            return end;
        }

        if (start.getMonth() != null && start.getDayOfMonth() != null) {
            Double durationDays = null;
            if (duration.getDays() != null) {
                durationDays = duration.getDays().doubleValue();
            }

            if (durationDays == null && duration.getWeeks() != null) {
                durationDays = 7 * duration.getWeeks().doubleValue();
            }

            if (durationDays != null) {
                if (start.getYear() != null) {
                    LocalDateTime d = LocalDateTime.of(start.getYear(), start.getMonth(), start.getDayOfMonth(), 0, 0,
                            0);
                    LocalDateTime d2 = d.plusDays(durationDays.longValue());
                    return new TimexProperty() {
                        {
                            setYear(d2.getYear());
                            setMonth(d2.getMonthValue());
                            setDayOfMonth(d2.getDayOfMonth());
                        }
                    };
                } else {
                    LocalDateTime d = LocalDateTime.of(2001, start.getMonth(), start.getDayOfMonth(), 0, 0, 0);
                    LocalDateTime d2 = d.plusDays(durationDays.longValue());
                    return new TimexProperty() {
                        {
                            setMonth(d2.getMonthValue());
                            setDayOfMonth(d2.getDayOfMonth());
                        }
                    };
                }
            }

            if (duration.getYears() != null) {
                if (start.getYear() != null) {
                    return new TimexProperty() {
                        {
                            setYear(start.getYear() + (int)Math.round(duration.getYears().doubleValue()));
                            setMonth(start.getMonth());
                            setDayOfMonth(start.getDayOfMonth());
                        }
                    };
                }
            }

            if (duration.getMonths() != null) {
                if (start.getMonth() != null) {
                    return new TimexProperty() {
                        {
                            setYear(start.getYear());
                            setMonth(start.getMonth() + (int)Math.round(duration.getMonths().doubleValue()));
                            setDayOfMonth(start.getDayOfMonth());
                        }
                    };
                }
            }
        }

        return start;
    }

    public static String generateCompoundDurationTimex(List<String> timexList) {
        Boolean isTimeDurationAlreadyExist = false;
        StringBuilder timexBuilder = new StringBuilder(Constants.GENERAL_PERIOD_PREFIX);

        for (String timexComponent : timexList) {
            // The Time Duration component occurs first time
            if (!isTimeDurationAlreadyExist && isTimeDurationTimex(timexComponent)) {
                timexBuilder.append(Constants.TIME_TIMEX_PREFIX.concat(getDurationTimexWithoutPrefix(timexComponent)));
                isTimeDurationAlreadyExist = true;
            } else {
                timexBuilder.append(getDurationTimexWithoutPrefix(timexComponent));
            }
        }

        return timexBuilder.toString();
    }

    public static String generateDateTimex(Integer year, Integer monthOrWeekOfYear, Integer day, Integer weekOfMonth,
            boolean byWeek) {
        String yearString = year == Constants.INVALID_VALUE ? Constants.TIMEX_FUZZY_YEAR
                : TimexDateHelpers.fixedFormatNumber(year, 4);
        String monthWeekString = monthOrWeekOfYear == Constants.INVALID_VALUE ? Constants.TIMEX_FUZZY_MONTH
                : TimexDateHelpers.fixedFormatNumber(monthOrWeekOfYear, 2);
        String dayString;
        if (byWeek) {
            dayString = day.toString();
            if (weekOfMonth != Constants.INVALID_VALUE) {
                monthWeekString = monthWeekString + String.format("-%s-", Constants.TIMEX_FUZZY_WEEK)
                        + weekOfMonth.toString();
            } else {
                monthWeekString = Constants.TIMEX_WEEK + monthWeekString;
            }
        } else {
            dayString = day == Constants.INVALID_VALUE ? Constants.TIMEX_FUZZY_DAY
                    : TimexDateHelpers.fixedFormatNumber(day, 2);
        }

        return String.join("-", yearString, monthWeekString, dayString);
    }

    public static String generateDurationTimex(TimexUnit unit, BigDecimal value) {
        if (value.intValue() == Constants.INVALID_VALUE) {
            return new String();
        }

        StringBuilder timexBuilder = new StringBuilder(Constants.GENERAL_PERIOD_PREFIX);
        if (TimeTimexUnitList.contains(unit)) {
            timexBuilder.append(Constants.TIME_TIMEX_PREFIX);
        }

        timexBuilder.append(value.toString());
        timexBuilder.append(TIMEX_UNIT_TO_STRING_MAP.get(unit));
        return timexBuilder.toString();
    }

    public static TimexProperty timexTimeAdd(TimexProperty start, TimexProperty duration) {

        TimexProperty result = start.clone();
        if (duration.getMinutes() != null) {
            result.setMinute(result.getMinute() + (int)Math.round(duration.getMinutes().doubleValue()));

            if (result.getMinute() > 59) {
                result.setHour(((result.getHour() != null) ? result.getHour() : 0) + 1);
                result.setMinute(result.getMinute() % 60);
            }
        }

        if (duration.getHours() != null) {
            result.setHour(result.getHour() + (int)Math.round(duration.getHours().doubleValue()));
        }

        if (result.getHour() != null && result.getHour() > 23) {
            Double days = Math.floor(result.getHour() / 24d);
            Integer hour = result.getHour() % 24;
            result.setHour(hour);

            if (result.getYear() != null && result.getMonth() != null && result.getDayOfMonth() != null) {
                LocalDateTime d = LocalDateTime.of(result.getYear(), result.getMonth(), result.getDayOfMonth(), 0, 0,
                        0);
                d = d.plusDays(days.longValue());

                result.setYear(d.getYear());
                result.setMonth(d.getMonthValue());
                result.setDayOfMonth(d.getDayOfMonth());

                return result;
            }

            if (result.getDayOfWeek() != null) {
                result.setDayOfWeek(result.getDayOfWeek() + (int)Math.round(days));
                return result;
            }
        }

        return result;
    }

    public static TimexProperty timexDateTimeAdd(TimexProperty start, TimexProperty duration) {
        return TimexHelpers.timexTimeAdd(TimexHelpers.timexDateAdd(start, duration), duration);
    }

    public static LocalDateTime dateFromTimex(TimexProperty timex) {
        Integer year = timex.getYear() != null ? timex.getYear() : 2001;
        Integer month = timex.getMonth() != null ? timex.getMonth() : 1;
        Integer day = timex.getDayOfMonth() != null ? timex.getDayOfMonth() : 1;
        Integer hour = timex.getHour() != null ? timex.getHour() : 0;
        Integer minute = timex.getMinute() != null ? timex.getMinute() : 0;
        Integer second = timex.getSecond() != null ? timex.getSecond() : 0;
        LocalDateTime date = LocalDateTime.of(year, month, day, hour, minute, second);

        return date;
    }

    public static Time timeFromTimex(TimexProperty timex) {
        Integer hour = timex.getHour() != null ? timex.getHour() : 0;
        Integer minute = timex.getMinute() != null ? timex.getMinute() : 0;
        Integer second = timex.getSecond() != null ? timex.getSecond() : 0;
        return new Time(hour, minute, second);
    }

    public static DateRange dateRangeFromTimex(TimexProperty timex) {
        TimexRange expanded = TimexHelpers.expandDateTimeRange(timex);
        return new DateRange() {
            {
                setStart(TimexHelpers.dateFromTimex(expanded.getStart()));
                setEnd(TimexHelpers.dateFromTimex(expanded.getEnd()));
            }
        };
    }

    public static TimeRange timeRangeFromTimex(TimexProperty timex) {
        TimexRange expanded = TimexHelpers.expandTimeRange(timex);
        return new TimeRange() {
            {
                setStart(TimexHelpers.timeFromTimex(expanded.getStart()));
                setEnd(TimexHelpers.timeFromTimex(expanded.getEnd()));
            }
        };
    }

    public static String formatResolvedDateValue(String dateValue, String timeValue) {
        return String.format("%1$s %2$s", dateValue, timeValue);
    }

    private static TimexProperty timeAdd(TimexProperty start, TimexProperty duration) {
        Integer second = start.getSecond() + (int)(duration.getSeconds() != null ? duration.getSeconds().intValue() : 0);
        Integer minute = start.getMinute() + second / 60
                + (duration.getMinutes() != null ? duration.getMinutes().intValue() : 0);
        Integer hour = start.getHour() + (minute / 60) + (duration.getHours() != null ? duration.getHours().intValue() : 0);

        return new TimexProperty() {
            {
                setHour((hour == 24 && minute % 60 == 0 && second % 60 == 0) ? hour : hour % 24);
                setMinute(minute % 60);
                setSecond(second % 60);
            }
        };
    }

    private static TimexProperty cloneDateTime(TimexProperty timex) {
        TimexProperty result = timex.clone();
        result.setYears(null);
        result.setMonths(null);
        result.setWeeks(null);
        result.setDays(null);
        result.setHours(null);
        result.setMinutes(null);
        result.setSeconds(null);
        return result;
    }

    private static TimexProperty cloneDuration(TimexProperty timex) {
        TimexProperty result = timex.clone();
        result.setYear(null);
        result.setMonth(null);
        result.setDayOfMonth(null);
        result.setDayOfWeek(null);
        result.setWeekOfYear(null);
        result.setWeekOfMonth(null);
        result.setSeason(null);
        result.setHour(null);
        result.setMinute(null);
        result.setSecond(null);
        result.setWeekend(null);
        result.setPartOfDay(null);
        return result;
    }

    private static Boolean isTimeDurationTimex(String timex) {
        return timex.startsWith(Constants.GENERAL_PERIOD_PREFIX.concat(Constants.TIME_TIMEX_PREFIX));
    }

    private static String getDurationTimexWithoutPrefix(String timex) {
        // Remove "PT" prefix for TimeDuration, Remove "P" prefix for DateDuration
        return timex.substring(isTimeDurationTimex(timex) ? 2 : 1);
    }
}
