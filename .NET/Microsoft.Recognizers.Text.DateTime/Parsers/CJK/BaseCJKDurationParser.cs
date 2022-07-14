// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Recognizers.Text.NumberWithUnit;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDurationParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DURATION;

        private readonly ICJKDurationParserConfiguration config;

        public BaseCJKDurationParser(ICJKDurationParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceTime = refDate;

            var dateTimeParseResult = ParseMergedDuration(er.Text, referenceTime);

            if (!dateTimeParseResult.Success)
            {
                dateTimeParseResult = DurationParsingUtil.ParseInexactNumberUnit(er.Text, this.config);
            }

            if (!dateTimeParseResult.Success)
            {
                 dateTimeParseResult = ParseAnUnit(er.Text);
            }

            if (!dateTimeParseResult.Success)
            {
                var parseResult = this.config.InternalParser.Parse(er);
                var unitResult = parseResult.Value as UnitValue;

                if (unitResult == null)
                {
                    return null;
                }

                var unitStr = unitResult.Unit;
                var number = string.IsNullOrEmpty(unitResult.Number) ? 1 : double.Parse(unitResult.Number, CultureInfo.InvariantCulture);

                dateTimeParseResult.Timex = TimexUtility.GenerateDurationTimex(number, unitStr, DurationParsingUtil.IsLessThanDay(unitStr));
                dateTimeParseResult.FutureValue = dateTimeParseResult.PastValue = number * this.config.UnitValueMap[unitStr];
                dateTimeParseResult.Success = true;
            }

            if (dateTimeParseResult.Success)
            {
                dateTimeParseResult.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DURATION, dateTimeParseResult.FutureValue.ToString() },
                    };

                dateTimeParseResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DURATION, dateTimeParseResult.PastValue.ToString() },
                    };
            }

            if (dateTimeParseResult.Success)
            {
                var moreOrLessMatch = config.MoreOrLessRegex.Match(er.Text);
                if (moreOrLessMatch.Success)
                {
                    if (moreOrLessMatch.Groups[Constants.LessGroupName].Success)
                    {
                        dateTimeParseResult.Mod = Constants.LESS_THAN_MOD;
                    }
                    else if (moreOrLessMatch.Groups[Constants.MoreGroupName].Success)
                    {
                        dateTimeParseResult.Mod = Constants.MORE_THAN_MOD;
                    }
                }
            }

            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = dateTimeParseResult,
                TimexStr = dateTimeParseResult.Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private DateTimeResolutionResult ParseAnUnit(string text)
        {
            var ret = new DateTimeResolutionResult();

            var match = this.config.AnUnitRegex.Match(text);

            if (match.Groups[Constants.AnotherGroupName].Success)
            {
                var numVal = match.Groups[Constants.HalfGroupName].Success ? 0.5 : 1;
                numVal = match.Groups[Constants.QuarterGroupName].Success ? 0.25 : numVal;
                numVal = match.Groups[Constants.ThreeQuarterGroupName].Success ? 0.75 : numVal;

                var srcUnit = match.Groups[Constants.UnitGroupName].Value;
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    var unitStr = this.config.UnitMap[srcUnit];

                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, unitStr, DurationParsingUtil.IsLessThanDay(unitStr));
                    ret.FutureValue = ret.PastValue = numVal * this.config.UnitValueMap[unitStr];
                    ret.Success = true;
                }

            }

            return ret;
        }

        private DateTimeResolutionResult ParseMergedDuration(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var durationExtractor = this.config.DurationExtractor;

            // DurationExtractor without parameter will not extract merged duration
            var ers = durationExtractor.Extract(text, referenceTime);

            // only handle merged duration cases like "1 month 21 days"
            if (ers.Count <= 1)
            {
                ret.Success = false;
                return ret;
            }

            var start = ers[0].Start ?? 0;
            if (start != 0)
            {
                var beforeStr = text.Substring(0, start - 1);
                if (!string.IsNullOrWhiteSpace(beforeStr))
                {
                    return ret;
                }
            }

            var end = ers[ers.Count - 1].Start + ers[ers.Count - 1].Length ?? 0;
            if (end != text.Length)
            {
                var afterStr = text.Substring(end);
                if (!string.IsNullOrWhiteSpace(afterStr))
                {
                    return ret;
                }
            }

            var prs = new List<DateTimeParseResult>();
            var timexDict = new Dictionary<string, string>();

            // insert timex into a dictionary
            foreach (var er in ers)
            {
                var unitRegex = this.config.DurationUnitRegex;
                var unitMatch = unitRegex.Match(er.Text);
                if (unitMatch.Success)
                {
                    var pr = (DateTimeParseResult)Parse(er);
                    if (pr != null && pr.Value != null)
                    {
                        timexDict.Add(this.config.UnitMap[unitMatch.Groups[Constants.UnitGroupName].Value], pr.TimexStr);
                        prs.Add(pr);
                    }
                }
            }

            // sort the timex using the granularity of the duration, "P1M23D" for "1 month 23 days" and "23 days 1 month"
            if (prs.Count > 0)
            {
                ret.Timex = TimexUtility.GenerateCompoundDurationTimex(timexDict, this.config.UnitValueMap);

                double value = 0;
                foreach (var pr in prs)
                {
                    value += double.Parse(((DateTimeResolutionResult)pr.Value).FutureValue.ToString(), CultureInfo.InvariantCulture);
                }

                ret.FutureValue = ret.PastValue = value;
            }

            ret.Success = true;
            return ret;
        }
    }
}