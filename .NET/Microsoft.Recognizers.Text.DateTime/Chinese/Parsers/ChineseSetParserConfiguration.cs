using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseSetParserConfiguration : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_SET;

        private static readonly IDateTimeExtractor DurationExtractor = new ChineseDurationExtractorConfiguration();
        private static readonly IDateTimeExtractor TimeExtractor = new ChineseTimeExtractorConfiguration();
        private static readonly IDateTimeExtractor DateExtractor = new ChineseDateExtractorConfiguration();
        private static readonly IDateTimeExtractor DateTimeExtractor = new ChineseDateTimeExtractorConfiguration();

        private readonly IFullDateTimeParserConfiguration config;

        public ChineseSetParserConfiguration(IFullDateTimeParserConfiguration configuration)
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
            return unit.Equals("S") || unit.Equals("M") || unit.Equals("H");
        }

        private DateTimeResolutionResult ParseEachDuration(string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();
            var ers = DurationExtractor.Extract(text, refDate);
            if (ers.Count != 1 || !string.IsNullOrWhiteSpace(text.Substring(ers[0].Start + ers[0].Length ?? 0)))
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            if (ChineseSetExtractorConfiguration.EachPrefixRegex.IsMatch(beforeStr))
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
            var match = ChineseSetExtractorConfiguration.EachUnitRegex.MatchExact(text, trim: true);

            if (match.Success)
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

        private DateTimeResolutionResult ParserTimeEveryday(string text, DateObject refDate)
        {
            var ret = new DateTimeResolutionResult();
            var ers = TimeExtractor.Extract(text, refDate);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            var match = ChineseSetExtractorConfiguration.EachDayRegex.Match(beforeStr);
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
            var ers = DateExtractor.Extract(text, refDate);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            var match = ChineseSetExtractorConfiguration.EachPrefixRegex.Match(beforeStr);
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
            var ers = DateTimeExtractor.Extract(text, refDate);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            var match = ChineseSetExtractorConfiguration.EachPrefixRegex.Match(beforeStr);
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