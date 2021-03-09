using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.InternalCache;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateExtractor : AbstractYearExtractor, IDateExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATE; // "Date";

        private static readonly ResultsCache<ExtractResult> ResultsCache = new ResultsCache<ExtractResult>();

        private readonly string keyPrefix;

        public BaseDateExtractor(IDateExtractorConfiguration config)
            : base(config)
        {

            keyPrefix = string.Intern(Config.Options + "_" + Config.LanguageMarker);

        }

        public static bool IsOverlapWithExistExtractions(Token er, List<Token> existErs)
        {
            foreach (var existEr in existErs)
            {
                if (er.Start < existEr.End && er.End > existEr.Start)
                {
                    return true;
                }
            }

            return false;
        }

        public override List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public override List<ExtractResult> Extract(string text, DateObject reference)
        {

            List<ExtractResult> results;

            if ((this.Config.Options & DateTimeOptions.NoProtoCache) != 0)
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

        // "In 3 days/weeks/months/years" = "3 days/weeks/months/years from now"
        public List<Token> ExtractRelativeDurationDateWithInPrefix(string text, List<ExtractResult> durationEr, DateObject reference)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();

            foreach (var durationExtraction in durationEr)
            {
                var match = Config.DateUnitRegex.Match(durationExtraction.Text);
                if (match.Success)
                {
                    durations.Add(new Token(
                        durationExtraction.Start ?? 0,
                        durationExtraction.Start + durationExtraction.Length ?? 0));
                }
            }

            foreach (var duration in durations)
            {
                var beforeStr = text.Substring(0, duration.Start);
                var afterStr = text.Substring(duration.Start + duration.Length);

                if (string.IsNullOrWhiteSpace(beforeStr) && string.IsNullOrWhiteSpace(afterStr))
                {
                    continue;
                }

                ret.AddRange(ExtractInConnector(text, beforeStr, afterStr, duration, out bool success, inPrefix: true));

                // Check also afterStr
                if (!success && Config.CheckBothBeforeAfter)
                {
                    ret.AddRange(ExtractInConnector(text, afterStr, beforeStr, duration, out success, inPrefix: false));
                }
            }

            return ret;
        }

        private static void StripInequality(ExtractResult er, Regex regex, bool inPrefix)
        {
            if (regex.IsMatch(er.Text))
            {
                var originalLength = er.Text.Length;
                er.Text = regex.Replace(er.Text, string.Empty).Trim();
                if (inPrefix)
                {
                    er.Start += originalLength - er.Text.Length;
                }

                er.Length = er.Text.Length;
                er.Data = string.Empty;
            }
        }

        private static bool IsMultipleDurationDate(ExtractResult er)
        {
            return er.Data != null && er.Data.ToString() == Constants.MultipleDuration_Date;
        }

        private static bool IsMultipleDuration(ExtractResult er)
        {
            return er.Data != null && er.Data.ToString().StartsWith(Constants.MultipleDuration_Prefix, StringComparison.Ordinal);
        }

        // Cases like "more than 3 days", "less than 4 weeks"
        private static bool IsInequalityDuration(ExtractResult er)
        {
            return er.Data != null && (er.Data.ToString() == Constants.MORE_THAN_MOD || er.Data.ToString() == Constants.LESS_THAN_MOD);
        }

        private List<ExtractResult> ExtractImpl(string text, DateObject reference)
        {
            var tokens = new List<Token>();
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(ImplicitDate(text));
            tokens.AddRange(NumberWithMonth(text, reference));
            tokens.AddRange(ExtractRelativeDurationDate(text, tokens, reference));

            var results = Token.MergeAllTokens(tokens, text, ExtractorName);

            return results;
        }

        // match basic patterns in DateRegexList
        private List<Token> BasicRegexMatch(string text)
        {

            var results = new List<Token>();

            foreach (var regex in this.Config.DateRegexList)
            {
                var matches = regex.Matches(text);

                foreach (Match match in matches)
                {
                    // some match might be part of the date range entity, and might be split in a wrong way
                    if (ValidateMatch(match, text))
                    {
                        // Cases that the relative term is before the detected date entity, like "this 5/12", "next friday 5/12"
                        var preText = text.Substring(0, match.Index);
                        var relativeRegex = this.Config.StrictRelativeRegex.MatchEnd(preText, trim: true);

                        if (relativeRegex.Success)
                        {
                            results.Add(new Token(relativeRegex.Index, match.Index + match.Length));
                        }
                        else
                        {
                            results.Add(new Token(match.Index, match.Index + match.Length));
                        }

                    }
                }
            }

            return results;
        }

        // this method is to validate whether the match is part of date range and is a correct split
        // For example: in case "10-1 - 11-7", "10-1 - 11" can be matched by some of the Regexes, but the full text is a date range, so "10-1 - 11" is not a correct split
        private bool ValidateMatch(Match match, string text)
        {
            // If the match doesn't contains "year" part, it will not be ambiguous and it's a valid match
            var isValidMatch = !match.Groups["year"].Success;

            if (!isValidMatch)
            {
                var yearGroup = match.Groups["year"];

                // If the "year" part is not at the end of the match, it's a valid match
                if (yearGroup.Index + yearGroup.Length != match.Index + match.Length)
                {
                    isValidMatch = true;
                }
                else
                {
                    var subText = text.Substring(yearGroup.Index);

                    // If the following text (include the "year" part) doesn't start with a Date entity, it's a valid match
                    if (!StartsWithBasicDate(subText))
                    {
                        isValidMatch = true;
                    }
                    else
                    {
                        // If the following text (include the "year" part) starts with a Date entity, but the following text (doesn't include the "year" part) also starts with a valid Date entity, the current match is still valid
                        // For example, "10-1-2018-10-2-2018". Match "10-1-2018" is valid because though "2018-10-2" a valid match (indicates the first year "2018" might belongs to the second Date entity), but "10-2-2018" is also a valid match.
                        subText = text.Substring(yearGroup.Index + yearGroup.Length).Trim();
                        subText = TrimStartRangeConnectorSymbols(subText);
                        isValidMatch = StartsWithBasicDate(subText);
                    }
                }

                // Expressions with mixed separators are not considered valid dates e.g. "30/4.85" (unless one is a comma "30/4, 2016")
                if (match.Groups["day"].Success && match.Groups["month"].Success)
                {
                    var noDateText = match.Value.Replace(match.Groups["year"].Value, string.Empty)
                        .Replace(match.Groups["month"].Value, string.Empty)
                        .Replace(match.Groups["day"].Value, string.Empty);
                    var separators = new List<char> { '/', '\\', '-', '.' };
                    if (separators.Where(separator => noDateText.Contains(separator)).Count() > 1)
                    {
                        isValidMatch = false;
                    }
                }
            }

            return isValidMatch;
        }

        // TODO: Simplify this method to improve its performance
        private string TrimStartRangeConnectorSymbols(string text)
        {
            var rangeConnectorSymbolMatches = Config.RangeConnectorSymbolRegex.Matches(text);

            foreach (Match symbolMatch in rangeConnectorSymbolMatches)
            {
                var startSymbolLength = -1;

                if (symbolMatch.Success && symbolMatch.Index == 0 && symbolMatch.Length > startSymbolLength)
                {
                    startSymbolLength = symbolMatch.Length;
                }

                if (startSymbolLength > 0)
                {
                    text = text.Substring(startSymbolLength);
                }
            }

            return text.Trim();
        }

        // TODO: Simplify this method to improve its performance
        private bool StartsWithBasicDate(string text)
        {
            foreach (var regex in this.Config.DateRegexList)
            {
                var match = regex.MatchBegin(text, trim: true);

                if (match.Success)
                {
                    return true;
                }
            }

            return false;
        }

        // match several other cases
        // including 'today', 'the day after tomorrow', 'on 13'
        private List<Token> ImplicitDate(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.Config.ImplicitDateList)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            return ret;
        }

        // Check every integers and ordinal number for date
        private List<Token> NumberWithMonth(string text, DateObject reference)
        {
            var ret = new List<Token>();

            var er = this.Config.OrdinalExtractor.Extract(text);
            er.AddRange(this.Config.IntegerExtractor.Extract(text));

            foreach (var result in er)
            {
                // Check that the extracted number is not part of a decimal number (e.g. 123.24)
                if (result.Start > 1 && (text[(int)result.Start - 1].Equals(',') || text[(int)result.Start - 1].Equals('.')) &&
                    char.IsDigit(text[(int)result.Start - 2]))
                {
                    continue;
                }

                var parsed = int.TryParse((this.Config.NumberParser.Parse(result).Value ?? 0).ToString(), out int num);

                if (!parsed || (num < 1 || num > 31))
                {
                    continue;
                }

                if (result.Start >= 0)
                {
                    // Handling cases like '(Monday,) Jan twenty two'
                    var prefixStr = text.Substring(0, result.Start ?? 0);

                    var match = this.Config.MonthEnd.Match(prefixStr);
                    if (match.Success)
                    {
                        var startIndex = match.Index;
                        var endIndex = match.Index + match.Length + (result.Length ?? 0);

                        ExtendWithWeekdayAndYear(
                            ref startIndex, ref endIndex, Config.MonthOfYear.GetValueOrDefault(match.Groups["month"].Value, reference.Month),
                            num, text, reference);

                        ret.Add(new Token(startIndex, endIndex));
                        continue;
                    }

                    // Handling cases like 'for the 25th'
                    var matches = this.Config.ForTheRegex.Matches(text);
                    bool isFound = false;
                    foreach (Match matchCase in matches)
                    {
                        if (matchCase.Success)
                        {
                            var ordinalNum = matchCase.Groups["DayOfMonth"].Value;
                            if (ordinalNum == result.Text)
                            {
                                var endLength = 0;
                                if (matchCase.Groups["end"].Value.Length > 0)
                                {
                                    endLength = matchCase.Groups["end"].Value.Length;
                                }

                                ret.Add(new Token(matchCase.Index, matchCase.Index + matchCase.Length - endLength));
                                isFound = true;
                            }
                        }
                    }

                    if (isFound)
                    {
                        continue;
                    }

                    // Handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
                    matches = this.Config.WeekDayAndDayOfMonthRegex.Matches(text);

                    foreach (Match matchCase in matches)
                    {
                        if (matchCase.Success)
                        {
                            var ordinalNum = matchCase.Groups["DayOfMonth"].Value;
                            if (ordinalNum == result.Text)
                            {
                                // Get week of day for the ordinal number which is regarded as a date of reference month
                                var date = DateObject.MinValue.SafeCreateFromValue(reference.Year, reference.Month, num);
                                var numWeekDayInt = (int)date.DayOfWeek;

                                // Get week day from text directly, compare it with the weekday generated above
                                // to see whether they refer to the same week day
                                var extractedWeekDayStr = matchCase.Groups["weekday"].Value;

                                // Calculate matchLength considering that matchCase can precede or follow result
                                var matchLength = matchCase.Index < result.Start ?
                                                      result.Start + result.Length - matchCase.Index :
                                                      matchCase.Index + matchCase.Length - result.Start;

                                if (!date.Equals(DateObject.MinValue) &&
                                    numWeekDayInt == Config.DayOfWeek[extractedWeekDayStr] &&
                                    matchCase.Length == matchLength)
                                {

                                    if (matchCase.Index < result.Start)
                                    {
                                        ret.Add(new Token(matchCase.Index, result.Start + result.Length ?? 0));
                                    }
                                    else
                                    {
                                        ret.Add(new Token((int)result.Start, matchCase.Index + matchCase.Length));
                                    }

                                    isFound = true;
                                }
                            }
                        }
                    }

                    if (isFound)
                    {
                        continue;
                    }

                    // Handling cases like 'Monday 21', which both 'Monday' and '21' refer to the same date
                    // The year of expected date can be different to the year of referenceDate.
                    matches = this.Config.WeekDayAndDayRegex.Matches(text);
                    foreach (Match matchCase in matches)
                    {
                        if (matchCase.Success)
                        {
                            var matchLength = result.Start + result.Length - matchCase.Index;

                            if (matchLength == matchCase.Length)
                            {
                                // check if day number is compatible with reference month
                                if (DateObjectExtension.IsValidDate(reference.Year, reference.Month, num) || !this.Config.CheckBothBeforeAfter)
                                {
                                    ret.Add(new Token(matchCase.Index, result.Start + result.Length ?? 0));
                                    isFound = true;
                                }
                            }
                        }
                    }

                    if (isFound)
                    {
                        continue;
                    }

                    // Handling cases like '20th of next month'
                    var suffixStr = text.Substring(result.Start + result.Length ?? 0);
                    var beginMatch = this.Config.RelativeMonthRegex.MatchBegin(suffixStr.Trim(), trim: true);

                    if (beginMatch.Success && beginMatch.Index == 0)
                    {
                        var spaceLen = suffixStr.Length - suffixStr.Trim().Length;
                        var resStart = result.Start;
                        var resEnd = resStart + result.Length + spaceLen + beginMatch.Length;

                        // Check if prefix contains 'the', include it if any
                        var prefix = text.Substring(0, resStart ?? 0);
                        var prefixMatch = this.Config.PrefixArticleRegex.Match(prefix);

                        if (prefixMatch.Success)
                        {
                            resStart = prefixMatch.Index;
                        }

                        ret.Add(new Token(resStart ?? 0, resEnd ?? 0));
                    }

                    // Handling cases like 'second Sunday'
                    suffixStr = text.Substring(result.Start + result.Length ?? 0);

                    beginMatch = this.Config.WeekDayRegex.MatchBegin(suffixStr.Trim(), trim: true);

                    if (beginMatch.Success && num >= 1 && num <= 5
                        && result.Type.Equals(Number.Constants.SYS_NUM_ORDINAL, StringComparison.Ordinal))
                    {
                        var weekDayStr = beginMatch.Groups["weekday"].Value;
                        if (this.Config.DayOfWeek.ContainsKey(weekDayStr))
                        {
                            var spaceLen = suffixStr.Length - suffixStr.Trim().Length;
                            ret.Add(new Token(result.Start ?? 0, result.Start + result.Length + spaceLen + beginMatch.Length ?? 0));
                        }
                    }
                }

                // For cases like "I'll go back twenty second of June"
                if (result.Start + result.Length < text.Length)
                {
                    var afterStr = text.Substring(result.Start + result.Length ?? 0);

                    var match = this.Config.OfMonth.Match(afterStr);
                    if (match.Success)
                    {
                        var startIndex = result.Start ?? 0;
                        var endIndex = (result.Start + result.Length ?? 0) + match.Length;

                        ExtendWithWeekdayAndYear(ref startIndex, ref endIndex,
                                                 Config.MonthOfYear.GetValueOrDefault(match.Groups["month"].Value, reference.Month),
                                                 num, text, reference);

                        ret.Add(new Token(startIndex, endIndex));
                    }
                }
            }

            return ret;
        }

        // TODO: Remove the parsing logic from here
        private void ExtendWithWeekdayAndYear(ref int startIndex, ref int endIndex, int month, int day, string text, DateObject reference)
        {
            var year = reference.Year;

            // Check whether there's a year
            var suffix = text.Substring(endIndex);
            var prefix = text.Substring(0, startIndex);
            endIndex += GetYearIndex(suffix, ref year, out bool success, inPrefix: false);

            // Check also in prefix
            if (!success && Config.CheckBothBeforeAfter)
            {
                startIndex -= GetYearIndex(prefix, ref year, out success, inPrefix: true);
            }

            var date = DateObject.MinValue.SafeCreateFromValue(year, month, day);

            // Check whether there's a weekday
            bool isMatchInSuffix = false;
            var matchWeekDay = this.Config.WeekDayEnd.Match(prefix);

            // Check for weekday in the suffix
            if (!matchWeekDay.Success)
            {
                matchWeekDay = this.Config.WeekDayStart.Match(suffix);
                isMatchInSuffix = matchWeekDay.Success;
            }

            if (matchWeekDay.Success)
            {
                // Get weekday from context directly, compare it with the weekday extraction above
                // to see whether they reference the same weekday
                var extractedWeekDayStr = matchWeekDay.Groups["weekday"].Value;
                var numWeekDayStr = date.DayOfWeek.ToString().ToLowerInvariant();

                if (Config.DayOfWeek.TryGetValue(numWeekDayStr, out var weekDay1) &&
                    Config.DayOfWeek.TryGetValue(extractedWeekDayStr, out var weekDay2))
                {
                    if (!date.Equals(DateObject.MinValue) && weekDay1 == weekDay2)
                    {
                        if (!isMatchInSuffix)
                        {
                            startIndex = matchWeekDay.Index;
                        }
                        else
                        {
                            endIndex += matchWeekDay.Length;
                        }
                    }
                }
            }
        }

        // Cases like "3 days from today", "5 weeks before yesterday", "2 months after tomorrow"
        // Note that these cases are of type "date"
        private List<Token> ExtractRelativeDurationDate(string text, List<Token> tokens, DateObject reference)
        {
            var ret = new List<Token>();
            var tempTokens = new List<Token>(tokens);
            var durationEr = Config.DurationExtractor.Extract(text, reference);

            foreach (var er in durationEr)
            {
                // if it is a multiple duration but its type is not equal to Date, skip it here
                if (IsMultipleDuration(er) && !IsMultipleDurationDate(er))
                {
                    continue;
                }

                // Some types of duration can be compounded with "before", "after" or "from" suffix to create a "date"
                // While some other types of durations, when compounded with such suffix, it will not create a "date", but create a "dateperiod"
                // For example, durations like "3 days", "2 weeks", "1 week and 2 days", can be compounded with such suffix to create a "date"
                // But "more than 3 days", "less than 2 weeks", when compounded with such suffix, it will become cases like "more than 3 days from today" which is a "dateperiod", not a "date"
                // As this parent method is aimed to extract RelativeDurationDate, so for cases with "more than" or "less than", we remove the prefix so as to extract the expected RelativeDurationDate
                if (IsInequalityDuration(er))
                {
                    StripInequalityDuration(er);
                }

                var match = Config.DateUnitRegex.Match(er.Text);

                if (match.Success)
                {
                    ret.AddRange(AgoLaterUtil.ExtractorDurationWithBeforeAndAfter(text, er, ret, Config.UtilityConfiguration));

                    // Take into account also holiday dates
                    if (ret.Count < 1)
                    {
                        var holidayEr = Config.HolidayExtractor.Extract(text, reference);
                        foreach (var holiday in holidayEr)
                        {
                            tempTokens.Add(new Token((int)holiday.Start, (int)(holiday.Start + holiday.Length)));
                        }
                    }

                    // Check for combined patterns Duration + Date, e.g. '3 days before Monday', '4 weeks after January 15th'
                    if (ret.Count < 1 && tempTokens.Count > 0 && er.Text != match.Value)
                    {
                        var afterStr = text.Substring((int)er.Start + (int)er.Length);
                        var connector = Config.BeforeAfterRegex.MatchBegin(afterStr, trim: true);
                        if (connector.Success)
                        {
                            foreach (var token in tempTokens)
                            {
                                var start = (int)er.Start + (int)er.Length + connector.Index + connector.Length;
                                var length = token.Start - start;
                                if (length > 0 && start + length < text.Length && string.IsNullOrWhiteSpace(text.Substring(start, length)))
                                {
                                    Token tok = new Token((int)er.Start, token.End);
                                    ret.Add(tok);
                                }
                            }
                        }
                    }
                }
            }

            // Extract cases like "in 3 weeks", which equals to "3 weeks from today"
            var relativeDurationDateWithInPrefix = ExtractRelativeDurationDateWithInPrefix(text, durationEr, reference);

            // For cases like "in 3 weeks from today", we should choose "3 weeks from today" as the extract result rather than "in 3 weeks" or "in 3 weeks from today"
            foreach (var extractResultWithInPrefix in relativeDurationDateWithInPrefix)
            {
                if (!IsOverlapWithExistExtractions(extractResultWithInPrefix, ret))
                {
                    ret.Add(extractResultWithInPrefix);
                }
            }

            return ret;
        }

        private void StripInequalityDuration(ExtractResult er)
        {
            if (this.Config.CheckBothBeforeAfter)
            {
                StripInequality(er, Config.MoreThanRegex, inPrefix: false);
                StripInequality(er, Config.LessThanRegex, inPrefix: false);
            }
            else
            {
                StripInequality(er, Config.MoreThanRegex, inPrefix: true);
                StripInequality(er, Config.LessThanRegex, inPrefix: true);
            }
        }

        // Used in ExtractRelativeDurationDateWithInPrefix to extract the connector "in" in cases like "In 3 days/weeks/months/years"
        private List<Token> ExtractInConnector(string text, string firstStr, string secondStr, Token duration, out bool success, bool inPrefix)
        {
            List<Token> ret = new List<Token>();

            var match = inPrefix ? Config.InConnectorRegex.MatchEnd(firstStr, trim: true) : Config.InConnectorRegex.MatchBegin(firstStr, trim: true);
            success = match.Success;

            if (match.Success)
            {
                var rangeUnitMatch = Config.RangeUnitRegex.Match(text.Substring(duration.Start, duration.Length));

                if (rangeUnitMatch.Success)
                {
                    var sinceYearMatch = Config.SinceYearSuffixRegex.Match(secondStr);

                    if (sinceYearMatch.Success)
                    {
                        var start = inPrefix ? match.Index : sinceYearMatch.Index;
                        var end = inPrefix ? duration.End + sinceYearMatch.Length : duration.End + match.Index + match.Length;
                        ret.Add(new Token(start, end));
                    }
                    else
                    {
                        var start = inPrefix ? match.Index : duration.Start;
                        var end = inPrefix ? duration.End : duration.End + match.Index + match.Length;
                        ret.Add(new Token(start, end));
                    }
                }
            }

            return ret;
        }

        private int GetYearIndex(string affix, ref int year, out bool success, bool inPrefix)
        {
            int index = 0;

            var matchYear = this.Config.YearSuffix.Match(affix);

            success = !inPrefix ?
                      matchYear.Success && matchYear.Index == 0 :
                      matchYear.Success && matchYear.Index + matchYear.Length == affix.TrimEnd().Length;

            if (success)
            {
                year = GetYearFromText(matchYear);

                if (year >= Constants.MinYearNum && year <= Constants.MaxYearNum)
                {
                    index = !inPrefix ?
                            matchYear.Length :
                            matchYear.Length + (affix.Length - affix.TrimEnd().Length);
                }
            }

            return index;
        }
    }
}
