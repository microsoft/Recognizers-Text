// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public class TimexProperty
    {
        private Time time;
        private HashSet<string> types;
        private bool? now;
        private decimal? years;
        private decimal? months;
        private decimal? weeks;
        private decimal? days;
        private decimal? hours;
        private decimal? minutes;
        private decimal? seconds;
        private int? year;
        private int? month;
        private int? dayOfMonth;
        private int? dayOfWeek;
        private string season;
        private int? weekOfYear;
        private bool? weekend;
        private int? weekOfMonth;
        private string partOfDay;

        public TimexProperty()
        {
        }

        public TimexProperty(string timex)
        {
            TimexParsing.ParseString(timex, this);
        }

        public TimexProperty(TimexProperty other)
        {
            now = other.Now;
            years = other.Years;
            months = other.Months;
            weeks = other.Weeks;
            days = other.Days;
            hours = other.Hours;
            minutes = other.Minutes;
            seconds = other.Seconds;
            year = other.Year;
            month = other.Month;
            dayOfMonth = other.DayOfMonth;
            dayOfWeek = other.DayOfWeek;
            season = other.Season;
            weekOfYear = other.WeekOfYear;
            weekend = other.Weekend;
            weekOfMonth = other.WeekOfMonth;
            partOfDay = other.PartOfDay;
            time = other.time == null ? null : new Time(other.time.Hour, other.time.Minute, other.time.Second);
        }

        public static TimexProperty Empty { get; } = new TimexProperty();

        public string TimexValue => TimexFormat.Format(this);

        public HashSet<string> Types => types?.Count > 0 ? types : types = TimexInference.Infer(this);

        public bool? Now { get => now; set => SetField(v => now = v, value); }

        public decimal? Years { get => years; set => SetField(v => years = v, value); }

        public decimal? Months { get => months; set => SetField(v => months = v, value); }

        public decimal? Weeks { get => weeks; set => SetField(v => weeks = v, value); }

        public decimal? Days { get => days; set => SetField(v => days = v, value); }

        public decimal? Hours { get => hours; set => SetField(v => hours = v, value); }

        public decimal? Minutes { get => minutes; set => SetField(v => minutes = v, value); }

        public decimal? Seconds { get => seconds; set => SetField(v => seconds = v, value); }

        public int? Year { get => year; set => SetField(v => year = v, value); }

        public int? Month { get => month; set => SetField(v => month = v, value); }

        public int? DayOfMonth { get => dayOfMonth; set => SetField(v => dayOfMonth = v, value); }

        public int? DayOfWeek { get => dayOfWeek; set => SetField(v => dayOfWeek = v, value); }

        public string Season { get => season; set => SetField(v => season = v, value); }

        public int? WeekOfYear { get => weekOfYear; set => SetField(v => weekOfYear = v, value); }

        public bool? Weekend { get => weekend; set => SetField(v => weekend = v, value); }

        public int? WeekOfMonth { get => weekOfMonth; set => SetField(v => weekOfMonth = v, value); }

        public int? Hour
        {
            get => time?.Hour;

            set
            {
                if (value.HasValue)
                {
                    if (time == null)
                    {
                        time = new Time(value.Value, 0, 0);
                    }
                    else
                    {
                        time.Hour = value.Value;
                    }
                }
                else
                {
                    time = null;
                }

                types?.Clear();
            }
        }

        public int? Minute
        {
            get => time?.Minute;

            set
            {
                if (value.HasValue)
                {
                    if (time == null)
                    {
                        time = new Time(0, value.Value, 0);
                    }
                    else
                    {
                        time.Minute = value.Value;
                    }
                }
                else
                {
                    time = null;
                }

                types?.Clear();
            }
        }

        public int? Second
        {
            get => time?.Second;

            set
            {
                if (value.HasValue)
                {
                    if (time == null)
                    {
                        time = new Time(0, 0, value.Value);
                    }
                    else
                    {
                        time.Second = value.Value;
                    }
                }
                else
                {
                    time = null;
                }

                types?.Clear();
            }
        }

        public string PartOfDay { get => partOfDay; set => SetField(v => partOfDay = v, value); }

        public bool IsSeason => Season != null;

        public bool IsPartOfDay => PartOfDay != null;

        public bool IsPresent => Types.Contains(Constants.TimexTypes.Present);

        public bool IsDefinite => Types.Contains(Constants.TimexTypes.Definite);

        public bool IsDateTime => Types.Contains(Constants.TimexTypes.DateTime);

        public bool IsTime => Types.Contains(Constants.TimexTypes.Time);

        public bool IsDate => Types.Contains(Constants.TimexTypes.Date);

        public bool IsTimeRange => Types.Contains(Constants.TimexTypes.TimeRange);

        public bool IsDuration => Types.Contains(Constants.TimexTypes.Duration);

        public bool IsDateRange => Types.Contains(Constants.TimexTypes.DateRange);

        public bool IsDateTimeRange => Types.Contains(Constants.TimexTypes.DateTimeRange);

        public static TimexProperty FromDate(DateObject date)
        {
            return new TimexProperty
            {
                Year = date.Year,
                Month = date.Month,
                DayOfMonth = date.Day,
            };
        }

        public static TimexProperty FromDateTime(DateObject datetime)
        {
            var timex = FromDate(datetime);
            timex.Hour = datetime.Hour;
            timex.Minute = datetime.Minute;
            timex.Second = datetime.Second;
            return timex;
        }

        public static TimexProperty FromTime(Time time)
        {
            return new TimexProperty
            {
                Hour = time.Hour,
                Minute = time.Minute,
                Second = time.Second,
            };
        }

        public override string ToString()
        {
            return TimexConvert.ConvertTimexToString(this);
        }

        public string ToNaturalLanguage(DateObject referenceDate)
        {
            return TimexRelativeConvert.ConvertTimexToStringRelative(this, referenceDate);
        }

        public TimexProperty Clone()
        {
            return new TimexProperty(this);
        }

        public void AssignProperties(IDictionary<string, string> source)
        {
            foreach (var item in source)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    continue;
                }

                switch (item.Key)
                {
                    case "year":
                        Year = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;

                    case "month":
                        Month = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;

                    case "dayOfMonth":
                        DayOfMonth = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;

                    case "dayOfWeek":
                        DayOfWeek = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;

                    case "season":
                        Season = item.Value;
                        break;

                    case "weekOfYear":
                        WeekOfYear = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;

                    case "weekend":
                        Weekend = true;
                        break;

                    case "weekOfMonth":
                        WeekOfMonth = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;

                    case "hour":
                        Hour = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;

                    case "minute":
                        Minute = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;
                    case "second":
                        Second = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;

                    case "partOfDay":
                        PartOfDay = item.Value;
                        break;

                    case "dateUnit":
                        AssignDateDuration(source);
                        break;

                    case "hourAmount":
                        Hours = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;

                    case "minuteAmount":
                        Minutes = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;

                    case "secondAmount":
                        Seconds = int.Parse(item.Value, CultureInfo.InvariantCulture);
                        break;
                }
            }
        }

        private void AssignDateDuration(IDictionary<string, string> source)
        {
            switch (source["dateUnit"])
            {
                case "Y":
                    Years = decimal.Parse(source["amount"], CultureInfo.InvariantCulture);
                    break;

                case "M":
                    Months = decimal.Parse(source["amount"], CultureInfo.InvariantCulture);
                    break;

                case "W":
                    Weeks = decimal.Parse(source["amount"], CultureInfo.InvariantCulture);
                    break;

                case "D":
                    Days = decimal.Parse(source["amount"], CultureInfo.InvariantCulture);
                    break;
            }
        }

        private void SetField<T>(Action<T> set, T value)
        {
            set(value);
            types?.Clear();
        }
    }
}
