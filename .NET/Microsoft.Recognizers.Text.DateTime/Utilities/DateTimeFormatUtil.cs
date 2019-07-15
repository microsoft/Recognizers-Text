using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class DateTimeFormatUtil
    {
        private static readonly Regex HourTimexRegex = new Regex(@"(?<!P)T(\d{2})");
        private static readonly Regex WeekDayTimexRegex = new Regex(@"XXXX-WXX-(\d)");

        public static string LuisDate(int year)
        {
            if (year == Constants.InvalidYear)
            {
                return Constants.TimexFuzzyYear;
            }

            return year.ToString("D4");
        }

        public static string LuisDate(int year, int month)
        {
            if (year == Constants.InvalidYear)
            {
                if (month == Constants.InvalidMonth)
                {
                    return string.Join(Constants.DateTimexConnector, Constants.TimexFuzzyYear, Constants.TimexFuzzyMonth);
                }

                return string.Join(Constants.DateTimexConnector, Constants.TimexFuzzyYear, month.ToString("D2"));
            }

            return string.Join(Constants.DateTimexConnector, year.ToString("D4"), month.ToString("D2"));
        }

        public static string LuisDate(int year, int month, int day)
        {
            if (year == -1)
            {
                if (month == -1)
                {
                    if (day == -1)
                    {
                        return string.Join(Constants.DateTimexConnector, Constants.TimexFuzzyYear, Constants.TimexFuzzyMonth, Constants.TimexFuzzyDay);
                    }

                    return string.Join(Constants.DateTimexConnector, Constants.TimexFuzzyYear, Constants.TimexFuzzyMonth, day.ToString("D2"));
                }

                return string.Join(Constants.DateTimexConnector, Constants.TimexFuzzyYear, month.ToString("D2"), day.ToString("D2"));
            }

            return string.Join(Constants.DateTimexConnector, year.ToString("D4"), month.ToString("D2"), day.ToString("D2"));
        }

        public static string LuisDate(DateObject date, DateObject alternativeDate = default(DateObject))
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            if (!alternativeDate.IsDefaultValue())
            {
                if (alternativeDate.Year != year)
                {
                    year = -1;
                }

                if (alternativeDate.Month != month)
                {
                    month = -1;
                }

                if (alternativeDate.Day != day)
                {
                    day = -1;
                }
            }

            return LuisDate(year, month, day);
        }

        public static string ShortTime(int hour, int min = Constants.InvalidSecond, int second = Constants.InvalidSecond)
        {
            string timeString;

            if (min == Constants.InvalidSecond && second == Constants.InvalidSecond)
            {
                timeString = $"{Constants.TimeTimexPrefix}{hour:D2}";
            }
            else if (second == Constants.InvalidSecond)
            {
                timeString = $"{Constants.TimeTimexPrefix}{LuisTime(hour, min)}";
            }
            else
            {
                timeString = $"{Constants.TimeTimexPrefix}{LuisTime(hour, min, second)}";
            }

            return timeString;
        }

        public static string LuisTime(int hour, int min, int second = Constants.InvalidSecond)
        {
            string result;

            if (second == Constants.InvalidSecond)
            {
                result = string.Join(Constants.TimeTimexConnector, hour.ToString("D2"), min.ToString("D2"));
            }
            else
            {
                result = string.Join(Constants.TimeTimexConnector, hour.ToString("D2"), min.ToString("D2"), second.ToString("D2"));
            }

            return result;
        }

        public static string LuisTime(DateObject time)
        {
            return LuisTime(time.Hour, time.Minute, time.Second);
        }

        public static string LuisDateTime(DateObject time)
        {
            return $"{LuisDate(time)}{Constants.TimeTimexPrefix}{LuisTime(time.Hour, time.Minute, time.Second)}";
        }

        // Only handle TimeSpan which is less than one day
        public static string LuisTimeSpan(System.TimeSpan timeSpan)
        {
            var timexBuilder = new StringBuilder($"{Constants.GeneralPeriodPrefix}{Constants.TimeTimexPrefix}");

            if (timeSpan.Hours > 0)
            {
                timexBuilder.Append($"{timeSpan.Hours}H");
            }

            if (timeSpan.Minutes > 0)
            {
                timexBuilder.Append($"{timeSpan.Minutes}M");
            }

            if (timeSpan.Seconds > 0)
            {
                timexBuilder.Append($"{timeSpan.Seconds}S");
            }

            return timexBuilder.ToString();
        }

        public static string FormatDate(DateObject date)
        {
            return string.Join(Constants.DateTimexConnector, date.Year.ToString("D4"), date.Month.ToString("D2"), date.Day.ToString("D2"));
        }

        public static string FormatTime(DateObject time)
        {
            return string.Join(Constants.TimeTimexConnector, time.Hour.ToString("D2"), time.Minute.ToString("D2"), time.Second.ToString("D2"));
        }

        public static string FormatDateTime(DateObject datetime)
        {
            return $"{FormatDate(datetime)} {FormatTime(datetime)}";
        }

        public static string AllStringToPm(string timeStr)
        {
            var matches = HourTimexRegex.Matches(timeStr);
            var splits = new List<string>();

            int lastPos = 0;
            foreach (Match match in matches)
            {
                if (lastPos != match.Index)
                {
                    splits.Add(timeStr.Substring(lastPos, match.Index - lastPos));
                }

                splits.Add(timeStr.Substring(match.Index, match.Length));
                lastPos = match.Index + match.Length;
            }

            if (!string.IsNullOrEmpty(timeStr.Substring(lastPos)))
            {
                splits.Add(timeStr.Substring(lastPos));
            }

            for (int i = 0; i < splits.Count; i += 1)
            {
                if (HourTimexRegex.IsMatch(splits[i]))
                {
                    splits[i] = ToPm(splits[i]);
                }
            }

            // Modify weekDay timex for the cases which cross day boundary
            if (splits.Count >= 4)
            {
                var weekDayStartMatch = WeekDayTimexRegex.Match(splits[0]).Groups[1];
                var weekDayEndMatch = WeekDayTimexRegex.Match(splits[2]).Groups[1];
                var hourStartMatch = HourTimexRegex.Match(splits[1]).Groups[1];
                var hourEndMatch = HourTimexRegex.Match(splits[3]).Groups[1];

                if (int.TryParse(weekDayStartMatch.Value, out var weekDayStart) &&
                    int.TryParse(weekDayEndMatch.Value, out var weekDayEnd) &&
                    int.TryParse(hourStartMatch.Value, out var hourStart) &&
                    int.TryParse(hourEndMatch.Value, out var hourEnd))
                {
                    if (hourEnd < hourStart && weekDayStart == weekDayEnd)
                    {
                        weekDayEnd = weekDayEnd == Constants.WeekDayCount ? 1 : weekDayEnd + 1;
                        splits[2] = splits[2].Substring(0, weekDayEndMatch.Index) + weekDayEnd;
                    }
                }
            }

            return string.Concat(splits);
        }

        public static string ToPm(string timeStr)
        {
            bool hasT = false;
            if (timeStr.StartsWith(Constants.TimeTimexPrefix))
            {
                hasT = true;
                timeStr = timeStr.Substring(1);
            }

            var splits = timeStr.Split(new[] { Constants.TimeTimexConnector }, StringSplitOptions.RemoveEmptyEntries);
            var hour = int.Parse(splits[0]);
            hour = hour >= Constants.HalfDayHourCount ? hour - Constants.HalfDayHourCount : hour + Constants.HalfDayHourCount;
            splits[0] = hour.ToString("D2");

            return hasT ? Constants.TimeTimexPrefix + string.Join(Constants.TimeTimexConnector, splits) : string.Join(Constants.TimeTimexConnector, splits);
        }

        public static string ToIsoWeekTimex(DateObject date)
        {
            var cal = DateTimeFormatInfo.InvariantInfo.Calendar;
            var thursday = cal.AddDays(date, DayOfWeek.Thursday - cal.GetDayOfWeek(date));

            return $"{thursday.Year:D4}-W{cal.GetWeekOfYear(thursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday):D2}";
        }
    }
}