﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseMergedDateTimeExtractor : IDateTimeExtractor
    {
        private static readonly Regex NumberOrConnectorRegex = new Regex(@"^[0-9-]+$", RegexOptions.Compiled);

        private readonly IMergedExtractorConfiguration config;

        public BaseMergedDateTimeExtractor(IMergedExtractorConfiguration config)
        {
            this.config = config;
        }

        public static bool HasTokenIndex(string text, Regex regex, out int index, bool inPrefix)
        {
            index = -1;

            // Support cases has two or more specific tokens
            // For example, "show me sales after 2010 and before 2018 or before 2000"
            // When extract "before 2000", we need the second "before" which will be matched in the second Regex match
            RegexOptions regexFlags = inPrefix ? RegexOptions.RightToLeft | RegexOptions.Singleline : RegexOptions.Singleline;
            var match = Regex.Match(text, regex.ToString(), regexFlags);

            if (match.Success)
            {
                var subStr = inPrefix ? text.Substring(match.Index + match.Length) : text.Substring(0, match.Index);
                if (string.IsNullOrEmpty(subStr))
                {
                    index = inPrefix ? match.Index : match.Length;
                    return true;
                }
            }

            return false;
        }

        public bool TryMergeModifierToken(ExtractResult er, Regex tokenRegex, string text, bool potentialAmbiguity = false)
        {
            var beforeStr = text.Substring(0, er.Start ?? 0);
            var afterStr = text.Substring(er.Start + er.Length ?? 0);

            // Avoid adding mod for ambiguity cases, such as "from" in "from ... to ..." should not add mod
            if (potentialAmbiguity && this.config.AmbiguousRangeModifierPrefix != null && this.config.AmbiguousRangeModifierPrefix.IsMatch(beforeStr))
            {
                var matches = this.config.PotentialAmbiguousRangeRegex.Matches(text).Cast<Match>();

                // Weak ambiguous matches are considered only if the extraction is of type range
                if (matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start && !(m.Groups[Constants.AmbiguousPattern].Success && !er.Type.EndsWith("range"))))
                {
                    return false;
                }
            }

            if (HasTokenIndex(beforeStr.TrimEnd(), tokenRegex, out var tokenIndex, inPrefix: true))
            {
                var modLength = beforeStr.Length - tokenIndex;

                er.Length += modLength;
                er.Start -= modLength;
                er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);

                er.Metadata = AssignModMetadata(er.Metadata);

                return true;
            }
            else if (this.config.CheckBothBeforeAfter)
            {
                // check also afterStr
                afterStr = text.Substring(er.Start + er.Length ?? 0);
                if (HasTokenIndex(afterStr.TrimStart(), tokenRegex, out tokenIndex, inPrefix: false))
                {
                    var modLength = tokenIndex + afterStr.Length - afterStr.TrimStart().Length;

                    er.Length += modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                    er.Data = Constants.HAS_MOD;
                    er.Metadata = AssignModMetadata(er.Metadata);

                    return true;
                }
            }

            return false;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject reference)
        {
            var ret = new List<ExtractResult>();

            if (((this.config.Options & DateTimeOptions.FailFast) != 0) && IsFailFastCase(text))
            {
                // @TODO needs better handling of holidays and timezones.
                // AddTo(ret, this.config.HolidayExtractor.Extract(text, reference), text);
                // ret = AddMod(ret, text);

                return ret;
            }

            var originText = text;
            List<MatchResult<string>> superfluousWordMatches = null;

            // Push
            if ((this.config.Options & DateTimeOptions.EnablePreview) != 0)
            {
                text = MatchingUtil.PreProcessTextRemoveSuperfluousWords(text, this.config.SuperfluousWordMatcher, out superfluousWordMatches);
            }

            // The order is important, since there can be conflicts in merging
            AddTo(ret, this.config.DateExtractor.Extract(text, reference), text);
            AddTo(ret, this.config.TimeExtractor.Extract(text, reference), text);
            AddTo(ret, this.config.DatePeriodExtractor.Extract(text, reference), text);
            AddTo(ret, this.config.DurationExtractor.Extract(text, reference), text);
            AddTo(ret, this.config.TimePeriodExtractor.Extract(text, reference), text);
            AddTo(ret, this.config.DateTimePeriodExtractor.Extract(text, reference), text);
            AddTo(ret, this.config.DateTimeExtractor.Extract(text, reference), text);
            AddTo(ret, this.config.SetExtractor.Extract(text, reference), text);
            AddTo(ret, this.config.HolidayExtractor.Extract(text, reference), text);

            if ((this.config.Options & DateTimeOptions.EnablePreview) != 0)
            {
                AddTo(ret, this.config.TimeZoneExtractor.Extract(text, reference), text);
                ret = this.config.TimeZoneExtractor.RemoveAmbiguousTimezone(ret);
            }

            // This should be the last extraction step, as it needs to determine if the previous text contains time or not
            AddTo(ret, NumberEndingRegexMatch(text, ret), text);

            // Modify time entity to an alternative DateTime expression if it follows a DateTime entity
            if ((this.config.Options & DateTimeOptions.ExtendedTypes) != 0)
            {
                ret = this.config.DateTimeAltExtractor.Extract(ret, text, reference);
            }

            ret = FilterUnspecificDatePeriod(ret);

            // Remove common ambiguous cases
            ret = FilterAmbiguity(ret, text);

            ret = AddMod(ret, text);

            // Filtering
            if ((this.config.Options & DateTimeOptions.CalendarMode) != 0)
            {
                ret = CheckCalendarModeFilters(ret, text);
            }

            ret = ret.OrderBy(p => p.Start).ToList();

            // Merge overlapping results
            ret = ExtractResultExtension.MergeAllResults(ret);

            // Pop
            if ((this.config.Options & DateTimeOptions.EnablePreview) != 0)
            {
                ret = MatchingUtil.PostProcessRecoverSuperfluousWords(ret, superfluousWordMatches, originText);
            }

            return ret;
        }

        private Metadata AssignModMetadata(Metadata metadata)
        {
            if (metadata == null)
            {
                metadata = new Metadata { HasMod = true };
            }
            else
            {
                metadata.HasMod = true;
            }

            return metadata;
        }

        private bool IsFailFastCase(string input)
        {
            return (config.FailFastRegex != null) && (!config.FailFastRegex.IsMatch(input));
        }

        private List<ExtractResult> CheckCalendarModeFilters(List<ExtractResult> ers, string text)
        {
            foreach (var er in ers.Reverse<ExtractResult>())
            {
                foreach (var regex in this.config.TermFilterRegexes)
                {
                    var match = regex.Match(er.Text);
                    if (match.Success)
                    {
                        ers.Remove(er);
                    }
                }
            }

            return ers;
        }

        private void AddTo(List<ExtractResult> dst, List<ExtractResult> src, string text)
        {
            foreach (var result in src)
            {
                if ((config.Options & DateTimeOptions.SkipFromToMerge) != 0)
                {
                    if (ShouldSkipFromToMerge(result))
                    {
                        continue;
                    }
                }

                var isFound = false;
                var overlapIndexes = new List<int>();
                var firstIndex = -1;
                for (var i = 0; i < dst.Count; i++)
                {
                    if (dst[i].IsOverlap(result))
                    {
                        isFound = true;
                        if (dst[i].IsCover(result))
                        {
                            if (firstIndex == -1)
                            {
                                firstIndex = i;
                            }

                            overlapIndexes.Add(i);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (!isFound)
                {
                    dst.Add(result);
                }
                else if (overlapIndexes.Any())
                {
                    var tempDst = dst.Where((_, i) => !overlapIndexes.Contains(i)).ToList();

                    // Insert at the first overlap occurence to keep the order
                    tempDst.Insert(firstIndex, result);
                    dst.Clear();
                    dst.AddRange(tempDst);
                }

                dst.Sort((a, b) => (int)(a.Start - b.Start));
            }
        }

        private bool ShouldSkipFromToMerge(ExtractResult er)
        {
            return config.FromToRegex.IsMatch(er.Text);
        }

        private List<ExtractResult> FilterUnspecificDatePeriod(List<ExtractResult> ers)
        {
            ers.RemoveAll(o => this.config.UnspecificDatePeriodRegex.IsMatch(o.Text));
            return ers;
        }

        private List<ExtractResult> FilterAmbiguity(List<ExtractResult> extractResults, string text)
        {
            if (this.config.AmbiguityFiltersDict != null)
            {
                foreach (var regex in this.config.AmbiguityFiltersDict)
                {
                    foreach (var extractResult in extractResults)
                    {
                        if (regex.Key.IsMatch(extractResult.Text))
                        {
                            var matches = regex.Value.Matches(text).Cast<Match>();
                            extractResults = extractResults.Where(er => !matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start))
                                .ToList();
                        }
                    }
                }
            }

            // @TODO: Refactor to remove this method and use the general ambiguity filter approach
            extractResults = extractResults.Where(er => !(NumberOrConnectorRegex.IsMatch(er.Text) &&
                    (text.Substring(0, (int)er.Start).Trim().EndsWith("-", StringComparison.Ordinal) || text.Substring((int)(er.Start + er.Length)).Trim().StartsWith("-", StringComparison.Ordinal))))
                    .ToList();

            return extractResults;
        }

        // Handle cases like "move 3pm appointment to 4"
        private List<ExtractResult> NumberEndingRegexMatch(string text, IEnumerable<ExtractResult> extractResults)
        {
            var tokens = new List<Token>();

            foreach (var extractResult in extractResults)
            {
                if (extractResult.Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal) ||
                    extractResult.Type.Equals(Constants.SYS_DATETIME_DATETIME, StringComparison.Ordinal))
                {
                    var stringAfter = text.Substring((int)extractResult.Start + (int)extractResult.Length);
                    var match = this.config.NumberEndingPattern.Match(stringAfter);
                    if (match.Success)
                    {
                        var newTime = match.Groups["newTime"];
                        var numRes = this.config.IntegerExtractor.Extract(newTime.ToString());
                        if (numRes.Count == 0)
                        {
                            continue;
                        }

                        var startPosition = (int)extractResult.Start + (int)extractResult.Length + newTime.Index;
                        tokens.Add(new Token(startPosition, startPosition + newTime.Length));
                    }
                }
            }

            return Token.MergeAllTokens(tokens, text, Constants.SYS_DATETIME_TIME);
        }

        private List<ExtractResult> AddMod(List<ExtractResult> ers, string text)
        {
            foreach (var er in ers)
            {
                // AroundRegex is matched non-exclusively before the other relative regexes in order to catch also combined modifiers e.g. "before around 1pm"
                TryMergeModifierToken(er, config.AroundRegex, text);

                // BeforeRegex in Dutch contains the term "voor" which is ambiguous (meaning both "for" and "before")
                var success = TryMergeModifierToken(er, config.BeforeRegex, text, potentialAmbiguity: true);

                if (!success)
                {
                    success = TryMergeModifierToken(er, config.AfterRegex, text);
                }

                if (!success)
                {
                    // SinceRegex in English contains the term "from" which is potentially ambiguous with ranges in the form "from X to Y"
                    success = TryMergeModifierToken(er, config.SinceRegex, text, potentialAmbiguity: true);
                }

                if (!success)
                {
                    success = TryMergeModifierToken(er, config.EqualRegex, text);
                }

                if (er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD, StringComparison.Ordinal) ||
                    er.Type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal) ||
                    er.Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal))
                {
                    // 2012 or after/above, 3 pm or later
                    var afterStr = text.Substring((er.Start ?? 0) + (er.Length ?? 0));

                    if (afterStr.Length > 1)
                    {

                        var match = config.SuffixAfterRegex.MatchBegin(afterStr.TrimStart(), trim: true);

                        if (match.Success && match.Value != ".")
                        {
                            var isFollowedByOtherEntity = true;

                            if (match.Length == afterStr.Trim().Length)
                            {
                                isFollowedByOtherEntity = false;
                            }
                            else
                            {
                                var nextStr = afterStr.Trim().Substring(match.Length).Trim();
                                var nextEr = ers.FirstOrDefault(t => t.Start > er.Start);

                                if (nextEr == null || !nextStr.StartsWith(nextEr.Text, StringComparison.Ordinal))
                                {
                                    isFollowedByOtherEntity = false;
                                }
                            }

                            if (!isFollowedByOtherEntity)
                            {
                                var modLength = match.Length + afterStr.IndexOf(match.Value, StringComparison.Ordinal);
                                er.Length += modLength;
                                er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);

                                er.Metadata = AssignModMetadata(er.Metadata);
                            }
                        }
                    }
                }
            }

            return ers;
        }
    }
}
