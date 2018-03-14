// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.DataTypes.DateTime
{
    public class Timex
    {
        private Time _time;

        public Timex()
        {
        }

        public Timex(string timex)
        {
            TimexParsing.ParseString(timex, this);
        }

        public static Timex FromDate(System.DateTime date)
        {
            return new Timex
            {
                Year = date.Year,
                Month = date.Month,
                DayOfMonth = date.Day
            };
        }
        public static Timex FromDateTime(System.DateTime datetime)
        {
            var timex = FromDate(datetime);
            timex.Hour = datetime.Hour;
            timex.Minute = datetime.Minute;
            timex.Second = datetime.Second;
            return timex;
        }
        public static Timex FromTime(Time time)
        {
            return new Timex
            {
                Hour = time.Hour,
                Minute = time.Minute,
                Second = time.Second
            };
        }

        public string TimexValue
        {
            get
            {
                return TimexFormat.Format(this);
            }
        }

        public HashSet<string> Types
        {
            get
            {
                return TimexInference.Infer(this);
            }
        }

        public override string ToString()
        {
            return TimexConvert.ConvertTimexToString(this);
        }

        public string ToNaturalLanguage(System.DateTime referenceDate)
        {
            return TimexRelativeConvert.ConvertTimexToStringRelative(this, referenceDate);
        }

        public Timex Clone()
        {
            return new Timex
            {
                Now = Now,
                Years = Years,
                Months = Months,
                Weeks = Weeks,
                Days = Days,
                Hours = Hours,
                Minutes = Minutes,
                Seconds = Seconds,
                Year = Year,
                Month = Month,
                DayOfMonth = DayOfMonth,
                DayOfWeek = DayOfWeek,
                Season = Season,
                WeekOfYear = WeekOfYear,
                Weekend = Weekend,
                WeekOfMonth = WeekOfMonth,
                Hour = Hour,
                Minute = Minute,
                Second = Second,
                PartOfDay = PartOfDay
            };
        }

        public bool? Now { get; set; }

        public decimal? Years { get; set; }

        public decimal? Months { get; set; }

        public decimal? Weeks { get; set; }

        public decimal? Days { get; set; }

        public decimal? Hours { get; set; }

        public decimal? Minutes { get; set; }

        public decimal? Seconds { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public int? DayOfMonth { get; set; }

        public int? DayOfWeek { get; set; }

        public string Season { get; set; }

        public int? WeekOfYear { get; set; }

        public bool? Weekend { get; set; }

        public int? WeekOfMonth { get; set; }

        public int? Hour
        {
            get { return _time?.Hour; }
            set
            {
                if (value.HasValue)
                {
                    if (_time == null)
                    {
                        _time = new Time(value.Value, 0, 0);
                    }
                    else
                    {
                        _time.Hour = value.Value;
                    }
                }
                else
                {
                    _time = null;
                }
            }
        }

        public int? Minute
        {
            get { return _time?.Minute; }
            set
            {
                if (value.HasValue)
                {
                    if (_time == null)
                    {
                        _time = new Time(0, value.Value, 0);
                    }
                    else
                    {
                        _time.Minute = value.Value;
                    }
                }
                else
                {
                    _time = null;
                }
            }
        }

        public int? Second
        {
            get { return _time?.Second; }
            set
            {
                if (value.HasValue)
                {
                    if (_time == null)
                    {
                        _time = new Time(0, 0, value.Value);
                    }
                    else
                    {
                        _time.Second = value.Value;
                    }
                }
                else
                {
                    _time = null;
                }
            }
        }

        public string PartOfDay { get; set; }

        public void AssignProperties(IDictionary<string, string> source)
        {
            foreach (var item in source)
            {
                switch (item.Key)
                {
                    case "year":
                        Year = int.Parse(item.Value);
                        break;
                    case "month":
                        Month = int.Parse(item.Value);
                        break;
                    case "dayOfMonth":
                        DayOfMonth = int.Parse(item.Value);
                        break;
                    case "dayOfWeek":
                        DayOfWeek = int.Parse(item.Value);
                        break;
                    case "season":
                        Season = item.Value;
                        break;
                    case "weekOfYear":
                        WeekOfYear = int.Parse(item.Value);
                        break;
                    case "weekend":
                        Weekend = true;
                        break;
                    case "weekOfMonth":
                        WeekOfMonth = int.Parse(item.Value);
                        break;
                    case "hour":
                        Hour = int.Parse(item.Value);
                        break;
                    case "minute":
                        Minute = int.Parse(item.Value);
                        break;
                    case "second":
                        Second = int.Parse(item.Value);
                        break;
                    case "partOfDay":
                        PartOfDay = item.Value;
                        break;
                    case "dateUnit":
                        AssignDateDuration(source);
                        break;
                    case "timeUnit":
                        AssignTimeDuration(source);
                        break;
                }
            }
        }

        private void AssignDateDuration(IDictionary<string, string> source)
        {
            switch (source["dateUnit"])
            {
                case "Y":
                    Years = decimal.Parse(source["amount"]);
                    break;
                case "M":
                    Months = decimal.Parse(source["amount"]);
                    break;
                case "W":
                    Weeks = decimal.Parse(source["amount"]);
                    break;
                case "D":
                    Days = decimal.Parse(source["amount"]);
                    break;
            }
        }

        private void AssignTimeDuration(IDictionary<string, string> source)
        {
            switch (source["timeUnit"])
            {
                case "H":
                    Hours = decimal.Parse(source["amount"]);
                    break;
                case "M":
                    Minutes = decimal.Parse(source["amount"]);
                    break;
                case "S":
                    Seconds = decimal.Parse(source["amount"]);
                    break;
            }
        }
    }
}
