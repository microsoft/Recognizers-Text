using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class AgoLaterUtil
    {
        public delegate int SwiftDayDelegate(string text);

        public static List<Token> ExtractorDurationWithBeforeAndAfter(string text, ExtractResult er, List<Token> ret,
            IDateTimeUtilityConfiguration utilityConfiguration)
        {
            var pos = (int)er.Start + (int)er.Length;

            if (pos <= text.Length)
            {
                var afterString = text.Substring(pos);
                var beforeString = text.Substring(0, (int)er.Start);
                var index = -1;
                var isTimeDuration = utilityConfiguration.TimeUnitRegex.Match(er.Text).Success;

                if (MatchingUtil.GetAgoLaterIndex(afterString, utilityConfiguration.AgoRegex, out index))
                {
                    // We don't support cases like "5 minutes from today" for now
                    // Cases like "5 minutes ago" or "5 minutes from now" are supported
                    // Cases like "2 days before today" or "2 weeks from today" are also supported
                    var isDayMatchInAfterString = utilityConfiguration.AgoRegex.Match(afterString).Groups["day"].Success;

                    if (!(isTimeDuration && isDayMatchInAfterString))
                    {
                        ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + index));
                    }
                }
                else if (MatchingUtil.GetAgoLaterIndex(afterString, utilityConfiguration.LaterRegex, out index))
                {
                    var isDayMatchInAfterString = utilityConfiguration.LaterRegex.Match(afterString).Groups["day"].Success;

                    if (!(isTimeDuration && isDayMatchInAfterString))
                    {
                        ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + index));
                    }
                }
                else if (MatchingUtil.GetTermIndex(beforeString, utilityConfiguration.InConnectorRegex, out index))
                {
                    // For range unit like "week, month, year", it should output dateRange or datetimeRange
                    if (!utilityConfiguration.RangeUnitRegex.IsMatch(er.Text))
                    {
                        if (er.Start != null && er.Length != null && (int)er.Start >= index)
                        {
                            ret.Add(new Token((int)er.Start - index, (int)er.Start + (int)er.Length));
                        }
                    }
                }
                else if (MatchingUtil.GetTermIndex(beforeString, utilityConfiguration.WithinNextPrefixRegex, out index))
                {
                    // For range unit like "week, month, year, day, second, minute, hour", it should output dateRange or datetimeRange
                    if (!utilityConfiguration.DateUnitRegex.IsMatch(er.Text) && !utilityConfiguration.TimeUnitRegex.IsMatch(er.Text))
                    {
                        if (er.Start != null && er.Length != null && (int)er.Start >= index)
                        {
                            ret.Add(new Token((int)er.Start - index, (int)er.Start + (int)er.Length));
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
            SwiftDayDelegate SwiftDay)
        {
            var ret = new DateTimeResolutionResult();
            var durationRes = durationExtractor.Extract(text, referenceTime);
            if (durationRes.Count > 0)
            {
                var pr = durationParser.Parse(durationRes[0], referenceTime);
                var matches = unitRegex.Matches(text);
                if (matches.Count > 0)
                {

                    var afterStr =
                            text.Substring((int)durationRes[0].Start + (int)durationRes[0].Length)
                                .Trim().ToLowerInvariant();

                    var beforeStr =
                        text.Substring(0, (int)durationRes[0].Start)
                            .Trim().ToLowerInvariant();

                    var mode = AgoLaterMode.Date;
                    if (pr.TimexStr.Contains("T"))
                    {
                        mode = AgoLaterMode.DateTime;
                    }

                    if (pr.Value != null)
                    {
                        return GetAgoLaterResult(pr, afterStr, beforeStr, referenceTime,
                                                 utilityConfiguration, mode, SwiftDay);
                    }
                }
            }
            return ret;
        }
        
        private static DateTimeResolutionResult GetAgoLaterResult(
            DateTimeParseResult durationParseResult,
            string afterStr,
            string beforeStr,
            System.DateTime referenceTime,
            IDateTimeUtilityConfiguration utilityConfiguration,
            AgoLaterMode mode,
            SwiftDayDelegate SwiftDay)
        {
            var ret = new DateTimeResolutionResult();
            var resultDateTime = referenceTime;
            var timex = durationParseResult.TimexStr;

            if (((DateTimeResolutionResult)durationParseResult.Value).Mod == Constants.MORE_THAN_MOD)
            {
                ret.Mod = Constants.MORE_THAN_MOD;
            }
            else if (((DateTimeResolutionResult)durationParseResult.Value).Mod == Constants.LESS_THAN_MOD)
            {
                ret.Mod = Constants.LESS_THAN_MOD;
            }

            if (MatchingUtil.ContainsAgoLaterIndex(afterStr, utilityConfiguration.AgoRegex))
            {
                var match = utilityConfiguration.AgoRegex.Match(afterStr);
                var swift = 0;

                // Handle cases like "3 days before yesterday"
                if (match.Success && !string.IsNullOrEmpty(match.Groups["day"].Value))
                {
                    swift = SwiftDay(match.Groups["day"].Value);
                }
                
                resultDateTime = DurationParsingUtil.ShiftDateTime(timex, referenceTime.AddDays(swift), false);
                
                ((DateTimeResolutionResult)durationParseResult.Value).Mod = Constants.BEFORE_MOD;
            }
            else if (MatchingUtil.ContainsAgoLaterIndex(afterStr, utilityConfiguration.LaterRegex) ||
                     MatchingUtil.ContainsTermIndex(beforeStr, utilityConfiguration.InConnectorRegex))
            {
                var match = utilityConfiguration.LaterRegex.Match(afterStr);
                var swift = 0;

                // Handle cases like "3 days after tomorrow"
                if (match.Success && !string.IsNullOrEmpty(match.Groups["day"].Value))
                {
                    swift = SwiftDay(match.Groups["day"].Value);
                }

                resultDateTime = DurationParsingUtil.ShiftDateTime(timex, referenceTime.AddDays(swift), true);

                ((DateTimeResolutionResult)durationParseResult.Value).Mod = Constants.AFTER_MOD;
            }

            if (resultDateTime != referenceTime)
            {
                if (mode.Equals(AgoLaterMode.Date))
                {
                    ret.Timex = $"{FormatUtil.LuisDate(resultDateTime)}";
                }
                else if (mode.Equals(AgoLaterMode.DateTime))
                {
                    ret.Timex = $"{FormatUtil.LuisDateTime(resultDateTime)}";
                }

                ret.FutureValue = ret.PastValue = resultDateTime;
                ret.SubDateTimeEntities = new List<object> { durationParseResult };
                ret.Success = true;
            }
            
            return ret;
        }

        public enum AgoLaterMode
        {
            Date = 0,
            DateTime
        }
    }
}