using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Utilities;
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

        public static bool IsLessThanDay(string unit)
        {
            return unit.Equals("S", StringComparison.Ordinal) ||
                   unit.Equals("M", StringComparison.Ordinal) ||
                   unit.Equals("H", StringComparison.Ordinal);
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refTime)
        {
            var referenceTime = refTime;

            object value = null;
            if (er.Type.Equals(ParserName, StringComparison.Ordinal))
            {
                var innerResult = ParseMergedDuration(er.Text, referenceTime);
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
                        { TimeTypeConstants.DURATION, innerResult.FutureValue.ToString() },
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DURATION, innerResult.PastValue.ToString() },
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
                TimexStr = value == null ? string.Empty : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        // check {and} suffix after a {number} {unit}
        private double ParseNumberWithUnitAndSuffix(string text)
        {
            double numVal = 0;

            var match = this.config.SuffixAndRegex.Match(text);
            if (match.Success)
            {
                var numStr = match.Groups["suffix_num"].Value;
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
            var suffixStr = text;

            // if there are spaces between number and unit
            var ers = ExtractNumbersBeforeUnit(text);

            if (ers.Count == 1)
            {
                var pr = this.config.NumberParser.Parse(ers[0]);

                // followed unit: {num} (<followed unit>and a half hours)
                var srcUnit = string.Empty;
                var noNum = text.Substring(ers[0].Start + ers[0].Length ?? 0).Trim();

                var match = this.config.FollowedUnit.Match(noNum);
                if (match.Success)
                {
                    srcUnit = match.Groups["unit"].Value;
                    suffixStr = match.Groups[Constants.SuffixGroupName].Value;

                    // check also beforeStr for "and an half"
                    if (this.config.CheckBothBeforeAfter && string.IsNullOrEmpty(suffixStr))
                    {
                        noNum = text.Substring(0, (int)ers[0].Start).Trim();
                        var prefixMatch = this.config.SuffixAndRegex.Match(noNum);
                        if (prefixMatch.Success)
                        {
                            suffixStr = prefixMatch.Groups[Constants.SuffixGroupName].Value;
                        }
                    }
                }

                if (match.Success && match.Groups[Constants.BusinessDayGroupName].Success)
                {
                    var numVal = int.Parse(pr.Value.ToString(), CultureInfo.InvariantCulture);
                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, Constants.TimexBusinessDay, false);

                    // The line below was containing this.config.UnitValueMap[srcUnit.Split()[1]]
                    // it was updated to accommodate single word "business day" expressions.
                    ret.FutureValue = ret.PastValue = numVal * this.config.UnitValueMap[srcUnit.Split()[srcUnit.Split().Length - 1]];
                    ret.Success = true;

                    return ret;
                }

                if (this.config.UnitMap.TryGetValue(srcUnit, out var unitStr))
                {
                    // First try to parse combined expression 'num + suffix'
                    double numVal;
                    var combStr = pr.Text + " " + suffixStr;
                    if (this.config.DoubleNumbers.ContainsKey(combStr))
                    {
                        numVal = ParseNumberWithUnitAndSuffix(combStr);
                    }
                    else
                    {
                        numVal = double.Parse(pr.Value.ToString(), CultureInfo.InvariantCulture) + ParseNumberWithUnitAndSuffix(suffixStr);
                    }

                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, unitStr, IsLessThanDay(unitStr));
                    ret.FutureValue = ret.PastValue = numVal * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;
                    return ret;
                }
            }

            return ret;
        }

        // @TODO improve re-use with Extractor
        private List<ExtractResult> ExtractNumbersBeforeUnit(string text)
        {
            var ers = this.config.CardinalExtractor.Extract(text);

            // In special cases some languages will treat "both" as a number to be combined with duration units.
            var specialNumberUnitTokens = Token.GetTokenFromRegex(config.SpecialNumberUnitRegex, text);

            foreach (var token in specialNumberUnitTokens)
            {
                var er = new ExtractResult
                {
                    Start = token.Start,
                    Length = token.Length,
                    Text = text.Substring(token.Start, token.Length),
                };

                ers.Add(er);
            }

            return ers;
        }

        private DateTimeResolutionResult ParseNumberCombinedUnit(string text)
        {
            var ret = new DateTimeResolutionResult();
            var suffixStr = text;

            // if there are NO spaces between number and unit
            var match = this.config.NumberCombinedWithUnit.Match(text);
            if (match.Success)
            {
                var numVal = double.Parse(match.Groups["num"].Value, CultureInfo.InvariantCulture) + ParseNumberWithUnitAndSuffix(suffixStr);

                var srcUnit = match.Groups["unit"].Value;
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    var unitStr = this.config.UnitMap[srcUnit];

                    if (numVal > 1000 && (unitStr.Equals(Constants.TimexYear, StringComparison.Ordinal) ||
                                                        unitStr.Equals(Constants.TimexMonthFull, StringComparison.Ordinal) ||
                                                        unitStr.Equals(Constants.TimexWeek, StringComparison.Ordinal)))
                    {
                        return ret;
                    }

                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, unitStr, IsLessThanDay(unitStr));
                    ret.FutureValue = ret.PastValue = numVal * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;

                    return ret;
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParseAnUnit(string text)
        {
            var ret = new DateTimeResolutionResult();
            var suffixStr = text;

            var match = this.config.AnUnitRegex.Match(text);
            if (!match.Success)
            {
                match = this.config.HalfDateUnitRegex.Match(text);
            }

            if (match.Success)
            {
                var numVal = match.Groups["half"].Success ? 0.5 : 1;
                numVal = match.Groups["quarter"].Success ? 0.25 : numVal;
                numVal = match.Groups["threequarter"].Success ? 0.75 : numVal;

                numVal += ParseNumberWithUnitAndSuffix(suffixStr);

                var srcUnit = match.Groups["unit"].Value;
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    var unitStr = this.config.UnitMap[srcUnit];

                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, unitStr, IsLessThanDay(unitStr));
                    ret.FutureValue = ret.PastValue = numVal * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;
                }
                else if (match.Groups[Constants.BusinessDayGroupName].Success)
                {
                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, Constants.TimexBusinessDay, false);

                    // The line below was containing this.config.UnitValueMap[srcUnit.Split()[1]]
                    // it was updated to accommodate single word "business day" expressions.
                    ret.FutureValue = ret.PastValue = numVal * this.config.UnitValueMap[srcUnit.Split()[srcUnit.Split().Length - 1]];
                    ret.Success = true;
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParseInexactNumberUnit(string text)
        {
            var ret = new DateTimeResolutionResult();

            var match = config.InexactNumberUnitRegex.Match(text);
            if (match.Success)
            {
                // set the inexact number "few", "some" to 3 for now
                double numVal = match.Groups["NumTwoTerm"].Success ? 2 : 3;
                var srcUnit = match.Groups["unit"].Value;

                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    var unitStr = this.config.UnitMap[srcUnit];

                    if (numVal > 1000 && (unitStr.Equals(Constants.TimexYear, StringComparison.Ordinal) ||
                                          unitStr.Equals(Constants.TimexMonthFull, StringComparison.Ordinal) ||
                                          unitStr.Equals(Constants.TimexWeek, StringComparison.Ordinal)))
                    {
                        return ret;
                    }

                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, unitStr, IsLessThanDay(unitStr));
                    ret.FutureValue = ret.PastValue = numVal * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;
                }
                else if (match.Groups[Constants.BusinessDayGroupName].Success)
                {
                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, Constants.TimexBusinessDay, false);

                    // The line below was containing this.config.UnitValueMap[srcUnit.Split()[1]]
                    // it was updated to accommodate single word "business day" expressions.
                    ret.FutureValue = ret.PastValue = numVal * this.config.UnitValueMap[srcUnit.Split()[srcUnit.Split().Length - 1]];
                    ret.Success = true;
                }
            }

            return ret;
        }

        // handle cases that don't contain numbers
        private DateTimeResolutionResult ParseImplicitDuration(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();

            // handle "all day" "all year"
            if (TryGetResultFromRegex(config.AllDateUnitRegex, text, "1", out var result))
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

            // handle "during/for the day/week/month/year"
            if ((config.Options & DateTimeOptions.CalendarMode) != 0 &&
                TryGetResultFromRegex(config.DuringRegex, text, "1", out result))
            {
                ret = result;
            }
            else
            {
                // handle cases like "the hour", which are special durations always not in CalendarMode
                if ((this.config.Options & DateTimeOptions.CalendarMode) == 0)
                {
                    var regex = this.config.PrefixArticleRegex;

                    if (regex != null)
                    {
                        var match = RegExpUtility.MatchBegin(regex, text, false);
                        if (match.Success)
                        {
                            var srcUnit = text.Substring(match.Length);
                            if (this.config.UnitValueMap.ContainsKey(srcUnit))
                            {
                                var numStr = "1";
                                var unitStr = this.config.UnitMap[srcUnit];
                                var numVal = double.Parse(numStr, CultureInfo.InvariantCulture);

                                ret.Timex = TimexUtility.GenerateDurationTimex(numVal, unitStr, IsLessThanDay(unitStr));
                                ret.FutureValue = ret.PastValue = numVal * this.config.UnitValueMap[srcUnit];
                                ret.Success = true;
                            }
                        }
                    }
                }

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
                    var numVal = double.Parse(numStr, CultureInfo.InvariantCulture);
                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, unitStr, IsLessThanDay(unitStr));
                    ret.FutureValue = ret.PastValue = numVal * this.config.UnitValueMap[srcUnit];
                    ret.Success = true;
                }
            }

            return match.Success;
        }

        private DateTimeResolutionResult ParseMergedDuration(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var durationExtractor = this.config.DurationExtractor;

            // DurationExtractor without parameter will not extract merged duration
            var ers = durationExtractor.Extract(text, referenceTime);

            // If the duration extractions do not start at 0, check if the input starts with an isolated unit.
            // This happens for example with patterns like "next week and 3 days" where "next" is not part of the extraction.
            var minStart = ers.Min(er => er.Start);
            if (minStart > 0)
            {
                var match = config.FollowedUnit.Match(text);
                if (match.Success)
                {
                    var er = new ExtractResult
                    {
                        Start = match.Index,
                        Length = match.Length,
                        Text = match.Value,
                        Type = ParserName,
                        Data = null,
                    };
                    ers.Insert(0, er);
                }
            }

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
                ret.Timex = TimexUtility.GenerateCompoundDurationTimex(timexDict, config.UnitValueMap);

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