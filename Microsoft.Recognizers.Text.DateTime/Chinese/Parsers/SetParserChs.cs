using System.Collections.Generic;
using Microsoft.Recognizers.Text.DateTime.Chinese.Extractors;
using DateObject = System.DateTime;
using Microsoft.Recognizers.Text.DateTime.Parsers;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Parsers
{
    public class SetParserChs : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_SET;

        private static readonly IExtractor _durationExtractor = new DurationExtractorChs();
        private static readonly IExtractor _timeExtractor = new TimeExtractorChs();
        private static readonly IExtractor _dateExtractor = new DateExtractorChs();
        private static readonly IExtractor _dateTimeExtractor = new DateTimeExtractorChs();

        private readonly IFullDateTimeParserConfiguration config;

        public SetParserChs(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceDate = refDate;
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
                // we must consider datetime before date
                if (!innerResult.Success)
                {
                    innerResult = ParseEachDateTime(er.Text);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseEachDate(er.Text);
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
                TimexStr = value == null ? "" : ((DTParseResult) value).Timex,
                ResolutionStr = ""
            };
            return ret;
        }

        private DTParseResult ParseEachDuration(string text)
        {
            var ret = new DTParseResult();
            var ers = _durationExtractor.Extract(text);
            if (ers.Count != 1 || !string.IsNullOrWhiteSpace(text.Substring(ers[0].Start + ers[0].Length ?? 0)))
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            if (SetExtractorChs.EachPrefixRegex.IsMatch(beforeStr))
            {
                var pr = this.config.DurationParser.Parse(ers[0], DateObject.Now);
                ret.Timex = pr.TimexStr;
                ret.FutureValue = ret.PastValue = "Set: " + pr.TimexStr;
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        private DTParseResult ParseEachUnit(string text)
        {
            var ret = new DTParseResult();
            // handle "each month"
            var match = SetExtractorChs.EachUnitRegex.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                var sourceUnit = match.Groups["unit"].Value;
                if (!string.IsNullOrEmpty(sourceUnit) && this.config.UnitMap.ContainsKey(sourceUnit))
                {
                    if (sourceUnit.Equals("天") || sourceUnit.Equals("日"))
                    {
                        ret.Timex = "P1D";
                    }
                    else if (sourceUnit.Equals("周") || sourceUnit.Equals("星期"))
                    {
                        ret.Timex = "P1W";
                    }
                    else if (sourceUnit.Equals("月"))
                    {
                        ret.Timex = "P1M";
                    }
                    else if (sourceUnit.Equals("年"))
                    {
                        ret.Timex = "P1Y";
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

        private DTParseResult ParserTimeEveryday(string text)
        {
            var ret = new DTParseResult();
            var ers = _timeExtractor.Extract(text);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            var match = SetExtractorChs.EachDayRegex.Match(beforeStr);
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

        private DTParseResult ParseEachDate(string text)
        {
            var ret = new DTParseResult();
            var ers = _dateExtractor.Extract(text);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            var match = SetExtractorChs.EachPrefixRegex.Match(beforeStr);
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

        private DTParseResult ParseEachDateTime(string text)
        {
            var ret = new DTParseResult();
            var ers = _dateTimeExtractor.Extract(text);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            var match = SetExtractorChs.EachPrefixRegex.Match(beforeStr);
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


        private static bool IsLessThanDay(string unit)
        {
            if (unit.Equals("S") || unit.Equals("M") || unit.Equals("H"))
            {
                return true;
            }
            return false;
        }
    }
}