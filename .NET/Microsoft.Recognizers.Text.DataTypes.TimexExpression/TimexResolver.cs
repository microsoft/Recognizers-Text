// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
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
                return ResolveDateTimeRange(timex);
            }

            if (types.Contains(Constants.TimexTypes.Definite) && types.Contains(Constants.TimexTypes.Time))
            {
                return ResolveDefiniteTime(timex);
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
                return ResolveTimeRange(timex);
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
                return ResolveTime(timex);
            }

            return new List<Resolution.Entry>();
        }

        private static List<Resolution.Entry> ResolveDefiniteTime(TimexProperty timex)
        {
            return new List<Resolution.Entry>
            {
                new Resolution.Entry
                {
                    Timex = timex.TimexValue,
                    Type = "datetime",
                    Value = $"{TimexValue.DateValue(timex)} {TimexValue.TimeValue(timex)}",
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
            return new List<Resolution.Entry>
            {
                new Resolution.Entry
                {
                    Timex = timex.TimexValue,
                    Type = "date",
                    Value = LastDateValue(timex, date),
                },
                new Resolution.Entry
                {
                    Timex = timex.TimexValue,
                    Type = "date",
                    Value = NextDateValue(timex, date),
                },
            };
        }

        private static string LastDateValue(TimexProperty timex, DateObject date)
        {
            if (timex.Month != null && timex.DayOfMonth != null)
            {
                return TimexValue.DateValue(new TimexProperty
                {
                    Year = date.Year - 1,
                    Month = timex.Month,
                    DayOfMonth = timex.DayOfMonth,
                });
            }

            if (timex.DayOfWeek != null)
            {
                var day = timex.DayOfWeek == 7 ? DayOfWeek.Sunday : (DayOfWeek)timex.DayOfWeek;
                var result = TimexDateHelpers.DateOfLastDay(day, date);
                return TimexValue.DateValue(new TimexProperty
                {
                    Year = result.Year,
                    Month = result.Month,
                    DayOfMonth = result.Day,
                });
            }

            return string.Empty;
        }

        private static string NextDateValue(TimexProperty timex, DateObject date)
        {
            if (timex.Month != null && timex.DayOfMonth != null)
            {
                return TimexValue.DateValue(new TimexProperty
                {
                    Year = date.Year,
                    Month = timex.Month,
                    DayOfMonth = timex.DayOfMonth,
                });
            }

            if (timex.DayOfWeek != null)
            {
                var day = timex.DayOfWeek == 7 ? DayOfWeek.Sunday : (DayOfWeek)timex.DayOfWeek;
                var result = TimexDateHelpers.DateOfNextDay(day, date);
                return TimexValue.DateValue(new TimexProperty
                {
                    Year = result.Year,
                    Month = result.Month,
                    DayOfMonth = result.Day,
                });
            }

            return string.Empty;
        }

        private static List<Resolution.Entry> ResolveTime(TimexProperty timex)
        {
            return new List<Resolution.Entry>
            {
                new Resolution.Entry
                {
                    Timex = timex.TimexValue,
                    Type = "time",
                    Value = TimexValue.TimeValue(timex),
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
            return new Tuple<string, string>(
                TimexValue.DateValue(new TimexProperty { Year = year, Month = 1, DayOfMonth = 1 }),
                TimexValue.DateValue(new TimexProperty { Year = year + 1, Month = 1, DayOfMonth = 1 }));
        }

        private static Tuple<string, string> MonthDateRange(int year, int month)
        {
            return new Tuple<string, string>(
                TimexValue.DateValue(new TimexProperty { Year = year, Month = month, DayOfMonth = 1 }),
                TimexValue.DateValue(new TimexProperty { Year = year, Month = month + 1, DayOfMonth = 1 }));
        }

        private static Tuple<string, string> WeekDateRange(int year, int weekOfYear)
        {
            var dateInWeek = new DateObject(year, 1, 1) + TimeSpan.FromDays((weekOfYear - 1) * 7);

            var start = TimexDateHelpers.DateOfLastDay(DayOfWeek.Monday, dateInWeek);
            var end = TimexDateHelpers.DateOfLastDay(DayOfWeek.Monday, dateInWeek + TimeSpan.FromDays(7));

            return new Tuple<string, string>(
                TimexValue.DateValue(new TimexProperty { Year = start.Year, Month = start.Month, DayOfMonth = start.Day }),
                TimexValue.DateValue(new TimexProperty { Year = end.Year, Month = end.Month, DayOfMonth = end.Day }));
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
                    var dateRange = WeekDateRange(timex.Year.Value, timex.WeekOfYear.Value);

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

        private static List<Resolution.Entry> ResolveTimeRange(TimexProperty timex)
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
                        Start = TimexValue.TimeValue(range.Start),
                        End = TimexValue.TimeValue(range.End),
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
                resolved.Value = $"{resolved.Value} {TimexValue.TimeValue(timex)}";
            }

            return resolvedDates;
        }

        private static List<Resolution.Entry> ResolveDateTimeRange(TimexProperty timex)
        {
            if (timex.PartOfDay != null)
            {
                var date = TimexValue.DateValue(timex);
                var timeRange = PartOfDayTimeRange(timex);
                return new List<Resolution.Entry>
                {
                    new Resolution.Entry
                    {
                        Timex = timex.TimexValue,
                        Type = "datetimerange",
                        Start = $"{date} {timeRange.Item1}",
                        End = $"{date} {timeRange.Item2}",
                    },
                };
            }
            else
            {
                var range = TimexHelpers.ExpandDateTimeRange(timex);
                return new List<Resolution.Entry>
                {
                    new Resolution.Entry
                    {
                        Timex = timex.TimexValue,
                        Type = "datetimerange",
                        Start = $"{TimexValue.DateValue(range.Start)} {TimexValue.TimeValue(range.Start)}",
                        End = $"{TimexValue.DateValue(range.End)} {TimexValue.TimeValue(range.End)}",
                    },
                };
            }
        }
    }
}
