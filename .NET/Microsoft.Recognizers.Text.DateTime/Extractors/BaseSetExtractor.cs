// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseSetExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        private readonly ISetExtractorConfiguration config;

        public BaseSetExtractor(ISetExtractorConfiguration config)
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

            tokens.AddRange(MatchEachUnit(text));
            tokens.AddRange(MatchEachDuration(text, reference));
            tokens.AddRange(TimeEveryday(text, reference));

            if ((config.Options & DateTimeOptions.TasksMode) != 0)
            {
                tokens.AddRange(DayEveryweek(text, reference));
            }

            tokens.AddRange(MatchEach(config.DateExtractor, text, reference));
            tokens.AddRange(MatchEach(config.TimeExtractor, text, reference));
            tokens.AddRange(MatchEach(config.DateTimeExtractor, text, reference));
            tokens.AddRange(MatchEach(config.DatePeriodExtractor, text, reference));
            tokens.AddRange(MatchEach(config.TimePeriodExtractor, text, reference));
            tokens.AddRange(MatchEach(config.DateTimePeriodExtractor, text, reference));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        public List<Token> MatchEachDuration(string text, DateObject reference)
        {
            var ret = new List<Token>();

            var ers = this.config.DurationExtractor.Extract(text, reference);
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
            }

            return ret;
        }

        // every month, weekly, quarterly etc
        public List<Token> MatchEachUnit(string text)
        {
            var ret = new List<Token>();

            // handle "daily", "monthly"
            var matches = this.config.PeriodicRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            // handle "each month"
            matches = this.config.EachUnitRegex.Matches(text);
            foreach (Match match in matches)
            {
                if (match.Groups["unit"].Value.Equals("month"))
                {
                    var beforeStr = text.Substring(0, match.Index);
                    var dayMatch = this.config.BeforeEachDayRegex.Match(beforeStr);

                    if (dayMatch.Success)
                    {
                        ret.Add(new Token(dayMatch.Index, match.Index + match.Length));
                    }
                    else
                    {
                        ret.Add(new Token(match.Index, match.Index + match.Length));
                    }
                }
                else
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            return ret;
        }

        public virtual List<Token> TimeEveryday(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var ers = this.config.TimeExtractor.Extract(text, reference);

            if ((config.Options & DateTimeOptions.TasksMode) != 0)
            {
                var ers1 = this.config.TimePeriodExtractor.Extract(text, reference);
                if (ers.Count == 0 && ers1.Count == 1)
                {
                    ers = ers1;
                }
            }

            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);

                var beforeStr = text.Substring(0, er.Start ?? 0);
                var beforeMatch = this.config.EachDayRegex.Match(beforeStr);
                var startIndexBeforeMatch = beforeMatch.Length + beforeMatch.Index - beforeMatch.Value.TrimStart().Length;
                if (beforeMatch.Success)
                {
                    ret.Add(new Token(startIndexBeforeMatch, er.Start + er.Length ?? 0));
                }

                var match = this.config.EachDayRegex.Match(afterStr);
                if (match.Success)
                {
                    ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length + match.Index));
                }
            }

            return ret;
        }

        // Handle cases like 19th of every month: For now specific to TasksMode
        public virtual List<Token> DayEveryweek(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var ers = this.config.DateExtractor.Extract(text, reference);
            if (NumberRecognizer.RecognizeOrdinal(text, config.Culture).Count > 0)
            {
                return ret;
            }

            if (ers.Count != 1)
            {
                return ret;
            }

            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);

                var beforeStr = text.Substring(0, er.Start ?? 0);
                var beforeMatch = MatchEachUnit(beforeStr);
                var timeBeforeErs = this.config.TimeExtractor.Extract(beforeStr, reference);
                var timeBeforeErs1 = this.config.TimePeriodExtractor.Extract(beforeStr, reference);
                if (timeBeforeErs.Count == 0 && (timeBeforeErs1.Count != 0))
                {
                    timeBeforeErs = timeBeforeErs1;
                }

                var match = MatchEachUnit(afterStr);
                var timeErs = this.config.TimeExtractor.Extract(afterStr, reference);
                var timeErs1 = this.config.TimePeriodExtractor.Extract(afterStr, reference);
                if (timeErs.Count == 0 && (timeErs1.Count != 0))
                {
                    timeErs = timeErs1;
                }

                if (beforeMatch.Count > 0)
                {
                    var beforeMatchInd = beforeMatch[0].Start;
                    if (timeBeforeErs.Count > 0)
                    {
                        beforeMatchInd = Math.Min(beforeMatchInd, (int)timeBeforeErs[0].Start);
                    }

                    var erEnd = er.Start + er.Length ?? 0;
                    if (timeErs.Count > 0)
                    {
                        erEnd += (int)timeErs[0].Start + (int)timeErs[0].Length;
                    }

                    ret.Add(new Token(beforeMatchInd, erEnd));
                }

                if (match.Count > 0)
                {
                    var matchInd = match[0].Length + match[0].Start;
                    if (timeErs.Count > 0)
                    {
                        matchInd = Math.Max(matchInd, (int)timeErs[0].Start + (int)timeErs[0].Length);
                    }

                    var erStart = er.Start ?? 0;
                    if (timeBeforeErs.Count > 0)
                    {
                        erStart = Math.Min(erStart, (int)timeBeforeErs[0].Start);
                    }

                    ret.Add(new Token(erStart, (er.Start + er.Length ?? 0) + matchInd));
                }
            }

            return ret;
        }

        public List<Token> MatchEach(IDateTimeExtractor extractor, string text, DateObject reference)
        {
            var ret = new List<Token>();
            var matches = config.SetEachRegex.Matches(text);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    // "3pm *each* day"
                    var trimmedText = text.Remove(match.Index, match.Length);

                    var ers = extractor.Extract(trimmedText, reference);
                    foreach (var er in ers)
                    {
                        if (er.Start <= match.Index && (er.Start + er.Length) > match.Index)
                        {
                            ret.Add(new Token(er.Start ?? 0, (er.Start + match.Length + er.Length) ?? 0));
                        }
                    }
                }
            }

            // handle "Mondays"
            matches = this.config.SetWeekDayRegex.Matches(text);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    Tuple<string, int> weekdayTuple = config.WeekDayGroupMatchTuple(match);
                    string weekday = weekdayTuple.Item1;
                    int del = weekdayTuple.Item2;

                    var trimmedText = text.Remove(match.Index, match.Length);
                    trimmedText = trimmedText.Insert(match.Index, weekday);

                    var ers = extractor.Extract(trimmedText, reference);
                    foreach (var er in ers)
                    {
                        if (er.Start <= match.Index && er.Text.Contains(match.Groups["weekday"].Value))
                        {
                            var len = (er.Length ?? 0) + del;
                            if (match.Groups[Constants.PrefixGroupName].ToString().Length > 0)
                            {
                                len += match.Groups[Constants.PrefixGroupName].ToString().Length;
                            }

                            if (match.Groups[Constants.SuffixGroupName].ToString().Length > 0)
                            {
                                len += match.Groups[Constants.SuffixGroupName].ToString().Length;
                            }

                            ret.Add(new Token(er.Start ?? 0, er.Start + len ?? 0));
                        }
                    }
                }
            }

            return ret;
        }
    }
}
