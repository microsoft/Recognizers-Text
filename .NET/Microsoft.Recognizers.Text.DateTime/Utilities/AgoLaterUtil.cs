using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public enum AgoLaterMode
    {
        /// <summary>
        /// Date
        /// </summary>
        Date = 0,

        /// <summary>
        /// Datetime
        /// </summary>
        DateTime,
    }

    public static class AgoLaterUtil
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
                var isTimeDuration = utilityConfiguration.TimeUnitRegex.Match(er.Text).Success;
                int index;
                bool isMatch = false;
                var agoLaterRegexes = new List<Regex>
                {
                    utilityConfiguration.AgoRegex,
                    utilityConfiguration.LaterRegex,
                };

                foreach (var regex in agoLaterRegexes)
                {
                    Token tokAfter = null, tokBefore = null;
                    bool isDayMatch = false;

                    // Check afterString
                    if (MatchingUtil.GetAgoLaterIndex(afterString, regex, out index, inSuffix: true))
                    {
                        // We don't support cases like "5 minutes from today" for now
                        // Cases like "5 minutes ago" or "5 minutes from now" are supported
                        // Cases like "2 days before today" or "2 weeks from today" are also supported
                        isDayMatch = regex.Match(afterString).Groups["day"].Success;

                        if (!(isTimeDuration && isDayMatch))
                        {
                            tokAfter = new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + index);
                            isMatch = true;
                        }
                    }

                    if (utilityConfiguration.CheckBothBeforeAfter)
                    {
                        // Check if regex match is split between beforeString and afterString
                        if (!isDayMatch && isMatch)
                        {
                            string beforeAfterStr = beforeString + afterString.Substring(0, index);
                            var isRangeMatch = utilityConfiguration.RangePrefixRegex.MatchBegin(afterString.Substring(index), trim: true).Success;
                            if (!isRangeMatch && MatchingUtil.GetAgoLaterIndex(beforeAfterStr, regex, out var indexStart, inSuffix: false))
                            {
                                isDayMatch = regex.Match(beforeAfterStr).Groups["day"].Success;

                                if (isDayMatch && !(isTimeDuration && isDayMatch))
                                {
                                    ret.Add(new Token(indexStart, (er.Start + er.Length ?? 0) + index));
                                    isMatch = true;
                                }
                            }
                        }

                        // Check also beforeString
                        if (MatchingUtil.GetAgoLaterIndex(beforeString, regex, out index, inSuffix: false))
                        {
                            isDayMatch = regex.Match(beforeString).Groups["day"].Success;
                            if (!(isTimeDuration && isDayMatch))
                            {
                                tokBefore = new Token(index, er.Start + er.Length ?? 0);
                                isMatch = true;
                            }
                        }
                    }

                    if (tokAfter != null && tokBefore != null && tokBefore.Start + tokBefore.Length > tokAfter.Start)
                    {
                        // Merge overlapping tokens
                        ret.Add(new Token(tokBefore.Start, tokAfter.Start + tokAfter.Length - tokBefore.Start));
                    }
                    else if (tokAfter != null)
                    {
                        ret.Add(tokAfter);
                    }
                    else if (tokBefore != null)
                    {
                        ret.Add(tokBefore);
                    }

                    if (isMatch)
                    {
                        break;
                    }
                }

                if (!isMatch)
                {
                    // Item1 is the main regex to be tested
                    // Item2 is a list of unit regexes used to validate the extraction (in case of match, the extraction is discarded)
                    var inWithinRegexTuples = new List<(Regex, List<Regex>)>
                    {
                        (utilityConfiguration.InConnectorRegex, new List<Regex> { utilityConfiguration.RangeUnitRegex }),
                        (utilityConfiguration.WithinNextPrefixRegex, new List<Regex> { utilityConfiguration.DateUnitRegex, utilityConfiguration.TimeUnitRegex }),
                    };

                    foreach (var regex in inWithinRegexTuples)
                    {
                        bool isMatchAfter = false;
                        if (MatchingUtil.GetTermIndex(beforeString, regex.Item1, out index))
                        {
                            isMatch = true;
                        }
                        else if (utilityConfiguration.CheckBothBeforeAfter && MatchingUtil.GetAgoLaterIndex(afterString, regex.Item1, out index, inSuffix: true))
                        {
                            // Check also afterString
                            isMatch = isMatchAfter = true;
                        }

                        if (isMatch)
                        {
                            // For InConnectorRegex and range unit like "week, month, year", it should output dateRange or datetimeRange
                            // For WithinNextPrefixRegex and range unit like "week, month, year, day, second, minute, hour", it should output dateRange or datetimeRange
                            bool isUnitMatch = false;
                            foreach (var unitRegex in regex.Item2)
                            {
                                isUnitMatch = isUnitMatch || unitRegex.IsMatch(er.Text);
                            }

                            if (!isUnitMatch)
                            {
                                if (er.Start != null && er.Length != null && ((int)er.Start >= index || isMatchAfter))
                                {
                                    int start = (int)er.Start - (!isMatchAfter ? index : 0);
                                    int end = (int)er.Start + (int)er.Length + (isMatchAfter ? index : 0);
                                    ret.Add(new Token(start, end));
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return ret;
        }

        public static DateTimeResolutionResult ParseDurationWithAgoAndLater(
            string text,
            DateObject referenceTime,
            IDateTimeExtractor durationExtractor,
            IDateTimeParser durationParser,
            IParser numberParser,
            IImmutableDictionary<string, string> unitMap,
            Regex unitRegex,
            IDateTimeUtilityConfiguration utilityConfiguration,
            SwiftDayDelegate swiftDay)
        {
            var ret = new DateTimeResolutionResult();
            var durationRes = durationExtractor.Extract(text, referenceTime);

            if (durationRes.Count > 0)
            {
                var pr = durationParser.Parse(durationRes[0], referenceTime);
                var matches = unitRegex.Matches(text);
                if (matches.Count > 0)
                {
                    var afterStr = text.Substring((int)durationRes[0].Start + (int)durationRes[0].Length).Trim();

                    var beforeStr = text.Substring(0, (int)durationRes[0].Start).Trim();

                    var mode = AgoLaterMode.Date;
                    if (pr.TimexStr.Contains("T"))
                    {
                        mode = AgoLaterMode.DateTime;
                    }

                    if (pr.Value != null)
                    {
                        return GetAgoLaterResult(pr, afterStr, beforeStr, referenceTime, numberParser, utilityConfiguration, mode, swiftDay);
                    }
                }
            }

            return ret;
        }

        private static DateTimeResolutionResult GetAgoLaterResult(
            DateTimeParseResult durationParseResult,
            string afterStr,
            string beforeStr,
            DateObject referenceTime,
            IParser numberParser,
            IDateTimeUtilityConfiguration utilityConfiguration,
            AgoLaterMode mode,
            SwiftDayDelegate swiftDay)
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

            int swift = 0;
            bool isMatch = false, isLater = false;
            string dayStr = null;

            // Item2 is a label identifying the regex defined in Item1
            var agoLaterRegexTuples = new List<(Regex, string)>
            {
                (utilityConfiguration.AgoRegex, Constants.AGO_LABEL),
                (utilityConfiguration.LaterRegex, Constants.LATER_LABEL),
            };

            // AgoRegex and LaterRegex cases
            foreach (var regex in agoLaterRegexTuples)
            {
                // Match in afterStr
                if (MatchingUtil.ContainsAgoLaterIndex(afterStr, regex.Item1, inSuffix: true))
                {
                    isMatch = true;
                    isLater = regex.Item2 == Constants.LATER_LABEL;
                    var match = regex.Item1.Match(afterStr);
                    dayStr = match.Groups["day"].Value;
                }

                if (utilityConfiguration.CheckBothBeforeAfter)
                {
                    // Match split between beforeStr and afterStr
                    if (string.IsNullOrEmpty(dayStr) && isMatch)
                    {
                        var match = regex.Item1.Match(beforeStr + " " + afterStr);
                        dayStr = match.Groups["day"].Value;
                    }

                    // Match in beforeStr
                    if (string.IsNullOrEmpty(dayStr) && MatchingUtil.ContainsAgoLaterIndex(beforeStr, regex.Item1, inSuffix: false))
                    {
                        isMatch = true;
                        isLater = regex.Item2 == Constants.LATER_LABEL;
                        var match = regex.Item1.Match(beforeStr);
                        dayStr = match.Groups["day"].Value;
                    }
                }

                if (isMatch)
                {
                    break;
                }
            }

            // InConnectorRegex cases
            if (!isMatch)
            {
                if (MatchingUtil.ContainsTermIndex(beforeStr, utilityConfiguration.InConnectorRegex))
                {
                    // Match in afterStr
                    isMatch = isLater = true;
                    var match = utilityConfiguration.LaterRegex.Match(afterStr);
                    dayStr = match.Groups["day"].Value;
                }
                else if (utilityConfiguration.CheckBothBeforeAfter && MatchingUtil.ContainsAgoLaterIndex(afterStr, utilityConfiguration.InConnectorRegex, inSuffix: true))
                {
                    // Match in beforeStr
                    isMatch = isLater = true;
                    var match = utilityConfiguration.LaterRegex.Match(beforeStr);
                    dayStr = match.Groups["day"].Value;
                }
            }

            if (isMatch)
            {
                // Handle cases like "3 days before yesterday", "3 days after tomorrow"
                if (!string.IsNullOrEmpty(dayStr))
                {
                    swift = swiftDay(dayStr);
                }

                if (isLater)
                {
                    var yearMatch = utilityConfiguration.SinceYearSuffixRegex.Match(afterStr);
                    if (yearMatch.Success)
                    {
                        var yearString = yearMatch.Groups[Constants.YearGroupName].Value;
                        var yearEr = new ExtractResult { Text = yearString };
                        var year = Convert.ToInt32((double)(numberParser.Parse(yearEr).Value ?? 0));
                        referenceTime = DateObject.MinValue.SafeCreateFromValue(year, 1, 1);
                    }
                }

                var isFuture = isLater;
                resultDateTime = DurationParsingUtil.ShiftDateTime(timex, referenceTime.AddDays(swift), future: isFuture);

                ((DateTimeResolutionResult)durationParseResult.Value).Mod = isLater ? Constants.AFTER_MOD : Constants.BEFORE_MOD;
            }

            if (resultDateTime != referenceTime)
            {
                if (mode.Equals(AgoLaterMode.Date))
                {
                    ret.Timex = $"{DateTimeFormatUtil.LuisDate(resultDateTime)}";
                }
                else if (mode.Equals(AgoLaterMode.DateTime))
                {
                    ret.Timex = $"{DateTimeFormatUtil.LuisDateTime(resultDateTime)}";
                }

                ret.FutureValue = ret.PastValue = resultDateTime;
                ret.SubDateTimeEntities = new List<object> { durationParseResult };
                ret.Success = true;
            }

            return ret;
        }
    }
}