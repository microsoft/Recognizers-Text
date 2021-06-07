using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimePeriodExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIMEPERIOD;

        private readonly IDateTimePeriodExtractorConfiguration config;

        public BaseDateTimePeriodExtractor(IDateTimePeriodExtractorConfiguration config)
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

            // Date and time Extractions should be extracted from the text only once, and shared in the methods below, passed by value
            var dateErs = config.SingleDateExtractor.Extract(text, reference);
            var timeErs = config.SingleTimeExtractor.Extract(text, reference);

            tokens.AddRange(MatchSimpleCases(text, reference));
            tokens.AddRange(MergeTwoTimePoints(text, reference, new List<ExtractResult>(dateErs), new List<ExtractResult>(timeErs)));
            tokens.AddRange(MatchDuration(text, reference));
            tokens.AddRange(MatchTimeOfDay(text, reference, new List<ExtractResult>(dateErs)));
            tokens.AddRange(MatchRelativeUnit(text));
            tokens.AddRange(MatchDateWithPeriodPrefix(text, reference, new List<ExtractResult>(dateErs)));
            tokens.AddRange(MergeDateWithTimePeriodSuffix(text, new List<ExtractResult>(dateErs), new List<ExtractResult>(timeErs)));

            var ers = Token.MergeAllTokens(tokens, text, ExtractorName);

            if ((this.config.Options & DateTimeOptions.EnablePreview) != 0)
            {
                ers = TimeZoneUtility.MergeTimeZones(ers, config.TimeZoneExtractor.Extract(text, reference), text);
            }

            return ers;
        }

        private static bool MatchPrefixRegexInSegment(string text, Match match, bool inPrefix)
        {
            string subStr = inPrefix ? text.Substring(match.Index + match.Length) : text.Substring(0, match.Index);
            var result = match.Success && string.IsNullOrWhiteSpace(subStr);
            return result;
        }

        // Cases like "today after 2:00pm", "1/1/2015 before 2:00 in the afternoon"
        private IEnumerable<Token> MergeDateWithTimePeriodSuffix(string text, List<ExtractResult> dateErs, List<ExtractResult> timeErs)
        {
            var ret = new List<Token>();

            if (!dateErs.Any())
            {
                return ret;
            }

            if (!timeErs.Any())
            {
                return ret;
            }

            var ers = dateErs;
            ers.AddRange(timeErs);

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
                        i = j + 1;
                        continue;
                    }

                    var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();

                    int length = GetValidConnectorIndexForDateAndTimePeriod(middleStr, inPrefix: true);
                    if (length != Constants.INVALID_CONNECTOR_CODE)
                    {
                        var begin = ers[i].Start ?? 0;
                        var end = (ers[j].Start ?? 0) + (ers[j].Length ?? 0);

                        ret.Add(new Token(begin, end));
                    }
                    else if (this.config.CheckBothBeforeAfter)
                    {
                        // Check also afterStr
                        var afterStart = ers[j].Start + ers[j].Length ?? 0;
                        var afterStr = text.Substring(afterStart);

                        length = GetValidConnectorIndexForDateAndTimePeriod(afterStr, inPrefix: false);
                        if (length != Constants.INVALID_CONNECTOR_CODE && this.config.PrepositionRegex.IsExactMatch(middleStr, trim: true))
                        {
                            var begin = ers[i].Start ?? 0;
                            var end = (ers[j].Start ?? 0) + (ers[j].Length ?? 0) + length;
                            ret.Add(new Token(begin, end));
                        }
                    }

                    i = j + 1;
                    continue;
                }

                i = j;
            }

            // Handle "in the afternoon" at the end of entity
            for (var idx = 0; idx < ret.Count; idx++)
            {
                var afterStr = text.Substring(ret[idx].End);
                var match = this.config.SuffixRegex.Match(afterStr);
                if (match.Success)
                {
                    ret[idx] = new Token(ret[idx].Start, ret[idx].End + match.Length);
                }
            }

            return ret;
        }

        // Cases like "today after 2:00pm", "1/1/2015 before 2:00 in the afternoon"
        // Valid connector in English for Before include: "before", "no later than", "in advance of", "prior to", "earlier than", "sooner than", "by", "till", "until"...
        // Valid connector in English for After include: "after", "later than"
        private int GetValidConnectorIndexForDateAndTimePeriod(string text, bool inPrefix)
        {
            int length = Constants.INVALID_CONNECTOR_CODE;
            var beforeAfterRegexes = new List<Regex>
            {
                this.config.BeforeRegex,
                this.config.AfterRegex,
            };

            foreach (var regex in beforeAfterRegexes)
            {
                var match = inPrefix ? regex.MatchExact(text, trim: true) : regex.MatchBegin(text, trim: true);
                if (match.Success)
                {
                    length = match.Length;
                    return length;
                }
            }

            return length;
        }

        // For cases like "Early in the day Wednesday"
        private IEnumerable<Token> MatchDateWithPeriodPrefix(string text, DateObject reference, List<ExtractResult> dateErs)
        {
            var ret = new List<Token>();

            foreach (var dateEr in dateErs)
            {
                var dateStrEnd = (int)(dateEr.Start + dateEr.Length);
                var beforeStr = text.Substring(0, (int)dateEr.Start).TrimEnd();
                var match = this.config.PrefixDayRegex.Match(beforeStr);
                if (match.Success)
                {
                    ret.Add(new Token(match.Index, dateStrEnd));
                }
                else if (this.config.CheckBothBeforeAfter)
                {
                    // Check also afterStr
                    var afterStr = text.Substring(dateStrEnd, text.Length - dateStrEnd);
                    var matchAfter = this.config.PrefixDayRegex.MatchBegin(afterStr, trim: true);
                    if (matchAfter.Success)
                    {
                        ret.Add(new Token((int)dateEr.Start, dateStrEnd + matchAfter.Index + matchAfter.Length));
                    }
                }
            }

            return ret;
        }

        private List<Token> MatchSimpleCases(string text, DateObject reference)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.SimpleCasesRegex)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    // Is there a date before it?
                    var hasBeforeDate = false;
                    var beforeStr = text.Substring(0, match.Index);
                    if (!string.IsNullOrEmpty(beforeStr))
                    {
                        var ers = this.config.SingleDateExtractor.Extract(beforeStr, reference);
                        if (ers.Count > 0)
                        {
                            var er = ers.Last();
                            var begin = er.Start ?? 0;

                            var middleStr = beforeStr.Substring(begin + (er.Length ?? 0)).Trim();
                            if (string.IsNullOrEmpty(middleStr) || this.config.PrepositionRegex.IsExactMatch(middleStr, true))
                            {
                                ret.Add(new Token(begin, match.Index + match.Length));
                                hasBeforeDate = true;
                            }
                        }
                    }

                    var followedStr = text.Substring(match.Index + match.Length);
                    if (!string.IsNullOrEmpty(followedStr) && !hasBeforeDate)
                    {
                        // Is it followed by a date?
                        var er = this.config.SingleDateExtractor.Extract(followedStr, reference);
                        if (er.Count > 0)
                        {
                            var begin = er[0].Start ?? 0;
                            var end = (er[0].Start ?? 0) + (er[0].Length ?? 0);
                            var middleStr = followedStr.Substring(0, begin).Trim();
                            if (string.IsNullOrEmpty(middleStr) || this.config.PrepositionRegex.IsExactMatch(middleStr, true))
                            {
                                ret.Add(new Token(match.Index, match.Index + match.Length + end));
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private List<Token> MergeTwoTimePoints(string text, DateObject reference, List<ExtractResult> dateErs, List<ExtractResult> timeErs)
        {
            var ret = new List<Token>();
            var dateTimeErs = this.config.SingleDateTimeExtractor.Extract(text, reference);
            var timePoints = new List<ExtractResult>();

            // Handle the overlap problem
            var j = 0;
            foreach (var er in dateTimeErs)
            {
                timePoints.Add(er);

                while (j < timeErs.Count && timeErs[j].Start + timeErs[j].Length < er.Start)
                {
                    timePoints.Add(timeErs[j]);
                    j++;
                }

                while (j < timeErs.Count && timeErs[j].IsOverlap(er))
                {
                    j++;
                }
            }

            for (; j < timeErs.Count; j++)
            {
                timePoints.Add(timeErs[j]);
            }

            timePoints = timePoints.OrderBy(o => o.Start).ToList();

            // Merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"
            var idx = 0;
            while (idx < timePoints.Count - 1)
            {
                // If both ends are Time. then this is a TimePeriod, not a DateTimePeriod
                if (timePoints[idx].Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal) &&
                    timePoints[idx + 1].Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal))
                {
                    idx++;
                    continue;
                }

                var middleBegin = timePoints[idx].Start + timePoints[idx].Length ?? 0;
                var middleEnd = timePoints[idx + 1].Start ?? 0;

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();

                // Handle "{TimePoint} to {TimePoint}"
                if (config.TillRegex.IsExactMatch(middleStr, trim: true))
                {
                    var periodBegin = timePoints[idx].Start ?? 0;
                    var periodEnd = (timePoints[idx + 1].Start ?? 0) + (timePoints[idx + 1].Length ?? 0);

                    // Handle "from"
                    var beforeStr = text.Substring(0, periodBegin).Trim();

                    if (this.config.GetFromTokenIndex(beforeStr, out int fromIndex) ||
                        this.config.GetBetweenTokenIndex(beforeStr, out fromIndex))
                    {
                        periodBegin = fromIndex;
                    }
                    else if (this.config.CheckBothBeforeAfter)
                    {
                        var afterStr = text.Substring(periodEnd, text.Length - periodEnd);
                        if (this.config.GetBetweenTokenIndex(afterStr, out var afterIndex))
                        {
                            // Handle "between" in afterStr
                            periodEnd += afterIndex;
                        }
                    }

                    ret.Add(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }

                // Handle "between {TimePoint} and {TimePoint}"
                if (this.config.HasConnectorToken(middleStr))
                {
                    var periodBegin = timePoints[idx].Start ?? 0;
                    var periodEnd = (timePoints[idx + 1].Start ?? 0) + (timePoints[idx + 1].Length ?? 0);

                    // Handle "between"
                    var beforeStr = text.Substring(0, periodBegin).Trim();

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

            // Regarding the phrase as-- {Date} {TimePeriod}, like "2015-9-23 1pm to 4"
            // Or {TimePeriod} on {Date}, like "1:30 to 4 on 2015-9-23"
            var timePeriodErs = config.TimePeriodExtractor.Extract(text, reference);

            // Mealtime periods (like "dinnertime") are not currently fully supported in merging.
            foreach (var timePeriod in timePeriodErs)
            {
                if (timePeriod.Metadata == null || !timePeriod.Metadata.IsMealtime)
                {
                    dateErs.Add(timePeriod);
                }
            }

            var points = dateErs.OrderBy(x => x.Start).ToList();

            for (idx = 0; idx < points.Count - 1; idx++)
            {
                if (points[idx].Type == points[idx + 1].Type)
                {
                    continue;
                }

                var midBegin = points[idx].Start + points[idx].Length ?? 0;
                var midEnd = points[idx + 1].Start ?? 0;

                if (midEnd - midBegin > 0)
                {
                    var midStr = text.Substring(midBegin, midEnd - midBegin);
                    bool isMatchTokenBeforeDate = string.IsNullOrWhiteSpace(midStr) ||
                                                  (midStr.TrimStart().StartsWith(config.TokenBeforeDate, StringComparison.Ordinal) &&
                                                   (points[idx + 1].Type == Constants.SYS_DATETIME_DATE || points[idx + 1].Type == Constants.SYS_DATETIME_DATETIME));

                    if (this.config.CheckBothBeforeAfter && !string.IsNullOrWhiteSpace(midStr))
                    {
                        List<string> tokenListBeforeDate = config.TokenBeforeDate.Split('|').ToList();
                        foreach (string token in tokenListBeforeDate.Where(n => !string.IsNullOrEmpty(n)))
                        {
                            if (midStr.Trim().Equals(token, StringComparison.OrdinalIgnoreCase))
                            {
                                isMatchTokenBeforeDate = true;
                                break;
                            }
                        }
                    }

                    if (isMatchTokenBeforeDate)
                    {
                        // Extend date extraction for cases like "Monday evening next week"
                        var extendedStr = points[idx].Text + text.Substring((int)(points[idx + 1].Start + points[idx + 1].Length));
                        var extendedDateEr = config.SingleDateExtractor.Extract(extendedStr, reference).FirstOrDefault();
                        var offset = 0;
                        if (extendedDateEr != null && extendedDateEr.Start == 0 && !this.config.CheckBothBeforeAfter)
                        {
                            offset = (int)(extendedDateEr.Length - points[idx].Length);
                        }

                        ret.Add(new Token(points[idx].Start ?? 0, offset + points[idx + 1].Start + points[idx + 1].Length ?? 0));
                        idx += 2;
                    }
                }
            }

            return ret;
        }

        private List<Token> MatchTimeOfDay(string text, DateObject reference, List<ExtractResult> dateErs)
        {
            var ret = new List<Token>();

            var matches = this.config.SpecificTimeOfDayRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            // Date followed by morning, afternoon or morning, afternoon followed by Date
            if (dateErs.Count == 0 && ret.Count == 0)
            {
                return ret;
            }

            foreach (var er in dateErs)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);

                var match = this.config.PeriodTimeOfDayWithDateRegex.Match(afterStr);

                if (match.Success)
                {
                    // For cases like "Friday afternoon between 1PM and 4 PM" which "Friday afternoon" need to be extracted first
                    if (string.IsNullOrWhiteSpace(afterStr.Substring(0, match.Index)))
                    {
                        var start = er.Start ?? 0;
                        var end = er.Start + er.Length + match.Groups[Constants.TimeOfDayGroupName].Index +
                                  match.Groups[Constants.TimeOfDayGroupName].Length ?? 0;

                        ret.Add(new Token(start, end));
                        continue;
                    }

                    var connectorStr = afterStr.Substring(0, match.Index);

                    // Trim here is set to false as the Regex might catch white spaces before or after the text
                    if (config.MiddlePauseRegex.IsExactMatch(connectorStr, trim: false))
                    {
                        var suffix = afterStr.Substring(match.Index + match.Length).TrimStart();

                        var endingMatch = config.GeneralEndingRegex.Match(suffix);
                        if (endingMatch.Success)
                        {
                            ret.Add(new Token(er.Start ?? 0, er.Start + er.Length + match.Index + match.Length ?? 0));
                        }
                    }
                }

                if (!match.Success)
                {
                    match = this.config.AmDescRegex.Match(afterStr);
                }

                if (!match.Success || !string.IsNullOrWhiteSpace(afterStr.Substring(0, match.Index)))
                {
                    match = this.config.PmDescRegex.Match(afterStr);
                }

                if (match.Success)
                {
                    if (string.IsNullOrWhiteSpace(afterStr.Substring(0, match.Index)))
                    {
                        ret.Add(new Token(er.Start ?? 0, er.Start + er.Length + match.Index + match.Length ?? 0));
                    }
                }

                var prefixStr = text.Substring(0, er.Start ?? 0);

                match = this.config.PeriodTimeOfDayWithDateRegex.Match(prefixStr);
                if (match.Success)
                {
                    if (string.IsNullOrWhiteSpace(prefixStr.Substring(match.Index + match.Length)))
                    {
                        var midStr = text.Substring(match.Index + match.Length, er.Start - match.Index - match.Length ?? 0);
                        if (!string.IsNullOrEmpty(midStr) && string.IsNullOrWhiteSpace(midStr))
                        {
                            ret.Add(new Token(match.Index, er.Start + er.Length ?? 0));
                        }
                    }
                    else
                    {
                        var connectorStr = prefixStr.Substring(match.Index + match.Length);

                        // Trim here is set to false as the Regex might catch white spaces before or after the text
                        if (config.MiddlePauseRegex.IsExactMatch(connectorStr, trim: false))
                        {
                            var suffix = text.Substring(er.Start + er.Length ?? 0).TrimStart(' ');

                            var endingMatch = config.GeneralEndingRegex.Match(suffix);
                            if (endingMatch.Success)
                            {
                                ret.Add(new Token(match.Index, er.Start + er.Length ?? 0));
                            }
                        }
                    }
                }
            }

            // Check whether there are adjacent time period strings, before or after
            foreach (var e in ret.ToArray())
            {
                // Try to extract a time period in before-string
                if (e.Start > 0)
                {
                    var beforeStr = text.Substring(0, e.Start);
                    if (!string.IsNullOrEmpty(beforeStr))
                    {
                        var timeErs = this.config.TimePeriodExtractor.Extract(beforeStr, reference);
                        if (timeErs.Count > 0)
                        {
                            foreach (var tp in timeErs)
                            {
                                var midStr = beforeStr.Substring(tp.Start + tp.Length ?? 0);
                                if (string.IsNullOrWhiteSpace(midStr) && (tp.Metadata == null || !tp.Metadata.IsMealtime))
                                {
                                    ret.Add(new Token(tp.Start ?? 0, tp.Start + tp.Length + midStr.Length + e.Length ?? 0));
                                }
                            }
                        }
                    }
                }

                // Try to extract a time period in after-string
                if (e.Start + e.Length <= text.Length)
                {
                    var afterStr = text.Substring(e.Start + e.Length);
                    if (!string.IsNullOrEmpty(afterStr))
                    {
                        var timeErs = this.config.TimePeriodExtractor.Extract(afterStr, reference);
                        if (timeErs.Count > 0)
                        {
                            foreach (var tp in timeErs)
                            {
                                var midStr = afterStr.Substring(0, tp.Start ?? 0);
                                if (string.IsNullOrWhiteSpace(midStr) && (tp.Metadata == null || !tp.Metadata.IsMealtime))
                                {
                                    ret.Add(new Token(e.Start, e.Start + e.Length + midStr.Length + tp.Length ?? 0));
                                }
                            }
                        }
                    }
                }

                // Try to extract a pure number period in before-string
                if (e.Start > 0)
                {
                    var beforeStr = text.Substring(0, e.Start);
                    if (!string.IsNullOrEmpty(beforeStr))
                    {
                        ret.AddRange(MatchPureNumberCases(beforeStr, e, before: true));
                    }
                }

                // Try to extract a pure number period in after-string
                if (e.End < text.Length)
                {
                    var afterStr = text.Substring(e.End);
                    if (!string.IsNullOrEmpty(afterStr))
                    {
                        ret.AddRange(MatchPureNumberCases(afterStr, e, before: false));
                    }
                }
            }

            return ret;
        }

        // TODO: this can be abstracted with the similar method in BaseDatePeriodExtractor
        private List<Token> MatchDuration(string text, DateObject reference)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var durationExtractions = config.DurationExtractor.Extract(text, reference);

            foreach (var durationExtraction in durationExtractions)
            {
                var timeUnitMatch = config.TimeUnitRegex.Match(durationExtraction.Text);
                if (!timeUnitMatch.Success)
                {
                    continue;
                }

                var isPlurarUnit = timeUnitMatch.Groups[Constants.PluralUnit].Success;
                var duration = new Token(durationExtraction.Start ?? 0, durationExtraction.Start + durationExtraction.Length ?? 0);
                var beforeStr = text.Substring(0, duration.Start);
                var afterStr = text.Substring(duration.Start + duration.Length);

                if (string.IsNullOrWhiteSpace(beforeStr) && string.IsNullOrWhiteSpace(afterStr))
                {
                    continue;
                }

                // within (the) (next) "Seconds/Minutes/Hours" should be handled as datetimeRange here
                // within (the) (next) XX days/months/years + "Seconds/Minutes/Hours" should also be handled as datetimeRange here
                Token token = MatchWithinNextPrefix(beforeStr, text, duration, inPrefix: true);
                if (token.Start >= 0)
                {
                    ret.Add(token);
                    continue;
                }

                // check also afterStr
                if (this.config.CheckBothBeforeAfter)
                {
                    token = MatchWithinNextPrefix(afterStr, text, duration, inPrefix: false);
                    if (token.Start >= 0)
                    {
                        ret.Add(token);
                        continue;
                    }
                }

                var match = this.config.PreviousPrefixRegex.Match(beforeStr);
                var index = -1;
                if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
                {
                    index = match.Index;
                }

                if (index < 0)
                {
                    match = this.config.NextPrefixRegex.Match(beforeStr);
                    if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
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

                var matchDateUnit = this.config.DateUnitRegex.Match(afterStr);
                if (!matchDateUnit.Success)
                {
                    match = this.config.PreviousPrefixRegex.Match(afterStr);
                    if (match.Success && string.IsNullOrWhiteSpace(afterStr.Substring(0, match.Index)))
                    {
                        ret.Add(new Token(duration.Start, duration.Start + duration.Length + match.Index + match.Length));
                        continue;
                    }

                    match = this.config.NextPrefixRegex.Match(afterStr);
                    if (match.Success && string.IsNullOrWhiteSpace(afterStr.Substring(0, match.Index)))
                    {
                        ret.Add(new Token(duration.Start, duration.Start + duration.Length + match.Index + match.Length));
                    }

                    match = this.config.FutureSuffixRegex.Match(afterStr);
                    if (match.Success && string.IsNullOrWhiteSpace(afterStr.Substring(0, match.Index)))
                    {
                        ret.Add(new Token(duration.Start, duration.Start + duration.Length + match.Index + match.Length));
                    }
                }
            }

            return ret;
        }

        private List<Token> MatchRelativeUnit(string text)
        {
            var ret = new List<Token>();

            var matches = config.RelativeTimeUnitRegex.Matches(text);
            if (matches.Count == 0)
            {
                matches = this.config.RestOfDateTimeRegex.Matches(text);
            }

            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        private Token MatchWithinNextPrefix(string subStr, string text, Token duration, bool inPrefix)
        {
            var startOut = -1;
            var endOut = -1;
            bool success = false;
            var match = config.WithinNextPrefixRegex.Match(subStr);
            if (MatchPrefixRegexInSegment(subStr, match, inPrefix))
            {
                var startToken = inPrefix ? match.Index : duration.Start;
                var endToken = duration.End + (inPrefix ? 0 : match.Index + match.Length);
                match = config.TimeUnitRegex.Match(text.Substring(duration.Start, duration.Length));
                success = match.Success;

                if (!inPrefix)
                {
                    // Match prefix for "next"
                    var beforeStr = text.Substring(0, duration.Start);
                    var matchNext = this.config.NextPrefixRegex.Match(beforeStr);
                    success = match.Success || matchNext.Success;
                    if (MatchPrefixRegexInSegment(beforeStr, matchNext, true))
                    {
                        startToken = matchNext.Index;
                    }
                }

                if (success)
                {
                    startOut = startToken;
                    endOut = endToken;
                }
            }

            return new Token(startOut, endOut);
        }

        // The method matches pure number ranges. It is used inside MatchTimeOfDay, so the condition IsNullOrWhiteSpace(midStr) implies
        // that the range must be contiguous to a TimeOfDay expression (e.g. "last night from 7 to 9").
        private List<Token> MatchPureNumberCases(string text, Token tok, bool before)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.SimpleCasesRegex)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    var midStr = before ? text.Substring(match.Index + match.Length) : text.Substring(0, match.Index);
                    if (string.IsNullOrWhiteSpace(midStr))
                    {
                        if (before)
                        {
                            ret.Add(new Token(match.Index, tok.Start + tok.Length));
                        }
                        else
                        {
                            ret.Add(new Token(tok.Start, tok.End + match.Index + match.Length));
                        }
                    }
                }
            }

            return ret;
        }
    }
}
