﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDurationExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DURATION;

        private readonly IDurationExtractorConfiguration config;

        private readonly bool merge;

        public BaseDurationExtractor(IDurationExtractorConfiguration config, bool merge = true)
        {
            this.config = config;
            this.merge = merge;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject reference)
        {
            var tokens = new List<Token>();
            var numberWithUnitTokens = NumberWithUnit(text);

            tokens.AddRange(numberWithUnitTokens);
            tokens.AddRange(NumberWithUnitAndSuffix(text, numberWithUnitTokens));
            tokens.AddRange(ImplicitDuration(text));

            var rets = Token.MergeAllTokens(tokens, text, ExtractorName);

            // First MergeMultipleDuration then ResolveMoreThanOrLessThanPrefix so cases like "more than 4 days and less than 1 week" will not be merged into one "multipleDuration"
            if (this.merge)
            {
                rets = MergeMultipleDuration(text, rets);
            }

            rets = TagInequalityPrefix(text, rets);

            return rets;
        }

        // handle cases look like: {more than | less than} {duration}?
        private List<ExtractResult> TagInequalityPrefix(string text, List<ExtractResult> ers)
        {
            foreach (var er in ers)
            {
                var beforeString = text.Substring(0, (int)er.Start);
                var afterString = text.Substring((int)er.Start + (int)er.Length);
                bool isInequalityPrefixMatched = false;
                bool isMatchAfter = false;

                var match = config.MoreThanRegex.MatchEnd(beforeString, trim: true);

                // check also afterString
                if (this.config.CheckBothBeforeAfter && !match.Success)
                {
                    match = config.MoreThanRegex.MatchesBegin(afterString, trim: true);
                    isMatchAfter = true;
                }

                // The second condition is necessary so for "1 week" in "more than 4 days and less than 1 week", it will not be tagged incorrectly as "more than"
                if (match.Success)
                {
                    er.Data = Constants.MORE_THAN_MOD;
                    isInequalityPrefixMatched = true;
                }

                if (!isInequalityPrefixMatched)
                {
                    match = config.LessThanRegex.MatchEnd(beforeString, trim: true);

                    // check also afterString
                    if (this.config.CheckBothBeforeAfter && !match.Success)
                    {
                        match = config.LessThanRegex.MatchesBegin(afterString, trim: true);
                        isMatchAfter = true;
                    }

                    if (match.Success)
                    {
                        er.Data = Constants.LESS_THAN_MOD;
                        isInequalityPrefixMatched = true;
                    }
                }

                if (isInequalityPrefixMatched)
                {
                    if (!isMatchAfter)
                    {
                        er.Length += er.Start - match.Index;
                        er.Start = match.Index;
                        er.Text = text.Substring((int)er.Start, (int)er.Length);
                    }
                    else
                    {
                        er.Length += match.Index + match.Length;
                        er.Text = text.Substring((int)er.Start, (int)er.Length);
                    }
                }
            }

            return ers;
        }

        // handle cases look like: {number} {unit}? and {an|a} {half|quarter} {unit}?
        // define the part "and {an|a} {half|quarter}" as Suffix
        private List<Token> NumberWithUnitAndSuffix(string text, List<Token> ers)
        {
            var ret = new List<Token>();
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length);
                var match = this.config.SuffixAndRegex.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(er.Start, (er.Start + er.Length) + match.Length));
                }
                else if (this.config.CheckBothBeforeAfter)
                {
                    // check also beforeStr
                    var beforeStr = text.Substring(0, er.Start);
                    match = this.config.SuffixAndRegex.MatchEnd(beforeStr, trim: true);
                    if (match.Success)
                    {
                        ret.Add(new Token(match.Index, er.Start + er.Length));
                    }
                }
            }

            return ret;
        }

        // simple cases of a number followed by unit
        private List<Token> NumberWithUnit(string text)
        {
            var ret = new List<Token>();
            var ers = ExtractNumbersBeforeUnit(text);

            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = this.config.FollowedUnit.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length));
                }
            }

            // handle "3hrs"
            ret.AddRange(Token.GetTokenFromRegex(config.NumberCombinedWithUnit, text));

            // handle "an hour"
            ret.AddRange(Token.GetTokenFromRegex(config.AnUnitRegex, text));

            // handle "few" related cases
            ret.AddRange(Token.GetTokenFromRegex(config.InexactNumberUnitRegex, text));

            return ret;
        }

        // @TODO improve re-use with Parser
        private List<ExtractResult> ExtractNumbersBeforeUnit(string text)
        {
            var ers = this.config.CardinalExtractor.Extract(text);

            // In special cases some languages will treat "both" as a number to be combined with duration units.
            var specialNumberUnitTokens = Token.GetTokenFromRegex(config.SpecialNumberUnitRegex, text);

            foreach (var token in specialNumberUnitTokens)
            {
                var er = new ExtractResult
                {
                    Start = token.Start,
                    Length = token.Length,
                    Text = text.Substring(token.Start, token.Length),
                };

                ers.Add(er);
            }

            return ers;
        }

        // handle cases that don't contain number
        private List<Token> ImplicitDuration(string text)
        {
            var ret = new List<Token>();

            // handle "all day", "all year"
            ret.AddRange(Token.GetTokenFromRegex(config.AllRegex, text));

            // handle "half day", "half year"
            ret.AddRange(Token.GetTokenFromRegex(config.HalfRegex, text));

            // handle "next day", "last year"
            ret.AddRange(Token.GetTokenFromRegex(config.RelativeDurationUnitRegex, text));

            // handle "during/for the day/week/month/year"
            if ((config.Options & DateTimeOptions.CalendarMode) != 0)
            {
                ret.AddRange(Token.GetTokenFromRegex(config.DuringRegex, text));
            }

            return ret;
        }

        private List<ExtractResult> MergeMultipleDuration(string text, List<ExtractResult> extractorResults)
        {
            if (extractorResults.Count <= 1)
            {
                return extractorResults;
            }

            var unitMap = this.config.UnitMap;
            var unitValueMap = this.config.UnitValueMap;
            var unitRegex = this.config.DurationUnitRegex;
            List<ExtractResult> results = new List<ExtractResult>();
            List<List<ExtractResult>> separateResults = new List<List<ExtractResult>>();

            var firstExtractionIndex = 0;
            var timeUnit = 0;
            var totalUnit = 0;
            while (firstExtractionIndex < extractorResults.Count)
            {
                string curUnit = null;
                var unitMatch = unitRegex.Match(extractorResults[firstExtractionIndex].Text);

                if (unitMatch.Success && unitMap.ContainsKey(unitMatch.Groups["unit"].ToString()))
                {
                    curUnit = unitMatch.Groups["unit"].ToString();
                    totalUnit++;
                    if (DurationParsingUtil.IsTimeDurationUnit(unitMap[curUnit]))
                    {
                        timeUnit++;
                    }
                }

                if (string.IsNullOrEmpty(curUnit))
                {
                    firstExtractionIndex++;
                    continue;
                }

                // Add extraction to list of separate results (needed in case the extractions should not be merged)
                List<ExtractResult> separateList = new List<ExtractResult>() { extractorResults[firstExtractionIndex] };

                var secondExtractionIndex = firstExtractionIndex + 1;
                while (secondExtractionIndex < extractorResults.Count)
                {
                    var valid = false;
                    var midStrBegin = extractorResults[secondExtractionIndex - 1].Start + extractorResults[secondExtractionIndex - 1].Length ?? 0;
                    var midStrEnd = extractorResults[secondExtractionIndex].Start ?? 0;
                    var midStr = text.Substring(midStrBegin, midStrEnd - midStrBegin);
                    var match = this.config.DurationConnectorRegex.Match(midStr);
                    if (match.Success)
                    {
                        unitMatch = unitRegex.Match(extractorResults[secondExtractionIndex].Text);
                        if (unitMatch.Success && unitMap.ContainsKey(unitMatch.Groups["unit"].ToString()))
                        {
                            var nextUnitStr = unitMatch.Groups["unit"].ToString();
                            if (unitValueMap[nextUnitStr] != unitValueMap[curUnit])
                            {
                                valid = true;
                                if (unitValueMap[nextUnitStr] < unitValueMap[curUnit])
                                {
                                    curUnit = nextUnitStr;
                                }
                            }

                            totalUnit++;
                            if (DurationParsingUtil.IsTimeDurationUnit(unitMap[nextUnitStr]))
                            {
                                timeUnit++;
                            }
                        }
                    }

                    if (!valid)
                    {
                        break;
                    }

                    // Add extraction to list of separate results (needed in case the extractions should not be merged)
                    separateList.Add(extractorResults[secondExtractionIndex]);

                    secondExtractionIndex++;
                }

                if (secondExtractionIndex - 1 > firstExtractionIndex)
                {
                    var node = new ExtractResult();
                    node.Start = extractorResults[firstExtractionIndex].Start;
                    node.Length = extractorResults[secondExtractionIndex - 1].Start + extractorResults[secondExtractionIndex - 1].Length - node.Start;
                    node.Text = text.Substring(node.Start ?? 0, node.Length ?? 0);
                    node.Type = extractorResults[firstExtractionIndex].Type;

                    // Add multiple duration type to extract result
                    string type = Constants.MultipleDuration_DateTime; // Default type
                    if (timeUnit == totalUnit)
                    {
                        type = Constants.MultipleDuration_Time;
                    }
                    else if (timeUnit == 0)
                    {
                        type = Constants.MultipleDuration_Date;
                    }

                    node.Data = type;

                    results.Add(node);

                    timeUnit = 0;
                    totalUnit = 0;
                }
                else
                {
                    results.Add(extractorResults[firstExtractionIndex]);
                }

                // Add list of separate extractions to separateResults, so that there is a 1 to 1 correspondence
                // between results (list of merged extractions) and separateResults (list of unmerged extractions)
                separateResults.Add(separateList);

                firstExtractionIndex = secondExtractionIndex;
            }

            // If the first and last elements of a group of contiguous extractions are both preceded/followed by modifiers,
            // they should not be merged, e.g. "last 2 weeks and 3 days ago"
            for (int i = results.Count - 1; i >= 0; i--)
            {
                var start = (int)results[i].Start;
                var end = start + (int)results[i].Length;
                var beforeStr = text.Substring(0, start);
                var afterStr = text.Substring(end);
                var beforeMod = this.config.ModPrefixRegex.MatchEnd(beforeStr, trim: true);
                var afterMod = this.config.ModSuffixRegex.MatchBegin(afterStr, trim: true);
                if (beforeMod.Success && afterMod.Success)
                {
                    results.RemoveAt(i);
                    results.InsertRange(i, separateResults[i]);
                }
            }

            return results;
        }
    }
}
