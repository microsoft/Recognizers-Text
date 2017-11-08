using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class AgoLaterUtil
    {
        public static List<Token> ExtractorDurationWithBeforeAndAfter(string text, ExtractResult er, List<Token> ret,
            IDateTimeUtilityConfiguration utilityConfiguration)
        {
            var pos = (int)er.Start + (int)er.Length;
            if (pos <= text.Length)
            {
                var afterString = text.Substring(pos);
                var beforeString = text.Substring(0, (int)er.Start);
                var index = -1;
                if (MatchingUtil.GetAgoLaterIndex(afterString, utilityConfiguration.AgoRegex, out index))
                {
                    ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + index));
                }
                else if (MatchingUtil.GetAgoLaterIndex(afterString, utilityConfiguration.LaterRegex, out index))
                {
                    ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + index));
                }
                else if (MatchingUtil.GetInIndex(beforeString, utilityConfiguration.InConnectorRegex, out index))
                {
                    // for range unit like "week, month, year", it should output dateRange or datetimeRange
                    if (!utilityConfiguration.RangeUnitRegex.IsMatch(er.Text))
                    {
                        if (er.Start != null && er.Length != null && (int) er.Start >= index)
                        {
                            ret.Add(new Token((int) er.Start - index, (int) er.Start + (int) er.Length));
                        }
                    }
                }
            }
            return ret;
        }

        public static DateTimeResolutionResult ParseDurationWithAgoAndLater(string text, 
            DateObject referenceTime, 
            IDateTimeExtractor durationExtractor,
            IDateTimeParser durationParser, 
            IImmutableDictionary<string, string> unitMap,
            Regex unitRegex,
            IDateTimeUtilityConfiguration utilityConfiguration,
            AgoLaterMode mode)
        {
            var ret = new DateTimeResolutionResult();
            var durationRes = durationExtractor.Extract(text, referenceTime);
            if (durationRes.Count > 0)
            {
                var pr = durationParser.Parse(durationRes[0], referenceTime);
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
                    var srcUnit = match.Groups["unit"].Value.ToLowerInvariant();

                    if (pr.Value!=null)
                    {
                        var durationResult = (DateTimeResolutionResult)pr.Value;
                        var numStr = durationResult.Timex.Substring(0, durationResult.Timex.Length - 1)
                            .Replace("P", "")
                            .Replace("T", "");

                        if (double.TryParse(numStr, out double number))
                        {
                            return GetAgoLaterResult(
                                pr,
                                number,
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
            }
            return ret;
        }

        private static DateTimeResolutionResult GetAgoLaterResult(
            DateTimeParseResult durationParseResult, 
            double number,
            IImmutableDictionary<string, string> unitMap,
            string srcUnit,
            string afterStr,
            string beforeStr,
            System.DateTime referenceTime,
            IDateTimeUtilityConfiguration utilityConfiguration,
            AgoLaterMode mode)
        {
            var ret = new DateTimeResolutionResult();
 
            if (unitMap.ContainsKey(srcUnit))
            {
                var unitStr = unitMap[srcUnit];
                var numStr = number.ToString(CultureInfo.InvariantCulture);
                var result = new DateTimeResolutionResult(); 

                if (MatchingUtil.ContainsAgoLaterIndex(afterStr, utilityConfiguration.AgoRegex))
                {
                    if (mode.Equals(AgoLaterMode.Date))
                    {
                        result = GetDateResult(unitStr, numStr, referenceTime, false);
                    }
                    else if (mode.Equals(AgoLaterMode.DateTime))
                    {
                        result = GetDateTimeResult(unitStr, numStr, referenceTime, false);
                    }

                    ((DateTimeResolutionResult)durationParseResult.Value).Mod = TimeTypeConstants.beforeMod;
                    result.SubDateTimeEntities = new List<object> { durationParseResult };
                    return result;
                }

                if (MatchingUtil.ContainsAgoLaterIndex(afterStr, utilityConfiguration.LaterRegex)
                    || MatchingUtil.ContainsInIndex(beforeStr, utilityConfiguration.InConnectorRegex))
                {
                    if (mode.Equals(AgoLaterMode.Date))
                    {
                        result = GetDateResult(unitStr, numStr, referenceTime, true);
                    }
                    else if (mode.Equals(AgoLaterMode.DateTime))
                    {
                        result = GetDateTimeResult(unitStr, numStr, referenceTime, true);
                    }

                    ((DateTimeResolutionResult)durationParseResult.Value).Mod = TimeTypeConstants.afterMod;
                    result.SubDateTimeEntities = new List<object> { durationParseResult };
                    return result;
                }
            }
            return ret;
        }

        private static DateTimeResolutionResult GetDateResult(string unitStr, string numStr, System.DateTime referenceDate, bool future)
        {
            System.DateTime date;
            var ret = new DateTimeResolutionResult();
            int futureOrPast = future ? 1 : -1;

            switch (unitStr)
            {
                case "D":
                    date = referenceDate.AddDays(double.Parse(numStr) * futureOrPast);
                    break;
                case "W":
                    date = referenceDate.AddDays(7 * double.Parse(numStr) * futureOrPast);
                    break;
                case "MON":
                    date = referenceDate.AddMonths(Convert.ToInt32(double.Parse(numStr)) * futureOrPast);
                    break;
                case "Y":
                    date = referenceDate.AddYears(Convert.ToInt32(double.Parse(numStr)) * futureOrPast);
                    break;
                default:
                    return ret;
            }

            ret.Timex = $"{FormatUtil.LuisDate(date)}";
            ret.FutureValue = ret.PastValue = date;
            ret.Success = true;
            return ret;
        }

        private static DateTimeResolutionResult GetDateTimeResult(string unitStr, string numStr, System.DateTime referenceTime, bool future)
        {
            System.DateTime time;
            var ret = new DateTimeResolutionResult();
            int futureOrPast = future ? 1 : -1;

            switch (unitStr)
            {
                case "H":
                    time = referenceTime.AddHours(double.Parse(numStr)*futureOrPast);
                    break;
                case "M":
                    time = referenceTime.AddMinutes(double.Parse(numStr) * futureOrPast);
                    break;
                case "S":
                    time = referenceTime.AddSeconds(double.Parse(numStr) * futureOrPast);
                    break;
                default:
                    return ret;
            }

            ret.Timex = $"{FormatUtil.LuisDateTime(time)}";
            ret.FutureValue = ret.PastValue = time;
            ret.Success = true;
            return ret;
        }

        public enum AgoLaterMode
        {
            Date = 0,
            DateTime
        }
    }
}