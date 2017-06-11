using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDurationParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DURATION;
        
        private readonly IDurationParserConfiguration config;

        public BaseDurationParser(IDurationParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refTime)
        {
            var referenceTime = refTime;

            object value = null;
            if (er.Type.Equals(ParserName))
            {
                var innerResult = new DTParseResult();

                innerResult = ParseNumerWithUnit(er.Text, referenceTime);
                if (!innerResult.Success)
                {
                    innerResult = ParseImplicitDuration(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DURATION, innerResult.FutureValue.ToString()}
                    };
                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DURATION, innerResult.PastValue.ToString()}
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

        // simple cases made by a number followed an unit
        private DTParseResult ParseNumerWithUnit(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            var numStr = string.Empty;
            var unitStr = string.Empty;

            // if there are spaces between nubmer and unit
            var ers = this.config.CardinalExtractor.Extract(text);
            if (ers.Count == 1)
            {
                var pr = this.config.NumberParser.Parse(ers[0]);
                var srcUnit = text.Substring(ers[0].Start + ers[0].Length ?? 0).Trim().ToLower();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    numStr = pr.Value.ToString();
                    unitStr = this.config.UnitMap[srcUnit];

                    ret.Timex = "P" + (IsLessThanDay(unitStr) ? "T" : "") + numStr + unitStr[0];
                    ret.FutureValue = ret.PastValue = (double)pr.Value * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;
                    return ret;
                }
            }

            // if there are NO spaces between number and unit
            var match = this.config.NumberCombinedWithUnit.Match(text);
            if (match.Success)
            {
                numStr = match.Groups["num"].Value;
                var srcUnit = match.Groups["unit"].Value.ToLower();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];

                    ret.Timex = "P" + (IsLessThanDay(unitStr) ? "T" : "") + numStr + unitStr[0];
                    ret.FutureValue = ret.PastValue = double.Parse(numStr) * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;
                    return ret;
                }
            }

            match = this.config.AnUnitRegex.Match(text);
            if (match.Success)
            {
                numStr = "1";
                var srcUnit = match.Groups["unit"].Value.ToLower();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];

                    ret.Timex = "P" + (IsLessThanDay(unitStr) ? "T" : "") + numStr + unitStr[0];
                    ret.FutureValue = ret.PastValue = double.Parse(numStr) * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;
                    return ret;
                }
            }

            return ret;
        }

        // handle cases that don't contain nubmer
        private DTParseResult ParseImplicitDuration(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            // handle "all day" "all year"
            var match = this.config.AllDateUnitRegex.Match(text);
            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value;
                if (this.config.UnitValueMap.ContainsKey(srcUnit))
                {
                    var unitStr = this.config.UnitMap[srcUnit];
                    ret.Timex = "P1" + unitStr[0];
                    ret.FutureValue = ret.PastValue = (double)this.config.UnitValueMap[srcUnit];
                    ret.Success = true;
                    return ret;
                }
            }

            return ret;
        }

        public static bool IsLessThanDay(string unit)
        {
            if (unit.Equals("S") || unit.Equals("M") || unit.Equals("H"))
            {
                return true;
            }
            return false;
        }
    }
}