using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;
using System;
using System.Linq;

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
            var simpleCasesResults = Token.MergeAllTokens(tokens, text, ExtractorName);
            tokens.AddRange(MergeTwoTimePoints(text, reference));
            tokens.AddRange(MatchDuration(text, reference));
            tokens.AddRange(SingleTimePointWithPatterns(text, reference));
            tokens.AddRange(MatchComplexCases(text, simpleCasesResults, reference));
            tokens.AddRange(MatchYearPeriod(text, reference));
            tokens.AddRange(MatchOrdinalNumberWithCenturySuffix(text, reference));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // Cases like "21st century"
        private List<Token> MatchOrdinalNumberWithCenturySuffix(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var ers = this.config.OrdinalExtractor.Extract(text);

            foreach (var er in ers)
            {
                if (er.Start + er.Length >= text.Length)
                {
                    continue;
                }

                var afterString = text.Substring((er.Start + er.Length).Value);
                var trimmedAfterString = afterString.TrimStart();
                var whiteSpacesCount = afterString.Length - trimmedAfterString.Length;
                var afterStringOffset = (er.Start + er.Length).Value + whiteSpacesCount;

                var match = this.config.CenturySuffixRegex.Match(trimmedAfterString);
                
                if (match.Success)
                {
                    ret.Add(new Token(er.Start.Value, afterStringOffset + match.Index + match.Length));
                }
            }

            return ret;
        }

        private List<Token> MatchYearPeriod(string text, DateObject referece)
        {
            var ret = new List<Token>();
            var metadata = new Metadata()
            {
                PossiblyIncludePeriodEnd = true
            };

            var matches = this.config.YearPeriodRegex.Matches(text);
            foreach (Match match in matches)
            {
                var matchYear = this.config.YearRegex.Match(match.Value);

                // Single year cases like "1998"
                if (matchYear.Success && matchYear.Length == match.Value.Length)
                {
                    var year = ((BaseDateExtractor)this.config.DatePointExtractor).GetYearFromText(matchYear);
                    if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum))
                    {
                        continue;
                    }
                    else
                    {
                        // Possibly include period end only apply for cases like "2014-2018", which are not single year cases
                        metadata.PossiblyIncludePeriodEnd = false;
                    }
                }

                ret.Add(new Token(match.Index, match.Index + match.Length, metadata));
            }

            return ret;
        }

        private List<Token> MatchSimpleCases(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.SimpleCasesRegexes)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    var matchYear = this.config.YearRegex.Match(match.Value);
                    if (matchYear.Success && matchYear.Length == match.Value.Length)
                    {
                        var year = ((BaseDateExtractor)this.config.DatePointExtractor).GetYearFromText(matchYear);
                        if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum))
                        {
                            continue;
                        }
                    }
                    ret.Add(new Token(match.Index, match.Index + match.Length));
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

        private List<Token> MergeTwoTimePoints(string text, DateObject reference)
        {
            var er = this.config.DatePointExtractor.Extract(text, reference);
            
            return MergeMultipleExtractions(text, er);
        }

        private List<Token> MergeMultipleExtractions(string text, List<ExtractResult> extractionResults)
        {
            var ret = new List<Token>();
            var metadata = new Metadata()
            {
                PossiblyIncludePeriodEnd = true
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

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLowerInvariant();
                var match = this.config.TillRegex.Match(middleStr);
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    var periodBegin = extractionResults[idx].Start ?? 0;
                    var periodEnd = (extractionResults[idx + 1].Start ?? 0) + (extractionResults[idx + 1].Length ?? 0);

                    // handle "from/between" together with till words (till/until/through...)
                    var beforeStr = text.Substring(0, periodBegin).Trim().ToLowerInvariant();
                    if (this.config.GetFromTokenIndex(beforeStr, out int fromIndex)
                        || this.config.GetBetweenTokenIndex(beforeStr, out fromIndex))
                    {
                        periodBegin = fromIndex;
                    }

                    ret.Add(new Token(periodBegin, periodEnd, metadata));

                    // merge two tokens here, increase the index by two
                    idx += 2;
                    continue;
                }

                if (this.config.HasConnectorToken(middleStr))
                {
                    var periodBegin = extractionResults[idx].Start ?? 0;
                    var periodEnd = (extractionResults[idx + 1].Start ?? 0) + (extractionResults[idx + 1].Length ?? 0);

                    // handle "between...and..." case
                    var beforeStr = text.Substring(0, periodBegin).Trim().ToLowerInvariant();
                    if (this.config.GetBetweenTokenIndex(beforeStr, out int beforeIndex))
                    {
                        periodBegin = beforeIndex;
                        ret.Add(new Token(periodBegin, periodEnd, metadata));

                        // merge two tokens here, increase the index by two
                        idx += 2;
                        continue;
                    }
                }
                idx++;
            }

            return ret;
        }

        // 1. Extract the month of date, week of date to a date range
        // 2. Extract cases like within two weeks from/before today/tomorrow/yesterday
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
                if (extractionResult.Start != null && extractionResult.Length != null)
                {
                    string beforeString = text.Substring(0, (int)extractionResult.Start);
                    ret.AddRange(GetTokenForRegexMatching(beforeString, config.WeekOfRegex, extractionResult));
                    ret.AddRange(GetTokenForRegexMatching(beforeString, config.MonthOfRegex, extractionResult));

                    // Cases like "3 days from today", "2 weeks before yesterday", "3 months after tomorrow"
                    if (IsRelativeDurationDate(extractionResult))
                    {
                        ret.AddRange(GetTokenForRegexMatching(beforeString, config.LessThanRegex, extractionResult));
                        ret.AddRange(GetTokenForRegexMatching(beforeString, config.MoreThanRegex, extractionResult));

                        // For "within" case, only duration with relative to "today" or "now" makes sense
                        // Cases like "within 3 days from yesterday/tomorrow" does not make any sense
                        if (IsDateRelativeToNowOrToday(extractionResult))
                        {
                            var match = this.config.WithinNextPrefixRegex.Match(beforeString);
                            if (match.Success)
                            {
                                var isNext = !string.IsNullOrEmpty(match.Groups[Constants.NextGroupName].Value);

                                // For "within" case
                                // Cases like "within the next 5 days before today" is not acceptable
                                if (!(isNext && IsAgoRelativeDurationDate(extractionResult)))
                                {
                                    ret.AddRange(GetTokenForRegexMatching(beforeString, config.WithinNextPrefixRegex, extractionResult));
                                }
                            }
                        }
                    }
                }
            }

            return ret;
        }

        // Cases like "3 days from today", "2 weeks before yesterday", "3 months after tomorrow"
        private bool IsRelativeDurationDate(ExtractResult er)
        {
            var isAgo = this.config.AgoRegex.Match(er.Text).Success;
            var isLater = this.config.LaterRegex.Match(er.Text).Success;

            return isAgo || isLater;
        }

        private bool IsAgoRelativeDurationDate(ExtractResult er)
        {
            return this.config.AgoRegex.Match(er.Text).Success;
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
                var afterStr = text.Substring(duration.Start + duration.Length).ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(beforeStr) && string.IsNullOrWhiteSpace(afterStr))
                {
                    continue;
                }

                // Match prefix
                var match = this.config.PastRegex.Match(beforeStr);
                if (MatchPrefixRegexInSegment(beforeStr, match))
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }

                // within "Days/Weeks/Months/Years" should be handled as dateRange here
                // if duration contains "Seconds/Minutes/Hours", it should be treated as datetimeRange
                match = Regex.Match(beforeStr, config.WithinNextPrefixRegex.ToString(),
                    RegexOptions.RightToLeft | config.WithinNextPrefixRegex.Options);
                if (MatchPrefixRegexInSegment(beforeStr, match))
                {
                    var startToken = match.Index;
                    var matchDate = config.DateUnitRegex.Match(text.Substring(duration.Start, duration.Length));
                    var matchTime = config.TimeUnitRegex.Match(text.Substring(duration.Start, duration.Length));

                    if (matchDate.Success && !matchTime.Success)
                    {
                        ret.Add(new Token(startToken, duration.End));
                    }
                }

                // For cases like "next five days"
                match = Regex.Match(beforeStr, config.FutureRegex.ToString(),
                    RegexOptions.RightToLeft | config.FutureRegex.Options);
                if (MatchPrefixRegexInSegment(beforeStr, match))
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }

                // Match suffix
                match = this.config.PastRegex.Match(afterStr);
                if (MatchSuffixRegexInSegment(afterStr, match))
                {
                    ret.Add(new Token(duration.Start, duration.End + match.Index + match.Length));
                    continue;
                }

                match = this.config.FutureRegex.Match(afterStr);
                if (MatchSuffixRegexInSegment(afterStr, match))
                {
                    ret.Add(new Token(duration.Start, duration.End + match.Index + match.Length));
                    continue;
                }

                match = this.config.FutureSuffixRegex.Match(afterStr);
                if (MatchSuffixRegexInSegment(afterStr, match))
                {
                    ret.Add(new Token(duration.Start, duration.End + match.Index + match.Length));
                    continue;
                }
            }

            return ret;
        }

        private bool MatchSuffixRegexInSegment(string afterStr, Match match)
        {
            var result = match.Success && string.IsNullOrWhiteSpace(afterStr.Substring(0, match.Index));
            return result;
        }

        private bool MatchPrefixRegexInSegment(string beforeStr, Match match)
        {
            var result = match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length));
            return result;
        }

        private bool IsDateRelativeToNowOrToday(ExtractResult er)
        {
            foreach (var flagWord in config.DurationDateRestrictions)
            {
                if (er.Text.Contains(flagWord))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
