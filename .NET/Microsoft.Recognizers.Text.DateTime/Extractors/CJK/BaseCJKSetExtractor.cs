// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKSetExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        private readonly ICJKSetExtractorConfiguration config;

        public BaseCJKSetExtractor(ICJKSetExtractorConfiguration config)
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
            tokens.AddRange(MatchEachUnit(text));
            tokens.AddRange(MatchEachDuration(text, referenceTime));
            tokens.AddRange(MatchEach(this.config.DateExtractor, text, referenceTime));
            tokens.AddRange(MatchEach(this.config.DateTimeExtractor, text, referenceTime));
            tokens.AddRange(MatchEach(this.config.TimePeriodExtractor, text, referenceTime));
            tokens.AddRange(MatchEach(this.config.TimeExtractor, text, referenceTime));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        public List<Token> MatchEachDuration(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();

            var ers = this.config.DurationExtractor.Extract(text, referenceTime);
            foreach (var er in ers)
            {
                // "each last summer" doesn't make sense
                if (this.config.LastRegex.IsMatch(er.Text))
                {
                    continue;
                }

                var beforeStr = text.Substring(0, er.Start ?? 0);
                var match = this.config.EachPrefixRegex.Match(beforeStr);
                if (match.Success)
                {
                    ret.Add(new Token(match.Index, er.Start + er.Length ?? 0));
                }
                else
                {
                    var afterStr = text.Substring(er.Start + er.Length ?? 0);
                    match = this.config.EachSuffixRegex.Match(afterStr);
                    if (match.Success)
                    {
                        ret.Add(new Token(er.Start ?? 0, er.Length + match.Length ?? 00));
                    }
                }
            }

            return ret;
        }

        public List<Token> MatchEachUnit(string text)
        {
            var ret = new List<Token>();

            // handle "each month"
            var matches = this.config.EachUnitRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        public List<Token> MatchEach(IDateTimeExtractor extractor, string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var ers = extractor.Extract(text, referenceTime);
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);
                var match = this.config.EachPrefixRegex.Match(beforeStr);
                if (match.Success)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length + (er.Length ?? 0)));
                }
                else if (er.Type == Constants.SYS_DATETIME_TIME || er.Type == Constants.SYS_DATETIME_DATE)
                {
                    // Cases like "every day at 2pm" or "every year on April 15th"
                    var eachRegex = er.Type == Constants.SYS_DATETIME_TIME ? this.config.EachDayRegex : this.config.EachDateUnitRegex;
                    match = eachRegex.Match(beforeStr);
                    if (match.Success)
                    {
                        ret.Add(new Token(match.Index, match.Index + match.Length + (er.Length ?? 0)));
                    }
                }
            }

            return ret;
        }
    }
}
