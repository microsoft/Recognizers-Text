using System;
using System.Collections.Generic;
using System.Linq;
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

                if (!innerResult.Success)
                {
                    innerResult = ParserTimeEveryday(er.Text, refDate);
                }

                // NOTE: Please do not change the order of following function
                // we must consider datetime before date
                if (!innerResult.Success)
                {
                    innerResult = ParseEachDateTime(er.Text, refDate);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEachDate(er.Text, refDate);
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

        private static bool IsLessThanDay(string unit)
        {
            return unit.Equals("S", StringComparison.Ordinal) ||
                   unit.Equals("M", StringComparison.Ordinal) ||
                   unit.Equals("H", StringComparison.Ordinal);
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
                ret.Timex = pr.TimexStr;
                ret.FutureValue = ret.PastValue = "Set: " + pr.TimexStr;
                ret.Success = true;
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

                    if (this.config.GetMatchedUnitTimex(sourceUnit, out string timex))
                    {
                        ret.Timex = timex;
                    }
                    else
                    {
                        return ret;
                    }

                    ret.FutureValue = ret.PastValue = "Set: " + ret.Timex;
                    ret.Success = true;
                    return ret;
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParserTimeEveryday(string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();
            var ers = this.config.TimeExtractor.Extract(text, refDate);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            var match = this.config.EachDayRegex.Match(beforeStr);
            if (match.Success)
            {
                var pr = this.config.TimeParser.Parse(ers[0], DateObject.Now);
                ret.Timex = pr.TimexStr;
                ret.FutureValue = ret.PastValue = "Set: " + ret.Timex;
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseEachDate(string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();
            var ers = this.config.DateExtractor.Extract(text, refDate);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            var match = this.config.EachPrefixRegex.Match(beforeStr);
            if (match.Success)
            {
                var pr = this.config.DateParser.Parse(ers[0], DateObject.Now);
                ret.Timex = pr.TimexStr;
                ret.FutureValue = ret.PastValue = "Set: " + ret.Timex;
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseEachDateTime(string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();
            var ers = this.config.DateTimeExtractor.Extract(text, refDate);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            var match = this.config.EachPrefixRegex.Match(beforeStr);
            if (match.Success)
            {
                var pr = this.config.DateTimeParser.Parse(ers[0], DateObject.Now);
                ret.Timex = pr.TimexStr;
                ret.FutureValue = ret.PastValue = "Set: " + ret.Timex;
                ret.Success = true;
                return ret;
            }

            return ret;
        }
    }
}