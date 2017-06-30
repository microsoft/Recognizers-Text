using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class AgoLaterUtil
    {
        public static DTParseResult ParserDurationWithAgoAndLater(string text, 
            DateObject referenceTime, 
            IExtractor durationExtractor,
            IExtractor cardinalExtractor,
            IParser numberParser,
            IImmutableDictionary<string, string> unitMap,
            Regex unitRegex,
            IDateTimeUtilityConfiguration utilityConfiguration,
            AgoLaterMode mode
            )
        {
            var ret = new DTParseResult();
            var durationRes = durationExtractor.Extract(text);
            if (durationRes.Count > 0)
            {
                var match = unitRegex.Match(text);
                if (match.Success)
                {
                    var afterStr =
                        text.Substring((int)durationRes[0].Start + (int)durationRes[0].Length)
                            .Trim()
                            .ToLowerInvariant();
                    var beforeStr =
                        text.Substring(0, (int)durationRes[0].Start)
                            .Trim()
                            .ToLowerInvariant();
                    //add space before the token
                    beforeStr = " " + beforeStr;
                    var srcUnit = match.Groups["unit"].Value.ToLowerInvariant();
                    var numberStr =
                        text.Substring((int)durationRes[0].Start, match.Index - (int)durationRes[0].Start)
                            .Trim()
                            .ToLowerInvariant();
                    var er = cardinalExtractor.Extract(numberStr);
                    if (er.Count != 0)
                    {
                        return GetAgoLaterResult(numberParser,
                            er[0],
                            unitMap,
                            srcUnit,
                            afterStr,
                            beforeStr,
                            referenceTime,
                            utilityConfiguration,
                            mode);
                    }
                }
            }
            return ret;
        }

        private static DTParseResult GetAgoLaterResult(IParser numberParser,
            ExtractResult er,
            IImmutableDictionary<string, string> unitMap,
            string srcUnit,
            string afterStr,
            string beforeStr,
            DateObject referenceTime,
            IDateTimeUtilityConfiguration utilityConfiguration,
            AgoLaterMode mode)
        {
            var ret = new DTParseResult();
            var pr = numberParser.Parse(er);

            var number = int.Parse(pr.ResolutionStr);
            if (unitMap.ContainsKey(srcUnit))
            {
                var unitStr = unitMap[srcUnit];
                var numStr = number.ToString();
                if (MatchingUtil.ContainsAgoLaterIndex(afterStr, utilityConfiguration.AgoStringList))
                {
                    if (mode.Equals(AgoLaterMode.Date))
                    {
                        return GetDateResult(unitStr, numStr, referenceTime, false);
                    }
                    if (mode.Equals(AgoLaterMode.DateTime))
                    {
                        return GetDateTimeResult(unitStr, numStr, referenceTime, false);
                    }
                }
                if (MatchingUtil.ContainsAgoLaterIndex(afterStr, utilityConfiguration.LaterStringList)
                    || MatchingUtil.ContainsInIndex(beforeStr, utilityConfiguration.InStringList))
                {
                    if (mode.Equals(AgoLaterMode.Date))
                    {
                        return GetDateResult(unitStr, numStr, referenceTime, true);
                    }
                    if (mode.Equals(AgoLaterMode.DateTime))
                    {
                        return GetDateTimeResult(unitStr, numStr, referenceTime, true);
                    }
                }
            }
            return ret;
        }

        private static DTParseResult GetDateResult(string unitStr, string numStr, DateObject referenceDate, bool future)
        {
            DateObject Date;
            var ret = new DTParseResult();
            int futureOrPast = future ? 1 : -1;
            switch (unitStr)
            {
                case "D":
                    Date = referenceDate.AddDays(double.Parse(numStr) * futureOrPast);
                    break;
                case "W":
                    Date = referenceDate.AddDays(7 * double.Parse(numStr) * futureOrPast);
                    break;
                case "MON":
                    Date = referenceDate.AddMonths(Convert.ToInt32(double.Parse(numStr)) * futureOrPast);
                    break;
                case "Y":
                    Date = referenceDate.AddYears(Convert.ToInt32(double.Parse(numStr)) * futureOrPast);
                    break;
                default:
                    return ret;
            }
            ret.Timex = $"{Util.LuisDate(Date)}";
            ret.FutureValue = ret.PastValue = Date;
            ret.Success = true;
            return ret;
        }

        private static DTParseResult GetDateTimeResult(string unitStr, string numStr, DateObject referenceTime, bool future)
        {
            DateObject Time;
            var ret = new DTParseResult();
            int futureOrPast = future ? 1 : -1;
            switch (unitStr)
            {
                case "H":
                    Time = referenceTime.AddHours(double.Parse(numStr)*futureOrPast);
                    break;
                case "M":
                    Time = referenceTime.AddMinutes(double.Parse(numStr) * futureOrPast);
                    break;
                case "S":
                    Time = referenceTime.AddSeconds(double.Parse(numStr) * futureOrPast);
                    break;
                default:
                    return ret;
            }
            ret.Timex = $"{Util.LuisDateTime(Time)}";
            ret.FutureValue = ret.PastValue = Time;
            ret.Success = true;
            return ret;
        }

        public enum AgoLaterMode
        {
            Date = 0,
            DateTime
        }
    }

    public class MatchingUtil
    {
        public static bool GetAgoLaterIndex(string text, List<string> stringList, out int index)
        {
            index = -1;

            foreach (var matchString in stringList)
            {
                if (text.TrimStart().ToLower().StartsWith(matchString))
                {
                    index = text.ToLower().LastIndexOf(matchString) + matchString.Length;
                    return true;
                }
            }
            return false;
        }

        public static bool GetInIndex(string text, List<string> stringList, out int index)
        {
            index = -1;

            foreach (var matchString in stringList)
            {
                if (text.TrimEnd().ToLower().EndsWith(matchString))
                {
                    index = text.Length - text.ToLower().LastIndexOf(matchString) - 1;
                    return true;
                }
            }
            return false;
        }

        public static bool ContainsAgoLaterIndex(string text, List<string> stringList)
        {
            int index = -1;
            return GetAgoLaterIndex(text, stringList, out index);
        }

        public static bool ContainsInIndex(string text, List<string> stringList)
        {
            int index = -1;
            return GetInIndex(text, stringList, out index);
        }
    }

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

        public static string LuisDateTime(DateObject time)
        {
            return $"{LuisDate(time)}T{LuisTime(time.Hour, time.Minute, time.Second)}";
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