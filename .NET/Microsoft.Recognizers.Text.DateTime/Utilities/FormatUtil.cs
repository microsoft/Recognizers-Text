using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

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
        
        public static string ShortTime(int hour, int min, int second)
        {
            if (min < 0 && second < 0)
            {
                return $"{Constants.TimeTimexPrefix}{hour.ToString("D2")}";
            }
            else if (second < 0)
            {
                return $"{Constants.TimeTimexPrefix}{string.Join(Constants.TimeTimexConnector, hour.ToString("D2"), min.ToString("D2"))}";
            }

            return $"{Constants.TimeTimexPrefix}{string.Join(Constants.TimeTimexConnector, hour.ToString("D2"), min.ToString("D2"), second.ToString("D2"))}";
        }

        public static string LuisTime(int hour, int min, int second)
        {
            return string.Join(Constants.TimeTimexConnector, hour.ToString("D2"), min.ToString("D2"), second.ToString("D2"));
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
            if (timeStr.StartsWith(Constants.TimeTimexPrefix))
            {
                hasT = true;
                timeStr = timeStr.Substring(1);
            }

            var splited = timeStr.Split(new string[] { Constants.TimeTimexConnector }, StringSplitOptions.RemoveEmptyEntries);
            var hour = int.Parse(splited[0]);
            hour = hour >= Constants.HalfDayHourCount ? hour - Constants.HalfDayHourCount: hour + Constants.HalfDayHourCount;
            splited[0] = hour.ToString("D2");

            return hasT ? Constants.TimeTimexPrefix + string.Join(Constants.TimeTimexConnector, splited) : string.Join(Constants.TimeTimexConnector, splited);
        }

        public static string ToIsoWeekTimex(DateObject monday)
        {
            var cal = DateTimeFormatInfo.InvariantInfo.Calendar;
            return monday.Year.ToString("D4") + "-W" + 
                cal.GetWeekOfYear(monday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
                .ToString("D2");
        }
    }
}