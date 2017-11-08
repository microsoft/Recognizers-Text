using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDatePeriodExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATEPERIOD; // "DatePeriod";

        private readonly IDatePeriodExtractorConfiguration config;

        public BaseDatePeriodExtractor(IDatePeriodExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject reference)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MatchSimpleCases(text));
            tokens.AddRange(MergeTwoTimePoints(text, reference));
            tokens.AddRange(MatchDuration(text, reference));
            tokens.AddRange(SingleTimePointWithPatterns(text, reference));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

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

        private List<Token> MergeTwoTimePoints(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var er = this.config.DatePointExtractor.Extract(text, reference);
            if (er.Count <= 1)
            {
                return ret;
            }

            // merge '{TimePoint} to {TimePoint}'
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

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLowerInvariant();
                var match = this.config.TillRegex.Match(middleStr);
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    var periodBegin = er[idx].Start ?? 0;
                    var periodEnd = (er[idx + 1].Start ?? 0) + (er[idx + 1].Length ?? 0);

                    // handle "desde"
                    var beforeStr = text.Substring(0, periodBegin).Trim().ToLowerInvariant();
                    if (this.config.GetFromTokenIndex(beforeStr, out int fromIndex)
                        || this.config.GetBetweenTokenIndex(beforeStr, out fromIndex))
                    {
                        periodBegin = fromIndex;
                    }

                    ret.Add(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }

                if (this.config.HasConnectorToken(middleStr))
                {
                    var periodBegin = er[idx].Start ?? 0;
                    var periodEnd = (er[idx + 1].Start ?? 0) + (er[idx + 1].Length ?? 0);

                    // handle "entre"
                    var beforeStr = text.Substring(0, periodBegin).Trim().ToLowerInvariant();
                    if (this.config.GetBetweenTokenIndex(beforeStr, out int beforeIndex))
                    {
                        periodBegin = beforeIndex;
                        ret.Add(new Token(periodBegin, periodEnd));
                        idx += 2;
                        continue;
                    }
                }
                idx++;
            }

            return ret;
        }

        //Extract the month of date, week of date to a date range
        private List<Token> SingleTimePointWithPatterns(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var er = this.config.DatePointExtractor.Extract(text, reference);
            if (er.Count < 1)
            {
                return ret;
            }

            foreach (var extractionResult in er)
            {
                if (extractionResult.Start != null && extractionResult.Length!=null)
                {
                    string beforeString = text.Substring(0, (int)extractionResult.Start);
                    ret.AddRange(GetTokenForRegexMatching(beforeString, config.WeekOfRegex, extractionResult));
                    ret.AddRange(GetTokenForRegexMatching(beforeString, config.MonthOfRegex, extractionResult));
                }
            }

            return ret;
        }

        private List<Token> GetTokenForRegexMatching(string text, Regex regex, ExtractResult er)
        {
            var ret = new List<Token>();
            var match = regex.Match(text);
            if (match.Success && text.Trim().EndsWith(match.Value.Trim()))
            {
                var startIndex = text.LastIndexOf(match.Value);
                ret.Add(new Token(startIndex, (int)er.Start + (int)er.Length));
            }
            return ret;
        }

        public List<Token> MatchDuration(string text, DateObject reference)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var durationExtractions = config.DurationExtractor.Extract(text, reference);
            foreach (var durationExtraction in durationExtractions)
            {
                var match = config.DateUnitRegex.Match(durationExtraction.Text);
                if (match.Success)
                {
                    durations.Add(new Token(durationExtraction.Start ?? 0,
                        (durationExtraction.Start + durationExtraction.Length ?? 0)));
                }
            }

            foreach (var duration in durations)
            {
                var beforeStr = text.Substring(0, duration.Start).ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(beforeStr))
                {
                    continue;
                }

                var match = this.config.PastRegex.Match(beforeStr);
                if (MatchRegexInPrefix(beforeStr, match))
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }

                match = this.config.FutureRegex.Match(beforeStr);
                if (MatchRegexInPrefix(beforeStr, match))
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }

                // in Range Weeks should be handled as dateRange here
                match = config.InConnectorRegex.Match(beforeStr);
                if (MatchRegexInPrefix(beforeStr, match))
                {
                    var startToken = match.Index;
                    match = config.RangeUnitRegex.Match(text.Substring(duration.Start, duration.Length));
                    if (match.Success)
                    {
                        ret.Add(new Token(startToken, duration.End));
                    }
                }
            }

            return ret;
        }

        private bool MatchRegexInPrefix(string beforeStr, Match match) {
            var result =  match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length));
            return result;
        }

    }

    
}
