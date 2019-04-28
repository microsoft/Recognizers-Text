using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Matcher;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseMergedDateTimeExtractor : IDateTimeExtractor
    {
        private readonly IMergedExtractorConfiguration config;

        public BaseMergedDateTimeExtractor(IMergedExtractorConfiguration config)
        {
            this.config = config;
        }

        public static bool HasTokenIndex(string text, Regex regex, out int index)
        {
            index = -1;

            // Support cases has two or more specific tokens
            // For example, "show me sales after 2010 and before 2018 or before 2000"
            // When extract "before 2000", we need the second "before" which will be matched in the second Regex match
            var match = Regex.Match(text, regex.ToString(), RegexOptions.RightToLeft | RegexOptions.Singleline);

            if (match.Success && string.IsNullOrEmpty(text.Substring(match.Index + match.Length)))
            {
                index = match.Index;
                return true;
            }

            return false;
        }

        public static bool TryMergeModifierToken(ExtractResult er, Regex tokenRegex, string text)
        {
            var beforeStr = text.Substring(0, er.Start ?? 0).ToLowerInvariant();

            if (HasTokenIndex(beforeStr.TrimEnd(), tokenRegex, out var tokenIndex))
            {
                var modLength = beforeStr.Length - tokenIndex;

                er.Length += modLength;
                er.Start -= modLength;
                er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);

                return true;
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

            var originText = text;
            List<MatchResult<string>> superfluousWordMatches = null;
            if ((this.config.Options & DateTimeOptions.EnablePreview) != 0)
            {
                text = MatchingUtil.PreProcessTextRemoveSuperfluousWords(
                    text,
                    this.config.SuperfluousWordMatcher,
                    out superfluousWordMatches);
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

            if ((this.config.Options & DateTimeOptions.EnablePreview) != 0)
            {
                ret = MatchingUtil.PosProcessExtractionRecoverSuperfluousWords(ret, superfluousWordMatches, originText);
            }

            return ret;
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

        private List<ExtractResult> FilterAmbiguity(List<ExtractResult> ers, string text)
        {
            if (this.config.AmbiguityFiltersDict != null)
            {
                foreach (var regex in config.AmbiguityFiltersDict)
                {
                    if (regex.Key.IsMatch(text))
                    {
                        var matches = regex.Value.Matches(text).Cast<Match>();
                        ers = ers.Where(er =>
                                !matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start))
                            .ToList();
                    }
                }
            }

            return ers;
        }

        private bool FilterAmbiguousSingleWord(ExtractResult er, string text)
        {
            if (config.SingleAmbiguousMonthRegex.IsMatch(er.Text.ToLowerInvariant()))
            {
                var stringBefore = text.Substring(0, (int)er.Start).TrimEnd();
                if (!config.PrepositionSuffixRegex.IsMatch(stringBefore))
                {
                    return true;
                }
            }

            return false;
        }

        // Handle cases like "move 3pm appointment to 4"
        private List<ExtractResult> NumberEndingRegexMatch(string text, IEnumerable<ExtractResult> extractResults)
        {
            var tokens = new List<Token>();

            foreach (var extractResult in extractResults)
            {
                if (extractResult.Type.Equals(Constants.SYS_DATETIME_TIME) ||
                    extractResult.Type.Equals(Constants.SYS_DATETIME_DATETIME))
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
                var success = TryMergeModifierToken(er, config.BeforeRegex, text);

                if (!success)
                {
                    success = TryMergeModifierToken(er, config.AfterRegex, text);
                }

                if (!success)
                {
                    success = TryMergeModifierToken(er, config.SinceRegex, text);
                }

                if (!success)
                {
                    TryMergeModifierToken(er, config.AroundRegex, text);
                }

                if (er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD) || er.Type.Equals(Constants.SYS_DATETIME_DATE) || er.Type.Equals(Constants.SYS_DATETIME_TIME))
                {
                    // 2012 or after/above, 3 pm or later
                    var afterStr = text.Substring((er.Start ?? 0) + (er.Length ?? 0)).ToLowerInvariant();

                    var match = config.SuffixAfterRegex.MatchBegin(afterStr.TrimStart(), trim: true);

                    if (match.Success)
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

                            if (nextEr == null || !nextStr.StartsWith(nextEr.Text))
                            {
                                isFollowedByOtherEntity = false;
                            }
                        }

                        if (!isFollowedByOtherEntity)
                        {
                            var modLength = match.Length + afterStr.IndexOf(match.Value, StringComparison.Ordinal);
                            er.Length += modLength;
                            er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                        }
                    }
                }
            }

            return ers;
        }
    }
}
