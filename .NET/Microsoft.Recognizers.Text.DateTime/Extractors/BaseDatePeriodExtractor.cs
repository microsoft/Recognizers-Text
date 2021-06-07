using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.InternalCache;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDatePeriodExtractor : IDateTimeExtractor
    {
        private const string ExtractorName = Constants.SYS_DATETIME_DATEPERIOD;

        private static readonly ResultsCache<ExtractResult> ResultsCache = new ResultsCache<ExtractResult>();

        private readonly IDatePeriodExtractorConfiguration config;

        private readonly string keyPrefix;

        public BaseDatePeriodExtractor(IDatePeriodExtractorConfiguration config)
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

        public List<Token> MatchDuration(string text, DateObject reference)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var durationExtractions = config.DurationExtractor.Extract(text, reference);

            foreach (var durationExtraction in durationExtractions)
            {
                var dateUnitMatch = config.DateUnitRegex.Match(durationExtraction.Text);
                if (!dateUnitMatch.Success)
                {
                    continue;
                }

                var isPlurarUnit = dateUnitMatch.Groups[Constants.PluralUnit].Success;
                var duration = new Token(durationExtraction.Start ?? 0, durationExtraction.Start + durationExtraction.Length ?? 0);
                var beforeStr = text.Substring(0, duration.Start);
                var afterStr = text.Substring(duration.Start + duration.Length);

                if (string.IsNullOrWhiteSpace(beforeStr) && string.IsNullOrWhiteSpace(afterStr))
                {
                    continue;
                }

                // within "Days/Weeks/Months/Years" should be handled as dateRange here
                // if duration contains "Seconds/Minutes/Hours", it should be treated as datetimeRange
                Token matchToken = MatchWithinNextAffixRegex(text, duration, inPrefix: true);
                if (matchToken.Start >= 0)
                {
                    ret.Add(matchToken);
                    continue;
                }

                // check also afterStr
                if (this.config.CheckBothBeforeAfter)
                {
                    matchToken = MatchWithinNextAffixRegex(text, duration, inPrefix: false);
                    if (matchToken.Start >= 0)
                    {
                        ret.Add(matchToken);
                        continue;
                    }
                }

                // Match prefix
                var match = this.config.PreviousPrefixRegex.MatchEnd(beforeStr, trim: true);

                var index = -1;

                if (match.Success)
                {
                    index = match.Index;
                }

                if (index < 0)
                {
                    // For cases like "next five days"
                    match = config.FutureRegex.MatchEnd(beforeStr, trim: true);

                    if (match.Success)
                    {
                        index = match.Index;
                    }
                }

                if (index >= 0)
                {
                    var prefix = beforeStr.Substring(0, index).Trim();
                    var durationText = text.Substring(duration.Start, duration.Length);

                    var numbersInPrefix = config.CardinalExtractor.Extract(prefix);
                    var numbersInDuration = config.CardinalExtractor.Extract(durationText);

                    // Cases like "2 upcoming days", should be supported here
                    // Cases like "2 upcoming 3 days" is invalid, only extract "upcoming 3 days" by default
                    if (numbersInPrefix.Any() && !numbersInDuration.Any() && isPlurarUnit)
                    {
                        var lastNumber = numbersInPrefix.OrderBy(t => t.Start + t.Length).Last();

                        // Prefix should ends with the last number
                        if (lastNumber.Start + lastNumber.Length == prefix.Length)
                        {
                            ret.Add(new Token(lastNumber.Start.Value, duration.End));
                        }
                    }
                    else
                    {
                        ret.Add(new Token(index, duration.End));
                    }

                    continue;
                }

                // Match suffix
                match = this.config.PreviousPrefixRegex.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(duration.Start, duration.End + match.Index + match.Length));
                    continue;
                }

                match = this.config.FutureSuffixRegex.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(duration.Start, duration.End + match.Index + match.Length));
                    continue;
                }
            }

            return ret;
        }

        private static List<Token> GetTokenForRegexMatching(string text, Regex regex, ExtractResult er, bool inPrefix)
        {
            var ret = new List<Token>();

            var match = regex.Match(text);
            bool isMatchAtEdge = inPrefix ?
                                 text.Trim().EndsWith(match.Value.Trim(), StringComparison.Ordinal) :
                                 text.Trim().StartsWith(match.Value.Trim(), StringComparison.Ordinal);

            if (match.Success && isMatchAtEdge)
            {
                var startIndex = inPrefix ? text.LastIndexOf(match.Value, StringComparison.Ordinal) : (int)er.Start;
                var endIndex = (int)er.Start + (int)er.Length;
                endIndex += inPrefix ? 0 : match.Index + match.Length;

                ret.Add(new Token(startIndex, endIndex));
            }

            return ret;
        }

        // Check whether the match is an infix of source
        private static bool InfixBoundaryCheck(Match match, string source)
        {
            bool isMatchInfixOfSource = false;
            if (match.Index > 0 && match.Index + match.Length < source.Length)
            {
                if (source.AsSpan(match.Index, match.Length).Equals(match.Value.AsSpan(), StringComparison.InvariantCulture))
                {
                    isMatchInfixOfSource = true;
                }
            }

            return isMatchInfixOfSource;
        }

        private static bool IsDigitChar(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        private static bool HasDashPrefix(Match match, string source, out int dashPrefixIndex)
        {
            bool hasDashPrefix = false;
            dashPrefixIndex = -1;

            for (var i = match.Index - 1; i >= 0; i--)
            {
                if (source[i] != ' ' && source[i] != '-')
                {
                    break;
                }
                else if (source[i] == '-')
                {
                    hasDashPrefix = true;
                    dashPrefixIndex = i;
                    break;
                }
            }

            return hasDashPrefix;
        }

        private static bool HasDashSuffix(Match match, string source, out int dashSuffixIndex)
        {
            bool hasDashSuffix = false;
            dashSuffixIndex = -1;

            for (var i = match.Index + match.Length; i < source.Length; i++)
            {
                if (source[i] != ' ' && source[i] != '-')
                {
                    break;
                }
                else if (source[i] == '-')
                {
                    hasDashSuffix = true;
                    dashSuffixIndex = i;
                    break;
                }
            }

            return hasDashSuffix;
        }

        private static bool HasDigitNumberBeforeDash(string source, int dashPrefixIndex, out int numberStartIndex)
        {
            bool hasDigitNumberBeforeDash = false;
            numberStartIndex = -1;

            for (var i = dashPrefixIndex - 1; i >= 0; i--)
            {
                if (source[i] == ' ')
                {
                    continue;
                }

                if (IsDigitChar(source[i]))
                {
                    hasDigitNumberBeforeDash = true;
                }

                if (!IsDigitChar(source[i]))
                {
                    if (hasDigitNumberBeforeDash)
                    {
                        numberStartIndex = i + 1;
                    }

                    break;
                }
            }

            if (hasDigitNumberBeforeDash && numberStartIndex == -1)
            {
                numberStartIndex = 0;
            }

            return hasDigitNumberBeforeDash;
        }

        private static bool HasDigitNumberAfterDash(string source, int dashSuffixIndex, out int numberEndIndex)
        {
            bool hasDigitNumberAfterDash = false;
            numberEndIndex = -1;

            for (var i = dashSuffixIndex + 1; i < source.Length; i++)
            {
                if (source[i] == ' ')
                {
                    continue;
                }

                if (IsDigitChar(source[i]))
                {
                    hasDigitNumberAfterDash = true;
                }

                if (!IsDigitChar(source[i]))
                {
                    if (hasDigitNumberAfterDash)
                    {
                        numberEndIndex = i;
                    }

                    break;
                }
            }

            if (hasDigitNumberAfterDash && numberEndIndex == -1)
            {
                numberEndIndex = source.Length;
            }

            return hasDigitNumberAfterDash;
        }

        private List<ExtractResult> ExtractImpl(string text, DateObject reference)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MatchSimpleCases(text));

            var simpleCasesResults = Token.MergeAllTokens(tokens, text, ExtractorName);
            var ordinalExtractions = config.OrdinalExtractor.Extract(text);

            tokens.AddRange(MergeTwoTimePoints(text, reference));
            tokens.AddRange(MatchDuration(text, reference));
            tokens.AddRange(SingleTimePointWithPatterns(text, new List<ExtractResult>(ordinalExtractions), reference));
            tokens.AddRange(MatchComplexCases(text, simpleCasesResults, reference));
            tokens.AddRange(MatchYearPeriod(text, reference));
            tokens.AddRange(MatchOrdinalNumberWithCenturySuffix(text, new List<ExtractResult>(ordinalExtractions)));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // Cases like "21st century"
        private List<Token> MatchOrdinalNumberWithCenturySuffix(string text, List<ExtractResult> ordinalExtractions)
        {
            var ret = new List<Token>();

            foreach (var er in ordinalExtractions)
            {
                if (er.Start + er.Length >= text.Length)
                {
                    continue;
                }

                var afterString = text.AsSpan((er.Start + er.Length).Value);
                var trimmedAfterString = afterString.TrimStart();
                var whiteSpacesCount = afterString.Length - trimmedAfterString.Length;
                var afterStringOffset = (er.Start + er.Length).Value + whiteSpacesCount;

                var match = this.config.CenturySuffixRegex.Match(trimmedAfterString.ToString());

                if (match.Success)
                {
                    ret.Add(new Token(er.Start.Value, afterStringOffset + match.Index + match.Length));
                }
            }

            return ret;
        }

        private List<Token> MatchYearPeriod(string text, DateObject referenceDate)
        {
            var ret = new List<Token>();
            var metadata = new Metadata
            {
                PossiblyIncludePeriodEnd = true,
            };

            var matches = this.config.YearPeriodRegex.Matches(text);
            foreach (Match match in matches)
            {
                var matchYear = this.config.YearRegex.Match(match.Value);

                // Single year cases like "1998"
                if (matchYear.Success && matchYear.Length == match.Value.Length)
                {
                    var year = config.DatePointExtractor.GetYearFromText(matchYear);
                    if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum))
                    {
                        continue;
                    }

                    // Possibly include period end only apply for cases like "2014-2018", which are not single year cases
                    metadata.PossiblyIncludePeriodEnd = false;
                }
                else
                {
                    var yearMatches = this.config.YearRegex.Matches(match.Value);
                    var allDigitYear = true;
                    var isValidYear = true;

                    foreach (Match yearMatch in yearMatches)
                    {
                        var year = config.DatePointExtractor.GetYearFromText(yearMatch);
                        if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum))
                        {
                            isValidYear = false;
                            break;
                        }
                        else if (yearMatch.Length != Constants.FourDigitsYearLength)
                        {
                            allDigitYear = false;
                        }
                    }

                    if (!isValidYear)
                    {
                        continue;
                    }

                    // Cases like "2010-2015"
                    if (allDigitYear)
                    {
                        // Filter out cases like "82-2010-2015" or "2010-2015-82" where "2010-2015" should not be extracted as a DateRange
                        if (HasInvalidDashContext(match, text))
                        {
                            continue;
                        }
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
                        var year = config.DatePointExtractor.GetYearFromText(matchYear);

                        if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum))
                        {
                            continue;
                        }
                    }

                    if (match.Length == Constants.FourDigitsYearLength && this.config.YearRegex.IsMatch(match.Value))
                    {
                        // handle single year which is surrounded by '-' at both sides, e.g., a single year falls in a GUID
                        if (InfixBoundaryCheck(match, text))
                        {
                            var substr = text.Substring(match.Index - 1, 6);

                            if (this.config.IllegalYearRegex.IsMatch(substr))
                            {
                                continue;
                            }
                        }

                        // filter out cases like "82-2010", "2010-82" where "2010" should not be extracted as DateRange
                        if (HasInvalidDashContext(match, text))
                        {
                            continue;
                        }
                    }

                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            return ret;
        }

        // This method is to detect the invalid dash context
        // Some match with invalid dash context might be false positives
        // For example, it can be part of the phone number like "Tel: 138-2010-2015"
        private bool HasInvalidDashContext(Match match, string text)
        {
            var hasInvalidDashContext = false;

            // Filter out cases like "82-2100" where "2100" should not be extracted as a DateRange
            // Filter out cases like "82-2010-2015" where "2010-2015" should not be extracted as a DateRange
            if (HasDashPrefix(match, text, out int dashPrefixIndex))
            {
                if (HasDigitNumberBeforeDash(text, dashPrefixIndex, out int numberStartIndex))
                {
                    var digitNumberStr = text.Substring(numberStartIndex, match.Index - 1 - numberStartIndex);

                    if (!this.config.MonthNumRegex.IsExactMatch(digitNumberStr, trim: true))
                    {
                        hasInvalidDashContext = true;
                    }
                }
            }

            // Filter out cases like "2100-82" where "2100" should not be extracted as a DateRange
            // Filter out cases like "2010-2015-82" where "2010-2015" should not be extracted as a DateRange
            if (HasDashSuffix(match, text, out int dashSuffixIndex))
            {
                if (HasDigitNumberAfterDash(text, dashSuffixIndex, out int numberEndIndex))
                {
                    var numberStartIndex = match.Index + match.Length + 1;
                    var digitNumberStr = text.Substring(numberStartIndex, numberEndIndex - numberStartIndex);

                    if (!this.config.MonthNumRegex.IsExactMatch(digitNumberStr, trim: true))
                    {
                        hasInvalidDashContext = true;
                    }
                }
            }

            return hasInvalidDashContext;
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

            // Handle "now"
            er = MatchNow(text, er);

            return MergeMultipleExtractions(text, er);
        }

        private List<Token> MergeTwoTimePoints(string text, DateObject reference)
        {
            var er = this.config.DatePointExtractor.Extract(text, reference);

            // Handle "now"
            er = MatchNow(text, er);

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
                    var beforeStr = text.Substring(0, periodBegin).Trim();

                    if (this.config.GetFromTokenIndex(beforeStr, out int fromIndex) ||
                        this.config.GetBetweenTokenIndex(beforeStr, out fromIndex))
                    {
                        periodBegin = fromIndex;
                    }

                    // handle "between...and..." case when "between" follows the datepoints
                    if (this.config.CheckBothBeforeAfter)
                    {
                        var afterStr = text.Substring(periodEnd, text.Length - periodEnd);
                        if (this.config.GetBetweenTokenIndex(afterStr, out int afterIndex))
                        {
                            periodEnd += afterIndex;
                            ret.Add(new Token(periodBegin, periodEnd, metadata));

                            // merge two tokens here, increase the index by two
                            idx += 2;
                            continue;
                        }
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
                    var beforeStr = text.Substring(0, periodBegin).Trim();
                    if (this.config.GetBetweenTokenIndex(beforeStr, out int beforeIndex))
                    {
                        periodBegin = beforeIndex;
                        ret.Add(new Token(periodBegin, periodEnd, metadata));

                        // merge two tokens here, increase the index by two
                        idx += 2;
                        continue;
                    }

                    // handle "between...and..." case when "between" follows the datepoints
                    if (this.config.CheckBothBeforeAfter)
                    {
                        var afterStr = text.Substring(periodEnd, text.Length - periodEnd);
                        if (this.config.GetBetweenTokenIndex(afterStr, out int afterIndex))
                        {
                            periodEnd += afterIndex;
                            ret.Add(new Token(periodBegin, periodEnd, metadata));

                            // merge two tokens here, increase the index by two
                            idx += 2;
                            continue;
                        }
                    }
                }

                idx++;
            }

            return ret;
        }

        // 1. Extract the month of date, week of date to a date range
        // 2. Extract cases like within two weeks from/before today/tomorrow/yesterday
        private List<Token> SingleTimePointWithPatterns(string text, List<ExtractResult> ordinalExtractions, DateObject reference)
        {
            var ret = new List<Token>();
            var datePoints = this.config.DatePointExtractor.Extract(text, reference);

            // For cases like "week of the 18th"
            datePoints.AddRange(ordinalExtractions.Where(o => !datePoints.Any(er => er.IsOverlap(o))));

            if (datePoints.Count < 1)
            {
                return ret;
            }

            foreach (var extractionResult in datePoints)
            {
                if (extractionResult.Start != null && extractionResult.Length != null)
                {
                    var beforeString = text.Substring(0, (int)extractionResult.Start);
                    var afterString = text.Substring((int)extractionResult.Start + (int)extractionResult.Length,
                                                     text.Length - (int)extractionResult.Start - (int)extractionResult.Length);

                    ret.AddRange(GetTokenForRegexMatching(beforeString, config.WeekOfRegex, extractionResult, inPrefix: true));
                    ret.AddRange(GetTokenForRegexMatching(beforeString, config.MonthOfRegex, extractionResult, inPrefix: true));

                    // Check also afterString
                    if (this.config.CheckBothBeforeAfter)
                    {
                        ret.AddRange(GetTokenForRegexMatching(afterString, config.WeekOfRegex, extractionResult, inPrefix: false));
                        ret.AddRange(GetTokenForRegexMatching(afterString, config.MonthOfRegex, extractionResult, inPrefix: false));
                    }

                    // Cases like "3 days from today", "2 weeks before yesterday", "3 months after tomorrow"
                    if (IsRelativeDurationDate(extractionResult))
                    {
                        ret.AddRange(GetTokenForRegexMatching(beforeString, config.LessThanRegex, extractionResult, inPrefix: true));
                        ret.AddRange(GetTokenForRegexMatching(beforeString, config.MoreThanRegex, extractionResult, inPrefix: true));

                        // Check also afterString
                        if (this.config.CheckBothBeforeAfter)
                        {
                            ret.AddRange(GetTokenForRegexMatching(afterString, config.LessThanRegex, extractionResult, inPrefix: false));
                            ret.AddRange(GetTokenForRegexMatching(afterString, config.MoreThanRegex, extractionResult, inPrefix: false));
                        }

                        // For "within" case, only duration with relative to "today" or "now" makes sense
                        // Cases like "within 3 days from yesterday/tomorrow" does not make any sense
                        if (IsDateRelativeToNowOrToday(extractionResult))
                        {
                            var tokens = ExtractWithinNextPrefix(beforeString, extractionResult, inPrefix: true);
                            ret.AddRange(tokens);

                            // check also afterString
                            if (this.config.CheckBothBeforeAfter && tokens.Count == 0)
                            {
                                tokens = ExtractWithinNextPrefix(afterString, extractionResult, inPrefix: false);
                                ret.AddRange(tokens);
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

        // Matches "within (the next)?" part (in beforeStr or afterStr) in "within Days/Weeks/Months/Years"
        private Token MatchWithinNextAffixRegex(string text, Token duration, bool inPrefix)
        {
            int startToken = -1;
            int endToken = -1;

            var beforeStr = text.Substring(0, duration.Start);
            var afterStr = text.Substring(duration.Start + duration.Length);

            var match = inPrefix ?
                        config.WithinNextPrefixRegex.MatchEnd(beforeStr, trim: true) :
                        config.WithinNextPrefixRegex.MatchBegin(afterStr, trim: true);

            if (match.Success)
            {
                var durationStr = text.Substring(duration.Start, duration.Length);

                var matchDate = config.DateUnitRegex.Match(durationStr);
                var matchTime = config.TimeUnitRegex.Match(durationStr);

                if (matchDate.Success && !matchTime.Success)
                {
                    startToken = inPrefix ? match.Index : duration.Start;
                    endToken = inPrefix ? duration.End : duration.End + match.Index + match.Length;

                    if (!inPrefix)
                    {
                        // Check prefix for "next"
                        match = config.FutureRegex.MatchEnd(beforeStr, trim: true);
                        if (match.Success)
                        {
                            startToken = match.Index;
                        }
                    }
                }
            }

            return new Token(startToken, endToken);
        }

        private List<Token> ExtractWithinNextPrefix(string subStr, ExtractResult extractionResult, bool inPrefix)
        {
            var tokens = new List<Token>();

            var match = this.config.WithinNextPrefixRegex.Match(subStr);

            if (match.Success)
            {
                var isNext = !string.IsNullOrEmpty(match.Groups[Constants.NextGroupName].Value);

                // For "within" case
                // Cases like "within the next 5 days before today" is not acceptable
                if (!(isNext && IsAgoRelativeDurationDate(extractionResult)))
                {
                    tokens = GetTokenForRegexMatching(subStr, config.WithinNextPrefixRegex, extractionResult, inPrefix);
                }
            }

            return tokens;
        }

        // Handle cases with "now"
        private List<ExtractResult> MatchNow(string text, List<ExtractResult> er)
        {
            var matches = this.config.NowRegex.Matches(text);
            if (matches.Count != 0)
            {
                foreach (Match match in matches)
                {
                    var nowEr = new ExtractResult
                    {
                        Start = match.Index,
                        Length = match.Length,
                        Text = text.Substring(match.Index, match.Length),
                    };

                    er.Add(nowEr);

                }

                er = er.OrderBy(o => o.Start).ToList();
            }

            return er;
        }
    }
}
