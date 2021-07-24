﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDateTimeExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIME; // "DateTime";

        private readonly ICJKDateTimeExtractorConfiguration config;

        public BaseCJKDateTimeExtractor(ICJKDateTimeExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MergeDateAndTime(text, referenceTime));
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(TimeOfToday(text, referenceTime));
            tokens.AddRange(DurationWithAgoAndLater(text, referenceTime));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // Match now
        public List<Token> BasicRegexMatch(string text)
        {
            var ret = new List<Token>();
            text = text.Trim();

            // handle "now"
            var matches = this.config.NowRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        // Merge a Date entity and a Time entity, like "明天早上七点"
        public List<Token> MergeDateAndTime(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var ers = this.config.DatePointExtractor.Extract(text, referenceTime);
            if (ers.Count == 0)
            {
                return ret;
            }

            ers.AddRange(this.config.TimePointExtractor.Extract(text, referenceTime));
            if (ers.Count < 2)
            {
                return ret;
            }

            ers = ers.OrderBy(o => o.Start).ToList();

            var i = 0;
            while (i < ers.Count - 1)
            {
                var j = i + 1;
                while (j < ers.Count && ers[i].IsOverlap(ers[j]))
                {
                    j++;
                }

                if (j >= ers.Count)
                {
                    break;
                }

                if (ers[i].Type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal) &&
                    ers[j].Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal))
                {
                    var middleBegin = ers[i].Start + ers[i].Length ?? 0;
                    var middleEnd = ers[j].Start ?? 0;
                    if (middleBegin > middleEnd)
                    {
                        break;
                    }

                    var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();
                    if (string.IsNullOrEmpty(middleStr) || middleStr.Equals(",") || this.config.PrepositionRegex.IsMatch(middleStr))
                    {
                        var begin = ers[i].Start ?? 0;
                        var end = (ers[j].Start ?? 0) + (ers[j].Length ?? 0);
                        ret.Add(new Token(begin, end));
                    }

                    i = j + 1;
                    continue;
                }

                i = j;
            }

            return ret;
        }

        // Parse a specific time of today, tonight, this afternoon, "今天下午七点"
        public List<Token> TimeOfToday(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var ers = this.config.TimePointExtractor.Extract(text, referenceTime);
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);

                // handle "今晚7点"
                var innerMatch = this.config.NightRegex.MatchBegin(er.Text, trim: true);

                if (innerMatch.Success)
                {
                    beforeStr = text.Substring(0, (er.Start ?? 0) + innerMatch.Length);
                }

                if (string.IsNullOrEmpty(beforeStr))
                {
                    continue;
                }

                var match = this.config.TimeOfTodayRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    var begin = match.Index;
                    var end = er.Start + er.Length ?? 0;
                    ret.Add(new Token(begin, end));
                }
            }

            return ret;
        }

        // Process case like "5分钟前" "二小时后"
        private List<Token> DurationWithAgoAndLater(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();

            var durationEr = this.config.DurationExtractor.Extract(text, referenceTime);

            foreach (var er in durationEr)
            {
                var pos = (int)er.Start + (int)er.Length;

                if (pos < text.Length)
                {
                    var suffix = text.Substring(pos);
                    var beforeMatch = this.config.BeforeRegex.Match(suffix);
                    var afterMatch = this.config.AfterRegex.Match(suffix);

                    if ((beforeMatch.Success && suffix.StartsWith(beforeMatch.Value, StringComparison.Ordinal)) ||
                        (afterMatch.Success && suffix.StartsWith(afterMatch.Value, StringComparison.Ordinal)))
                    {
                        var metadata = new Metadata() { IsDurationWithAgoAndLater = true };
                        ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + 1, metadata));
                    }
                }
            }

            return ret;
        }
    }
}
