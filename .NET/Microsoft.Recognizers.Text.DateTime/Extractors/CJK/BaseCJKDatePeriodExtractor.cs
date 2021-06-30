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
            var simpleCasesResults = Token.MergeAllTokens(tokens, text, ExtractorName);
            tokens.AddRange(MatchComplexCases(text, simpleCasesResults, referenceTime));
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

                    // handle suffix
                    var afterStr = text.Substring(periodEnd);
                    var match = this.config.RangeSuffixRegex.MatchBegin(afterStr, true);
                    if (match.Success)
                    {
                        periodEnd = periodEnd + match.Index + match.Length;
                    }

                    // handle prefix
                    var beforeStr = text.Substring(0, periodBegin);
                    match = this.config.RangePrefixRegex.MatchEnd(beforeStr, true);
                    if (match.Success)
                    {
                        periodBegin = match.Index;
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

        // Complex cases refer to the combination of daterange and datepoint
        // For Example: from|between {DateRange|DatePoint} to|till|and {DateRange|DatePoint}
        private List<Token> MatchComplexCases(string text, List<ExtractResult> simpleDateRangeResults, DateObject reference)
        {
            var er = this.config.DatePointExtractor.Extract(text, reference);

            // Filter out DateRange results that are part of DatePoint results
            // For example, "Feb 1st 2018" => "Feb" and "2018" should be filtered out here
            er.AddRange(simpleDateRangeResults
                .Where(simpleDateRange => !er.Any(datePoint => (datePoint.Start <= simpleDateRange.Start && datePoint.Start + datePoint.Length >= simpleDateRange.Start + simpleDateRange.Length))));

            er = er.OrderBy(t => t.Start).ToList();

            return MergeMultipleExtractions(text, er);
        }

        private List<Token> MergeMultipleExtractions(string text, List<ExtractResult> extractionResults)
        {
            var ret = new List<Token>();
            var metadata = new Metadata
            {
                PossiblyIncludePeriodEnd = true,
            };

            if (extractionResults.Count <= 1)
            {
                return ret;
            }

            var idx = 0;

            while (idx < extractionResults.Count - 1)
            {
                var middleBegin = extractionResults[idx].Start + extractionResults[idx].Length ?? 0;
                var middleEnd = extractionResults[idx + 1].Start ?? 0;
                if (middleBegin >= middleEnd)
                {
                    idx++;
                    continue;
                }

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();
                var endPointStr = extractionResults[idx + 1].Text;

                if (config.TillRegex.IsExactMatch(middleStr, trim: true) || (string.IsNullOrEmpty(middleStr) &&
                    config.TillRegex.MatchBegin(endPointStr, trim: true).Success))
                {
                    var periodBegin = extractionResults[idx].Start ?? 0;
                    var periodEnd = (extractionResults[idx + 1].Start ?? 0) + (extractionResults[idx + 1].Length ?? 0);

                    // handle "from/between" together with till words (till/until/through...)
                    var beforeStr = text.Substring(0, periodBegin);

                    var beforeMatch = this.config.RangePrefixRegex.MatchEnd(beforeStr, trim: true);

                    if (beforeMatch.Success)
                    {
                        periodBegin = beforeMatch.Index;
                    }
                    else
                    {
                        var afterStr = text.Substring(periodEnd);

                        var afterMatch = this.config.RangeSuffixRegex.MatchBegin(afterStr, trim: true);

                        if (afterMatch.Success)
                        {
                            periodEnd += afterMatch.Index + afterMatch.Length;
                        }
                    }

                    ret.Add(new Token(periodBegin, periodEnd, metadata));

                    // merge two tokens here, increase the index by two
                    idx += 2;
                    continue;
                }

                idx++;
            }

            return ret;
        }
    }
}
