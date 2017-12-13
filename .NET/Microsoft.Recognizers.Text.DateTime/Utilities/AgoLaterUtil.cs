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

                    // For range unit like "week, month, year", it should output dateRange or datetimeRange
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
            IDateTimeUtilityConfiguration utilityConfiguration)
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
                                                 utilityConfiguration, mode);
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
            AgoLaterMode mode)
        {
            var ret = new DateTimeResolutionResult();
            var resultDateTime = referenceTime;
            var timex = durationParseResult.TimexStr;

            if (MatchingUtil.ContainsAgoLaterIndex(afterStr, utilityConfiguration.AgoRegex))
            {
                resultDateTime = DurationParsingUtil.ShiftDateTime(timex, referenceTime, false);
                
                ((DateTimeResolutionResult)durationParseResult.Value).Mod = TimeTypeConstants.beforeMod;
            }
            else if (MatchingUtil.ContainsAgoLaterIndex(afterStr, utilityConfiguration.LaterRegex) ||
                     MatchingUtil.ContainsInIndex(beforeStr, utilityConfiguration.InConnectorRegex))
            {
                resultDateTime = DurationParsingUtil.ShiftDateTime(timex, referenceTime, true);

                ((DateTimeResolutionResult)durationParseResult.Value).Mod = TimeTypeConstants.afterMod;
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