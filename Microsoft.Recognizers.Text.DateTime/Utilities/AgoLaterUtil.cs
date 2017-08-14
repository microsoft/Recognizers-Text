using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

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
                if (MatchingUtil.GetAgoLaterIndex(afterString, utilityConfiguration.AgoStringList, out index))
                {
                    ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + index));
                }
                else if (MatchingUtil.GetAgoLaterIndex(afterString, utilityConfiguration.LaterStringList, out index))
                {
                    ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + index));
                }
                else if (MatchingUtil.GetInIndex(beforeString, utilityConfiguration.InStringList, out index))
                {
                    if (er.Start != null && er.Length != null && (int)er.Start > index)
                    {
                        ret.Add(new Token((int)er.Start - index, (int)er.Start + (int)er.Length));
                    }
                }
            }
            return ret;
        }

        public static DateTimeResolutionResult ParserDurationWithAgoAndLater(string text, 
            System.DateTime referenceTime, 
            IExtractor durationExtractor,
            IExtractor cardinalExtractor,
            IParser numberParser,
            IImmutableDictionary<string, string> unitMap,
            Regex unitRegex,
            IDateTimeUtilityConfiguration utilityConfiguration,
            AgoLaterMode mode)
        {
            var ret = new DateTimeResolutionResult();
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

        private static DateTimeResolutionResult GetAgoLaterResult(IParser numberParser,
            ExtractResult er,
            IImmutableDictionary<string, string> unitMap,
            string srcUnit,
            string afterStr,
            string beforeStr,
            System.DateTime referenceTime,
            IDateTimeUtilityConfiguration utilityConfiguration,
            AgoLaterMode mode)
        {
            var ret = new DateTimeResolutionResult();
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