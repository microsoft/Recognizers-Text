using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.InternalCache;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDatePeriodExtractor : IDateTimeExtractor
    {
        private const string ExtractorName = Constants.SYS_DATETIME_DATEPERIOD;

        private static readonly ResultsCache<ExtractResult> ResultsCache = new ResultsCache<ExtractResult>();

        private readonly ICJKDatePeriodExtractorConfiguration config;

        private readonly string keyPrefix;

        public BaseCJKDatePeriodExtractor(ICJKDatePeriodExtractorConfiguration config)
        {
            this.config = config;
            keyPrefix = string.Intern(config.Options + "_" + config.LanguageMarker);
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MatchSimpleCases(text));
            tokens.AddRange(MergeTwoTimePoints(text, referenceTime));
            tokens.AddRange(MatchNumberWithUnit(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // match pattern in simple case
        private List<Token> MatchSimpleCases(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.SimpleCasesRegexes)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            return ret;
        }

        // merge two date
        private List<Token> MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var er = this.config.DatePointExtractor.Extract(text, referenceTime);
            if (er.Count <= 1)
            {
                return ret;
            }

            // merge '{TimePoint} 到 {TimePoint}'
            var idx = 0;
            while (idx < er.Count - 1)
            {
                var middleBegin = er[idx].Start + er[idx].Length ?? 0;
                var middleEnd = er[idx + 1].Start ?? 0;
                if (middleBegin >= middleEnd)
                {
                    idx++;
                    continue;
                }

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();

                if (this.config.TillRegex.IsExactMatch(middleStr, trim: true))
                {
                    var periodBegin = er[idx].Start ?? 0;
                    var periodEnd = (er[idx + 1].Start ?? 0) + (er[idx + 1].Length ?? 0);

                    // handle "从"
                    var beforeStr = text.Substring(0, periodBegin);
                    if (beforeStr.Trim().EndsWith("从", StringComparison.Ordinal))
                    {
                        periodBegin = beforeStr.LastIndexOf("从", StringComparison.Ordinal);
                    }

                    ret.Add(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }

                idx++;
            }

            return ret;
        }

        // extract case like "前两年" "前三个月"
        private List<Token> MatchNumberWithUnit(string text)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var ers = this.config.IntegerExtractor.Extract(text);

            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = this.config.FollowedUnit.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    durations.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length));
                }
            }

            if (this.config.NumberCombinedWithUnit.IsMatch(text))
            {
                var matches = this.config.NumberCombinedWithUnit.Matches(text);
                foreach (Match match in matches)
                {
                    durations.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            foreach (var duration in durations)
            {
                var beforeStr = text.Substring(0, duration.Start);
                if (string.IsNullOrWhiteSpace(beforeStr))
                {
                    continue;
                }

                var match = this.config.PastRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }

                match = this.config.FutureRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(match.Index, duration.End));
                }
            }

            return ret;
        }
    }
}
