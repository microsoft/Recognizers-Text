// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseSetParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_SET;

        private readonly ISetParserConfiguration config;

        public BaseSetParser(ISetParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            object value = null;

            if (er.Type.Equals(ParserName, StringComparison.Ordinal))
            {
                var innerResult = ParseEachUnit(er.Text.Trim());

                if (!innerResult.Success)
                {
                    innerResult = ParseEachDuration(er.Text.Trim(), refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParserTimeEveryday(er.Text.Trim(), refDate);
                }

                // NOTE: Do not change the order of the following calls, due to type precedence
                // datetimeperiod > dateperiod > timeperiod > datetime > date > time
                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.DateTimePeriodExtractor, config.DateTimePeriodParser, er.Text.Trim(), refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.DatePeriodExtractor, config.DatePeriodParser, er.Text.Trim(), refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.TimePeriodExtractor, config.TimePeriodParser, er.Text.Trim(), refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.DateTimeExtractor, config.DateTimeParser, er.Text.Trim(), refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.DateExtractor, config.DateParser, er.Text.Trim(), refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.TimeExtractor, config.TimeParser, er.Text.Trim(), refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParserDayEveryweek(er.Text.Trim(), refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParserSingleNumbermonth(er.Text.Trim(), refDate);
                }

                if (innerResult.Success)
                {
                    if ((config.Options & DateTimeOptions.TasksMode) != 0)
                    {
                        innerResult = TasksModeSetHandler.TasksModeAddResolution(ref innerResult, er, refDate);
                    }
                    else
                    {
                        innerResult.FutureResolution = new Dictionary<string, string>
                        {
                            { TimeTypeConstants.SET, (string)innerResult.FutureValue },
                        };

                        innerResult.PastResolution = new Dictionary<string, string>
                        {
                            { TimeTypeConstants.SET, (string)innerResult.PastValue },
                        };
                    }

                    value = innerResult;
                }
            }

            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = value,
                TimexStr = value == null ? string.Empty : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private DateTimeResolutionResult ParseEachDuration(string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();

            var ers = this.config.DurationExtractor.Extract(text, refDate);

            if (ers.Count != 1 || !string.IsNullOrWhiteSpace(text.Substring(ers[0].Start + ers[0].Length ?? 0)))
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            if (this.config.EachPrefixRegex.IsMatch(beforeStr))
            {
                var pr = this.config.DurationParser.Parse(ers[0], DateObject.Now);

                ret = SetHandler.ResolveSet(ref ret, pr.TimexStr);

                if ((config.Options & DateTimeOptions.TasksMode) != 0)
                {
                    ret = TasksModeSetHandler.TasksModeResolveSet(ref ret, pr.TimexStr);
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParseEachUnit(string text)
        {
            var ret = new DateTimeResolutionResult();

            // handle "daily", "weekly"
            var match = this.config.PeriodicRegex.Match(text);
            if (match.Success)
            {
                // @TODO refactor to pass match
                if (!this.config.GetMatchedDailyTimex(text, out string timex))
                {
                    return ret;
                }

                ret = SetHandler.ResolveSet(ref ret, timex);

                if ((config.Options & DateTimeOptions.TasksMode) != 0)
                {
                    ret = TasksModeSetHandler.TasksModeResolveSet(ref ret, timex);
                }

                return ret;
            }

            // handle "each month"
            var exactMatch = this.config.EachUnitRegex.MatchExact(text, trim: true);

            if (exactMatch.Success)
            {
                var sourceUnit = exactMatch.Groups["unit"].Value;
                if (string.IsNullOrEmpty(sourceUnit))
                {
                    sourceUnit = exactMatch.Groups["specialUnit"].Value;
                }

                if (!string.IsNullOrEmpty(sourceUnit) && this.config.UnitMap.ContainsKey(sourceUnit))
                {
                    // @TODO refactor to pass match
                    if (!this.config.GetMatchedUnitTimex(sourceUnit, out string timex))
                    {
                        return ret;
                    }

                    // "handle every other month"
                    if (exactMatch.Groups["other"].Success)
                    {
                        timex = timex.Replace("1", "2");
                    }

                    ret = SetHandler.ResolveSet(ref ret, timex);

                    if ((config.Options & DateTimeOptions.TasksMode) != 0)
                    {
                        ret = TasksModeSetHandler.TasksModeResolveSet(ref ret, timex);
                    }
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParserTimeEveryday(string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();

            var ers = this.config.TimeExtractor.Extract(text, refDate);
            var ers1 = this.config.TimePeriodExtractor.Extract(text, refDate);

            if (ers.Count == 1)
            {
                var afterStr = text.Replace(ers[0].Text, string.Empty);
                var match = this.config.EachDayRegex.Match(afterStr);

                if (match.Success)
                {
                    var pr = this.config.TimeParser.Parse(ers[0], DateObject.Now);
                    ret = SetHandler.ResolveSet(ref ret, pr.TimexStr);

                    if ((config.Options & DateTimeOptions.TasksMode) != 0)
                    {
                        ret = TasksModeSetHandler.TasksModeResolveSet(ref ret, pr.TimexStr + "P1D");
                    }

                }
            }
            else if (ers1.Count == 1)
            {
                var afterStr = text.Replace(ers1[0].Text, string.Empty);
                var match = this.config.EachDayRegex.Match(afterStr);

                if (match.Success)
                {
                    var pr = this.config.TimePeriodParser.Parse(ers1[0], DateObject.Now);
                    ret = SetHandler.ResolveSet(ref ret, pr.TimexStr);

                    if ((config.Options & DateTimeOptions.TasksMode) != 0)
                    {
                        ret = TasksModeSetHandler.TasksModeResolveSet(ref ret, pr.TimexStr + "P1D");
                    }

                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParserDayEveryweek(string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();

            var ers = this.config.DateExtractor.Extract(text, refDate);

            if (ers.Count != 1)
            {
                return ret;
            }

            var afterStr = text.Replace(ers[0].Text, string.Empty);
            var timeErs = this.config.TimeExtractor.Extract(afterStr, refDate);
            var timeErs1 = this.config.TimePeriodExtractor.Extract(afterStr, refDate);

            if (timeErs.Count == 0)
            {
                timeErs = timeErs1;
            }

            var match = this.config.EachUnitRegex.Match(afterStr);
            if (!match.Success)
            {
                match = this.config.PeriodicRegex.Match(text);
            }

            if (match.Success)
            {
                var pr = this.config.DateParser.Parse(ers[0], DateObject.Now);
                var eachResult = ParseEachUnit(match.Value);

                if (timeErs.Count > 0)
                {
                    var timePr = this.config.TimeParser.Parse(timeErs[0], DateObject.Now);
                    ret = SetHandler.ResolveSet(ref ret, pr.TimexStr + timePr.TimexStr);
                    if ((config.Options & DateTimeOptions.TasksMode) != 0)
                    {
                        ret = TasksModeSetHandler.TasksModeResolveSet(ref ret, pr.TimexStr + timePr.TimexStr + eachResult.Timex);
                    }

                }
                else
                {
                    ret = SetHandler.ResolveSet(ref ret, pr.TimexStr);
                    if ((config.Options & DateTimeOptions.TasksMode) != 0)
                    {
                        ret = TasksModeSetHandler.TasksModeResolveSet(ref ret, pr.TimexStr + eachResult.Timex);
                    }
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParserSingleNumbermonth(string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();

            List<ExtractResult> ers = null;
            var success = false;

            // remove key words of set type from text
            var match = config.SetEachRegex.Match(text);
            if (match.Success)
            {
                var trimmedText = text.Replace(match.Value, "this ");

                ers = this.config.DateExtractor.Extract(trimmedText, refDate);
                if (ers.Count == 1 && ers.First().Length == trimmedText.Length)
                {
                    success = true;
                }
            }

            if (success)
            {
                var eachMatch = this.config.EachUnitRegex.Match(text);
                if (!eachMatch.Success)
                {
                    eachMatch = this.config.PeriodicRegex.Match(text);
                }

                if (eachMatch.Success)
                {
                    var pr = this.config.DateParser.Parse(ers[0], DateObject.Now);
                    var eachResult = ParseEachUnit(eachMatch.Value);

                    if ((config.Options & DateTimeOptions.TasksMode) != 0)
                    {
                        ret = TasksModeSetHandler.TasksModeResolveSet(ref ret, "XXXX-XX-" + pr.TimexStr.Substring(8) + eachResult.Timex, pr);
                    }
                    else
                    {
                        ret = SetHandler.ResolveSet(ref ret, "XXXX-XX-" + pr.TimexStr.Substring(8));

                    }
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParseEach(IDateTimeExtractor extractor, IDateTimeParser parser, string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();

            List<ExtractResult> ers = null;
            var success = false;

            // remove key words of set type from text
            var match = config.SetEachRegex.Match(text);
            if (match.Success)
            {
                var trimmedText = text.Remove(match.Index, match.Length);

                ers = extractor.Extract(trimmedText, refDate);
                if (ers.Count == 1 && ers.First().Length == trimmedText.Length)
                {
                    success = true;
                }
            }

            // remove suffix 's' and "on" if existed and re-try
            var match1 = this.config.SetWeekDayRegex.Match(text);
            if (match1.Success)
            {
                var trimmedText = text.Remove(match1.Index, match1.Length);
                trimmedText = trimmedText.Insert(match1.Index, config.WeekDayGroupMatchString(match1));

                ers = extractor.Extract(trimmedText, refDate);
                if (ers.Count == 1 && ers.First().Length == trimmedText.Length)
                {
                    success = true;
                }
            }

            if (success)
            {
                var pr = parser.Parse(ers[0], refDate);

                if ((config.Options & DateTimeOptions.TasksMode) != 0)
                {
                    if (match.Success)
                    {
                        pr.TimexStr = TasksModeSetHandler.TasksModeTimexIntervalExt(pr.TimexStr);
                    }

                    if (match.Groups["other"].Success)
                    {
                        pr.TimexStr = pr.TimexStr.Replace("P1", "P2");
                    }

                    ret = TasksModeSetHandler.TasksModeResolveSet(ref ret, pr.TimexStr, pr);

                }
                else
                {
                    ret = SetHandler.ResolveSet(ref ret, pr.TimexStr);
                }
            }

            return ret;
        }
    }
}