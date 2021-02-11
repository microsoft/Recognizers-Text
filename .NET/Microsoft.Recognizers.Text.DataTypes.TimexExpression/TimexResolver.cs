// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexResolver
    {
        public static Resolution Resolve(string[] timexArray, DateObject date = default(DateObject))
        {
            var resolution = new Resolution();
            foreach (var timex in timexArray)
            {
                var t = new TimexProperty(timex);
                var r = ResolveTimex(t, date);
                resolution.Values.AddRange(r);
            }

            return resolution;
        }

        private static List<Resolution.Entry> ResolveTimex(TimexProperty timex, DateObject date)
        {
            var types = timex.Types.Count != 0 ? timex.Types : TimexInference.Infer(timex);

            if (types.Contains(Constants.TimexTypes.DateTimeRange))
            {
                return ResolveDateTimeRange(timex, date);
            }

            if (types.Contains(Constants.TimexTypes.Definite) && types.Contains(Constants.TimexTypes.Time))
            {
                return ResolveDefiniteTime(timex, date);
            }

            if (types.Contains(Constants.TimexTypes.Definite) && types.Contains(Constants.TimexTypes.DateRange))
            {
                return ResolveDefiniteDateRange(timex, date);
            }

            if (types.Contains(Constants.TimexTypes.DateRange))
            {
                return ResolveDateRange(timex, date);
            }

            if (types.Contains(Constants.TimexTypes.Definite))
            {
                return ResolveDefinite(timex);
            }

            if (types.Contains(Constants.TimexTypes.TimeRange))
            {
                return ResolveTimeRange(timex, date);
            }

            if (types.Contains(Constants.TimexTypes.DateTime))
            {
                return ResolveDateTime(timex, date);
            }

            if (types.Contains(Constants.TimexTypes.Duration))
            {
                return ResolveDuration(timex);
            }

            if (types.Contains(Constants.TimexTypes.Date))
            {
                return ResolveDate(timex, date);
            }

            if (types.Contains(Constants.TimexTypes.Time))
            {
                return ResolveTime(timex, date);
            }

            return new List<Resolution.Entry>();
        }

        private static List<Resolution.Entry> ResolveDefiniteTime(TimexProperty timex, DateObject date)
        {
            return new List<Resolution.Entry>
            {
                new Resolution.Entry
                {
                    Timex = timex.TimexValue,
                    Type = "datetime",
                    Value = $"{TimexValue.DateValue(timex)} {TimexValue.TimeValue(timex, date)}",
                },
            };
        }

        private static List<Resolution.Entry> ResolveDefinite(TimexProperty timex)
        {
            return new List<Resolution.Entry>
            {
                new Resolution.Entry
                {
                    Timex = timex.TimexValue,
                    Type = "date",
                    Value = TimexValue.DateValue(timex),
                },
            };
        }

        private static List<Resolution.Entry> ResolveDefiniteDateRange(TimexProperty timex, DateObject date)
        {
            var range = TimexHelpers.ExpandDateTimeRange(timex);

            return new List<Resolution.Entry>
            {
                new Resolution.Entry
                {
                    Timex = timex.TimexValue,
                    Type = "daterange",
                    Start = TimexValue.DateValue(range.Start),
                    End = TimexValue.DateValue(range.End),
                },
            };
        }

        private static List<Resolution.Entry> ResolveDate(TimexProperty timex, DateObject date)
        {
            var dateValueList = GetDateValues(timex, date);
            var result = new List<Resolution.Entry> { };
            foreach (string dateValue in dateValueList)
            {
                result.Add(new Resolution.Entry
                {
                    Timex = timex.TimexValue,
                    Type = "date",
                    Value = dateValue,
                });
            }

            return result;
        }

        private static string LastDateValue(TimexProperty timex, DateObject date)
        {
            if (timex.DayOfMonth != null)
            {
                var year = date.Year;
                var month = date.Month;
                if (timex.Month != null)
                {
                    month = timex.Month.Value;
                    if (date.Month <= month || (date.Month == month && date.Day <= timex.DayOfMonth))
                    {
                        year--;
                    }
                }
                else
                {
                    if (date.Day <= timex.DayOfMonth)
                    {
                        month--;
                        if (month < 1)
                        {
                            month = (month + 12) % 12;
                            year--;
                        }
                    }
                }

                return TimexValue.DateValue(new TimexProperty
                {
                    Year = year,
                    Month = month,
                    DayOfMonth = timex.DayOfMonth,
                });
            }

            if (timex.DayOfWeek != null)
            {
                var start = GenerateWeekDate(timex, date, true);
                return TimexValue.DateValue(new TimexProperty
                {
                    Year = start.Year,
                    Month = start.Month,
                    DayOfMonth = start.Day,
                });
            }

            return string.Empty;
        }

        private static string NextDateValue(TimexProperty timex, DateObject date)
        {
            if (timex.DayOfMonth != null)
            {
                var year = date.Year;
                var month = date.Month;
                if (timex.Month != null)
                {
                    month = timex.Month.Value;
                    if (date.Month > month || (date.Month == month && date.Day > timex.DayOfMonth))
                    {
                        year++;
                    }
                }
                else
                {
                    if (date.Day > timex.DayOfMonth)
                    {
                        month++;
                        if (month > 12)
                        {
                            month = month % 12;
                            year--;
                        }
                    }
                }

                return TimexValue.DateValue(new TimexProperty
                {
                    Year = year,
                    Month = month,
                    DayOfMonth = timex.DayOfMonth,
                });
            }

            if (timex.DayOfWeek != null)
            {
                var start = GenerateWeekDate(timex, date, false);
                return TimexValue.DateValue(new TimexProperty
                {
                    Year = start.Year,
                    Month = start.Month,
                    DayOfMonth = start.Day,
                });
            }

            return string.Empty;
        }

        private static List<Resolution.Entry> ResolveTime(TimexProperty timex, DateObject date)
        {
            return new List<Resolution.Entry>
            {
                new Resolution.Entry
                {
                    Timex = timex.TimexValue,
                    Type = "time",
                    Value = TimexValue.TimeValue(timex, date),
                },
            };
        }

        private static List<Resolution.Entry> ResolveDuration(TimexProperty timex)
        {
            return new List<Resolution.Entry>
            {
                new Resolution.Entry
                {
                    Timex = timex.TimexValue,
                    Type = "duration",
                    Value = TimexValue.DurationValue(timex),
                },
            };
        }

        private static Tuple<string, string> YearDateRange(int year)
        {
            var yearDateRange = TimexHelpers.YearDateRange(year);

            return new Tuple<string, string>(
                TimexValue.DateValue(yearDateRange.Item1),
                TimexValue.DateValue(yearDateRange.Item2));
        }

        private static Tuple<string, string> MonthDateRange(int year, int month)
        {
            var monthDateRange = TimexHelpers.MonthDateRange(year, month);

            return new Tuple<string, string>(
                TimexValue.DateValue(monthDateRange.Item1),
                TimexValue.DateValue(monthDateRange.Item2));
        }

        private static Tuple<string, string> YearWeekDateRange(int year, int weekOfYear, bool? isWeekend)
        {
            var yearWeekDateRange = TimexHelpers.YearWeekDateRange(year, weekOfYear, isWeekend);

            return new Tuple<string, string>(
                TimexValue.DateValue(yearWeekDateRange.Item1),
                TimexValue.DateValue(yearWeekDateRange.Item2));
        }

        private static Tuple<string, string> MonthWeekDateRange(int year, int month, int weekOfMonth)
        {
            var monthWeekDateRange = TimexHelpers.MonthWeekDateRange(year, month, weekOfMonth);

            return new Tuple<string, string>(
                TimexValue.DateValue(monthWeekDateRange.Item1),
                TimexValue.DateValue(monthWeekDateRange.Item2));
        }

        private static DateObject GenerateWeekDate(TimexProperty timex, DateObject date, bool isBefore)
        {
            DateObject start;
            if (timex.WeekOfMonth == null && timex.WeekOfYear == null)
            {
                var day = timex.DayOfWeek == 7 ? DayOfWeek.Sunday : (DayOfWeek)timex.DayOfWeek;
                if (isBefore)
                {
                    start = TimexDateHelpers.DateOfLastDay(day, date);
                }
                else
                {
                    start = TimexDateHelpers.DateOfNextDay(day, date);
                }
            }
            else
            {
                int dayOfWeek = timex.DayOfWeek.Value - 1;
                int year = timex.Year ?? date.Year;
                if (timex.WeekOfYear != null)
                {
                    int weekOfYear = timex.WeekOfYear.Value;
                    start = TimexHelpers.FirstDateOfWeek(year, weekOfYear, CultureInfo.InvariantCulture).AddDays(dayOfWeek);
                    if (timex.Year == null)
                    {
                        if (isBefore && start > date)
                        {
                            start = TimexHelpers.FirstDateOfWeek(year - 1, weekOfYear, CultureInfo.InvariantCulture).AddDays(dayOfWeek);
                        }
                        else if (!isBefore && start < date)
                        {
                            start = TimexHelpers.FirstDateOfWeek(year + 1, weekOfYear, CultureInfo.InvariantCulture).AddDays(dayOfWeek);
                        }
                    }
                }
                else
                {
                    int month = timex.Month ?? date.Month;
                    int weekOfMonth = timex.WeekOfMonth.Value;
                    start = TimexHelpers.GenerateMonthWeekDateStart(year, month, weekOfMonth).AddDays(dayOfWeek);
                    if (timex.Year == null || timex.Month == null)
                    {
                        if (isBefore && start > date)
                        {
                            start = TimexHelpers.GenerateMonthWeekDateStart(timex.Month != null ? year - 1 : year, timex.Month == null ? month - 1 : month, weekOfMonth).AddDays(dayOfWeek);
                        }
                        else if (!isBefore && start < date)
                        {
                            start = TimexHelpers.GenerateMonthWeekDateStart(timex.Month != null ? year + 1 : year, timex.Month == null ? month + 1 : month, weekOfMonth).AddDays(dayOfWeek);
                        }
                    }
                }
            }

            return start;
        }

        private static List<Resolution.Entry> ResolveDateRange(TimexProperty timex, DateObject date)
        {
            if (timex.Season != null)
            {
                return new List<Resolution.Entry>
                {
                    new Resolution.Entry
                    {
                        Timex = timex.TimexValue,
                        Type = "daterange",
                        Value = "not resolved",
                    },
                };
            }
            else
            {
                if (timex.Month != null && timex.WeekOfMonth != null)
                {
                    var yearDateRangeList = GetMonthWeekDateRange(timex.Year ?? Constants.InvalidValue, timex.Month.Value, timex.WeekOfMonth.Value, date.Year);
                    var result = new List<Resolution.Entry> { };
                    foreach (Tuple<string, string> yearDateRange in yearDateRangeList)
                    {
                        result.Add(new Resolution.Entry
                        {
                            Timex = timex.TimexValue,
                            Type = "daterange",
                            Start = yearDateRange.Item1,
                            End = yearDateRange.Item2,
                        });
                    }

                    return result;
                }

                if (timex.Year != null && timex.Month != null)
                {
                    var dateRange = MonthDateRange(timex.Year.Value, timex.Month.Value);
                    return new List<Resolution.Entry>
                    {
                        new Resolution.Entry
                        {
                            Timex = timex.TimexValue,
                            Type = "daterange",
                            Start = dateRange.Item1,
                            End = dateRange.Item2,
                        },
                    };
                }

                if (timex.Year != null && timex.WeekOfYear != null)
                {
                    var dateRange = YearWeekDateRange(timex.Year.Value, timex.WeekOfYear.Value, timex.Weekend);

                    return new List<Resolution.Entry>
                    {
                        new Resolution.Entry
                        {
                            Timex = timex.TimexValue,
                            Type = "daterange",
                            Start = dateRange.Item1,
                            End = dateRange.Item2,
                        },
                    };
                }

                if (timex.Month != null)
                {
                    var y = date.Year;
                    var lastYearDateRange = MonthDateRange(y - 1, timex.Month.Value);
                    var thisYearDateRange = MonthDateRange(y, timex.Month.Value);

                    return new List<Resolution.Entry>
                    {
                        new Resolution.Entry
                        {
                            Timex = timex.TimexValue,
                            Type = "daterange",
                            Start = lastYearDateRange.Item1,
                            End = lastYearDateRange.Item2,
                        },
                        new Resolution.Entry
                        {
                            Timex = timex.TimexValue,
                            Type = "daterange",
                            Start = thisYearDateRange.Item1,
                            End = thisYearDateRange.Item2,
                        },
                    };
                }

                if (timex.Year != null)
                {
                    var dateRange = YearDateRange(timex.Year.Value);

                    return new List<Resolution.Entry>
                    {
                        new Resolution.Entry
                        {
                            Timex = timex.TimexValue,
                            Type = "daterange",
                            Start = dateRange.Item1,
                            End = dateRange.Item2,
                        },
                    };
                }

                return new List<Resolution.Entry>();
            }
        }

        private static Tuple<string, string> PartOfDayTimeRange(TimexProperty timex)
        {
            switch (timex.PartOfDay)
            {
                case "MO": return new Tuple<string, string>("08:00:00", "12:00:00");
                case "AF": return new Tuple<string, string>("12:00:00", "16:00:00");
                case "EV": return new Tuple<string, string>("16:00:00", "20:00:00");
                case "NI": return new Tuple<string, string>("20:00:00", "24:00:00");
            }

            return new Tuple<string, string>("not resolved", "not resolved");
        }

        private static List<Resolution.Entry> ResolveTimeRange(TimexProperty timex, DateObject date)
        {
            if (timex.PartOfDay != null)
            {
                var range = PartOfDayTimeRange(timex);
                return new List<Resolution.Entry>
                {
                    new Resolution.Entry
                    {
                        Timex = timex.TimexValue,
                        Type = "timerange",
                        Start = range.Item1,
                        End = range.Item2,
                    },
                };
            }
            else
            {
                var range = TimexHelpers.ExpandTimeRange(timex);
                return new List<Resolution.Entry>
                {
                    new Resolution.Entry
                    {
                        Timex = timex.TimexValue,
                        Type = "timerange",
                        Start = TimexValue.TimeValue(range.Start, date),
                        End = TimexValue.TimeValue(range.End, date),
                    },
                };
            }
        }

        private static List<Resolution.Entry> ResolveDateTime(TimexProperty timex, DateObject date)
        {
            var resolvedDates = ResolveDate(timex, date);
            foreach (var resolved in resolvedDates)
            {
                resolved.Type = "datetime";
                resolved.Value = $"{resolved.Value} {TimexValue.TimeValue(timex, date)}";
            }

            return resolvedDates;
        }

        private static List<string> GetDateValues(TimexProperty timex, DateObject date)
        {
            List<string> result = new List<string> { };
            if (timex.Year != null && timex.Month != null && timex.DayOfMonth != null)
            {
                result.Add(TimexValue.DateValue(timex));
            }
            else
            {
                result.Add(LastDateValue(timex, date));
                if (timex.Year == null)
                {
                    result.Add(NextDateValue(timex, date));
                }
            }

            return result;
        }

        private static List<Tuple<string, string>> GetMonthWeekDateRange(int year, int month, int weekOfMonth, int referYear)
        {
            var result = new List<Tuple<string, string>> { };
            if (year == Constants.InvalidValue)
            {
                result.Add(MonthWeekDateRange(referYear - 1, month, weekOfMonth));
                result.Add(MonthWeekDateRange(referYear, month, weekOfMonth));
            }
            else
            {
                result.Add(MonthWeekDateRange(year, month, weekOfMonth));
            }

            return result;
        }

        private static List<Resolution.Entry> ResolveDateTimeRange(TimexProperty timex, DateObject date)
        {
            if (timex.PartOfDay != null)
            {
                var dateValues = GetDateValues(timex, date);
                var timeRange = PartOfDayTimeRange(timex);
                var result = new List<Resolution.Entry> { };
                foreach (string dateValue in dateValues)
                {
                    result.Add(
                        new Resolution.Entry
                        {
                            Timex = timex.TimexValue,
                            Type = "datetimerange",
                            Start = TimexHelpers.FormatResolvedDateValue(dateValue, timeRange.Item1),
                            End = TimexHelpers.FormatResolvedDateValue(dateValue, timeRange.Item2),
                        });
                }

                return result;
            }
            else
            {
                var range = TimexHelpers.ExpandDateTimeRange(timex);
                var startDateValues = GetDateValues(range.Start, date);
                var endDateValues = GetDateValues(range.End, date);
                var result = new List<Resolution.Entry> { };
                foreach (var dateRange in startDateValues.Zip(endDateValues, (n, w) => new { start = n, end = w }))
                {
                    result.Add(
                        new Resolution.Entry
                        {
                            Timex = timex.TimexValue,
                            Type = "datetimerange",
                            Start = TimexHelpers.FormatResolvedDateValue(dateRange.start, TimexValue.TimeValue(range.Start, date)),
                            End = TimexHelpers.FormatResolvedDateValue(dateRange.end, TimexValue.TimeValue(range.End, date)),
                        });
                }

                return result;
            }
        }
    }
}
