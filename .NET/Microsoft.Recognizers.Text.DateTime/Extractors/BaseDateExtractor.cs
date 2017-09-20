using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateExtractor : IExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATE; // "Date";

        private readonly IDateExtractorConfiguration config;

        public BaseDateExtractor(IDateExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(ImplicitDate(text));
            tokens.AddRange(NumberWithMonth(text));
            tokens.AddRange(DurationWithBeforeAndAfter(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
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

        // check every integers and ordinal number for date
        private List<Token> NumberWithMonth(string text)
        {
            var ret = new List<Token>();
            var er = this.config.OrdinalExtractor.Extract(text);
            er.AddRange(this.config.IntegerExtractor.Extract(text));
            foreach (var result in er)
            {
                var num = 0;

                Int32.TryParse((this.config.NumberParser.Parse(result).Value ?? 0).ToString(), out num);

                if (num < 1 || num > 31)
                {
                    continue;
                }

                if (result.Start > 0)
                {
                    var frontStr = text.Substring(0, result.Start ?? 0);

                    var match = this.config.MonthEnd.Match(frontStr);
                    if (match.Success)
                    {
                        ret.Add(new Token(match.Index, match.Index + match.Length + (result.Length ?? 0)));
                        continue;
                    }

                    // handling cases like 'for the 25th'
                    match = this.config.ForTheRegex.Match(text);
                    if (match.Success)
                    {
                        var ordinalNum = match.Groups["DayOfMonth"].Value;
                        if (ordinalNum == result.Text)
                        {
                            var endLenght = 0;
                            if (match.Groups["end"].Value != string.Empty)
                            {
                                endLenght = match.Groups["end"].Value.Length;
                            }
                            ret.Add(new Token(match.Index, match.Index + match.Length - endLenght));
                            continue;
                        }
                    }

                    // handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
                    match = this.config.WeekDayAndDayOfMothRegex.Match(text);
                    if (match.Success)
                    {
                        // create a extract result which content ordinal string of text
                        ExtractResult erTmp = new ExtractResult();
                        erTmp.Text = match.Groups["DayOfMonth"].Value;
                        erTmp.Start = match.Groups["DayOfMonth"].Index;
                        erTmp.Length = match.Groups["DayOfMonth"].Length;
                        var day = Convert.ToInt32((double)(this.config.NumberParser.Parse(erTmp).Value ?? 0));

                        if (day == num)
                        {
                            ret.Add(new Token(match.Index, result.Start + result.Length ?? 0));
                            continue;
                        }
                    }

                    // handling cases like '20th of next month'
                    var suffixStr = text.Substring(result.Start + result.Length ?? 0);
                    match = this.config.RelativeMonthRegex.Match(suffixStr.Trim());
                    if (match.Success && match.Index == 0)
                    {
                        var spaceLen = suffixStr.Length - suffixStr.Trim().Length;
                        ret.Add(new Token(result.Start ?? 0, result.Start + result.Length + spaceLen + match.Length ?? 0));
                    }

                    // handling cases like 'second Sunday'
                    suffixStr = text.Substring(result.Start + result.Length ?? 0);
                    match = this.config.WeekDayRegex.Match(suffixStr.Trim());
                    if (match.Success && match.Index == 0 && num >= 1 && num <= 5)
                    {
                        var weekDayStr = match.Groups["weekday"].Value.ToLower();
                        if (this.config.DayOfWeek.ContainsKey(weekDayStr))
                        {
                            var spaceLen = suffixStr.Length - suffixStr.Trim().Length;
                            ret.Add(new Token(result.Start ?? 0, result.Start + result.Length + spaceLen + match.Length ?? 0));
                        }
                    }
                }

                if (result.Start + result.Length < text.Length)
                {
                    var afterStr = text.Substring(result.Start + result.Length ?? 0);

                    var match = this.config.OfMonth.Match(afterStr);
                    if (match.Success)
                    {
                        ret.Add(new Token(result.Start ?? 0, (result.Start + result.Length ?? 0) + match.Length));
                        continue;
                    }
                }
            }
            return ret;
        }

        private List<Token> DurationWithBeforeAndAfter(string text)
        {
            var ret = new List<Token>();
            var durationEr = config.DurationExtractor.Extract(text);

            foreach (var er in durationEr)
            {
                var match = config.DateUnitRegex.Match(er.Text);
                if (match.Success)
                {
                    ret = AgoLaterUtil.ExtractorDurationWithBeforeAndAfter(text,
                        er,
                        ret,
                        config.UtilityConfiguration);
                }
            }

            return ret;
        }

    }
}
