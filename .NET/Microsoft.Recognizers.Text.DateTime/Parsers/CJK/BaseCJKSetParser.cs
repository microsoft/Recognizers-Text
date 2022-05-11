// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKSetParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_SET;

        private readonly ICJKSetParserConfiguration config;

        public BaseCJKSetParser(ICJKSetParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            object value = null;
            if (er.Type.Equals(ParserName, StringComparison.Ordinal))
            {
                var innerResult = ParseEachUnit(er.Text);
                if (!innerResult.Success)
                {
                    innerResult = ParseEachDuration(er.Text, refDate);
                }

                // NOTE: Please do not change the order of following function
                // we must consider datetime before date
                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.DateTimeExtractor, config.DateTimeParser, er.Text, refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.DateExtractor, config.DateParser, er.Text, refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.TimePeriodExtractor, config.TimePeriodParser, er.Text, refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.TimeExtractor, config.TimeParser, er.Text, refDate);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.SET, (string)innerResult.FutureValue },
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.SET, (string)innerResult.PastValue },
                    };

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

            if (ers.Count != 1 || string.IsNullOrWhiteSpace(text.Substring(ers[0].Start + ers[0].Length ?? 0)))
            {
                return ret;
            }

            var afterStr = text.Substring(ers[0].Start + ers[0].Length ?? 0);
            if (this.config.EachPrefixRegex.IsMatch(afterStr))
            {
                var pr = this.config.DurationParser.Parse(ers[0], DateObject.Now);
                ret = SetHandler.ResolveSet(ref ret, pr.TimexStr);
                return ret;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseEachUnit(string text)
        {
            var ret = new DateTimeResolutionResult();

            // handle "each month"
            var match = this.config.EachUnitRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var sourceUnit = match.Groups["unit"].Value;
                if (!string.IsNullOrEmpty(sourceUnit) && this.config.UnitMap.ContainsKey(sourceUnit))
                {

                    if (this.config.GetMatchedUnitTimex(sourceUnit, out string timexStr))
                    {
                        ret = SetHandler.ResolveSet(ref ret, timexStr);
                    }
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParseEach(IDateTimeExtractor extractor, IDateTimeParser parser, string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();
            var ers = extractor.Extract(text, refDate);
            var success = false;
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);
                var match = this.config.EachPrefixRegex.Match(beforeStr);

                if (match.Success && match.Length + er.Length == text.Length)
                {
                    success = true;
                }
                else if (er.Type == Constants.SYS_DATETIME_TIME || er.Type == Constants.SYS_DATETIME_DATE)
                {
                    // Cases like "every day at 2pm" or "every year on April 15th"
                    var eachRegex = er.Type == Constants.SYS_DATETIME_TIME ? this.config.EachDayRegex : this.config.EachDateUnitRegex;
                    match = eachRegex.Match(beforeStr);
                    if (match.Success && match.Length + er.Length == text.Length)
                    {
                        success = true;
                    }
                }

                if (success)
                {
                    var pr = parser.Parse(er, refDate);
                    ret = SetHandler.ResolveSet(ref ret, pr.TimexStr);
                    break;
                }
            }

            return ret;
        }
    }
}