using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class FormatUtil
    {
        public static readonly Regex HourTimexRegex = new Regex(@"(?<!P)T(\d{2})");
        public static readonly Regex WeekDayTimexRegex = new Regex(@"XXXX-WXX-(\d)");

        public static string LuisDate(int year, int month, int day)
        {
            if (year == -1)
            {
                if (month == -1)
                {
                    if (day == -1)
                    {
                        return string.Join("-", "XXXX", "XX", "XX");
                    }
                    
                    return string.Join("-", "XXXX", "XX", day.ToString("D2"));
                }

                return string.Join("-", "XXXX", month.ToString("D2"), day.ToString("D2"));
            }

            return string.Join("-", year.ToString("D4"), month.ToString("D2"), day.ToString("D2"));
        }

        public static string LuisDate(System.DateTime date)
        {
            return LuisDate(date.Year, date.Month, date.Day);
        }

        public static string ShortTime(int hour, int min, int second)
        {
            if (min < 0 && second < 0)
            {
                return $"T{hour.ToString("D2")}";
            }
            else if (second < 0)
            {
                return $"T{string.Join(":", hour.ToString("D2"), min.ToString("D2"))}";
            }

            return $"T{string.Join(":", hour.ToString("D2"), min.ToString("D2"), second.ToString("D2"))}";
        }

        public static string LuisTime(int hour, int min, int second)
        {
            return string.Join(":", hour.ToString("D2"), min.ToString("D2"), second.ToString("D2"));
        }

        public static string LuisTime(System.DateTime time)
        {
            return LuisTime(time.Hour, time.Minute, time.Second);
        }

        public static string LuisDateTime(System.DateTime time)
        {
            return $"{LuisDate(time)}T{LuisTime(time.Hour, time.Minute, time.Second)}";
        }

        // Only handle TimeSpan which is less than one day
        public static string LuisTimeSpan(System.TimeSpan timeSpan)
        {
            var result = "PT";

            if (timeSpan.Hours > 0)
            {
                result += $"{timeSpan.Hours}H";
            }

            if (timeSpan.Minutes > 0)
            {
                result += $"{timeSpan.Minutes}M";
            }

            if (timeSpan.Seconds > 0)
            {
                result += $"{timeSpan.Seconds}S";
            }

            return result;
        }

        public static string FormatDate(System.DateTime date)
        {
            return string.Join("-", date.Year.ToString("D4"), date.Month.ToString("D2"), date.Day.ToString("D2"));
        }

        public static string FormatTime(System.DateTime time)
        {
            return string.Join(":", time.Hour.ToString("D2"), time.Minute.ToString("D2"), time.Second.ToString("D2"));
        }

        public static string FormatDateTime(System.DateTime datetime)
        {
            return FormatDate(datetime) + " " + FormatTime(datetime);
        }

        public static string AllStringToPm(string timeStr)
        {
            var matches = HourTimexRegex.Matches(timeStr);
            var splited = new List<string>();

            int lastPos = 0;
            foreach (Match match in matches)
            {
                if (lastPos != match.Index)
                {
                    splited.Add(timeStr.Substring(lastPos, match.Index - lastPos));
                }
                splited.Add(timeStr.Substring(match.Index, match.Length));
                lastPos = match.Index + match.Length;
            }

            if (!string.IsNullOrEmpty(timeStr.Substring(lastPos)))
            {
                splited.Add(timeStr.Substring(lastPos));
            }

            for (int i = 0; i < splited.Count; i += 1)
            {
                if (HourTimexRegex.IsMatch(splited[i]))
                {
                    splited[i] = ToPm(splited[i]);
                }
            }

            // Modify weekDay timex for the cases which cross day boundary
            if (splited.Count >= 4)
            {
                var weekDayStartMatch = WeekDayTimexRegex.Match(splited[0]).Groups[1];
                var weekDayEndMatch = WeekDayTimexRegex.Match(splited[2]).Groups[1];
                var hourStartMatch = HourTimexRegex.Match(splited[1]).Groups[1];
                var hourEndMatch = HourTimexRegex.Match(splited[3]).Groups[1];

                if (int.TryParse(weekDayStartMatch.Value, out var weekDayStart) &&
                    int.TryParse(weekDayEndMatch.Value, out var weekDayEnd) &&
                    int.TryParse(hourStartMatch.Value, out var hourStart) &&
                    int.TryParse(hourEndMatch.Value, out var hourEnd))
                {
                    if (hourEnd < hourStart && weekDayStart == weekDayEnd)
                    {
                        weekDayEnd = weekDayEnd == Constants.WeekDayCount ? 1 : weekDayEnd + 1;
                        splited[2] = splited[2].Substring(0, weekDayEndMatch.Index) + weekDayEnd;
                    }
                }
            }

            return string.Concat(splited);
        }

        public static string ToPm(string timeStr)
        {
            bool hasT = false;
            if (timeStr.StartsWith("T"))
            {
                hasT = true;
                timeStr = timeStr.Substring(1);
            }

            var splited = timeStr.Split(':');
            var hour = int.Parse(splited[0]);
            hour = hour >= 12 ? hour - 12: hour + 12;
            splited[0] = hour.ToString("D2");

            return hasT ? "T" + string.Join(":", splited) : string.Join(":", splited);
        }

        public static string ToIsoWeekTimex(System.DateTime monday)
        {
            var cal = DateTimeFormatInfo.InvariantInfo.Calendar;
            return monday.Year.ToString("D4") + "-W" + 
                cal.GetWeekOfYear(monday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
                .ToString("D2");
        }
    }
}