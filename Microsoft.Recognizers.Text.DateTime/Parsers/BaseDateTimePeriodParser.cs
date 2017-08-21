using System;
using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIMEPERIOD;
        
        protected readonly IDateTimePeriodParserConfiguration Config;

        public BaseDateTimePeriodParser(IDateTimePeriodParserConfiguration configuration)
        {
            Config = configuration;
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
                    innerResult = ParseSpecificNight(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseNumberWithUnit(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_DATETIME,
                            FormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_DATETIME,
                            FormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item2)
                        }
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_DATETIME,
                            FormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_DATETIME,
                            FormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item2)
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
            var trimedText = text.Trim().ToLower();

            var match = this.Config.PureNumberFromToRegex.Match(trimedText);
            if (!match.Success)
            {
                match = this.Config.PureNumberBetweenAndRegex.Match(trimedText);
            }

            if (match.Success && match.Index == 0)
            {
                // this "from .. to .." pattern is valid if followed by a Date OR "pm"
                var hasAm = false;
                var hasPm = false;
                var dateStr = "XXXX-XX-XX";

                // get hours
                var hourGroup = match.Groups["hour"];
                var hourStr = hourGroup.Captures[0].Value;
                var beginHour = 0;

                if (this.Config.Numbers.ContainsKey(hourStr))
                {
                    beginHour = this.Config.Numbers[hourStr];
                }
                else
                {
                    beginHour = int.Parse(hourStr);
                }

                hourStr = hourGroup.Captures[1].Value;
                var endHour = 0;

                if (this.Config.Numbers.ContainsKey(hourStr))
                {
                    endHour = this.Config.Numbers[hourStr];
                }
                else
                {
                    endHour = int.Parse(hourStr);
                }

                // parse following date
                var er = this.Config.DateExtractor.Extract(trimedText.Substring(match.Length));

                DateObject futureTime;
                DateObject pastTime;
                if (er.Count > 0)
                {
                    var pr = this.Config.DateParser.Parse(er[0], referenceTime);
                    if (pr.Value != null)
                    {
                        futureTime = (DateObject) ((DateTimeResolutionResult) pr.Value).FutureValue;
                        pastTime = (DateObject) ((DateTimeResolutionResult) pr.Value).PastValue;

                        dateStr = pr.TimexStr;
                    }
                    else
                    {
                        return ret;
                    }
                }
                else
                {
                    return ret;
                }

                // parse "pm" 
                var pmStr = match.Groups["pm"].Value;
                var amStr = match.Groups["am"].Value;
                var descStr = match.Groups["desc"].Value;
                if (!string.IsNullOrEmpty(amStr) || !string.IsNullOrEmpty(descStr) && descStr.StartsWith("a"))
                {
                    if (beginHour >= 12)
                    {
                        beginHour -= 12;
                    }

                    if (endHour >= 12)
                    {
                        endHour -= 12;
                    }

                    hasAm = true;
                }
                else if (!string.IsNullOrEmpty(pmStr) || !string.IsNullOrEmpty(descStr) && descStr.StartsWith("p"))
                {
                    if (beginHour < 12)
                    {
                        beginHour += 12;
                    }

                    if (endHour < 12)
                    {
                        endHour += 12;
                    }

                    hasPm = true;
                }

                var ampmStr = string.Empty;
                if (!hasAm && !hasPm && beginHour <= 12 && endHour <= 12)
                {
                    //ampmStr = "ampm";
                    ret.Comment = "ampm";
                }

                var beginStr = dateStr + "T" + beginHour.ToString("D2") + ampmStr;
                var endStr = dateStr + "T" + endHour.ToString("D2") + ampmStr;

                ret.Timex = $"({beginStr},{endStr},PT{endHour - beginHour}H)";

                ret.FutureValue = new Tuple<DateObject, DateObject>(
                    new DateObject(futureTime.Year, futureTime.Month, futureTime.Day, beginHour, 0, 0),
                    new DateObject(futureTime.Year, futureTime.Month, futureTime.Day, endHour, 0, 0));

                ret.PastValue = new Tuple<DateObject, DateObject>(
                    new DateObject(pastTime.Year, pastTime.Month, pastTime.Day, beginHour, 0, 0),
                    new DateObject(pastTime.Year, pastTime.Month, pastTime.Day, endHour, 0, 0));

                ret.Success = true;

                return ret;
            }

            return ret;
        }

        private DateTimeResolutionResult MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            DateTimeParseResult pr1 = null, pr2 = null;
            bool bothHasDate = false, beginHasDate = false, endHasDate = false;

            var er1 = this.Config.TimeExtractor.Extract(text);

            var er2 = this.Config.DateTimeExtractor.Extract(text);
            if (er2.Count == 2)
            {
                pr1 = this.Config.DateTimeParser.Parse(er2[0], referenceTime);
                pr2 = this.Config.DateTimeParser.Parse(er2[1], referenceTime);
                bothHasDate = true;
            }
            else if (er2.Count == 1 && er1.Count == 2)
            {
                if (!er2[0].IsOverlap(er1[0]))
                {
                    pr1 = this.Config.TimeParser.Parse(er1[0], referenceTime);
                    pr2 = this.Config.DateTimeParser.Parse(er2[0], referenceTime);
                    endHasDate = true;
                }
                else
                {
                    pr1 = this.Config.DateTimeParser.Parse(er2[0], referenceTime);
                    pr2 = this.Config.TimeParser.Parse(er1[1], referenceTime);
                    beginHasDate = true;
                }
            }
            else if (er2.Count == 1 && er1.Count == 1)
            {
                if (er1[0].Start < er2[0].Start)
                {
                    pr1 = this.Config.TimeParser.Parse(er1[0], referenceTime);
                    pr2 = this.Config.DateTimeParser.Parse(er2[0], referenceTime);
                    endHasDate = true;
                }
                else
                {
                    pr1 = this.Config.DateTimeParser.Parse(er2[0], referenceTime);
                    pr2 = this.Config.TimeParser.Parse(er1[0], referenceTime);
                    beginHasDate = true;
                }
            }
            else if (er1.Count == 2)
            {
                // if both ends are Time. then this is a TimePeriod, not a DateTimePeriod
                return ret;
            }
            else
            {
                return ret;
            }

            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            DateObject futureBegin = (DateObject) ((DateTimeResolutionResult) pr1.Value).FutureValue,
                futureEnd = (DateObject) ((DateTimeResolutionResult) pr2.Value).FutureValue;

            DateObject pastBegin = (DateObject) ((DateTimeResolutionResult) pr1.Value).PastValue,
                pastEnd = (DateObject) ((DateTimeResolutionResult) pr2.Value).PastValue;

            if (bothHasDate)
            {
                if (futureBegin > futureEnd)
                {
                    futureBegin = pastBegin;
                }

                if (pastEnd < pastBegin)
                {
                    pastEnd = futureEnd;
                }
            }

            if (bothHasDate)
            {
                ret.Timex =
                    $"({pr1.TimexStr},{pr2.TimexStr},PT{Convert.ToInt32((futureEnd - futureBegin).TotalHours)}H)";
                // do nothing
            }
            else if (beginHasDate)
            {
                futureEnd = new DateObject(futureBegin.Year, futureBegin.Month, futureBegin.Day,
                    futureEnd.Hour, futureEnd.Minute, futureEnd.Second);

                pastEnd = new DateObject(pastBegin.Year, pastBegin.Month, pastBegin.Day,
                    pastEnd.Hour, pastEnd.Minute, pastEnd.Second);

                var dateStr = pr1.TimexStr.Split('T')[0];
                ret.Timex =
                    $"({pr1.TimexStr},{dateStr + pr2.TimexStr},PT{Convert.ToInt32((futureEnd - futureBegin).TotalHours)}H)";
            }
            else if (endHasDate)
            {
                futureBegin = new DateObject(futureEnd.Year, futureEnd.Month, futureEnd.Day,
                    futureBegin.Hour, futureBegin.Minute, futureBegin.Second);

                pastBegin = new DateObject(pastEnd.Year, pastEnd.Month, pastEnd.Day,
                    pastBegin.Hour, pastBegin.Minute, pastBegin.Second);

                var dateStr = pr2.TimexStr.Split('T')[0];
                ret.Timex =
                    $"({dateStr + pr1.TimexStr},{pr2.TimexStr},PT{Convert.ToInt32((futureEnd - futureBegin).TotalHours)}H)";
            }

            var ampmStr1 = ((DateTimeResolutionResult)pr1.Value).Comment;
            var ampmStr2 = ((DateTimeResolutionResult)pr2.Value).Comment;
            if (!string.IsNullOrEmpty(ampmStr1) && ampmStr1.EndsWith("ampm") && !string.IsNullOrEmpty(ampmStr2) && ampmStr2.EndsWith("ampm"))
            {
                ret.Comment = "ampm";
            }

            ret.FutureValue = new Tuple<DateObject, DateObject>(futureBegin, futureEnd);
            ret.PastValue = new Tuple<DateObject, DateObject>(pastBegin, pastEnd);
            ret.Success = true;

            return ret;
        }


        // parse "this night"
        protected virtual DateTimeResolutionResult ParseSpecificNight(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimedText = text.Trim().ToLowerInvariant();
            
            // handle morning, afternoon..
            int beginHour, endHour, endMin = 0;
            string timeStr;
            if (!this.Config.GetMatchedTimeRange(trimedText, out timeStr, out beginHour, out endHour, out endMin))
            {
                return ret;
            }

            var match = this.Config.SpecificNightRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var swift = this.Config.GetSwiftPrefix(trimedText);

                var date = referenceTime.AddDays(swift).Date;
                int day = date.Day, month = date.Month, year = date.Year;

                ret.Timex = FormatUtil.FormatDate(date) + timeStr;

                ret.FutureValue =
                    ret.PastValue =
                        new Tuple<DateObject, DateObject>(new DateObject(year, month, day, beginHour, 0, 0),
                            new DateObject(year, month, day, endHour, endMin, endMin));

                ret.Success = true;
                return ret;
            }


            // handle Date followed by morning, afternoon
            match = this.Config.NightRegex.Match(trimedText);
            if (match.Success)
            {
                var beforeStr = trimedText.Substring(0, match.Index).Trim();
                var ers = this.Config.DateExtractor.Extract(beforeStr);
                if (ers.Count == 0 || ers[0].Length != beforeStr.Length)
                {
                    return ret;
                }

                var pr = this.Config.DateParser.Parse(ers[0], referenceTime);
                var futureDate = (DateObject) ((DateTimeResolutionResult) pr.Value).FutureValue;
                var pastDate = (DateObject) ((DateTimeResolutionResult) pr.Value).PastValue;

                ret.Timex = pr.TimexStr + timeStr;

                ret.FutureValue =
                    new Tuple<DateObject, DateObject>(
                        new DateObject(futureDate.Year, futureDate.Month, futureDate.Day, beginHour, 0, 0),
                        new DateObject(futureDate.Year, futureDate.Month, futureDate.Day, endHour, endMin, endMin));

                ret.PastValue =
                    new Tuple<DateObject, DateObject>(
                        new DateObject(pastDate.Year, pastDate.Month, pastDate.Day, beginHour, 0, 0),
                        new DateObject(pastDate.Year, pastDate.Month, pastDate.Day, endHour, endMin, endMin));

                ret.Success = true;

                return ret;
            }

            return ret;
        }

        // parse "in 20 minutes"
        private DateTimeResolutionResult ParseNumberWithUnit(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            string numStr, unitStr;

            // if there are spaces between nubmer and unit
            var ers = this.Config.CardinalExtractor.Extract(text);
            if (ers.Count == 1)
            {
                var pr = this.Config.NumberParser.Parse(ers[0]);
                var srcUnit = text.Substring(ers[0].Start + ers[0].Length ?? 0).Trim().ToLower();
                var beforeStr = text.Substring(0, ers[0].Start ?? 0).Trim().ToLowerInvariant();

                if (this.Config.UnitMap.ContainsKey(srcUnit))
                {
                    numStr = pr.ResolutionStr;
                    unitStr = this.Config.UnitMap[srcUnit];
                    var prefixMatch = this.Config.PastRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "H":
                                beginDate = referenceTime.AddHours(-(double) pr.Value);
                                endDate = referenceTime;
                                break;
                            case "M":
                                beginDate = referenceTime.AddMinutes(-(double) pr.Value);
                                endDate = referenceTime;
                                break;
                            case "S":
                                beginDate = referenceTime.AddSeconds(-(double) pr.Value);
                                endDate = referenceTime;
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({FormatUtil.LuisDate(beginDate)}T{FormatUtil.LuisTime(beginDate)},{FormatUtil.LuisDate(endDate)}T{FormatUtil.LuisTime(endDate)},PT{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }

                    prefixMatch = this.Config.FutureRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "H":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddHours((double) pr.Value);
                                break;
                            case "M":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddMinutes((double) pr.Value);
                                break;
                            case "S":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddSeconds((double) pr.Value);
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({FormatUtil.LuisDate(beginDate)}T{FormatUtil.LuisTime(beginDate)},{FormatUtil.LuisDate(endDate)}T{FormatUtil.LuisTime(endDate)},PT{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;

                        return ret;
                    }
                }
            }

            // if there are NO spaces between number and unit
            var match = this.Config.NumberCombinedWithUnitRegex.Match(text);
            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value.ToLower();
                var beforeStr = text.Substring(0, match.Index).Trim().ToLowerInvariant();
                if (this.Config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.Config.UnitMap[srcUnit];
                    numStr = match.Groups["num"].Value;

                    var prefixMatch = this.Config.PastRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "H":
                                beginDate = referenceTime.AddHours(-double.Parse(numStr));
                                endDate = referenceTime;
                                break;
                            case "M":
                                beginDate = referenceTime.AddMinutes(-double.Parse(numStr));
                                endDate = referenceTime;
                                break;
                            case "S":
                                beginDate = referenceTime.AddSeconds(-double.Parse(numStr));
                                endDate = referenceTime;
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({FormatUtil.LuisDate(beginDate)}T{FormatUtil.LuisTime(beginDate)},{FormatUtil.LuisDate(endDate)}T{FormatUtil.LuisTime(endDate)},PT{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;

                        return ret;
                    }

                    prefixMatch = this.Config.FutureRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "H":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddHours(double.Parse(numStr));
                                break;
                            case "M":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddMinutes(double.Parse(numStr));
                                break;
                            case "S":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddSeconds(double.Parse(numStr));
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({FormatUtil.LuisDate(beginDate)}T{FormatUtil.LuisTime(beginDate)},{FormatUtil.LuisDate(endDate)}T{FormatUtil.LuisTime(endDate)},PT{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;

                        return ret;
                    }
                }
            }

            // handle "last hour"
            match = this.Config.UnitRegex.Match(text);
            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value.ToLower();
                var beforeStr = text.Substring(0, match.Index).Trim().ToLowerInvariant();
                if (this.Config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.Config.UnitMap[srcUnit];
                    var prefixMatch = this.Config.PastRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "H":
                                beginDate = referenceTime.AddHours(-1);
                                endDate = referenceTime;
                                break;
                            case "M":
                                beginDate = referenceTime.AddMinutes(-1);
                                endDate = referenceTime;
                                break;
                            case "S":
                                beginDate = referenceTime.AddSeconds(-1);
                                endDate = referenceTime;
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({FormatUtil.LuisDate(beginDate)}T{FormatUtil.LuisTime(beginDate)},{FormatUtil.LuisDate(endDate)}T{FormatUtil.LuisTime(endDate)},PT1{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;

                        return ret;
                    }

                    prefixMatch = this.Config.FutureRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "H":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddHours(1);
                                break;
                            case "M":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddMinutes(1);
                                break;
                            case "S":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddSeconds(1);
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({FormatUtil.LuisDate(beginDate)}T{FormatUtil.LuisTime(beginDate)},{FormatUtil.LuisDate(endDate)}T{FormatUtil.LuisTime(endDate)},PT1{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;

                        return ret;
                    }
                }
            }

            return ret;
        }
    }
}