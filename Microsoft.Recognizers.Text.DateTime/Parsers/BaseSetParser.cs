using System.Collections.Generic;
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
            if (er.Type.Equals(ParserName))
            {
                var innerResult = ParseEachUnit(er.Text);
                if (!innerResult.Success)
                {
                    innerResult = ParseEachDuration(er.Text);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParserTimeEveryday(er.Text);
                }

                // NOTE: Please do not change the order of following function
                // datetimeperiod>dateperiod>timeperiod>datetime>date>time
                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.DateTimePeriodExtractor, config.DateTimePeriodParser, er.Text);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.DatePeriodExtractor, config.DatePeriodParser, er.Text);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.TimePeriodExtractor, config.TimePeriodParser, er.Text);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.DateTimeExtractor, config.DateTimeParser, er.Text);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.DateExtractor, config.DateParser, er.Text);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseEach(config.TimeExtractor, config.TimeParser, er.Text);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.SET, (string) innerResult.FutureValue}
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.SET, (string) innerResult.PastValue}
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
                TimexStr = value == null ? "" : ((DateTimeResolutionResult) value).Timex,
                ResolutionStr = ""
            };

            return ret;
        }

        private DateTimeResolutionResult ParseEachDuration(string text)
        {
            var ret = new DateTimeResolutionResult();
            var ers = this.config.DurationExtractor.Extract(text);
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
            // handle "daily", "weekly"
            var match = this.config.PeriodicRegex.Match(text);
            if (match.Success)
            {
                string timex;
                if (!this.config.GetMatchedDailyTimex(text, out timex))
                {
                    return ret;
                }

                ret.Timex = timex;
                ret.FutureValue = ret.PastValue = "Set: " + ret.Timex;
                ret.Success = true;

                return ret;
            }

            // handle "each month"
            match = this.config.EachUnitRegex.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                var sourceUnit = match.Groups["unit"].Value;
                if (!string.IsNullOrEmpty(sourceUnit) && this.config.UnitMap.ContainsKey(sourceUnit))
                {
                    string timex;
                    if (!this.config.GetMatchedUnitTimex(sourceUnit, out timex))
                    {
                        return ret;
                    }

                    ret.Timex = timex;
                    ret.FutureValue = ret.PastValue = "Set: " + ret.Timex;
                    ret.Success = true;
                    return ret;
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParserTimeEveryday(string text)
        {
            var ret = new DateTimeResolutionResult();
            var ers = this.config.TimeExtractor.Extract(text);
            if (ers.Count != 1)
            {
                return ret;
            }

            var afterStr = text.Replace(ers[0].Text, "");
            var match = this.config.EachDayRegex.Match(afterStr);
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

        private DateTimeResolutionResult ParseEach(IExtractor extractor, IDateTimeParser parser, string text)
        {
            var ret = new DateTimeResolutionResult();
            var ers = extractor.Extract(text);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            var match = this.config.EachPrefixRegex.Match(beforeStr);
            if (match.Success)
            {
                var pr = parser.Parse(ers[0], DateObject.Now);
                ret.Timex = pr.TimexStr;
                ret.FutureValue = ret.PastValue = "Set: " + ret.Timex;
                ret.Success = true;
                return ret;
            }

            return ret;
        }
    }
}