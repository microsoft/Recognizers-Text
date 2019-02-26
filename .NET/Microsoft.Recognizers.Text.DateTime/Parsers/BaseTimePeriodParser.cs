using System;
using System.Collections.Generic;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";

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
                DateTimeResolutionResult innerResult;

                if (TimeZoneUtility.ShouldResolveTimeZone(er, config.Options))
                {
                    var metadata = er.Data as Dictionary<string, object>;
                    var timezoneEr = metadata[Constants.SYS_DATETIME_TIMEZONE] as ExtractResult;
                    var timezonePr = config.TimeZoneParser.Parse(timezoneEr);

                    innerResult = InternalParse(
                        er.Text.Substring(0, (int)(er.Length - timezoneEr.Length)),
                        referenceTime);

                    if (timezonePr != null && timezonePr.Value != null)
                    {
                        innerResult.TimeZoneResolution = ((DateTimeResolutionResult)timezonePr.Value).TimeZoneResolution;
                    }
                }
                else
                {
                    innerResult = InternalParse(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)innerResult.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)innerResult.FutureValue).Item2)
                        },
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)innerResult.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)innerResult.PastValue).Item2)
                        },
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

        private DateTimeResolutionResult InternalParse(string entityText, DateObject referenceTime)
        {
            var innerResult = ParseSimpleCases(entityText, referenceTime);

            if (!innerResult.Success)
            {
                innerResult = MergeTwoTimePoints(entityText, referenceTime);
            }

            if (!innerResult.Success)
            {
                innerResult = ParseTimeOfDay(entityText, referenceTime);
            }

            return innerResult;
        }

        // Cases like "from 3 to 5am" or "between 3:30 and 5" are parsed here
        private DateTimeResolutionResult ParseSimpleCases(string text, DateObject referenceTime)
        {
            // Cases like "from 3 to 5pm" or "between 4 and 6am", time point is pure number without colon
            var ret = ParsePureNumCases(text, referenceTime);

            if (!ret.Success)
            {
                // Cases like "from 3:30 to 5" or "between 3:30am to 6pm", at least one of the time point contains colon
                ret = ParseSpecificTimeCases(text, referenceTime);
            }

            if (!ret.Success)
            {
                // Cases like "between 0730-0930"
                ret = ParsePureDigitNumCases(text, referenceTime);
            }

            return ret;
        }

        // Cases like "between 0730 to 0930", only this case is handled in this method
        private DateTimeResolutionResult ParsePureDigitNumCases(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceTime.Year, month = referenceTime.Month, day = referenceTime.Day;
            var trimmedText = text.Trim().ToLower();

            var match = this.config.PureNumberBetweenAndRegex.MatchBegin(trimmedText, trim: true);

            if (match.Success)
            {
                // get hours
                var hourGroup = match.Groups[Constants.HourGroupName];
                var minuteGroup = match.Groups[Constants.MinuteGroupName];

                if (hourGroup.Captures.Count == 2 && minuteGroup.Captures.Count == 2)
                {
                    var beginHourEndIndex = hourGroup.Captures[0].Index + hourGroup.Captures[0].Length;
                    var beginMinuteStartIndex = minuteGroup.Captures[0].Index;
                    var endHourEndIndex = hourGroup.Captures[1].Index + hourGroup.Captures[1].Length;
                    var endMinuteStartIndex = minuteGroup.Captures[1].Index;

                    // falls into the case "between 0730 to 0930"
                    if (beginHourEndIndex == beginMinuteStartIndex && endHourEndIndex == endMinuteStartIndex)
                    {
                        var startHourStr = hourGroup.Captures[0].Value;
                        var startMinuteStr = minuteGroup.Captures[0].Value;
                        var endHourStr = hourGroup.Captures[1].Value;
                        var endMinuteStr = minuteGroup.Captures[1].Value;

                        if (!this.config.Numbers.TryGetValue(startHourStr, out int beginHour))
                        {
                            beginHour = int.Parse(startHourStr);
                        }

                        if (!this.config.Numbers.TryGetValue(startMinuteStr, out int beginMinute))
                        {
                            beginMinute = int.Parse(startMinuteStr);
                        }

                        if (!this.config.Numbers.TryGetValue(endHourStr, out int endHour))
                        {
                            endHour = int.Parse(endHourStr);
                        }

                        if (!this.config.Numbers.TryGetValue(endMinuteStr, out int endMinute))
                        {
                            endMinute = int.Parse(endMinuteStr);
                        }

                        var beginDateTime = DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, beginMinute, 0);
                        var endDateTime = DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMinute, 0);

                        if (beginHour <= Constants.HalfDayHourCount && endHour <= Constants.HalfDayHourCount)
                        {
                            if (beginHour > endHour)
                            {
                                if (beginHour == Constants.HalfDayHourCount)
                                {
                                    beginDateTime = beginDateTime.AddHours(-Constants.HalfDayHourCount);
                                }
                                else
                                {
                                    endDateTime = endDateTime.AddHours(Constants.HalfDayHourCount);
                                }
                            }

                            ret.Comment = Constants.Comment_AmPm;
                        }

                        if (endDateTime < beginDateTime)
                        {
                            endDateTime = endDateTime.AddHours(24);
                        }

                        var beginStr = DateTimeFormatUtil.ShortTime(beginDateTime.Hour, beginMinute);
                        var endStr = DateTimeFormatUtil.ShortTime(endDateTime.Hour, endMinute);

                        ret.Timex = $"({beginStr},{endStr},{DateTimeFormatUtil.LuisTimeSpan(endDateTime - beginDateTime)})";

                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(
                            beginDateTime,
                            endDateTime);

                        ret.Success = true;
                    }
                }
            }

            return ret;
        }

        // Cases like "from 3 to 5pm" or "between 4 and 6am", time point is pure number without colon
        private DateTimeResolutionResult ParsePureNumCases(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceTime.Year, month = referenceTime.Month, day = referenceTime.Day;
            var trimmedText = text.Trim().ToLower();

            var match = this.config.PureNumberFromToRegex.MatchBegin(trimmedText, trim: true);

            if (!match.Success)
            {
                match = this.config.PureNumberBetweenAndRegex.MatchBegin(trimmedText, trim: true);
            }

            if (match.Success)
            {
                // this "from .. to .." pattern is valid if followed by a Date OR "pm"
                var isValid = false;

                // get hours
                var hourGroup = match.Groups[Constants.HourGroupName];
                var hourStr = hourGroup.Captures[0].Value;
                var afterHourIndex = hourGroup.Captures[0].Index + hourGroup.Captures[0].Length;

                // hard to integrate this part into the regex
                if (afterHourIndex == trimmedText.Length || !trimmedText.Substring(afterHourIndex).Trim().StartsWith(":"))
                {
                    if (!this.config.Numbers.TryGetValue(hourStr, out int beginHour))
                    {
                        beginHour = int.Parse(hourStr);
                    }

                    hourStr = hourGroup.Captures[1].Value;
                    afterHourIndex = hourGroup.Captures[1].Index + hourGroup.Captures[1].Length;

                    if (afterHourIndex == trimmedText.Length || !trimmedText.Substring(afterHourIndex).Trim().StartsWith(":"))
                    {
                        if (!this.config.Numbers.TryGetValue(hourStr, out int endHour))
                        {
                            endHour = int.Parse(hourStr);
                        }

                        // parse "pm"
                        var leftDesc = match.Groups["leftDesc"].Value;
                        var rightDesc = match.Groups["rightDesc"].Value;
                        var matchPmStr = match.Groups[Constants.PmGroupName].Value;
                        var matchAmStr = match.Groups[Constants.AmGroupName].Value;
                        var descStr = match.Groups[Constants.DescGroupName].Value;

                        // The "ampm" only occurs in time, we don't have to consider it here
                        if (string.IsNullOrEmpty(leftDesc))
                        {
                            var rightAmValid = !string.IsNullOrEmpty(rightDesc) &&
                                                    config.UtilityConfiguration.AmDescRegex.Match(rightDesc.ToLower()).Success;
                            var rightPmValid = !string.IsNullOrEmpty(rightDesc) &&
                                            config.UtilityConfiguration.PmDescRegex.Match(rightDesc.ToLower()).Success;

                            if (!string.IsNullOrEmpty(matchAmStr) || rightAmValid)
                            {
                                if (endHour >= Constants.HalfDayHourCount)
                                {
                                    endHour -= Constants.HalfDayHourCount;
                                }

                                if (beginHour >= Constants.HalfDayHourCount && beginHour - Constants.HalfDayHourCount < endHour)
                                {
                                    beginHour -= Constants.HalfDayHourCount;
                                }

                                // Resolve case like "11 to 3am"
                                if (beginHour < Constants.HalfDayHourCount && beginHour > endHour)
                                {
                                    beginHour += Constants.HalfDayHourCount;
                                }

                                isValid = true;
                            }
                            else if (!string.IsNullOrEmpty(matchPmStr) || rightPmValid)
                            {
                                if (endHour < Constants.HalfDayHourCount)
                                {
                                    endHour += Constants.HalfDayHourCount;
                                }

                                // Resolve case like "11 to 3pm"
                                if (beginHour + Constants.HalfDayHourCount < endHour)
                                {
                                    beginHour += Constants.HalfDayHourCount;
                                }

                                isValid = true;
                            }
                        }

                        if (isValid)
                        {
                            var beginStr = "T" + beginHour.ToString("D2");
                            var endStr = "T" + endHour.ToString("D2");

                            if (endHour >= beginHour)
                            {
                                ret.Timex = $"({beginStr},{endStr},PT{endHour - beginHour}H)";
                            }
                            else
                            {
                                ret.Timex = $"({beginStr},{endStr},PT{endHour - beginHour + 24}H)";
                            }

                            // Try to get the timezone resolution
                            var timeErs = config.TimeExtractor.Extract(trimmedText);
                            foreach (var er in timeErs)
                            {
                                var pr = config.TimeParser.Parse(er, referenceTime);
                                if (((DateTimeResolutionResult)pr.Value).TimeZoneResolution != null)
                                {
                                    ret.TimeZoneResolution = ((DateTimeResolutionResult)pr.Value).TimeZoneResolution;
                                    break;
                                }
                            }

                            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(
                                DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, 0, 0),
                                DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, 0, 0));

                            ret.Success = true;
                        }
                    }
                }
            }

            return ret;
        }

        // Cases like "from 3:30 to 5" or "between 3:30am to 6pm", at least one of the time point contains colon
        private DateTimeResolutionResult ParseSpecificTimeCases(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            int year = referenceTime.Year, month = referenceTime.Month, day = referenceTime.Day;

            // Handle cases like "from 4:30 to 5"
            var match = config.SpecificTimeFromToRegex.MatchExact(text, trim: true);

            if (!match.Success)
            {
                // Handle cases like "between 5:10 and 7"
                match = config.SpecificTimeBetweenAndRegex.MatchExact(text, trim: true);
            }

            if (match.Success)
            {
                // Cases like "half past seven" are not handled here
                if (match.Groups[Constants.PrefixGroupName].Success)
                {
                    return ret;
                }

                // Cases like "4" is different with "4:00" as the Timex is different "T04H" vs "T04H00M"
                int beginHour;
                int beginMinute = Constants.InvalidMinute;
                int beginSecond = Constants.InvalidSecond;
                int endHour;
                int endMinute = Constants.InvalidMinute;
                int endSecond = Constants.InvalidSecond;

                // Get time1 and time2
                var hourGroup = match.Groups[Constants.HourGroupName];

                var hourStr = hourGroup.Captures[0].Value;

                if (config.Numbers.ContainsKey(hourStr))
                {
                    beginHour = config.Numbers[hourStr];
                }
                else
                {
                    beginHour = int.Parse(hourStr);
                }

                hourStr = hourGroup.Captures[1].Value;

                if (config.Numbers.ContainsKey(hourStr))
                {
                    endHour = config.Numbers[hourStr];
                }
                else
                {
                    endHour = int.Parse(hourStr);
                }

                var time1StartIndex = match.Groups["time1"].Index;
                var time1EndIndex = time1StartIndex + match.Groups["time1"].Length;
                var time2StartIndex = match.Groups["time2"].Index;
                var time2EndIndex = time2StartIndex + match.Groups["time2"].Length;

                // Get beginMinute (if exists) and endMinute (if exists)
                for (int i = 0; i < match.Groups[Constants.MinuteGroupName].Captures.Count; i++)
                {
                    var minuteCapture = match.Groups[Constants.MinuteGroupName].Captures[i];
                    if (minuteCapture.Index >= time1StartIndex && minuteCapture.Index + minuteCapture.Length <= time1EndIndex)
                    {
                        beginMinute = int.Parse(minuteCapture.Value);
                    }
                    else if (minuteCapture.Index >= time2StartIndex && minuteCapture.Index + minuteCapture.Length <= time2EndIndex)
                    {
                        endMinute = int.Parse(minuteCapture.Value);
                    }
                }

                // Get beginSecond (if exists) and endSecond (if exists)
                for (int i = 0; i < match.Groups[Constants.SecondGroupName].Captures.Count; i++)
                {
                    var secondCapture = match.Groups[Constants.SecondGroupName].Captures[i];
                    if (secondCapture.Index >= time1StartIndex && secondCapture.Index + secondCapture.Length <= time1EndIndex)
                    {
                        beginSecond = int.Parse(secondCapture.Value);
                    }
                    else if (secondCapture.Index >= time2StartIndex && secondCapture.Index + secondCapture.Length <= time2EndIndex)
                    {
                        endSecond = int.Parse(secondCapture.Value);
                    }
                }

                // Desc here means descriptions like "am / pm / o'clock"
                // Get leftDesc (if exists) and rightDesc (if exists)
                var leftDesc = match.Groups["leftDesc"].Value;
                var rightDesc = match.Groups["rightDesc"].Value;

                for (int i = 0; i < match.Groups[Constants.DescGroupName].Captures.Count; i++)
                {
                    var descCapture = match.Groups[Constants.DescGroupName].Captures[i];
                    if (descCapture.Index >= time1StartIndex && descCapture.Index + descCapture.Length <= time1EndIndex && string.IsNullOrEmpty(leftDesc))
                    {
                        leftDesc = descCapture.Value;
                    }
                    else if (descCapture.Index >= time2StartIndex && descCapture.Index + descCapture.Length <= time2EndIndex && string.IsNullOrEmpty(rightDesc))
                    {
                        rightDesc = descCapture.Value;
                    }
                }

                var beginDateTime = DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, beginMinute >= 0 ? beginMinute : 0, beginSecond >= 0 ? beginSecond : 0);
                var endDateTime = DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMinute >= 0 ? endMinute : 0, endSecond >= 0 ? endSecond : 0);

                var hasLeftAm = !string.IsNullOrEmpty(leftDesc) && leftDesc.ToLower().StartsWith("a");
                var hasLeftPm = !string.IsNullOrEmpty(leftDesc) && leftDesc.ToLower().StartsWith("p");
                var hasRightAm = !string.IsNullOrEmpty(rightDesc) && rightDesc.ToLower().StartsWith("a");
                var hasRightPm = !string.IsNullOrEmpty(rightDesc) && rightDesc.ToLower().StartsWith("p");
                var hasLeft = hasLeftAm || hasLeftPm;
                var hasRight = hasRightAm || hasRightPm;

                // Both time point has description like 'am' or 'pm'
                if (hasLeft && hasRight)
                {
                    if (hasLeftAm)
                    {
                        if (beginHour >= Constants.HalfDayHourCount)
                        {
                            beginDateTime = beginDateTime.AddHours(-Constants.HalfDayHourCount);
                        }
                    }
                    else
                    {
                        if (beginHour < Constants.HalfDayHourCount)
                        {
                            beginDateTime = beginDateTime.AddHours(Constants.HalfDayHourCount);
                        }
                    }

                    if (hasRightAm)
                    {
                        if (endHour > Constants.HalfDayHourCount)
                        {
                            endDateTime = endDateTime.AddHours(-Constants.HalfDayHourCount);
                        }
                    }
                    else
                    {
                        if (endHour < Constants.HalfDayHourCount)
                        {
                            endDateTime = endDateTime.AddHours(Constants.HalfDayHourCount);
                        }
                    }
                }
                else if (hasLeft || hasRight)
                {
                    // one of the time point has description like 'am' or 'pm'
                    if (hasLeftAm)
                    {
                        if (beginHour >= Constants.HalfDayHourCount)
                        {
                            beginDateTime = beginDateTime.AddHours(-Constants.HalfDayHourCount);
                        }

                        if (endHour < Constants.HalfDayHourCount)
                        {
                            if (endDateTime < beginDateTime)
                            {
                                endDateTime = endDateTime.AddHours(Constants.HalfDayHourCount);
                            }
                        }
                    }
                    else if (hasLeftPm)
                    {
                        if (beginHour < Constants.HalfDayHourCount)
                        {
                            beginDateTime = beginDateTime.AddHours(Constants.HalfDayHourCount);
                        }

                        if (endHour < Constants.HalfDayHourCount)
                        {
                            if (endDateTime < beginDateTime)
                            {
                                var span = beginDateTime - endDateTime;
                                endDateTime = endDateTime.AddHours(span.TotalHours >= Constants.HalfDayHourCount ?
                                    24 :
                                    Constants.HalfDayHourCount);
                            }
                        }
                    }

                    if (hasRightAm)
                    {
                        if (endHour >= Constants.HalfDayHourCount)
                        {
                            endDateTime = endDateTime.AddHours(-Constants.HalfDayHourCount);
                        }

                        if (beginHour < Constants.HalfDayHourCount)
                        {
                            if (endDateTime < beginDateTime)
                            {
                                beginDateTime = beginDateTime.AddHours(-Constants.HalfDayHourCount);
                            }
                        }
                    }
                    else if (hasRightPm)
                    {
                        if (endHour < Constants.HalfDayHourCount)
                        {
                            endDateTime = endDateTime.AddHours(Constants.HalfDayHourCount);
                        }

                        if (beginHour < Constants.HalfDayHourCount)
                        {
                            if (endDateTime < beginDateTime)
                            {
                                beginDateTime = beginDateTime.AddHours(-Constants.HalfDayHourCount);
                            }
                            else
                            {
                                var span = endDateTime - beginDateTime;
                                if (span.TotalHours > Constants.HalfDayHourCount)
                                {
                                    beginDateTime = beginDateTime.AddHours(Constants.HalfDayHourCount);
                                }
                            }
                        }
                    }
                }

                // No 'am' or 'pm' indicator
                else if (beginHour <= Constants.HalfDayHourCount && endHour <= Constants.HalfDayHourCount)
                {
                    if (beginHour > endHour)
                    {
                        if (beginHour == Constants.HalfDayHourCount)
                        {
                            beginDateTime = beginDateTime.AddHours(-Constants.HalfDayHourCount);
                        }
                        else
                        {
                            endDateTime = endDateTime.AddHours(Constants.HalfDayHourCount);
                        }
                    }

                    ret.Comment = Constants.Comment_AmPm;
                }

                if (endDateTime < beginDateTime)
                {
                    endDateTime = endDateTime.AddHours(24);
                }

                var beginStr = DateTimeFormatUtil.ShortTime(beginDateTime.Hour, beginMinute, beginSecond);
                var endStr = DateTimeFormatUtil.ShortTime(endDateTime.Hour, endMinute, endSecond);

                ret.Success = true;

                ret.Timex = $"({beginStr},{endStr},{DateTimeFormatUtil.LuisTimeSpan(endDateTime - beginDateTime)})";

                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(
                    beginDateTime,
                    endDateTime);

                ret.SubDateTimeEntities = new List<object>();

                // In SplitDateAndTime mode, time points will be get from these SubDateTimeEntities
                // Cases like "from 4 to 5pm", "4" should not be treated as SubDateTimeEntity
                if (hasLeft || beginMinute != Constants.InvalidMinute || beginSecond != Constants.InvalidSecond)
                {
                    var er = new ExtractResult()
                    {
                        Start = time1StartIndex,
                        Length = time1EndIndex - time1StartIndex,
                        Text = text.Substring(time1StartIndex, time1EndIndex - time1StartIndex),
                        Type = $"{Constants.SYS_DATETIME_TIME}",
                    };

                    var pr = this.config.TimeParser.Parse(er, referenceTime);
                    ret.SubDateTimeEntities.Add(pr);
                }

                // Cases like "from 4am to 5", "5" should not be treated as SubDateTimeEntity
                if (hasRight || endMinute != Constants.InvalidMinute || endSecond != Constants.InvalidSecond)
                {
                    var er = new ExtractResult
                    {
                        Start = time2StartIndex,
                        Length = time2EndIndex - time2StartIndex,
                        Text = text.Substring(time2StartIndex, time2EndIndex - time2StartIndex),
                        Type = $"{Constants.SYS_DATETIME_TIME}",
                    };

                    var pr = this.config.TimeParser.Parse(er, referenceTime);
                    ret.SubDateTimeEntities.Add(pr);
                }

                ret.Success = true;
            }

            return ret;
        }

        private DateTimeResolutionResult MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            DateTimeParseResult pr1 = null, pr2 = null;
            var validTimeNumber = false;

            var ers = this.config.TimeExtractor.Extract(text, referenceTime);
            if (ers.Count != 2)
            {
                if (ers.Count == 1)
                {
                    var numErs = this.config.IntegerExtractor.Extract(text);

                    foreach (var num in numErs)
                    {
                        int midStrBegin = 0, midStrEnd = 0;

                        // ending number
                        if (num.Start > ers[0].Start + ers[0].Length)
                        {
                            midStrBegin = ers[0].Start + ers[0].Length ?? 0;
                            midStrEnd = num.Start - midStrBegin ?? 0;
                        }
                        else if (num.Start + num.Length < ers[0].Start)
                        {
                            midStrBegin = num.Start + num.Length ?? 0;
                            midStrEnd = ers[0].Start - midStrBegin ?? 0;
                        }

                        // check if the middle string between the time point and the valid number is a connect string.
                        var middleStr = text.Substring(midStrBegin, midStrEnd);
                        var tillMatch = this.config.TillRegex.Match(middleStr);
                        if (tillMatch.Success)
                        {
                            num.Data = null;
                            num.Type = Constants.SYS_DATETIME_TIME;
                            ers.Add(num);
                            validTimeNumber = true;
                            break;
                        }
                    }

                    ers.Sort((x, y) => (x.Start - y.Start ?? 0));
                }

                if (!validTimeNumber)
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

            var beginTime = (DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue;
            var endTime = (DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue;

            if (!string.IsNullOrEmpty(ampmStr2) && ampmStr2.EndsWith(Constants.Comment_AmPm) && endTime <= beginTime && endTime.AddHours(Constants.HalfDayHourCount) > beginTime)
            {
                endTime = endTime.AddHours(Constants.HalfDayHourCount);
                ((DateTimeResolutionResult)pr2.Value).FutureValue = endTime;
                pr2.TimexStr = $"T{endTime.Hour}";
                if (endTime.Minute > 0)
                {
                    pr2.TimexStr = $"{pr2.TimexStr}:{endTime.Minute}";
                }
            }

            if (!string.IsNullOrEmpty(ampmStr1) && ampmStr1.EndsWith(Constants.Comment_AmPm) && endTime > beginTime.AddHours(Constants.HalfDayHourCount))
            {
                beginTime = beginTime.AddHours(Constants.HalfDayHourCount);
                ((DateTimeResolutionResult)pr1.Value).FutureValue = beginTime;
                pr1.TimexStr = $"T{beginTime.Hour}";
                if (beginTime.Minute > 0)
                {
                    pr1.TimexStr = $"{pr1.TimexStr}:{beginTime.Minute}";
                }
            }

            if (endTime < beginTime)
            {
                endTime = endTime.AddDays(1);
            }

            var minutes = (endTime - beginTime).Minutes;
            var hours = (endTime - beginTime).Hours;
            ret.Timex = $"({pr1.TimexStr},{pr2.TimexStr}," +
                        $"PT{(hours > 0 ? hours + "H" : string.Empty)}{(minutes > 0 ? minutes + "M" : string.Empty)})";
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginTime, endTime);
            ret.Success = true;

            if (!string.IsNullOrEmpty(ampmStr1) && ampmStr1.EndsWith(Constants.Comment_AmPm) &&
                !string.IsNullOrEmpty(ampmStr2) && ampmStr2.EndsWith(Constants.Comment_AmPm))
            {
                ret.Comment = Constants.Comment_AmPm;
            }

            if (((DateTimeResolutionResult)pr1.Value).TimeZoneResolution != null)
            {
                ret.TimeZoneResolution = ((DateTimeResolutionResult)pr1.Value).TimeZoneResolution;
            }
            else if (((DateTimeResolutionResult)pr2.Value).TimeZoneResolution != null)
            {
                ret.TimeZoneResolution = ((DateTimeResolutionResult)pr2.Value).TimeZoneResolution;
            }

            ret.SubDateTimeEntities = new List<object> { pr1, pr2 };

            return ret;
        }

        // parse "morning", "afternoon", "night"
        private DateTimeResolutionResult ParseTimeOfDay(string text, DateObject referenceTime)
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
                    text = text.Replace(early, string.Empty);
                    hasEarly = true;
                    ret.Comment = Constants.Comment_Early;
                }

                if (!hasEarly && !string.IsNullOrEmpty(match.Groups["late"].Value))
                {
                    var late = match.Groups["late"].Value;
                    text = text.Replace(late, string.Empty);
                    hasLate = true;
                    ret.Comment = Constants.Comment_Late;
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
                DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMinSeg, endMinSeg));

            ret.Success = true;

            return ret;
        }
    }
}