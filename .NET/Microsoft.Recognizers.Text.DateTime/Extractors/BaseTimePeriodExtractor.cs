﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.InternalCache;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimePeriodExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";

        private static readonly ResultsCache<ExtractResult> ResultsCache = new ResultsCache<ExtractResult>();

        private readonly ITimePeriodExtractorConfiguration config;

        private readonly string keyPrefix;

        public BaseTimePeriodExtractor(ITimePeriodExtractorConfiguration config)
        {
            this.config = config;
            keyPrefix = string.Intern(config.Options + "_" + config.LanguageMarker);
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject reference)
        {
            List<ExtractResult> results;

            if ((this.config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                results = ExtractImpl(text, reference);
            }
            else
            {
                var key = (keyPrefix, text, reference);

                results = ResultsCache.GetOrCreate(key, () => ExtractImpl(text, reference));
            }

            return results;
        }

        private List<ExtractResult> ExtractImpl(string text, DateObject reference)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MatchSimpleCases(text));
            tokens.AddRange(MergeTwoTimePoints(text, reference));
            tokens.AddRange(MatchTimeOfDay(text));

            // Handle pure number cases like "from 6 to 7" cannot be extracted as time ranges under Calendar Mode
            if ((this.config.Options & DateTimeOptions.CalendarMode) != 0)
            {
                tokens.AddRange(MatchPureNumberCases(text));
            }

            var timePeriodErs = Token.MergeAllTokens(tokens, text, ExtractorName);

            if ((this.config.Options & DateTimeOptions.EnablePreview) != 0)
            {
                timePeriodErs = TimeZoneUtility.MergeTimeZones(timePeriodErs, config.TimeZoneExtractor.Extract(text, reference), text);
            }

            // Filter ambiguous extractions e.g. 'morgen' in German and Dutch
            timePeriodErs = this.config.ApplyPotentialPeriodAmbiguityHotfix(text, timePeriodErs);

            return timePeriodErs;
        }

        // Cases like "from 3 to 5am" or "between 3:30 and 5" are extracted here
        // Note that cases like "from 3 to 5" will not be extracted here because no "am/pm" or "hh:mm" to infer it's a time period
        // Also cases like "from 3:30 to 4 people" shuold not be extracted as a time period
        private List<Token> MatchSimpleCases(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.SimpleCasesRegex)
            {
                var matches = regex.Matches(text);

                foreach (Match match in matches)
                {
                    // Cases like "from 10:30 to 11", don't necessarily need "am/pm"
                    if (match.Groups[Constants.MinuteGroupName].Success || match.Groups[Constants.SecondGroupName].Success)
                    {
                        // Cases like "from 3:30 to 4" should be supported
                        // Cases like "from 3:30 to 4 on 1/1/2015" should be supported
                        // Cases like "from 3:30 to 4 people" is considered not valid
                        bool endWithValidToken = false;

                        // "No extra tokens after the time period"
                        if (match.Index + match.Length == text.Length)
                        {
                            endWithValidToken = true;
                        }
                        else
                        {
                            var afterStr = text.Substring(match.Index + match.Length);

                            // "End with general ending tokens or "TokenBeforeDate" (like "on")
                            var endWithGeneralEndings = this.config.GeneralEndingRegex.Match(afterStr).Success;
                            var endWithAmPm = match.Groups[Constants.RightAmPmGroupName].Success;

                            if (endWithGeneralEndings || endWithAmPm || afterStr.TrimStart().StartsWith(this.config.TokenBeforeDate, StringComparison.Ordinal))
                            {
                                endWithValidToken = true;
                            }
                            else if ((this.config.Options & DateTimeOptions.EnablePreview) != 0)
                            {
                                endWithValidToken = StartsWithTimeZone(afterStr);
                            }
                        }

                        if (endWithValidToken)
                        {
                            ret.Add(new Token(match.Index, match.Index + match.Length));
                        }
                    }
                    else
                    {
                        // Is there "pm" or "am"?
                        var matchPmStr = match.Groups[Constants.PmGroupName].Value;
                        var matchAmStr = match.Groups[Constants.AmGroupName].Value;
                        var descStr = match.Groups[Constants.DescGroupName].Value;

                        // Check "pm", "am"
                        if (!string.IsNullOrEmpty(matchPmStr) || !string.IsNullOrEmpty(matchAmStr) || !string.IsNullOrEmpty(descStr))
                        {
                            ret.Add(new Token(match.Index, match.Index + match.Length));
                        }
                        else
                        {
                            var afterStr = text.Substring(match.Index + match.Length);

                            if ((this.config.Options & DateTimeOptions.EnablePreview) != 0 && StartsWithTimeZone(afterStr))
                            {
                                ret.Add(new Token(match.Index, match.Index + match.Length));
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private bool StartsWithTimeZone(string afterText)
        {
            var startsWithTimeZone = false;

            var timeZoneErs = config.TimeZoneExtractor.Extract(afterText);
            var firstTimeZone = timeZoneErs.OrderBy(t => t.Start).FirstOrDefault();

            if (firstTimeZone != null)
            {
                var beforeText = afterText.Substring(0, firstTimeZone.Start ?? 0);

                if (string.IsNullOrWhiteSpace(beforeText))
                {
                    startsWithTimeZone = true;
                }
            }

            return startsWithTimeZone;
        }

        private List<Token> MergeTwoTimePoints(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var ers = this.config.SingleTimeExtractor.Extract(text, reference);

            // Merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"

            // Handling ending number as a time point.
            var numErs = this.config.IntegerExtractor.Extract(text);

            // Check if it is an ending number
            if (numErs.Count > 0)
            {
                var timeNumbers = new List<ExtractResult>();

                // check if it is a ending number
                var endingNumber = false;
                var num = numErs[numErs.Count - 1];
                if (num.Start + num.Length == text.Length)
                {
                    endingNumber = true;
                }
                else
                {
                    var afterStr = text.Substring(num.Start + num.Length ?? 0);
                    var endingMatch = this.config.GeneralEndingRegex.Match(afterStr);
                    if (endingMatch.Success)
                    {
                        endingNumber = true;
                    }
                }

                if (endingNumber)
                {
                    timeNumbers.Add(num);
                }

                var i = 0;
                var j = 0;
                while (i < numErs.Count)
                {
                    // find subsequent time point
                    var numEndPoint = numErs[i].Start + numErs[i].Length;
                    while (j < ers.Count && ers[j].Start <= numEndPoint)
                    {
                        j++;
                    }

                    if (j >= ers.Count)
                    {
                        break;
                    }

                    // check connector string
                    var midStr = text.Substring(numEndPoint ?? 0, ers[j].Start - numEndPoint ?? 0);

                    if (config.TillRegex.IsExactMatch(midStr, trim: true) || config.IsConnectorToken(midStr.Trim()))
                    {
                        timeNumbers.Add(numErs[i]);
                    }

                    i++;
                }

                // check overlap
                foreach (var timeNum in timeNumbers)
                {
                    var overlap = false;
                    foreach (var er in ers)
                    {
                        if (er.Start <= timeNum.Start && er.Start + er.Length >= timeNum.Start)
                        {
                            overlap = true;
                        }
                    }

                    if (!overlap)
                    {
                        ers.Add(timeNum);
                    }
                }

                ers.Sort((x, y) => x.Start - y.Start ?? 0);
            }

            var idx = 0;
            while (idx < ers.Count - 1)
            {
                var middleBegin = ers[idx].Start + ers[idx].Length ?? 0;
                var middleEnd = ers[idx + 1].Start ?? 0;

                if (middleEnd - middleBegin <= 0)
                {
                    idx++;
                    continue;
                }

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();

                // Handle "{TimePoint} to {TimePoint}"
                if (config.TillRegex.IsExactMatch(middleStr, trim: true))
                {
                    var periodBegin = ers[idx].Start ?? 0;
                    var periodEnd = (ers[idx + 1].Start ?? 0) + (ers[idx + 1].Length ?? 0);

                    // Handle "from"
                    var beforeStr = text.Substring(0, periodBegin).TrimEnd();
                    var afterStr = text.Substring(periodEnd, text.Length - periodEnd);
                    if (this.config.GetFromTokenIndex(beforeStr, out var fromIndex))
                    {
                        // Handle "from"
                        periodBegin = fromIndex;
                    }
                    else if (this.config.GetBetweenTokenIndex(beforeStr, out var betweenIndex))
                    {
                        // Handle "between"
                        periodBegin = betweenIndex;
                    }
                    else if (this.config.CheckBothBeforeAfter && this.config.GetBetweenTokenIndex(afterStr, out var afterIndex))
                    {
                        // Handle "between" in afterStr
                        periodEnd += afterIndex;
                    }

                    ret.Add(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }

                // Handle "between {TimePoint} and {TimePoint}"
                if (this.config.IsConnectorToken(middleStr))
                {
                    var periodBegin = ers[idx].Start ?? 0;
                    var periodEnd = (ers[idx + 1].Start ?? 0) + (ers[idx + 1].Length ?? 0);

                    // Handle "between"
                    var beforeStr = text.Substring(0, periodBegin).Trim();
                    if (this.config.GetBetweenTokenIndex(beforeStr, out int betweenIndex))
                    {
                        periodBegin = betweenIndex;
                        ret.Add(new Token(periodBegin, periodEnd));
                        idx += 2;
                        continue;
                    }

                    // handle "between...and..." case when "between" follows the datepoints
                    var afterStr = text.Substring(periodEnd, text.Length - periodEnd);
                    if (this.config.CheckBothBeforeAfter && this.config.GetBetweenTokenIndex(afterStr, out int afterIndex))
                    {
                        periodEnd += afterIndex;
                        ret.Add(new Token(periodBegin, periodEnd));

                        // merge two tokens here, increase the index by two
                        idx += 2;
                        continue;
                    }
                }

                idx++;
            }

            return ret;
        }

        private List<Token> MatchTimeOfDay(string text)
        {
            var ret = new List<Token>();
            var matches = this.config.TimeOfDayRegex.Matches(text);
            foreach (Match match in matches)
            {
                Metadata metadata = null;

                if (match.Groups[Constants.MealTimeGroupName].Success)
                {
                    metadata = new Metadata
                    {
                        IsMealtime = true,
                    };
                }

                var token = new Token(match.Index, match.Index + match.Length, metadata);

                ret.Add(token);
            }

            return ret;
        }

        // Support cases like "from 6 to 7" which are pure number ranges
        // Only when the number range is at the end of a sentence, it will be considered as a time range
        private List<Token> MatchPureNumberCases(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.PureNumberRegex)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    var afterStr = text.Substring(match.Index + match.Length);
                    var endingMatch = this.config.GeneralEndingRegex.Match(afterStr);
                    if (endingMatch.Success)
                    {
                        ret.Add(new Token(match.Index, match.Index + match.Length));
                    }
                }
            }

            return ret;
        }
    }
}
