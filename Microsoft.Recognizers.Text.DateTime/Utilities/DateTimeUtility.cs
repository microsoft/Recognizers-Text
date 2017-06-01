using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public class Token
    {
        public Token(int s, int e)
        {
            Start = s;
            End = e;
        }

        public int Start { get; }
        public int End { get; }

        public int Length
        {
            get
            {
                if (End < Start)
                {
                    return 0;
                }
                return End - Start;
            }
        }

        public static List<ExtractResult> MergeAllTokens(List<Token> tokens, string text, string extractorName)
        {
            var ret = new List<ExtractResult>();

            tokens = tokens.OrderBy(s => s.Start).ThenByDescending(s => s.Length).ToList();
            var mergedTokens = new List<Token>();
            foreach (var token in tokens)
            {
                if (token != null)
                {
                    var bAdd = true;
                    for (var index = 0; index < mergedTokens.Count && bAdd; index++)
                    {
                        //is included in one of the current token
                        if (token.Start >= mergedTokens[index].Start && token.End <= mergedTokens[index].End)
                        {
                            bAdd = false;
                        }
                        //if it contains overlap
                        if (token.Start > mergedTokens[index].Start && token.Start < mergedTokens[index].End)
                        {
                            bAdd = false;
                        }
                        // include one of the token, should replace the included one
                        if (token.Start <= mergedTokens[index].Start && token.End >= mergedTokens[index].End)
                        {
                            bAdd = false;
                            mergedTokens[index] = token;
                        }
                    }

                    if (bAdd)
                    {
                        mergedTokens.Add(token);
                    }
                }
            }

            foreach (var token in mergedTokens)
            {
                var start = token.Start;
                var length = token.Length;
                var substr = text.Substring(start, length);
                var er = new ExtractResult
                {
                    Start = start,
                    Length = length,
                    Text = substr,
                    Type = extractorName,
                    Data = null
                };
                ret.Add(er);
            }

            return ret;
        }
    }

    // some methods for datetime format
    public class Util
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

        public static string LuisDate(DateObject date)
        {
            return LuisDate(date.Year, date.Month, date.Day);
        }

        public static string LuisTime(int hour, int min, int second)
        {
            return string.Join(":", hour.ToString("D2"), min.ToString("D2"), second.ToString("D2"));
        }

        public static string LuisTime(DateObject time)
        {
            return LuisTime(time.Hour, time.Minute, time.Second);
        }

        public static string FormatDate(DateObject date)
        {
            return string.Join("-", date.Year.ToString("D4"), date.Month.ToString("D2"), date.Day.ToString("D2"));
        }

        public static string FormatTime(DateObject time)
        {
            return string.Join(":", time.Hour.ToString("D2"), time.Minute.ToString("D2"), time.Second.ToString("D2"));
        }

        public static string FormatDateTime(DateObject datetime)
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
                splited.Add(timeStr.Substring(lastPos));

            for (int i = 0; i < splited.Count; i += 1)
            {
                if (HourTimexRegex.IsMatch(splited[i]))
                    splited[i] = ToPm(splited[i]);
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
            splited[0] = (hour + 12).ToString("D2");

            return hasT ? "T" + string.Join(":", splited) : string.Join(":", splited);
        }

    }

    public class DTParseResult
    {
        public DTParseResult()
        {
            Success = false;
        }

        public bool Success { get; set; }

        public string Timex { get; set; }

        public bool IsLunar { get; set; }

        public string mod { get; set; }

        public string comment { get; set; }

        public Dictionary<string, string> FutureResolution { get; set; }

        public Dictionary<string, string> PastResolution { get; set; }

        public object FutureValue { get; set; }

        public object PastValue { get; set; }
    }

    // add extension method "This" and "Next" for System.DateTime
    public static class DateObjectExtension
    {
        public static DateObject Next(this DateObject from, DayOfWeek dayOfWeek)
        {
            var start = (int) from.DayOfWeek;
            var target = (int) dayOfWeek;

            if (start == 0)
            {
                start = 7;
            }
            if (target == 0)
            {
                target = 7;
            }

            return from.AddDays(target - start + 7);
        }

        public static DateObject This(this DateObject from, DayOfWeek dayOfWeek)
        {
            var start = (int) from.DayOfWeek;
            var target = (int) dayOfWeek;

            if (start == 0)
            {
                start = 7;
            }
            if (target == 0)
            {
                target = 7;
            }

            return from.AddDays(target - start);
        }

        public static DateObject Last(this DateObject from, DayOfWeek dayOfWeek)
        {
            var start = (int) from.DayOfWeek;
            var target = (int) dayOfWeek;

            if (start == 0)
            {
                start = 7;
            }
            if (target == 0)
            {
                target = 7;
            }

            return from.AddDays(target - start - 7);
        }
    }

    // add "Overlap" method to "ExtractResult" class
    public static class ExtractResultExtension
    {
        public static bool IsOverlap(this ExtractResult er1, ExtractResult er2)
        {
            if (er1.Start >= er2.Start + er2.Length || er2.Start >= er1.Start + er1.Length)
            {
                return false;
            }
            return true;
        }
    }
}