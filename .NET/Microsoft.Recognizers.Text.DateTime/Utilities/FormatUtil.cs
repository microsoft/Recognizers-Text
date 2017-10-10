using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class FormatUtil
    {
        public static readonly Regex HourTimexRegex = new Regex(@"(?<!P)T\d{2}");

        public static string LuisDate(int year, int month, int day)
        {
            if (year == -1)
            {
                if (month == -1)
                {
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
                    splited.Add(timeStr.Substring(lastPos, match.Index - lastPos));
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
            hour = (hour == 12 ? 0 : hour + 12);
            splited[0] = hour.ToString("D2");

            return hasT ? "T" + string.Join(":", splited) : string.Join(":", splited);
        }

    }
}