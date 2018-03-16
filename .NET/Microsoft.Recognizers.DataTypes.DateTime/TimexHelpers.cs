// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Recognizers.DataTypes.DateTime
{
    public static class TimexHelpers
    {
        public static TimexRange ExpandDateTimeRange(Timex timex)
        {
            var types = timex.Types.Count != 0 ? timex.Types : TimexInference.Infer(timex);

            if (types.Contains(Constants.TimexTypes.Duration))
            {
                var start = CloneDateTime(timex);
                var duration = CloneDuration(timex);
                return new TimexRange
                {
                    Start = start,
                    End = TimexDateTimeAdd(start, duration),
                    Duration = duration
                };
            }
            else
            {
                if (timex.Year != null)
                {
                    var range = new TimexRange { Start = new Timex { Year = timex.Year }, End = new Timex() };
                    if (timex.Month != null)
                    {
                        range.Start.Month = timex.Month;
                        range.Start.DayOfMonth = 1;
                        range.End.Year = timex.Year;
                        range.End.Month = timex.Month + 1;
                        range.End.DayOfMonth = 1;
                    }
                    else
                    {
                        range.Start.Month = 1;
                        range.Start.DayOfMonth = 1;
                        range.End.Year = timex.Year + 1;
                        range.End.Month = 1;
                        range.End.DayOfMonth = 1;
                    }

                    return range;
                }
            }

            return new TimexRange { Start = new Timex(), End = new Timex() };
        }

        public static TimexRange ExpandTimeRange(Timex timex)
        {
            var start = new Timex { Hour = timex.Hour, Minute = timex.Minute, Second = timex.Second };
            var duration = CloneDuration(timex);

            return new TimexRange
            {
                Start = start,
                End = TimeAdd(start, duration),
                Duration = duration
            };
        }
        public static Timex TimexDateAdd(Timex start, Timex duration)
        {
            if (start.DayOfWeek != null)
            {
                var end = start.Clone();
                if (duration.Days != null)
                {
                    end.DayOfWeek += (int)duration.Days;
                }

                return end;
            }

            if (start.Month != null && start.DayOfMonth != null)
            {
                if (duration.Days != null)
                {
                    if (start.Year != null)
                    {
                        var d = new System.DateTime(start.Year.Value, start.Month.Value, start.DayOfMonth.Value, 0, 0, 0);
                        d = d.AddDays((double)duration.Days.Value);

                        return new Timex {
                            Year = d.Year,
                            Month = d.Month,
                            DayOfMonth = d.Day
                        };
                    }
                    else
                    {
                        var d = new System.DateTime(2001, start.Month.Value, start.DayOfMonth.Value, 0, 0, 0);
                        d = d.AddDays((double)duration.Days.Value);

                        return new Timex
                        {
                            Month = d.Month,
                            DayOfMonth = d.Day
                        };
                    }
                }

                if (duration.Years != null)
                {
                    if (start.Year != null)
                    {
                        return new Timex
                        {
                            Year = (int)(start.Year.Value + duration.Years.Value),
                            Month = start.Month,
                            DayOfMonth = start.DayOfMonth
                        };
                    }
                }

                if (duration.Month != null)
                {
                    if (start.Month != null)
                    {
                        return new Timex
                        {
                            Year = start.Year,
                            Month = (int)(start.Month + duration.Months),
                            DayOfMonth = start.DayOfMonth
                        };
                    }
                }
            }
            return start;
        }

        public static Timex TimexTimeAdd(Timex start, Timex duration)
        {
            if (duration.Hours != null)
            {
                var result = start.Clone();
                result.Hour += (int)duration.Hours.Value;
                if (result.Hour.Value > 23)
                {
                    var days = Math.Floor(result.Hour.Value / 24m);
                    var hour = result.Hour.Value % 24;
                    result.Hour = hour;

                    if (result.Year != null && result.Month != null && result.DayOfMonth != null)
                    {
                        var d = new System.DateTime(result.Year.Value, result.Month.Value, result.DayOfMonth.Value, 0, 0, 0);
                        d = d.AddDays((double)days);

                        result.Year = d.Year;
                        result.Month = d.Month;
                        result.DayOfMonth = d.Day;

                        return result;
                    }

                    if (result.DayOfWeek != null)
                    {
                        result.DayOfWeek += (int)days;
                        return result;
                    }
                }

                return result;
            }

            if (duration.Minutes != null)
            {
                var result = start.Clone();
                result.Minute += (int)duration.Minutes.Value;

                if (result.Minute.Value > 59)
                {
                    result.Hour++;
                    result.Minute = 0;
                }

                return result;
            }

            return start;
        }

        public static Timex TimexDateTimeAdd(Timex start, Timex duration)
        {
            return TimexTimeAdd(TimexDateAdd(start, duration), duration);
        }

        public static System.DateTime DateFromTimex(Timex timex)
        {
            return new System.DateTime(timex.Year ?? 2001, timex.Month ?? 1, timex.DayOfMonth ?? 1, timex.Hour ?? 0, timex.Minute ?? 0, timex.Second ?? 0);
        }

        public static Time TimeFromTimex(Timex timex)
        {
            return new Time(timex.Hour ?? 0, timex.Minute ?? 0, timex.Second ?? 0);
        }

        public static DateRange DateRangeFromTimex(Timex timex)
        {
            var expanded = ExpandDateTimeRange(timex);
            return new DateRange
            {
                Start = DateFromTimex(expanded.Start),
                End = DateFromTimex(expanded.End)
            };
        }

        public static TimeRange TimeRangeFromTimex(Timex timex)
        {
            var expanded = ExpandTimeRange(timex);
            return new TimeRange
            {
                Start = TimeFromTimex(expanded.Start),
                End = TimeFromTimex(expanded.End)
            };
        }

        private static Timex TimeAdd(Timex start, Timex duration)
        {
            return new Timex
            {
                Hour = (int)(start.Hour.Value + duration.Hours ?? 0),
                Minute = (int)(start.Minute + duration.Minutes ?? 0),
                Second = (int)(start.Second + duration.Seconds ?? 0)
            };
        }

        private static Timex CloneDateTime(Timex timex)
        {
            var result = timex.Clone();
            result.Years = null;
            result.Months = null;
            result.Weeks = null;
            result.Days = null;
            result.Hours = null;
            result.Minutes = null;
            result.Seconds = null;
            return result;
        }

        private static Timex CloneDuration(Timex timex)
        {
            var result = timex.Clone();
            result.Year = null;
            result.Month = null;
            result.DayOfMonth = null;
            result.DayOfWeek = null;
            result.WeekOfYear = null;
            result.WeekOfMonth = null;
            result.Season = null;
            result.Hour = null;
            result.Minute = null;
            result.Second = null;
            result.Weekend = null;
            result.PartOfDay = null;
            return result;
        }
    }
}
