using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
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
                var innerResult = new DateTimeResolutionResult();

                innerResult = ParseMergedDuration(er.Text, referenceTime);
                if (!innerResult.Success)
                {
                    innerResult = ParseNumberWithUnit(er.Text, referenceTime);
                }

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

            var res = (DateTimeResolutionResult)value;
            if (res != null && er.Data != null)
            {
                if (er.Data.Equals(Constants.MORE_THAN_MOD))
                {
                    res.Mod = Constants.MORE_THAN_MOD;
                }
                else if (er.Data.Equals(Constants.LESS_THAN_MOD))
                {
                    res.Mod = Constants.LESS_THAN_MOD;
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
                TimexStr = value == null ? "" : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = ""
            };
            return ret;
        }

        // check {and} suffix after a {number} {unit}
        private double ParseNumberWithUnitAndSuffix(string text)
        {
            double numVal = 0;

            var match = this.config.SuffixAndRegex.Match(text);
            if (match.Success) {
                var numStr = match.Groups["suffix_num"].Value.ToLower();
                if (this.config.DoubleNumbers.ContainsKey(numStr))
                {
                    numVal = this.config.DoubleNumbers[numStr];
                }
            }

            return numVal;
        }

        // simple cases made by a number followed an unit
        private DateTimeResolutionResult ParseNumberWithUnit(string text, DateObject referenceTime)
        {

            DateTimeResolutionResult ret;

            if ((ret = ParseNumberSpaceUnit(text)).Success)
            {
                return ret;
            }

            if ((ret = ParseNumberCombinedUnit(text)).Success)
            {
                return ret;
            }

            if ((ret = ParseAnUnit(text)).Success)
            {
                return ret;
            }

            if ((ret = ParseInexactNumberUnit(text)).Success)
            {
                return ret;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseNumberSpaceUnit(string text)
        {
            var ret = new DateTimeResolutionResult();
            double numVal;
            string numStr, unitStr;
            var suffixStr = text;
            Match match;

            // if there are spaces between nubmer and unit
            var ers = this.config.CardinalExtractor.Extract(text);
            if (ers.Count == 1)
            {
                var pr = this.config.NumberParser.Parse(ers[0]);

                // followed unit: {num} (<followed unit>and a half hours)
                var srcUnit = string.Empty;
                var noNum = text.Substring(ers[0].Start + ers[0].Length ?? 0).Trim().ToLower();

                match = this.config.FollowedUnit.Match(noNum);
                if (match.Success)
                {
                    srcUnit = match.Groups["unit"].Value.ToLower();
                    suffixStr = match.Groups[Constants.SuffixGroupName].Value.ToLower();
                }

                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    numVal = double.Parse(pr.Value.ToString()) + ParseNumberWithUnitAndSuffix(suffixStr);
                    numStr = numVal.ToString(CultureInfo.InvariantCulture);

                    unitStr = this.config.UnitMap[srcUnit];

                    ret.Timex = "P" + (IsLessThanDay(unitStr) ? "T" : "") + numStr + unitStr[0];
                    ret.FutureValue = ret.PastValue = (double)numVal * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;
                    return ret;
                }
            }
            return ret;
        }

        private DateTimeResolutionResult ParseNumberCombinedUnit(string text)
        {
            var ret = new DateTimeResolutionResult();
            double numVal = 0;
            string numStr, unitStr;
            var suffixStr = text;
            Match match;

            // if there are NO spaces between number and unit
            match = this.config.NumberCombinedWithUnit.Match(text);
            if (match.Success)
            {
                numVal = double.Parse(match.Groups["num"].Value) + ParseNumberWithUnitAndSuffix(suffixStr);
                numStr = numVal.ToString(CultureInfo.InvariantCulture);

                var srcUnit = match.Groups["unit"].Value.ToLower();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];

                    if ((double.Parse(numStr) > 1000) && (unitStr.Equals("Y") || unitStr.Equals("MON") || unitStr.Equals("W")))
                    {
                        return ret;
                    }

                    ret.Timex = "P" + (IsLessThanDay(unitStr) ? "T" : "") + numStr + unitStr[0];
                    ret.FutureValue = ret.PastValue = double.Parse(numStr) * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;

                    return ret;
                }
            }
            return ret;
        }

        private DateTimeResolutionResult ParseAnUnit(string text)
        {
            var ret = new DateTimeResolutionResult();
            double numVal = 0;
            string numStr, unitStr;
            var suffixStr = text;
            Match match;

            match = this.config.AnUnitRegex.Match(text);
            if (!match.Success)
            {
                match = this.config.HalfDateUnitRegex.Match(text);
            }

            if (match.Success)
            {
                numVal = match.Groups["half"].Success ? 0.5 : 1;
                numVal += ParseNumberWithUnitAndSuffix(suffixStr);
                numStr = numVal.ToString(CultureInfo.InvariantCulture);

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

        private DateTimeResolutionResult ParseInexactNumberUnit(string text)
        {
            var ret = new DateTimeResolutionResult();
            double numVal = 0;
            string numStr, unitStr;
            Match match;

            match = config.InexactNumberUnitRegex.Match(text);
            if (match.Success)
            {
                if (match.Groups["NumTwoTerm"].Success)
                {
                    numVal = 2;
                }
                else
                {
                    // set the inexact number "few", "some" to 3 for now
                    numVal = 3;
                }

                numStr = numVal.ToString(CultureInfo.InvariantCulture);

                var srcUnit = match.Groups["unit"].Value.ToLower();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];

                    if ((double.Parse(numStr) > 1000) && (unitStr.Equals("Y") || unitStr.Equals("MON") || unitStr.Equals("W")))
                    {
                        return ret;
                    }

                    ret.Timex = "P" + (IsLessThanDay(unitStr) ? "T" : "") + numStr + unitStr[0];
                    ret.FutureValue = ret.PastValue = double.Parse(numStr) * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;

                    return ret;
                }
            }
            return ret;
        }

        // handle cases that don't contain nubmer
        private DateTimeResolutionResult ParseImplicitDuration(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var result = new DateTimeResolutionResult();

            // handle "all day" "all year"
            if (TryGetResultFromRegex(config.AllDateUnitRegex, text, "1", out result))
            {
                ret = result;
            }

            // handle "during/for the day/week/month/year"
            if ((config.Options & DateTimeOptions.CalendarMode) != 0 &&
                TryGetResultFromRegex(config.DuringRegex, text, "1", out result))
            {
                ret = result;
            }

            // handle "half day", "half year"
            if (TryGetResultFromRegex(config.HalfDateUnitRegex, text, "0.5", out result))
            {
                ret = result;
            }

            // handle single duration unit, it is filtered in the extraction that there is a relative word in advance
            if (TryGetResultFromRegex(config.FollowedUnit, text, "1", out result))
            {
                ret = result;
            }

            return ret;
        }

        private bool TryGetResultFromRegex(Regex regex, string text, string numStr, out DateTimeResolutionResult ret)
        {
            ret = new DateTimeResolutionResult();

            var match = regex.Match(text);
            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value;
                if (this.config.UnitValueMap.ContainsKey(srcUnit))
                {
                    var unitStr = this.config.UnitMap[srcUnit];
                    ret.Timex = "P" + (IsLessThanDay(unitStr) ? "T" : "") + numStr + unitStr[0];
                    ret.FutureValue = ret.PastValue = double.Parse(numStr) * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;
                }
            }

            return match.Success;
        }

        public static bool IsLessThanDay(string unit) {
            return unit.Equals("S") || unit.Equals("M") || unit.Equals("H");
        }

        private DateTimeResolutionResult ParseMergedDuration(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var DurationExtractor = this.config.DurationExtractor;

            //DurationExtractor without parameter will not extract merged duration
            var ers = DurationExtractor.Extract(text);

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

            var end = ers[ers.Count - 1].Start + ers[ers.Count - 1].Length?? 0;
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
                    if (pr.Value != null)
                    {
                        timexDict.Add(unitMatch.Groups["unit"].Value, pr.TimexStr);
                        prs.Add(pr);
                    }
                }
            }

            // sort the timex using the granularity of the duration, "P1M23D" for "1 month 23 days" and "23 days 1 month"
            if (prs.Count == ers.Count)
            {
                var unitList = new List<string>(timexDict.Keys);
                unitList.Sort((x, y) => (this.config.UnitValueMap[x] < this.config.UnitValueMap[y]? 1 : -1));
                ret.Timex = "P";
                foreach (var unit in unitList)
                {
                    ret.Timex = $"{ret.Timex}{timexDict[unit].Substring(1)}";
                }

                double value = 0;
                foreach (var pr in prs)
                {
                    value += double.Parse(((DateTimeResolutionResult)(pr.Value)).FutureValue.ToString());
                }

                ret.FutureValue = ret.PastValue = value;
            }

            ret.Success = true;
            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }
    }
}