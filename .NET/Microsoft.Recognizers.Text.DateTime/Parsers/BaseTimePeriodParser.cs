using System;
using System.Collections.Generic;

using Microsoft.Recognizers.Text.Number;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_TIMEPERIOD; //"TimePeriod";
        
        private readonly ITimePeriodParserConfiguration config;

        public BaseTimePeriodParser(ITimePeriodParserConfiguration configuration)
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
                var innerResult = ParseSimpleCases(er.Text, referenceTime);
                if (!innerResult.Success)
                {
                    innerResult = MergeTwoTimePoints(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseNight(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            FormatUtil.FormatTime(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            FormatUtil.FormatTime(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item2)
                        }
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            FormatUtil.FormatTime(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            FormatUtil.FormatTime(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item2)
                        }
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

        private DateTimeResolutionResult ParseSimpleCases(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceTime.Year, month = referenceTime.Month, day = referenceTime.Day;
            var trimedText = text.Trim().ToLower();

            var match = this.config.PureNumberFromToRegex.Match(trimedText);
            if (!match.Success)
            {
                match = this.config.PureNumberBetweenAndRegex.Match(trimedText);
            }

            if (match.Success && match.Index == 0)
            {
                // this "from .. to .." pattern is valid if followed by a Date OR "pm"
                var isValid = false;

                // get hours
                var hourGroup = match.Groups["hour"];
                var hourStr = hourGroup.Captures[0].Value;

                if (!this.config.Numbers.TryGetValue(hourStr, out int beginHour))
                {
                    beginHour = int.Parse(hourStr);
                }

                hourStr = hourGroup.Captures[1].Value;

                if (!this.config.Numbers.TryGetValue(hourStr, out int endHour))
                {
                    endHour = int.Parse(hourStr);
                }

                // parse "pm" 
                var leftDesc = match.Groups["leftDesc"].Value;
                var rightDesc = match.Groups["rightDesc"].Value;
                var pmStr = match.Groups["pm"].Value;
                var amStr = match.Groups["am"].Value;
                var descStr = match.Groups["desc"].Value;

                // The "ampm" only occurs in time, we don't have to consider it here
                if (string.IsNullOrEmpty(leftDesc))
                {

                    bool rightAmValid = !string.IsNullOrEmpty(rightDesc) &&
                                            config.UtilityConfiguration.AmDescRegex.Match(rightDesc.ToLower()).Success;
                    bool rightPmValid = !string.IsNullOrEmpty(rightDesc) &&
                                    config.UtilityConfiguration.PmDescRegex.Match(rightDesc.ToLower()).Success;

                    if (!string.IsNullOrEmpty(amStr) || rightAmValid)
                    {
                        
                        if (beginHour >= 12)
                        {
                            beginHour -= 12;
                        }

                        if (endHour >= 12)
                        {
                            endHour -= 12;
                        }

                        isValid = true;

                    }
                    else if (!string.IsNullOrEmpty(pmStr) || rightPmValid)
                    {

                        if (beginHour < 12)
                        {
                            beginHour += 12;
                        }

                        if (endHour < 12)
                        {
                            endHour += 12;
                        }

                        isValid = true;

                    }
                }

                if (isValid)
                {
                    var beginStr = "T" + beginHour.ToString("D2");
                    var endStr = "T" + endHour.ToString("D2");

                    ret.Timex = $"({beginStr},{endStr},PT{endHour - beginHour}H)";

                    ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(
                        DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, 0, 0),
                        DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, 0, 0));

                    ret.Success = true;

                    return ret;
                }
            }

            return ret;
        }

        private DateTimeResolutionResult MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            DateTimeParseResult pr1 = null, pr2 = null;
            var endingTimePoint = false;

            var ers = this.config.TimeExtractor.Extract(text, referenceTime);
            if (ers.Count != 2)
            {
                if (ers.Count == 1)
                {
                    var numErs = this.config.IntegerExtractor.Extract(text);

                    foreach (var num in numErs)
                    {
                        if (num.Start > ers[0].Start + ers[0].Length)
                        {
                            // check if it is a ending number
                            var endingNumber = false;
                            if (num.Start + num.Length == text.Length)
                            {
                                endingNumber = true;
                            }
                            else
                            {
                                var afterStr = text.Substring(num.Start + num.Length?? 0);
                                var endingMatch = this.config.GeneralEndingRegex.Match(afterStr);
                                if (endingMatch.Success)
                                {
                                    endingNumber = true;
                                }
                            }

                            // check if the middle string between the time point and the ending number is a connect string.
                            if (endingNumber)
                            {
                                var midStrBegin = ers[0].Start + ers[0].Length;
                                var midStrEnd = num.Start - midStrBegin;
                                var middleStr = text.Substring(midStrBegin?? 0, midStrEnd?? 0);
                                var tillMatch = this.config.TillRegex.Match(middleStr);
                                if (tillMatch.Success)
                                {
                                    num.Data = null;
                                    num.Type = Constants.SYS_DATETIME_TIME;
                                    ers.Add(num);
                                    endingTimePoint = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!endingTimePoint)
                {
                    return ret;
                }
            }

            pr1 = this.config.TimeParser.Parse(ers[0], referenceTime);
            pr2 = this.config.TimeParser.Parse(ers[1], referenceTime);

            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            var ampmStr1 = ((DateTimeResolutionResult)pr1.Value).Comment;
            var ampmStr2 = ((DateTimeResolutionResult)pr2.Value).Comment;

            var beginTime = (DateObject) ((DateTimeResolutionResult) pr1.Value).FutureValue;
            var endTime = (DateObject) ((DateTimeResolutionResult) pr2.Value).FutureValue;

            if (!string.IsNullOrEmpty(ampmStr2) && ampmStr2.EndsWith("ampm") 
                && endTime <= beginTime && endTime.Hour<12)
            {
                endTime = endTime.AddHours(12);
                ((DateTimeResolutionResult) pr2.Value).FutureValue = endTime;
                pr2.TimexStr = $"T{endTime.Hour}";
            }

            ret.Timex = $"({pr1.TimexStr},{pr2.TimexStr},PT{Convert.ToInt32((endTime - beginTime).TotalHours)}H)";
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginTime, endTime);
            ret.Success = true;

            
            if (!string.IsNullOrEmpty(ampmStr1) && ampmStr1.EndsWith("ampm") && !string.IsNullOrEmpty(ampmStr2) && ampmStr2.EndsWith("ampm"))
            {
                ret.Comment = "ampm";
            }

            ret.SubDateTimeEntities = new List<object> {pr1, pr2};

            return ret;
        }

        // parse "morning", "afternoon", "night"
        private DateTimeResolutionResult ParseNight(string text, DateObject referenceTime)
        {
            int day = referenceTime.Day,
                month = referenceTime.Month,
                year = referenceTime.Year;
            var ret = new DateTimeResolutionResult();

            // extract early/late prefix from text
            var match = this.config.TimeOfDayRegex.Match(text);
            bool hasEarly = false, hasLate = false;
            if (match.Success)
            {
                if (!string.IsNullOrEmpty(match.Groups["early"].Value))
                {
                    var early = match.Groups["early"].Value;
                    text = text.Replace(early, "");
                    hasEarly = true;
                    ret.Comment = "early";
                }

                if (!hasEarly && !string.IsNullOrEmpty(match.Groups["late"].Value))
                {
                    var late = match.Groups["late"].Value;
                    text = text.Replace(late, "");
                    hasLate = true;
                    ret.Comment = "late";
                }
            }

            if (!this.config.GetMatchedTimexRange(text, out string timex, out int beginHour, out int endHour, out int endMinSeg))
            {
                return new DateTimeResolutionResult();
            }

            // modify time period if "early" or "late" is existed
            if (hasEarly)
            {
                endHour = beginHour + 2;
                // handling case: night end with 23:59
                if (endMinSeg == 59)
                {
                    endMinSeg = 0;
                }
            }
            else if (hasLate)
            {
                beginHour = beginHour + 2;
            }

            ret.Timex = timex;

            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(
                DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, 0, 0),
                DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMinSeg, endMinSeg)
                );

            ret.Success = true;

            return ret;
        }
    }
}