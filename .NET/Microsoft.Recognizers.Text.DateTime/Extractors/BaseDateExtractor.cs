using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATE; // "Date";

        private readonly IDateExtractorConfiguration config;

        public BaseDateExtractor(IDateExtractorConfiguration config)
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
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(ImplicitDate(text));
            tokens.AddRange(NumberWithMonth(text, reference));
            tokens.AddRange(DurationWithBeforeAndAfter(text, reference));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        public int GetYearFromText(Match match)
        {
            int year = Constants.InvalidYear;

            var yearStr = match.Groups["year"].Value;
            if (!string.IsNullOrEmpty(yearStr))
            {
                year = int.Parse(yearStr);
                if (year < 100 && year >= Constants.MinTwoDigitYearPastNum)
                {
                    year += 1900;
                }
                else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum)
                {
                    year += 2000;
                }
            }
            else
            {
                var firstTwoYearNumStr = match.Groups["firsttwoyearnum"].Value;
                if (!string.IsNullOrEmpty(firstTwoYearNumStr))
                {
                    var er = new ExtractResult
                    {
                        Text = firstTwoYearNumStr,
                        Start = match.Groups["firsttwoyearnum"].Index,
                        Length = match.Groups["firsttwoyearnum"].Length
                    };

                    var firstTwoYearNum = Convert.ToInt32((double)(this.config.NumberParser.Parse(er).Value ?? 0));

                    var lastTwoYearNum = 0;
                    var lastTwoYearNumStr = match.Groups["lasttwoyearnum"].Value;
                    if (!string.IsNullOrEmpty(lastTwoYearNumStr))
                    {
                        er.Text = lastTwoYearNumStr;
                        er.Start = match.Groups["lasttwoyearnum"].Index;
                        er.Length = match.Groups["lasttwoyearnum"].Length;

                        lastTwoYearNum = Convert.ToInt32((double)(this.config.NumberParser.Parse(er).Value ?? 0));
                    }

                    // Exclude pure number like "nineteen", "twenty four"
                    if (firstTwoYearNum < 100 && lastTwoYearNum == 0 || firstTwoYearNum < 100 && firstTwoYearNum % 10 == 0 && lastTwoYearNumStr.Trim().Split(' ').Length == 1)
                    {
                        year = Constants.InvalidYear;
                        return year;
                    }

                    if (firstTwoYearNum >= 100)
                    {
                        year = firstTwoYearNum + lastTwoYearNum;
                    }
                    else
                    {
                        year = firstTwoYearNum * 100 + lastTwoYearNum;
                    }
                }
            }

            return year;
        }

        // match basic patterns in DateRegexList
        private List<Token> BasicRegexMatch(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.DateRegexList)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            return ret;
        }

        // match several other cases
        // including 'today', 'the day after tomorrow', 'on 13'
        private List<Token> ImplicitDate(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.ImplicitDateList)
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

            var er = this.config.OrdinalExtractor.Extract(text);
            er.AddRange(this.config.IntegerExtractor.Extract(text));

            foreach (var result in er)
            {

                Int32.TryParse((this.config.NumberParser.Parse(result).Value ?? 0).ToString(), out int num);

                if (num < 1 || num > 31)
                {
                    continue;
                }

                if (result.Start >= 0)
                {
                    // Handling cases like '(Monday,) Jan twenty two'
                    var frontStr = text.Substring(0, result.Start ?? 0);

                    var match = this.config.MonthEnd.Match(frontStr);
                    if (match.Success)
                    {
                        var startIndex = match.Index;
                        var endIndex = match.Index + match.Length + (result.Length ?? 0);

                        ExtendWithWeekdayAndYear(ref startIndex, ref endIndex,
                            config.MonthOfYear.GetValueOrDefault(match.Groups["month"].Value.ToLower(), reference.Month),
                            num, text, reference);

                        ret.Add(new Token(startIndex, endIndex));
                        continue;
                    }

                    // Handling cases like 'for the 25th'
                    var matches = this.config.ForTheRegex.Matches(text);
                    bool isFound = false;
                    foreach (Match matchCase in matches)
                    {
                        if (matchCase.Success)
                        {
                            var ordinalNum = matchCase.Groups["DayOfMonth"].Value;
                            if (ordinalNum == result.Text)
                            {
                                var endLenght = 0;
                                if (matchCase.Groups["end"].Value != string.Empty)
                                {
                                    endLenght = matchCase.Groups["end"].Value.Length;
                                }
                                ret.Add(new Token(matchCase.Index, matchCase.Index + matchCase.Length - endLenght));
                                isFound = true;
                            }
                        }
                    }

                    if (isFound)
                    {
                        continue;
                    }

                    // Handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
                    matches = this.config.WeekDayAndDayOfMonthRegex.Matches(text);
                    foreach (Match matchCase in matches)
                    {
                        if (matchCase.Success)
                        {
                            var ordinalNum = matchCase.Groups["DayOfMonth"].Value;
                            if (ordinalNum == result.Text)
                            {
                                // Get week of day for the ordinal number which is regarded as a date of reference month
                                var date = DateObject.MinValue.SafeCreateFromValue(reference.Year, reference.Month, num);
                                var numWeekDayStr = date.DayOfWeek.ToString().ToLower();

                                // Get week day from text directly, compare it with the weekday generated above
                                // to see whether they refer to the same week day
                                var extractedWeekDayStr = matchCase.Groups["weekday"].Value.ToLower();
                                if (!date.Equals(DateObject.MinValue) &&
                                    config.DayOfWeek[numWeekDayStr] == config.DayOfWeek[extractedWeekDayStr])
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
                    match = this.config.RelativeMonthRegex.Match(suffixStr.Trim());
                    if (match.Success && match.Index == 0)
                    {
                        var spaceLen = suffixStr.Length - suffixStr.Trim().Length;
                        var resStart = result.Start;
                        var resEnd = resStart + result.Length + spaceLen + match.Length;

                        // Check if prefix contains 'the', include it if any
                        var prefix = text.Substring(0, resStart ?? 0);
                        var prefixMatch = this.config.PrefixArticleRegex.Match(prefix);
                        if (prefixMatch.Success)
                        {
                            resStart = prefixMatch.Index;
                        }

                        ret.Add(new Token(resStart ?? 0, resEnd?? 0));
                    }

                    // Handling cases like 'second Sunday'
                    suffixStr = text.Substring(result.Start + result.Length ?? 0);

                    match = this.config.WeekDayRegex.Match(suffixStr.Trim());

                    if (match.Success && match.Index == 0 && num >= 1 && num <= 5 
                        && result.Type.Equals(Number.Constants.SYS_NUM_ORDINAL))
                    {

                        var weekDayStr = match.Groups["weekday"].Value.ToLower();
                        if (this.config.DayOfWeek.ContainsKey(weekDayStr))
                        {
                            var spaceLen = suffixStr.Length - suffixStr.Trim().Length;
                            ret.Add(new Token(result.Start ?? 0, result.Start + result.Length + spaceLen + match.Length ?? 0));
                        }
                    }
                }

                // For cases like "I'll go back twenty second of June"
                if (result.Start + result.Length < text.Length)
                {
                    var afterStr = text.Substring(result.Start + result.Length ?? 0);

                    var match = this.config.OfMonth.Match(afterStr);
                    if (match.Success)
                    {
                        var startIndex = result.Start ?? 0;
                        var endIndex = (result.Start + result.Length ?? 0) + match.Length;

                        ExtendWithWeekdayAndYear(ref startIndex, ref endIndex,
                            config.MonthOfYear.GetValueOrDefault(match.Groups["month"].Value.ToLower(), reference.Month),
                            num, text, reference);

                        ret.Add(new Token(startIndex, endIndex));
                    }
                }
            }

            return ret;
        }

        // TODO: Remove the parsing logic from here
        private void ExtendWithWeekdayAndYear(ref int startIndex,
            ref int endIndex, int month, int day, string text, DateObject reference)
        {
            var year = reference.Year;

            // Check whether there's a year
            var suffix = text.Substring(endIndex);
            var matchYear = this.config.YearSuffix.Match(suffix);
            if (matchYear.Success && matchYear.Index == 0)
            {
                year = GetYearFromText(matchYear);
                endIndex += matchYear.Length;
            }           

            var date = DateObject.MinValue.SafeCreateFromValue(year, month, day);

            // Check whether there's a weekday
            var prefix = text.Substring(0, startIndex);
            var matchWeekDay = this.config.WeekDayEnd.Match(prefix);
            if (matchWeekDay.Success)
            {

                // Get weekday from context directly, compare it with the weekday extraction above
                // to see whether they are referred to the same weekday
                var extractedWeekDayStr = matchWeekDay.Groups["weekday"].Value.ToLower();
                var numWeekDayStr = date.DayOfWeek.ToString().ToLower();

                if (config.DayOfWeek.TryGetValue(numWeekDayStr, out var weekDay1) &&
                    config.DayOfWeek.TryGetValue(extractedWeekDayStr, out var weekDay2))
                {
                    if (!date.Equals(DateObject.MinValue) && weekDay1 == weekDay2)
                    {
                        startIndex = matchWeekDay.Index;
                    }
                }
            }
        }

        private List<Token> DurationWithBeforeAndAfter(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var durationEr = config.DurationExtractor.Extract(text, reference);

            foreach (var er in durationEr)
            {
                // if it is a multiple duration and its type is not equal to Date than skip it.
                if (er.Data != null && er.Data.ToString() != Constants.MultipleDuration_Date)
                {
                    continue;
                }

                var match = config.DateUnitRegex.Match(er.Text);
                if (match.Success)
                {
                    ret = AgoLaterUtil.ExtractorDurationWithBeforeAndAfter(text,
                        er, ret, config.UtilityConfiguration);
                }
            }

            return ret;
        }

    }
}
